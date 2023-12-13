using AspNetCore.Authentication.Identity.DTOs;
using AspNetCore.Authentication.Identity.Email;
using AspNetCore.Authentication.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace AspNetCore.Authentication.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = Schemes)]
//[Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},Identity.Application")]
public class UserController : ControllerBase
{
    // cái nào handle event thì để ở cuối
    private const string Schemes = $"Identity.Application,{JwtBearerDefaults.AuthenticationScheme}";

    #region Ctor
    private readonly ILogger<UserController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IEmailSender _emailSender;
    private readonly UrlEncoder _urlEncoder;

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public UserController(
        ILogger<UserController> logger,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        IEmailSender emailSender,
        UrlEncoder urlEncoder)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailSender = emailSender;
        _urlEncoder = urlEncoder;
    }
    #endregion

    #region Update Profile
    [HttpPut("Profile")]
    public async Task<IActionResult> Profile(UpdateProfileModel Input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
                detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                return Problem(
                    detail: "Unexpected error when trying to set phone number.",
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }

        //await _signInManager.RefreshSignInAsync(user); // refresh lại cookie (reset time cookie)

        return Ok("Your profile has been updated");
    }
    #endregion

    #region Update Info
    [HttpPut("info")]
    public async Task<IActionResult> Profile(InfoRequest infoRequest)
    {
        if (await _userManager.GetUserAsync(User) is not { } user) return NotFound();

        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
        {
            if (string.IsNullOrEmpty(infoRequest.OldPassword))
            {

                ModelState.AddModelError("OldPasswordRequired", "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");
                return ValidationProblem(modelStateDictionary: ModelState);
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                changePasswordResult.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                return ValidationProblem(modelStateDictionary: ModelState);
            }
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
        {
            var email = await _userManager.GetEmailAsync(user);

            if (email != infoRequest.NewEmail)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, infoRequest.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Link("ConfirmEmailChange", new { userId, email = infoRequest.NewEmail, code });

                await _emailSender.SendEmailAsync(
                    infoRequest.NewEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
        }

        if (!string.IsNullOrEmpty(infoRequest.PhoneNumber))
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            if (infoRequest.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, infoRequest.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return Problem(
                        detail: "Unexpected error when trying to set phone number.",
                        statusCode: StatusCodes.Status400BadRequest);
                }
            }
        }

        return Ok(await CreateInfoResponseAsync(user, User, _userManager));
    }
    #endregion

    #region GET info

    [HttpGet("info")]
    public async Task<IActionResult> Profile()
    {
        if (await _userManager.GetUserAsync(User) is not { } user) return NotFound();

        return Ok(await CreateInfoResponseAsync(user, User, _userManager));
    }

    #endregion

    #region SetPassword

    [HttpPost("SetPassword")]
    public async Task<IActionResult> SetPassword(SetPasswordModel Input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);

        if (hasPassword)
        {
            return Problem(
             detail: $"You account is haved password, pls ChangePassword.",
             statusCode: StatusCodes.Status400BadRequest);
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            foreach (var error in addPasswordResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelStateDictionary: ModelState);
        }

        //await _signInManager.RefreshSignInAsync(user);

        return Ok("Your password has been set.");
    }
    #endregion

    #region ChangePassword

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel Input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
        {
            return Problem(
             detail: $"You account is not have password, pls SetPassword.",
             statusCode: StatusCodes.Status400BadRequest);
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelStateDictionary: ModelState);
        }

        //await _signInManager.RefreshSignInAsync(user);

        _logger.LogInformation("User changed their password successfully.");
        return Ok("Your password has been changed.");
    }
    #endregion

    #region DownloadPersonalData

    [HttpPost("DownloadPersonalData")]
    public async Task<IActionResult> DownloadPersonalData()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

        // Only include personal data for download
        var personalData = new Dictionary<string, string>();
        var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
        foreach (var p in personalDataProps)
        {
            personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
        }

        var logins = await _userManager.GetLoginsAsync(user);
        foreach (var l in logins)
        {
            personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
        }

        personalData.Add($"Authenticator Key", await _userManager.GetAuthenticatorKeyAsync(user));

        Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
        return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
    }
    #endregion

    #region DeletePersonalData

    [HttpDelete("DeletePersonalData")]
    public async Task<IActionResult> DeletePersonalData(DeletePersonalDataModel Input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var RequirePassword = await _userManager.HasPasswordAsync(user);

        if (RequirePassword)
        {
            if (Input.Password == null)
            {
                return Problem(
                    detail: "Password is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                return Problem(
                    detail: "Incorrect password.",
                    statusCode: StatusCodes.Status400BadRequest);
            }
        }

        var result = await _userManager.DeleteAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred deleting user.");
        }

        await _signInManager.SignOutAsync();

        _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        return Ok($"Your account '{userId}' has been deleted.");
    }
    #endregion

    #region IsHasPassword

    [HttpGet("IsHasPassword")]
    public async Task<IActionResult> IsHasPassword()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }
        var IsHasPassword = await _userManager.HasPasswordAsync(user);
        return Ok(IsHasPassword);
    }
    #endregion

    #region IsEmailConfirmed

    [HttpGet("IsEmailConfirmed")]
    public async Task<IActionResult> IsEmailConfirmed()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }
        var IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        return Ok(IsEmailConfirmed);
    }
    #endregion

    #region SendVerificationEmail

    [HttpPost("SendVerificationEmail")]
    public async Task<IActionResult> SendVerificationEmail(string returnUrl = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Link("ConfirmEmail", new { userId, code, returnUrl });

        await _emailSender.SendEmailAsync(
            email,
            "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return Ok("Verification email sent. Please check your email.");
    }
    #endregion

    #region ChangeEmail

    [HttpPut("ChangeEmail/{NewEmail}")]
    public async Task<IActionResult> ChangeEmail([EmailAddress] string NewEmail, string returnUrl = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var email = await _userManager.GetEmailAsync(user);
        if (NewEmail != email)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Link("ConfirmEmailChange", new { userId, email = NewEmail, code, returnUrl });

            await _emailSender.SendEmailAsync(
                NewEmail,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return Ok("Confirmation link to change email sent. Please check your email.");
        }
        return Ok("Your email is unchanged.");
    }
    #endregion

    #region GenerateRecoveryCodes
    [HttpPost("GenerateRecoveryCodes")]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (!isTwoFactorEnabled)
        {
            throw new InvalidOperationException("Cannot generate recovery codes for user because they do not have 2FA enabled.");
        }

        var userId = await _userManager.GetUserIdAsync(user);

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        _logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
        return Ok(new { StatusMessage = "You have generated new recovery codes.", recoveryCodes });
    }
    #endregion

    #region ResetAuthenticator

    [HttpPut("ResetAuthenticator")]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", userId);

        //await _signInManager.RefreshSignInAsync(user);

        return Ok("Your authenticator app key has been reset, this process disables 2FA until you verify your authenticator app. You will need to configure your authenticator app using the new key.");
    }
    #endregion

    #region Disable2fa

    [HttpPut("Disable2fa")]
    public async Task<IActionResult> Disable2fa()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            throw new InvalidOperationException($"Cannot disable 2FA for user as it's not currently enabled.");
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2faResult.Succeeded)
        {
            throw new InvalidOperationException($"Unexpected error occurred disabling 2FA.");
        }

        _logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userManager.GetUserId(User));

        // Disabling 2FA does not change the keys used in authenticator apps.
        // If you wish to change the key used in an authenticator app you should reset your authenticator keys.
        return Ok("2fa has been disabled. You can reenable 2fa when you setup an authenticator app");
    }
    #endregion

    #region 2fa
    [HttpPost("2fa")]
    public async Task<IActionResult> EnableTwoFactorAuthenticator(TwoFactorRequest tfaRequest)
    {
        if (await _userManager.GetUserAsync(User) is not { } user) return NotFound();

        if (tfaRequest.Enable == true)
        {
            if (tfaRequest.ResetSharedKey)
            {
                ModelState.AddModelError("CannotResetSharedKeyAndEnable", "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
                return ValidationProblem(ModelState);
            }
            else if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
            {
                ModelState.AddModelError("RequiresTwoFactor", "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
                return ValidationProblem(ModelState);
            }
            else if (!await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
            {
                ModelState.AddModelError("InvalidTwoFactorCode", "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");
                return ValidationProblem(ModelState);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }
        else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }

        if (tfaRequest.ResetSharedKey)
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
        }

        string[]? recoveryCodes = null;
        if (tfaRequest.ResetRecoveryCodes || tfaRequest.Enable == true && await _userManager.CountRecoveryCodesAsync(user) == 0)
        {
            var recoveryCodesEnumerable = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if (tfaRequest.ForgetMachine)
        {
            await _signInManager.ForgetTwoFactorClientAsync();
        }

        var key = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
            {
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
            }
        }
        _logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", user.Id);
        return Ok(new TwoFactorResponse
        {
            SharedKey = FormatKey(key),
            RecoveryCodes = recoveryCodes,
            RecoveryCodesLeft = recoveryCodes?.Length ?? await _userManager.CountRecoveryCodesAsync(user),
            IsTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
            AuthenticatorUri = GenerateQrCodeUri(user.Email, key)
        });
    }
    #endregion

    #region GET EnableAuthenticator
    [HttpGet("EnableAuthenticator")]
    public async Task<IActionResult> EnableAuthenticator()
    {

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        // Load the authenticator key & QR code URI to display on the form
        var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var sharedKey = FormatKey(unformattedKey);

        var email = await _userManager.GetEmailAsync(user);
        var authenticatorUri = GenerateQrCodeUri(email, unformattedKey);

        return Ok(new
        {
            StatusMessage = $"Scan the QR Code or enter this key '{sharedKey}' into your two factor authenticator app. Once you have scanned the QR code or input the key above, your two factor authentication app will provide you with a unique code.",
            sharedKey,
            authenticatorUri
        });
    }
    #endregion

    #region POST EnableAuthenticator
    [HttpPost("EnableAuthenticator")]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorModel Input)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        // Strip spaces and hyphens
        var verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2faTokenValid)
        {
            ModelState.AddModelError("Code", "Verification code is invalid.");
            return ValidationProblem(ModelState);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        var userId = await _userManager.GetUserIdAsync(user);
        _logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

        if (await _userManager.CountRecoveryCodesAsync(user) == 0)
        {
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return Ok(new
            {
                StatusMessage = "Your authenticator app has been verified and you have generated new recovery codes.",
                recoveryCodes
            });

        }

        return Ok(new { StatusMessage = "Your authenticator app has been verified." });
    }
    #endregion

    #region FormatKey
    private string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }
    #endregion

    #region GenerateQrCodeUri
    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            AuthenticatorUriFormat,
            _urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }
    #endregion

    #region ForgetBrowser
    [HttpPost("ForgetBrowser")]
    public async Task<IActionResult> ForgetBrowser()
    {
        // api ForgetBrowser logout Machine khỏi thiết bị hiện tại, là lần sau đăng nhập sẽ phải nhập mã 2fa
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }
        await _signInManager.ForgetTwoFactorClientAsync();
        return Ok("The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.");
    }
    #endregion

    #region IsMachineRemembered

    [HttpGet("IsMachineRemembered")]
    public async Task<IActionResult> IsMachineRemembered()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var isMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
        return Ok(isMachineRemembered);
    }
    #endregion

    #region HasAuthenticator

    [HttpGet("HasAuthenticator")]
    public async Task<IActionResult> HasAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var hasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
        return Ok(hasAuthenticator);
    }
    #endregion

    #region Is2faEnabled

    [HttpGet("Is2faEnabled")]
    public async Task<IActionResult> Is2faEnabled()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }
        var is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        return Ok(is2faEnabled);
    }
    #endregion

    #region RecoveryCodesLeft

    [HttpGet("RecoveryCodesLeft")]
    public async Task<IActionResult> RecoveryCodesLeft()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }
        var recoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
        return Ok(recoveryCodesLeft);
    }
    #endregion

    #region ExternalLogins/CurrentLogins

    [HttpGet("ExternalLogins/CurrentLogins")]
    public async Task<IActionResult> ExternalLoginsCurrentLogins()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var CurrentLogins = await _userManager.GetLoginsAsync(user);

        if (CurrentLogins.IsNullOrEmpty())
        {
            return Problem(
                     detail: $"Your are no external authentication services configured",
                     statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(CurrentLogins.Select(l => new { l.LoginProvider, l.ProviderKey }));
    }
    #endregion

    #region ExternalLogins/OtherLogins

    [HttpGet("ExternalLogins/OtherLogins")]
    public async Task<IActionResult> ExternalLoginsOtherLogins()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var CurrentLogins = await _userManager.GetLoginsAsync(user);
        var OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
             .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
             .ToList();

        if (OtherLogins.IsNullOrEmpty())
        {
            return Problem(
                     detail: $"your haved link all external, so you not have any other external logins",
                     statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(OtherLogins.Select(l => new { l.Name }));
    }
    #endregion

    #region ExternalLogins/RemoveLogin
    [HttpDelete("ExternalLogins/RemoveLogin/{loginProvider}/{providerKey}")]
    public async Task<IActionResult> ExternalLoginsRemoveLogin(string loginProvider, string providerKey)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var CurrentLogins = await _userManager.GetLoginsAsync(user);

        string passwordHash = null;
        if (_userStore is IUserPasswordStore<ApplicationUser> userPasswordStore)
        {
            passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
        }

        if (!(passwordHash != null || CurrentLogins.Count > 1))
        {
            return Problem(
                    detail: $"Your account is not protected by a password and you do not have any external logins to remove.",
                    statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
        if (!result.Succeeded)
        {
            return Problem(
                       detail: $"The external login was not removed.",
                       statusCode: StatusCodes.Status400BadRequest);
        }

        //await _signInManager.RefreshSignInAsync(user);

        return Ok("The external login was removed.");
    }
    #endregion

    #region ExternalLogins/LinkLogin
    /// <remarks>
    /// 
    /// [Link Google](https://localhost:7217/api/User/ExternalLogins/LinkLogin?provider=Google)
    /// 
    /// [Link Facebook](https://localhost:7217/api/User/ExternalLogins/LinkLogin?provider=Facebook)
    /// 
    /// [Link Microsoft](https://localhost:7217/api/User/ExternalLogins/LinkLogin?provider=Microsoft)
    /// 
    /// [Link Twitter](https://localhost:7217/api/User/ExternalLogins/LinkLogin?provider=Twitter)
    /// 
    /// </remarks>

    [HttpGet("ExternalLogins/LinkLogin")]
    public async Task<IActionResult> ExternalLoginsLinkLogin(string provider, string returnUrl = null)
    {
        // Clear the existing external cookie to ensure a clean login process
        // xóa cookie bên ngoài hiện tại để đảm bảo quá trình đăng nhập sạch sẽ
        // xóa thông tin đăng nhập bên ngoài như google,.. trong session
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Kiểm tra yêu cầu dịch vụ provider tồn tại
        var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        var provider_process = listprovider.Find((m) => m.Name == provider);
        if (provider_process == null)
        {
            return Problem(
                     detail: $"Dịch vụ không chính xác: {provider}",
                     statusCode: StatusCodes.Status404NotFound);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var CurrentLogins = await _userManager.GetLoginsAsync(user);

        var isHasProviderLogin = CurrentLogins.Any(ul => ul.LoginProvider == provider);

        if (isHasProviderLogin)
        {
            return Problem(
                    detail: $"Your haved link login: {provider}",
                    statusCode: StatusCodes.Status400BadRequest);
        }

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Link("ExternalLoginsLinkLoginCallback", new { returnUrl });

        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
        return new ChallengeResult(provider, properties);
    }
    #endregion

    #region ExternalLogins/LinkLoginCallback

    [HttpGet("ExternalLogins/LinkLoginCallback", Name = "ExternalLoginsLinkLoginCallback")]
    public async Task<IActionResult> ExternalLoginsLinkLoginCallback(string returnUrl = null)
    {

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Problem(
             detail: $"Unable to load user with ID '{_userManager.GetUserId(User)}'.",
             statusCode: StatusCodes.Status404NotFound);
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var info = await _signInManager.GetExternalLoginInfoAsync(userId);
        if (info == null)
        {
            throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
        }

        var CurrentLogins = await _userManager.GetLoginsAsync(user);

        var isHasProviderLogin = CurrentLogins.Any(ul => ul.LoginProvider == info.LoginProvider);

        if (isHasProviderLogin)
        {
            return Problem(
                    detail: $"Your haved link login: {info.LoginProvider}",
                    statusCode: StatusCodes.Status400BadRequest);
        }

        var result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            return Problem(
                     detail: "The external login was not added. External logins can only be associated with one account.",
                     statusCode: StatusCodes.Status400BadRequest);
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        if (returnUrl is not null) return Redirect(returnUrl);
        return Ok("The external login was added.");
    }
    #endregion

    #region CreateInfoResponseAsync
    private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, ClaimsPrincipal claimsPrincipal, UserManager<TUser> userManager) where TUser : class
    {
        return new()
        {
            Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            Claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value),
        };
    }
    #endregion
}
