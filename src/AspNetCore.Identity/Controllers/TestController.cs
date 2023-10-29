using AspNetCore.Identity.Helpers;
using AspNetCore.Identity.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Octokit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace AspNetCore.Identity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public const string ResetPasswordTokenPurpose = "ResetPassword";
        public const string ChangePhoneNumberTokenPurpose = "ChangePhoneNumber";
        public const string ConfirmEmailTokenPurpose = "EmailConfirmation";

        public static string GetChangeEmailTokenPurpose(string newEmail) => "ChangeEmail:" + newEmail;

        #region Ctor
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public TestController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserStore<ApplicationUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
        }
        #endregion

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Test()
        {

            //  query database

            #region Crud
            //await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //await _userManager.AddPasswordAsync(user, Input.NewPassword);
            //await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            //await _userManager.DeleteAsync(user);
            //await _userManager.CreateAsync(user, Input.Password); 
            //await _userManager.UpdateAsync(user);
            #endregion

            #region Login
            //await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: true);
            //await _signInManager.RefreshSignInAsync(user); // refresh lại cookie (reset time cookie)
            //await _signInManager.SignInAsync(user, isPersistent: false); // đăng nhập
            //await _signInManager.SignInWithClaimsAsync(user, true, User.Claims); // đăng nhập với claim
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity)); 
            #endregion

            #region Logout
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //await _signInManager.SignOutAsync(); 
            #endregion

            #region Verifi token
            //await _userManager.ConfirmEmailAsync(user, codeEmail);
            //await _userManager.ResetPasswordAsync(user, codePassword, "aA123456!");
            //await _userManager.ChangeEmailAsync(user, "trinmse150418@fpt.edu.vn", codeChangeEmail);
            //await _userManager.ChangePhoneNumberAsync(user, "0123456789", codeChangePhoneNumber); 
            #endregion

            #region Find User
            var user = await _userManager.GetUserAsync(User);
            //var user2 = await _userManager.Users.Where(u => u.Id == user.Id).Include(u => u.Backpack).FirstOrDefaultAsync();
            //var user3 = await _userManager.FindByEmailAsync("nonic54519@twugg.com");
            //var user4 = await _signInManager.UserManager.FindByNameAsync("admin");
            //var user5 = await _userManager.FindByIdAsync(Guid.NewGuid().ToString()); 
            #endregion

            #region Get Info
            var claims = await _userManager.GetClaimsAsync(user); // tìm kiếm claim trên table AspNetUserClaims
            var roles = await _userManager.GetRolesAsync(user);
            var logins = await _userManager.GetLoginsAsync(user);
            var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var a = _userManager.Users.Where(u => u.Id == userId).Include(u => u.Backpack).FirstOrDefaultAsync();

            #endregion

            // get info from Claim and object (không có query trên database)
            #region Get Id
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId2 = _userManager.GetUserId(User);
            var userId3 = await _userManager.GetUserIdAsync(user);
            #endregion

            #region Get info
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var username = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            var hasPassword = await _userManager.HasPasswordAsync(user);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            var twoFactorClientRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            var twoFactorClientRemembered2 = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            #endregion

            #region Check Password
            var checkPassword1 = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, "aA123456!");
            var checkPassword2 = await _userManager.CheckPasswordAsync(user, "aA123456!");
            var checkPassword3 = await _signInManager.UserManager.CheckPasswordAsync(user, "aA123456!");
            #endregion

            #region Generate token
            var codeConfirmEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var codePasswordReset = await _userManager.GeneratePasswordResetTokenAsync(user);
            var codeChangeEmail = await _userManager.GenerateChangeEmailTokenAsync(user, "trinmse150418@fpt.edu.vn");
            var codeChangePhoneNumber = await _userManager.GenerateChangePhoneNumberTokenAsync(user, "0123456789");
            #endregion

            return Ok(new { user });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Code()
        {
            var user = await _userManager.GetUserAsync(User);

            #region codeEmail
            var codeEmail = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var resultcodeEmail = await _userManager.ConfirmEmailAsync(user, codeEmail);

            var codeEmail2 = await _userManager.GenerateUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose);
            var resultcodeEmail2 = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose, codeEmail2);
            #endregion

            #region codePassword
            var codePassword = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resultcodePassword = await _userManager.ResetPasswordAsync(user, codePassword, "aA123456!");

            var codePassword2 = await _userManager.GenerateUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose);
            var resultcodePassword2 = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, codePassword2);

            #endregion

            #region codeChangeEmail
            var codeChangeEmail = await _userManager.GenerateChangeEmailTokenAsync(user, "trinmse150418@fpt.edu.vn");
            var resultcodeChangeEmail = await _userManager.ChangeEmailAsync(user, "trinmse150418@fpt.edu.vn", codeChangeEmail);

            var codeChangeEmail2 = await _userManager.GenerateUserTokenAsync(user, _userManager.Options.Tokens.ChangeEmailTokenProvider, GetChangeEmailTokenPurpose("trinmse150418@fpt.edu.vn"));
            var resultcodeChangeEmail2 = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.ChangeEmailTokenProvider, GetChangeEmailTokenPurpose("trinmse150418@fpt.edu.vn"), codeChangeEmail2);

            #endregion

            #region codeChangePhoneNumber
            var codeChangePhoneNumber = await _userManager.GenerateChangePhoneNumberTokenAsync(user, "0123456789");
            var resultcodeChangePhoneNumber = await _userManager.ChangePhoneNumberAsync(user, "0123456789", codeChangePhoneNumber);

            var codeChangePhoneNumber2 = await _userManager.GenerateUserTokenAsync(user, _userManager.Options.Tokens.ChangePhoneNumberTokenProvider, ChangePhoneNumberTokenPurpose + ":" + "0123456789");
            var resultcodeChangePhoneNumber2 = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.ChangePhoneNumberTokenProvider, ChangePhoneNumberTokenPurpose + ":" + "0123456789", codeChangePhoneNumber2);
            var resultcodeChangePhoneNumber3 = await _userManager.VerifyChangePhoneNumberTokenAsync(user, codeChangePhoneNumber2, "0123456789").ConfigureAwait(false);

            #endregion

            #region GenerateUserTokenAsync - VerifyUserTokenAsync
            var code5 = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultEmailProvider, "Custom");
            var code2 = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "Custom");
            var code3 = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "Custom");
            var code4 = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, "Custom");

            var result1 = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultEmailProvider, "Custom", code5);

            #endregion
            #region GenerateTwoFactorTokenAsync - VerifyTwoFactorTokenAsync
            var code6 = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            var code7 = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
            var code8 = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultProvider);
            var code9 = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider);

            var result2 = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code6);

            #endregion

            #region Base64UrlEncode
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var DEcode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            #endregion
            return Ok(DEcode);

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Claims()
        {
            return Ok(User.Claims.Select(x => new { x.Type, x.Value }));
        }

        [HttpGet]
        public async Task<IActionResult> SecureDataFormatProtect([FromServices] IDataProtectionProvider dataProtectionProvider)
        {
            // "Test" is purpose , thì hàm CreateProtector của Unprotect cũng phải có purpose giống với Protect
            // nếu có áp dụng subpurpose thì Unprotect cũng phải có subpurpose giống với Protect
            var dataProtector = dataProtectionProvider.CreateProtector("Test");
            var secureDataFormat = new SecureDataFormat<string>(CustomDataSerializer<string>.Default, dataProtector);

            var token = secureDataFormat.Protect("123"); // nếu có áp dụng purpose thì Unprotect cũng phải có purpose giống với Protect

            //var timeLimitedProtector = dataProtector.ToTimeLimitedDataProtector();
            //var tokenTimeLimitedData = timeLimitedProtector.Protect("Test timed protector", lifetime: TimeSpan.FromSeconds(20));

            return Ok(new { token });
        }

        [HttpGet]
        public async Task<IActionResult> SecureDataFormatUnprotect(string token, [FromServices] IDataProtectionProvider dataProtectionProvider)
        {
            IDataProtector dataProtector = dataProtectionProvider.CreateProtector("Test");
            var secureDataFormat = new SecureDataFormat<string>(CustomDataSerializer<string>.Default, dataProtector);

            var data = secureDataFormat.Unprotect(token); // nếu có áp dụng purpose thì Unprotect cũng phải có purpose giống với Protect

            //var timeLimitedProtector = dataProtector.ToTimeLimitedDataProtector();
            //var dataTimeLimited = timeLimitedProtector.Unprotect(token);

            return Ok(new { data });
        }

        // login google client js
        [HttpPost]
        public async Task<IActionResult> Get(Token token)
        {
            string TokenInfoEndpoint = "https://oauth2.googleapis.com/tokeninfo";
            var client = new HttpClient();
            var response = await client.PostAsync($"{TokenInfoEndpoint}?id_token={token.token}", null);
            var responseContent = await response.Content.ReadFromJsonAsync<IDictionary<string, object>>();

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token.token);
            return Ok(new { token, payload, responseContent });
        }

        // login google client html
        [HttpPost]
        public async Task<IActionResult> Get22([FromForm] string credential)
        {
            string TokenInfoEndpoint = "https://oauth2.googleapis.com/tokeninfo";
            var client = new HttpClient();
            var response = await client.PostAsync($"{TokenInfoEndpoint}?id_token={credential}", null);
            var responseContent = await response.Content.ReadFromJsonAsync<IDictionary<string, object>>();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(credential);
            var tokenS = jsonToken as JwtSecurityToken;
            var jwtSecurityToken = handler.ReadJwtToken(credential);

            var claim = tokenS.Claims.Select(c => new { c.Issuer, c.OriginalIssuer, c.Type, c.Value });
            var claim2 = jwtSecurityToken.Claims.Select(c => new { c.Issuer, c.OriginalIssuer, c.Type, c.Value });

            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(credential);
            return Ok(new { credential, claim, claim2, payload, responseContent });
        }

        [HttpGet]
        public async Task<ActionResult> GithubLogin(string code)
        {
            var client = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
            { "client_id", "ae306bb89df81bf7f5a4" },
            { "client_secret", "8ad08f231fc290339878e25ba221fc1a358d739e"},
            { "code", code },
            { "redirect_uri", "https://localhost:7217/api/test/GithubLogin"}
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync("https://github.com/login/oauth/access_token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var values = HttpUtility.ParseQueryString(responseContent);
            var access_token = values["access_token"];

            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Anything");
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {access_token}");
            response = await client.GetAsync($"https://api.github.com/user");
            var htmlText = await response.Content.ReadAsStringAsync();

            response = await client.GetAsync($"https://api.github.com/user/emails");
            htmlText = await response.Content.ReadAsStringAsync();

            var client1 = new GitHubClient(new ProductHeaderValue("Code2night"));
            var tokenAuth = new Credentials(access_token);
            client1.Credentials = tokenAuth;
            var user = await client1.User.Current();
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> GoogleLogin(string code)
        {
            string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
            string TokenEndpoint = "https://oauth2.googleapis.com/token";
            string TokenInfoEndpoint = "https://oauth2.googleapis.com/tokeninfo";
            string UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";

            var client = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
            { "client_id", "748375698529-ul7shovile6hhu77ucj6snib9gqec3hc.apps.googleusercontent.com" },
            { "client_secret", "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"},
            { "code", code },
            { "redirect_uri", "https://localhost:7217/api/test/GoogleLogin"},
             { "grant_type", "authorization_code" },
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(TokenEndpoint, content);
            var responseContent = await response.Content.ReadFromJsonAsync<IDictionary<string, object>>();

            var access_token = responseContent["access_token"]?.ToString();
            var token_type = responseContent["token_type"]?.ToString();
            var expires_in = responseContent["expires_in"]?.ToString();
            var id_token = responseContent["id_token"]?.ToString();
            var scope = responseContent["scope"]?.ToString();

            response = await client.PostAsync($"{TokenInfoEndpoint}?id_token={id_token}", null);
            responseContent = await response.Content.ReadFromJsonAsync<IDictionary<string, object>>();

            //GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
            //{
            //    Audience = new[] { "748375698529-ul7shovile6hhu77ucj6snib9gqec3hc.apps.googleusercontent.com" }
            //};
            //GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(id_token, settings);
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(id_token);

            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Anything");
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {access_token}");
            response = await client.GetAsync(UserInformationEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Google user information ({response.StatusCode}). Please check if the authentication information is correct.");
            }
            var user = await response.Content.ReadFromJsonAsync<object>();

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(id_token);
            var tokenS = jsonToken as JwtSecurityToken;
            var jwtSecurityToken = handler.ReadJwtToken(id_token);

            return Ok(new { id_token, tokenS, jwtSecurityToken, user, responseContent, payload });
        }

        [HttpGet]
        public async Task<ActionResult> PinterestLogin()
        {
            var AuthorizationEndpoint = "https://www.pinterest.com/oauth/";
            var TokenEndpoint = "https://api.pinterest.com/v5/oauth/token";
            var UserInformationEndpoint = "https://api.pinterest.com/v5/user_account";

            var client = new HttpClient();

            string access_token = "pina_AMAZTPYWACO2QAQAGCAFMDRJLDW2HCQBQBIQDJJ3IKK6FKZC4YZRGUGA2KUYTO36VIJAGBW5KOMV3N3PEZJGVWK344PF7OYA";

            client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Anything");
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {access_token}");
            var response = await client.GetAsync(UserInformationEndpoint);
            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new HttpRequestException($"An error occurred when retrieving Google user information ({response.StatusCode}). Please check if the authentication information is correct.");
            //}
            var user = await response.Content.ReadFromJsonAsync<object>();

            return Ok(user);
        }

    }

    public class Token
    {
        public string? token { get; set; }
        public IDictionary<string, object>? data { get; set; }
    }
}
