using AspNetCore.Identity.DTOs;
using AspNetCore.Identity.Email;
using AspNetCore.Identity.Helpers;
using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace AspNetCore.Identity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    #region Param

    private readonly ILogger<AuthController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IEmailSender _emailSender;

    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;

    #endregion

    #region Ctor
    public AuthController(
      ILogger<AuthController> logger,
      SignInManager<ApplicationUser> signInManager,
      UserManager<ApplicationUser> userManager,
      RoleManager<ApplicationRole> roleManager,
      IUserStore<ApplicationUser> userStore,
      IEmailSender emailSender)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
        _emailSender = emailSender;
        _roleManager = roleManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
    }
    #endregion

    #region Register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registration, string returnUrl = null)
    {
        //var identityUser = new IdentityUser() { UserName = registration.Email, Email = registration.Email };

        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");
        }

        var user = CreateUser();
        await _userStore.SetUserNameAsync(user, registration.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, registration.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            return ValidationProblem(detail: "password not validate!", modelStateDictionary: ModelState);
        }

        _logger.LogInformation("User created a new account with password.");

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            return Problem(detail: "Role User not exist!", statusCode: StatusCodes.Status500InternalServerError);
        }

        await _userManager.AddToRoleAsync(user, "User");

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Link("ConfirmEmail", new { userId, code, returnUrl });

        await _emailSender.SendEmailAsync(registration.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return Created(callbackUrl, new { StatusMessage = "Please check your email to confirm your account." });

    }
    #endregion

    #region Login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest login,
        bool? useCookies,
        bool? useSessionCookies,
        [FromServices] IConfiguration configuration,
        [FromServices] IDataProtectionProvider dataProtectionProvider)
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);

        #region cach 1

        //var identityUser = await _userManager.FindByNameAsync(login.Email);
        //if (identityUser == null)
        //{
        //    return Problem(detail: "Login failed", statusCode: StatusCodes.Status401Unauthorized);
        //}
        //var isValid = await _userManager.CheckPasswordAsync(identityUser, login.Password);
        //if (!isValid)
        //{
        //    return Problem(detail: "Login failed", statusCode: StatusCodes.Status401Unauthorized);
        //}
        //var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Email, identityUser.Email),
        //    new Claim(ClaimTypes.Name, identityUser.UserName)
        //};
        //await _signInManager.SignInWithClaimsAsync(identityUser, true, claims);

        #endregion

        #region Note PasswordSignInAsync
        // AccessFailedCount mặc định max là 5, có thể thay đổi trong Program.cs
        // nếu lockoutOnFailure: true thì mỗi lần nhập sai mật khẩu thì AccessFailedCount +1 nếu đủ 5 lần thì reset lại 0
        // lockoutOnFailure: true nếu nhập sai mật khẩu quá 5 lần thì nó sẽ set date cho colunm LockoutEnd
        // lockoutOnFailure: true nếu login đúng thì AccessFailedCount sẽ reset lại 0
        // còn lockoutOnFailure: false thì AccessFailedCount sẽ không tăng lên khi nhập sai mật khẩu
        // nếu colunm LockoutEnabled là true thì  có 2 trường hợp (không liên quan lockoutOnFailure)
        // 1. nếu LockoutEnd != null và LockoutEnd > DateTime.Now thì login đúng hay sai đều trả về IsLockedOut = true (cho đến khi trường hợp 2 )
        // 2. nếu LockoutEnd == null hoặc LockoutEnd < DateTime.Now nếu login sai mật khẩu thì hàm PasswordSignInAsync trả về Failed (nếu lockoutOnFailure: true thì failed 5 lần sẽ giống như trường hợp 1) còn nếu login đúng thì trả về Success
        // nếu colunm LockoutEnabled là false thì dù LockoutEnd > DateTime.Now thì login đúng thì return Succeeded và sai return Failed
        // options.SignIn.RequireConfirmedAccount = true thì nó check colunm EmailConfirmed nếu true thì Success còn false NotAllow 
        // options.SignIn.RequireConfirmedEmail = true thì tương tự trên
        // options.SignIn.RequireConfirmedPhoneNumber = true thì check colunm PhoneNumberConfirmed nếu true thì Success còn false NotAllow
        // nếu options.SignIn mà false cái nào thì không check cái đó 
        #endregion

        var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(login.TwoFactorCode))
            {
                result = await _signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
            }
            else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
            {
                result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
            }
        }

        //if (result.RequiresTwoFactor)
        //{
        //    return Ok(new
        //    {
        //        Messages = "Your login is protected with an authenticator app. Please log in to 2fa or Login With Recovery to complete the login!",
        //        IsRequiresTwoFactor = true
        //    });
        //}
        //if (result.IsLockedOut)
        //{
        //    _logger.LogWarning("User account locked out.");
        //    return Problem(detail: "This account has been locked out, please try again later.", statusCode: StatusCodes.Status401Unauthorized);
        //}
        //if (result.IsNotAllowed)
        //{
        //    return Problem(detail: "NotAllow", statusCode: StatusCodes.Status403Forbidden);
        //}

        if (!result.Succeeded)
        {
            return Problem(detail: result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }

        AccessTokenResponse token = null;
        var loginType = "Cookies";

        if (!useCookieScheme)
        {
            // PasswordSignInAsync, TwoFactorAuthenticatorSignInAsync, TwoFactorRecoveryCodeSignInAsync về cuối cùng thì nó vào hàm Httpcontext.Signin lưu cookies cho nên không cần cookies thì xóa đi
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            loginType = "Bearer";
            token = await TokenHelper<ApplicationUser>
                               .CreateToken(
                                    username: login.Email,
                                    signInManager: _signInManager,
                                    configuration: configuration,
                                    dataProtectionProvider: dataProtectionProvider);
        }

        _logger.LogInformation("User logged in.");
        return Ok(new { Messages = "login success!", IsRequiresTwoFactor = false, loginType, token });

    }
    #endregion

    #region Refresh
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        RefreshRequest refreshRequest,
        [FromServices] IDataProtectionProvider dataProtectionProvider,
        [FromServices] IConfiguration configuration)
    {

        var user = await TokenHelper<ApplicationUser>.CheckValidRefreshToken(
                     signInManager: _signInManager,
                     refreshToken: refreshRequest.RefreshToken,
                     dataProtectionProvider: dataProtectionProvider);

        if (user is null) return Unauthorized();

        var response = await TokenHelper<ApplicationUser>.CreateToken(
                    user: user,
                    signInManager: _signInManager,
                    configuration: configuration,
                    dataProtectionProvider: dataProtectionProvider);

        return Ok(response);
    }
    #endregion

    #region Logout
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return Ok("You have successfully logged out of the application.");
    }
    #endregion

    #region ConfirmEmail
    [HttpGet("ConfirmEmail", Name = "ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl = null)
    {
        if (userId == null || code == null) return BadRequest();

        if (await _userManager.FindByIdAsync(userId) is not { } user) return Unauthorized();

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            return ValidationProblem(detail: "Error confirming your email.", modelStateDictionary: ModelState);
        }

        if (returnUrl is not null) return Redirect(returnUrl);
        return Ok("Thank you for confirming your email.");
    }
    #endregion

    #region GetLinkConfirmEmail
    [HttpGet("GetLinkConfirmEmail")]
    public async Task<IActionResult> GetLinkConfirmEmail([EmailAddress] string email, string returnUrl = null)
    {
        if (email == null)
        {
            return BadRequest();
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return Problem(
                detail: $"Unable to load user with email '{email}'.",
                statusCode: StatusCodes.Status404NotFound);
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var emailConfirmationUrl = Url.Link("ConfirmEmail", new { userId = userId, code = code, returnUrl = returnUrl });

        return Ok(emailConfirmationUrl);
    }
    #endregion

    #region ResendEmailConfirmation (dùng cho lúc ForgotPassword mà email chưa confirm và token confirm email hết hạn)
    [HttpPost("resendConfirmationEmail")]
    public async Task<IActionResult> ResendEmailConfirmation(ResendEmailRequest resendRequest, string returnUrl = null)
    {
        if (resendRequest.Email == null ||
            await _userManager.FindByEmailAsync(resendRequest.Email) is not { } user) return Unauthorized();

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var callbackUrl = Url.Link("ConfirmEmail", new { userId, code, returnUrl });

        await _emailSender.SendEmailAsync(resendRequest.Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        return Ok("Verification email sent. Please check your email.");
    }
    #endregion

    #region ForgotPassword
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest resetRequest, string returnUrl = null)
    {
        if (await _userManager.FindByEmailAsync(resetRequest.Email) is not { } user ||
            !await _userManager.IsEmailConfirmedAsync(user))
            return Unauthorized();

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        await _emailSender.SendEmailAsync(
            resetRequest.Email, "Reset your password",
            $"Reset your password using the following code: {HtmlEncoder.Default.Encode(code)}");

        return Ok("Please check your email to reset your password.");
    }
    #endregion

    #region ResetPassword
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetRequest)
    {

        if (await _userManager.FindByEmailAsync(resetRequest.Email) is not { } user || !(await _userManager.IsEmailConfirmedAsync(user)))
        {
            return BadRequest();
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
        var result = await _userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            return ValidationProblem(ModelState);
        }

        return Ok("Your password has been reset.");
    }
    #endregion

    #region ConfirmEmailChange
    [HttpGet("ConfirmEmailChange", Name = "ConfirmEmailChange")]
    public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code, string returnUrl = null)
    {
        if (userId == null || email == null || code == null) return Unauthorized();

        if (await _userManager.FindByIdAsync(userId) is not { } user) return Unauthorized();

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        var result = await _userManager.ChangeEmailAsync(user, email, code);
        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            return ValidationProblem(detail: "Error changing email.", modelStateDictionary: ModelState);
        }

        // In our UI email and user name are one and the same, so when we update the email
        // we need to update the user name.
        result = await _userManager.SetUserNameAsync(user, email);
        if (!result.Succeeded)
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
            return ValidationProblem(detail: "Error changing user name.", modelStateDictionary: ModelState);
        }

        if (returnUrl is not null) return Redirect(returnUrl);
        return Ok("Thank you for confirming your email.");
    }
    #endregion

    #region LoginWith2fa
    [HttpPost("LoginWith2fa")]
    public async Task<IActionResult> LoginWith2fa(LoginWith2faModel Input)
    {
        // Ensure the user has gone through the username & password screen first
        // nếu user có enable 2fa thì phải login username và password trước rùi mới login bằng 2fa
        // hoặc _signInManager.ExternalLoginSignInAsync(bypassTwoFactor: false) 
        // nếu user không có enable 2fa thì login username và password thì GetTwoFactorAuthenticationUserAsync vẫn null
        // GetTwoFactorAuthenticationUserAsync khác null khi user đã login bằng username và password
        // và GetTwoFactorAuthenticationUserAsync trả về null khi user chưa login bằng username và password
        // hoặc khi user đã login2fa TwoFactorAuthenticatorSignInAsync hay recovery TwoFactorRecoveryCodeSignInAsync, nếu TwoFactorAuthenticatorSignInAsync hoặc TwoFactorRecoveryCodeSignInAsync trả về success thì GetTwoFactorAuthenticationUserAsync trả về null
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        // RememberMachine nếu là true thì sẽ lưu cookie 2fa ở máy tính đó, nếu là false thì sẽ không lưu cookie 2fa ở máy tính đó
        // nếu RememberMachine là true thì khi login lần sau sẽ không cần nhập code 2fa nữa
        // nếu RememberMachine là false thì khi login lần sau sẽ cần nhập code 2fa
        // nếu muốn đăng xuất khỏi máy tính hay cần đăng nhập 2fa cho lần sau thì dùng api Forgetbrowser
        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, false, Input.RememberMachine);

        var userId = await _userManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
            return Ok("login success!");
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("User with ID '{UserId}' account locked out.", userId);
            return Problem(
                detail: "This account has been locked out, please try again later.",
                statusCode: StatusCodes.Status400BadRequest);
        }
        else
        {
            _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", userId);
            return Problem(detail: "Invalid authenticator code.", statusCode: StatusCodes.Status400BadRequest);
        }
    }
    #endregion

    #region LoginWithRecoveryCode
    [HttpPost("LoginWithRecoveryCode")]
    public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeModel Input)
    {
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load two-factor authentication user.");
        }

        var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

        var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        if (result.Succeeded)
        {
            _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
            return Ok("login success!");
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return Problem(
                detail: "This account has been locked out, please try again later.",
                statusCode: StatusCodes.Status400BadRequest);
        }
        else
        {
            _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
            return Problem(detail: "Invalid recovery code entered.", statusCode: StatusCodes.Status400BadRequest);
        }
    }
    #endregion

    #region ExternalLogin
    /// <remarks>
    /// 
    /// [Login Google](https://localhost:7217/api/Auth/ExternalLogin?provider=Google)
    /// 
    /// [Login Facebook](https://localhost:7217/api/Auth/ExternalLogin?provider=Facebook)
    /// 
    /// [Login Microsoft](https://localhost:7217/api/Auth/ExternalLogin?provider=Microsoft)
    /// 
    /// [Login Twitter](https://localhost:7217/api/Auth/ExternalLogin?provider=Twitter)
    /// 
    /// </remarks>
    [HttpGet("ExternalLogin")]
    public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
    {
        // Kiểm tra yêu cầu dịch vụ provider tồn tại
        var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        var provider_process = listprovider.Find((m) => m.Name == provider);
        if (provider_process == null)
        {
            return Problem(
                    detail: $"Dịch vụ không chính xác: {provider}",
                    statusCode: StatusCodes.Status404NotFound);
        }

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Link("ExternalLoginCallback", new { returnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
        //return new ChallengeResult(provider, properties);
    }
    #endregion

    #region ExternalLoginCallback
    [HttpGet("ExternalLoginCallback", Name = "ExternalLoginCallback")]
    public async Task<IActionResult> ExternalLoginCallback(string remoteError = null, string returnUrl = null)
    {
        if (remoteError != null)
        {
            return Problem(
                detail: $"Error from external provider: {remoteError}",
                statusCode: StatusCodes.Status400BadRequest);
        }

        //sau khi login thành công bằng external provider thì sẽ đưa thông tin của user đó vào link callback https://localhost:7217/signin-google
        // sau đó thì nó lưu thông tin vào session và chuyển hướng đến link https://localhost:7217/api/auth/ExternalLoginCallback
        // để lấy thông tin user vừa login bằng external provider bằng cách _signInManager.GetExternalLoginInfoAsync();
        // nếu chưa login bằng _signInManager.ExternalLoginSignInAsync() thì thông tin vẫn còn trong session 
        // nếu đã login bằng _signInManager.ExternalLoginSignInAsync() thì thông tin sẽ bị xóa khỏi session
        // và call _signInManager.GetExternalLoginInfoAsync(); sẽ trả về null
        // hoặc logout await _signInManager.SignOutAsync(); là thông tin cũng sẽ bị xóa khỏi session và logout user đang login
        // logout Bằng await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme); thì chỉ xóa thông tin trong session chứ không logout uses đang login
        // lưu ý GetExternalLoginInfoAsync() == null khi trong claim không có ClaimTypes.NameIdentifier
        // thường thì khi dùng ClaimActions.MapAll(); mà không có MapJsonKey ClaimTypes.NameIdentifier thì sẽ bị null khi call GetExternalLoginInfoAsync(), chi tiết xem config từng provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Problem(
                     detail: $"Error loading external login information.",
                     statusCode: StatusCodes.Status400BadRequest);
        }

        // Sign in the user with this external login provider if the user already has a login.
        // sau khi login thành công thì nó xóa thông tin trong session
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            if (returnUrl is not null) return Redirect(returnUrl);
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
            return Ok(new { Messages = $"login {info.ProviderDisplayName} success", IsRequiresTwoFactor = false });
        }
        if (result.RequiresTwoFactor)
        {
            return Ok(new
            {
                Messages = "Your login is protected with an authenticator app. Please log in to 2fa or Login With Recovery to complete the login!",
                IsRequiresTwoFactor = true
            });
        }
        if (result.IsLockedOut)
        {
            return Problem(
                      detail: "User account locked out.",
                      statusCode: StatusCodes.Status400BadRequest);
        }
        if (result.IsNotAllowed)
        {
            return Problem(detail: "NotAllow", statusCode: StatusCodes.Status403Forbidden);
        }
        else
        {
            if (returnUrl is not null) return Redirect(returnUrl);// chuyen huong den link confirm email
            // If the user does not have an account, then ask the user to create an account.
            return Ok($"You've successfully authenticated with {info.ProviderDisplayName}, " +
                $"Please register an email address for this site below to finish logging in");
        }
    }
    #endregion

    #region ExternalLoginConfirmation
    [HttpPost("ExternalLoginConfirmation")]
    public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginModel Input, string returnUrl = null)
    {
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Problem(
                    detail: $"Error loading external login information during confirmation.",
                    statusCode: StatusCodes.Status400BadRequest);
        }

        var user = CreateUser();

        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Link("ConfirmEmail", new { userId, code, returnUrl });

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                // If account confirmation is required, we need to show the link if we don't have a real email sender
                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                //{
                //}

                //await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                return Created(callbackUrl, new { StatusMessage = "Please check your email to confirm your account." });
            }
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem(ModelState);
    }
    #endregion

    #region ExternalLoginAndRegister
    /// <remarks>
    /// 
    /// [Login Google](https://localhost:7217/api/Auth/ExternalLoginAndRegister?provider=Google)
    /// 
    /// [Login Facebook](https://localhost:7217/api/Auth/ExternalLoginAndRegister?provider=Facebook)
    /// 
    /// [Login Microsoft](https://localhost:7217/api/Auth/ExternalLoginAndRegister?provider=Microsoft)
    /// 
    /// [Login Twitter](https://localhost:7217/api/Auth/ExternalLoginAndRegister?provider=Twitter)
    /// 
    /// </remarks>
    [HttpGet("ExternalLoginAndRegister")]
    public async Task<IActionResult> ExternalLoginAndRegister(string provider, string returnUrl = null)
    {
        // Kiểm tra yêu cầu dịch vụ provider tồn tại
        var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        var provider_process = listprovider.Find((m) => m.Name == provider);
        if (provider_process == null)
        {
            return Problem(
                     detail: $"Dịch vụ không chính xác: {provider}",
                     statusCode: StatusCodes.Status404NotFound);
        }

        // Request a redirect to the external login provider.
        var redirectUrl = Url.Link("ExternalLoginAndRegisterCallback", new { returnUrl = returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
        //return new ChallengeResult(provider, properties);
    }
    #endregion

    #region ExternalLoginAndRegisterCallback
    [HttpGet("ExternalLoginAndRegisterCallback", Name = "ExternalLoginAndRegisterCallback")]
    public async Task<IActionResult> ExternalLoginAndRegisterCallback(string remoteError = null, string returnUrl = null)
    {
        if (remoteError != null)
        {
            return Problem(
                detail: $"Error from external provider: {remoteError}",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Problem(
                     detail: $"Error loading external login information.",
                     statusCode: StatusCodes.Status400BadRequest);
        }

        var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user == null)
        {
            if (!info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                return Problem(
                       detail: $"Error from external provider: {info.LoginProvider} provider does not provide email.",
                       statusCode: StatusCodes.Status400BadRequest);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            user = CreateUser();
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);

            var resultCreate = await _userManager.CreateAsync(user);
            if (!resultCreate.Succeeded)
            {
                resultCreate.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                return ValidationProblem(ModelState);
            }

            var resultAddLogin = await _userManager.AddLoginAsync(user, info);
            if (!resultAddLogin.Succeeded)
            {
                resultAddLogin.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                return ValidationProblem(ModelState);
            }
            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
        }

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (result.Succeeded)
        {
            if (returnUrl is not null) return Redirect(returnUrl);
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
            return Ok(new { Messages = $"login {info.ProviderDisplayName} success", IsRequiresTwoFactor = false });
        }
        if (result.RequiresTwoFactor)
        {
            return Ok(new
            {
                Messages = "Your login is protected with an authenticator app. Please log in to 2fa or Login With Recovery to complete the login!",
                IsRequiresTwoFactor = true
            });
        }
        if (result.IsLockedOut)
        {
            return Problem(
                      detail: "User account locked out.",
                      statusCode: StatusCodes.Status400BadRequest);
        }
        if (result.IsNotAllowed)
        {
            return Problem(detail: "NotAllow", statusCode: StatusCodes.Status403Forbidden);
        }
        else
        {
            return Problem(
                    detail: $"Error from external provider: {info.LoginProvider}",
                    statusCode: StatusCodes.Status400BadRequest);
        }
    }
    #endregion

    #region ExternalAuthenticationSchemes
    [HttpGet("ExternalAuthenticationSchemes")]
    public async Task<IActionResult> ExternalAuthenticationSchemes()
    {
        var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
        return Ok(externalLogins.Select(c => new { c.Name, c.DisplayName }));
    }
    #endregion

    #region CreateInstance User
    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
    #endregion

    #region GetEmailStore
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }
    #endregion

}