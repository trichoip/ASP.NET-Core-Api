
using AspNetCore.Identity.Data;
using AspNetCore.Identity.Data.SeedData;
using AspNetCore.Identity.Email;
using AspNetCore.Identity.Extensions;
using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AspNetCore.Identity;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region AddDbContext Mysql
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors()
                              .UseLazyLoadingProxies()); // cấu hình lazy loading (Microsoft.EntityFrameworkCore.Proxies) 
        #endregion

        #region AddIdentity
        // Microsoft.AspNetCore.Identity.UI
        //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //     .AddRoles<ApplicationRole>()
        //     .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // nếu không cấu hình thì nó sẽ lấy mặc định như này

            // Default SignIn settings.
            options.SignIn.RequireConfirmedAccount = false; // này giống RequireConfirmedEmail
            options.SignIn.RequireConfirmedEmail = false; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

            //  Default Password settings.
            options.Password.RequireDigit = true; // Không bắt phải có số
            options.Password.RequireLowercase = true; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = true; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = true; // Không bắt buộc chữ in
            options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            // Default Lockout settings. - Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5;  // Thất bại 5 lần thì khóa
            options.Lockout.AllowedForNewUsers = true; // Xác định xem người dùng mới có thể bị khóa hay không.

            // Default User settings.. - Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;  // Email là duy nhất

            //options.Stores.ProtectPersonalData = true;

        }).AddEntityFrameworkStores<ApplicationDbContext>()
          //.AddDefaultUI()
          //.AddPersonalDataProtection<LookupProtector, KeyRing>()
          .AddDefaultTokenProviders();
        #endregion

        #region Token life time (token comfirm email,.. )
        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
       {
           // Token life time (token comfirm email,.. )
           // nếu token đã dùng thì nó expire luôn
           // _userManager.GeneratePasswordResetTokenAsync(user);
           // _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
           // _userManager.ConfirmEmailAsync(user, code);
           options.TokenLifespan = TimeSpan.FromDays(2);
       });
        #endregion

        #region config cookies of IdentityConstants.ApplicationScheme
        builder.Services.ConfigureApplicationCookie(options =>
       {
           // cấu hình chung cho cookies cho ApplicationScheme , thường thì ApplicationScheme sài khi login
           // nếu còn ExpireTimeSpan thì khi có request [Authorize] hoặc User thì nó sẽ tự động reset lại ExpireTimeSpan
           //  _signInManager.SignInAsync(user, isPersistent: false);
           // _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: true);
           // [Authorize]
           // User
           // Cookie life time
           options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
           options.SlidingExpiration = true;

           //options.Events = new CookieAuthenticationEvents
           //{
           //    OnRedirectToLogin = ctx =>
           //    {
           //        ctx.Response.StatusCode = 401;
           //        return Task.CompletedTask;
           //    },
           //    OnRedirectToAccessDenied = ctx =>
           //    {
           //        ctx.Response.StatusCode = 403;
           //        return Task.CompletedTask;
           //    }
           //};

       });
        #endregion

        #region config cookies of IdentityConstants.ExternalScheme
        builder.Services.ConfigureExternalCookie(options =>
        {
            // cấu hình chung cho cookies cho ExternalScheme , thường thì ExternalScheme sài khi login bằng google, facebook,..
            // Cookie life time of external login
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
        });
        #endregion

        #region PasswordHasherOptions
        builder.Services.Configure<PasswordHasherOptions>(option =>
        {
            option.IterationCount = 12000;
        });
        #endregion

        #region SecurityStampValidatorOptions
        // chỉ áp dụng cho cookies 
        // này là cứ 40 giây nó sẽ check lại security stamp của user, nếu security stamp trong cookies khác với trong database thì nó sẽ logout xóa cookies
        // thường thì khi hàm thay đổi thông tin user thì nó có hàm  thay đổi security stamp của user
        // như ResetPasswordAsync, CreateAsync, SetUserNameAsync,... nhưng hàm nào thay đổi thông tin thì đều có call hàm UpdateSecurityStampAsync, f12 là rõ
        // Force Identity's security stamp to be validated every minute.
        builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                           o.ValidationInterval = TimeSpan.FromSeconds(40));
        #endregion

        #region AddAuthentication
        // keyword: OAuth2 api, api profile 
        builder.Services.AddAuthentication(options =>
           {
               #region AddAuthentication

               // cấu hình Default... là chỉ áp dụng cái cuối cùng được cấu hình
               // nếu có nhiều cái cấu hình thì nó sẽ áp dụng cái cuối cùng được cấu hình
               // ví dụ như nếu DefaultAuthenticateScheme là "Abc"
               // mà ở dưới lại cấu hình DefaultAuthenticateScheme là "Bbbb" thì nó sẽ áp dụng DefaultAuthenticateScheme là "Bbbb"
               // ví dụ 2 là : ở trong AddIdentity có cấu hình DefaultAuthenticateScheme là IdentityConstants.ApplicationScheme;
               // và ở đây lại cấu hình DefaultAuthenticateScheme là JwtBearerDefaults.AuthenticationScheme; thì nó sẽ áp dụng DefaultAuthenticateScheme là JwtBearerDefaults.AuthenticationScheme;
               // và lưu ý là phải cấu hình ở dưới AddIdentity vì nếu cấu hình ở trên AddIdentity thì nó sẽ áp dụng DefaultAuthenticateScheme là IdentityConstants.ApplicationScheme; vì cái này là cái cuối cùng được cấu hình nên nó sẽ áp dụng cái này
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // nếu có cấu hình DefaultChallengeScheme và không cấu hình DefaultForbidScheme thì DefaultForbidScheme là DefaultChallengeScheme

               //options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme; // nếu cấu hình DefaultChallengeScheme thì DefaultForbidScheme là DefaultChallengeScheme
               //options.DefaultSignInScheme = IdentityConstants.ExternalScheme; // nếu có cấu hình DefaultSignInScheme và không cấu hình DefaultSignOutScheme thì DefaultSignOutScheme là DefaultSignInScheme
               //options.DefaultSignOutScheme = IdentityConstants.ExternalScheme; // nếu có cấu hình DefaultSignInScheme thì DefaultSignOutScheme là DefaultSignInScheme

               //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; cấu hình này là builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
               // lưu ý: nếu dùng DefaultScheme thì nó chỉ áp dụng cho các trường hợp mà không có cấu hình DefaultAuthenticateScheme, DefaultChallengeScheme, DefaultSignInScheme, DefaultSignOutScheme
               // ví dụ như cấu hình ở trên là DefaultAuthenticateScheme là "aaa" và ở dưới cấu hình DefaultScheme là "bbb" thì nó sẽ áp dụng DefaultAuthenticateScheme là "aaa" chứ không phải là "bbb" vì nó đã có cấu hình DefaultAuthenticateScheme rồi
               // và các Default.. còn lại như DefaultChallengeScheme,DefaultSignInScheme  nếu không có cấu hình thì nó sẽ áp dụng là "bbb"
               // nếu không có cấu hình DefaultAuthenticateScheme thì nó sẽ áp dụng là "bbb"

               // tất cả các AuthenticateScheme như jwt, Cookies đều kế thừa AuthenticationHandler và trong class AuthenticationHandler có 1 method là HandleAuthenticateAsync giúp xác thực 
               // nếu là jwt HandleAuthenticateAsync sẽ đọc token trên header và xác thực
               // nếu là cookies HandleAuthenticateAsync sẽ đọc cookies và xác thực

               // DefaultAuthenticateScheme là nó áp dụng cho những trường hợp:
               // 1: method không có [Authorize] là có nghĩa là không có [Authorize] thì nó vẫn xác thực ,nếu thất bại thì vẫn vào được method và nó xác thực theo DefaultAuthenticateScheme
               // 2: method có [Authorize] mà không có cấu hình AuthenticationSchemes trong [Authorize] thì nó áp dụng Authenticate là DefaultAuthenticateScheme 
               // 3: là dùng HttpContext.AuthenticateAsync(); thì nó sẽ áp dụng Authenticate là DefaultAuthenticateScheme 
               //  nếu DefaultAuthenticateScheme là jwt thì nó sẽ xác thực theo jwt, nếu DefaultAuthenticateScheme là cookies thì nó sẽ xác thực theo cookies

               // DefaultChallengeScheme là nó áp dụng cho những trường hợp:
               // 1: method có [Authorize] mà không có cấu hình AuthenticationSchemes trong [Authorize] thì nó áp dụng Challenge là DefaultChallengeScheme
               // lưu ý thêm là nếu không có cấu hình DefaultForbidScheme thì DefaultForbidScheme mặc định là DefaultChallengeScheme
               // nếu method chỉ cấu hình [Authorize] thì:
               // nếu xác thực không thành công thì không vào được hàm mà nó qua DefaultChallengeScheme , còn nếu lỗi phân quyên thì nó nhảy vào DefaultForbidScheme 

               // [Authorize] chỉ giúp xác thực thành công thì nó cho phép vào method còn nếu xác thực không thành công thì nó không cho vào method mà nó nhảy vào ChallengeScheme và Forbid 

               // nếu method có cấu hình như này -> [Authorize(AuthenticationSchemes = "AAA")]
               // thì nó ấp dụng xác thực Schemes là "AAA" chứ không phải là DefaultAuthenticateScheme và nếu lỗi xác thực hoặc lỗi phân quyên thì nó nhảy vào ChallengeScheme và Forbid của AAA

               // nếu method có cấu hình nhiều AuthenticationSchemes như này -> [Authorize(AuthenticationSchemes = "AAA,BBB,CCC")]
               // thì nếu 1 trong các scheme mà xác thực thành công thì nó vào được method và nó lỗi chỉ khi tất cả các scheme đều lỗi, và nếu lỗi thì nó nhảy vào ChallengeScheme và Forbid của schema cuối cùng

               // nếu method có nhiều AuthenticationSchemes như này -> [Authorize(AuthenticationSchemes = "AAA,BBB,CCC")]
               // mà xác thực thành công từ 2 trở đi thì nó cộng dòn các claims của các scheme đó lại vào User.claims
               // nếu muốn xem claims của từng scheme thì dùng User.identity.claims

               // ta có thể gọi xác thực bằng tay hay lấy Savetoken bằng cách -> await HttpContext.AuthenticateAsync(); -> cái này đã nói ở trên DefaultAuthenticateScheme
               // nếu nếu muốn xác thực schema khác thì truyền name schema vào -> await HttpContext.AuthenticateAsync("Identity.External");
               // có thể lấy Savetoken bằng cách sau khi AuthenticateAsync thì lấy ra từ properties của kết quả của AuthenticateAsync
               // method không có [Authorize] thì nó xác thực giống như HttpContext.AuthenticateAsync(); -> cái này đã nói ở trên DefaultAuthenticateScheme
               // nếu method không có [Authorize] mà nếu truyền vào method là Bearer và Cookies "Identity.External" thì nó check Authenticate là DefaultAuthenticateScheme
               // nhưng ta có thể check Authenticate của Bearer và "Identity.External" bằng cách dùng await HttpContext.AuthenticateAsync("Bearer"); và await HttpContext.AuthenticateAsync("Identity.External");

               // còn cách lấy SaveToken khác là -> await HttpContext.GetTokenAsync("access_token"); cái này nó chỉ tìm trong schema là DefaultAuthenticateScheme
               // còn nếu muốn tìm trong schema khác thì truyền name schema vào -> await HttpContext.GetTokenAsync("Identity.External", "access_token"); // cách này giống await HttpContext.AuthenticateAsync("Identity.External"); -> f12 để xem rõ

               // ví dụ lấy SaveToken của Bearer và "Identity.External" thì dùng await HttpContext.GetTokenAsync("Bearer", "access_token"); và await HttpContext.GetTokenAsync("Identity.External", "access_token");
               // hoặc var auth = await HttpContext.AuthenticateAsync("Bearer");
               // var token = auth.Properties.GetTokenValue("access_token"); 
               // var token = auth.Properties.Items; // lấy tất cả các token

               #endregion
           })
           .AddGoogle(googleOptions =>
           {
               #region AddGoogle

               // dashboard: https://console.cloud.google.com/apis/credentials
               // callback: https://localhost:7217/signin-google

               googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
               googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;

               // nếu không dùng ClaimActions.MapAll(); thì nó tự động map vào ClaimTypes.NameIdentifier và những thông tin khác cho nên không bị lỗi null khi call _signInManager.GetExternalLoginInfoAsync();
               // lưu ý nếu dùng ClaimActions.MapAll(); thì nó vẫn lấy được thông tin nhưng nó không map vào Claimtypes bất kỳ thông tin nào
               // mà không map vào Claimtypes thì nó không map vào ClaimTypes.NameIdentifier
               // nếu không có ClaimTypes.NameIdentifier thì khi call _signInManager.GetExternalLoginInfoAsync(); sẽ bị lỗi null
               // _signInManager.GetExternalLoginInfoAsync(); phải có ClaimTypes.NameIdentifier mới lấy được thông tin
               // cho nên nếu dùng ClaimActions.MapAll(); thì phải có MapJsonKey(ClaimTypes.NameIdentifier, "id", "string"); thì nó mới map vào ClaimTypes.NameIdentifier và không bị lỗi null khi call _signInManager.GetExternalLoginInfoAsync();
               // dùng ClaimActions.MapAll(); cũng không map email vào claim cho nên muốn map email vào claim thì phải MapJsonKey(ClaimTypes.Email, "email", "string");
               // có thể f12  _signInManager.GetExternalLoginInfoAsync(); để xem chi tiết hơn
               // MapAll() phải để trước các MapJsonKey vì MapAll() nó sẽ clear hết các claim rùi mới add lại claims
               // cho nên nếu để sau thì các MapJsonKey thì nó sẽ clear hết các claim rùi chỉ add lại các claim 
               //googleOptions.ClaimActions.MapAll(); // Map tất cả các thông tin từ google về, bật để xem các thông tin có 
               //googleOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id", "string");
               //googleOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", "string");

               //googleOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name", "string");
               googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
               googleOptions.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
               //googleOptions.AuthorizationEndpoint += "?prompt=consent"; // hack để lấy refresh token

               //googleOptions.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");

               // Cấu hình Url callback lại từ Google (không thiết lập thì mặc định là /signin-google)
               //googleOptions.CallbackPath = "/dang-nhap-tu-google";

               // lưu lại token trong properties.Items của AuthenticationProperties và nó lưu properties trong cookies 
               // khi ta lấy cookies thì nó sẽ lấy ra properties và trong đó có token trong properties.Items
               // để lấy thì dùng:
               // var auth = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
               // auth.Properties
               // cách khác là string accessToken = await HttpContext.GetTokenAsync("access_token");
               // nếu không truyền schema thì nó sẽ lấy schema mặc định là options.DefaultAuthenticateScheme
               // cho nên là phải truyền schema vào như này -> string accessToken = await HttpContext.GetTokenAsync(IdentityConstants.ExternalScheme, "access_token");
               // khi ta HttpContext.AuthenticateAsync() thì nó sẽ lấy ra cookies và lấy ra properties trong cookies
               // cơ chế lưu lại token trong properties và lấy ra là:
               // khi ta login bằng google thì nó sẽ nhảy vào hàm HandleRemoteAuthenticateAsync() trong class OAuthHandler kế thừa RemoteAuthenticationHandler
               // thì nếu ta có bật SaveTokens = true thì token được lưu lại trong properties.Items (có thể vào hàm HandleRemoteAuthenticateAsync() xem phần if (Options.SaveTokens) )
               // sau đó thì nó nhảy qua hàm HandleRequestAsync() trong class RemoteAuthenticationHandler
               // và trong hàm này nó sẽ call hàm HandleRemoteAuthenticateAsync() của class OAuthHandler như bên dưới
               // hàm HandleRequestAsync(): var authResult = await HandleRemoteAuthenticateAsync(); nếu SaveTokens = true thì authResult sẽ có chứa token trong properties.Items
               // sau đó thì truyền properties vào hàm await Context.SignInAsync(SignInScheme, Principal!, Properties);
               // SignInScheme là nó lấy tên của options.DefaultSignInScheme như mặc định của ientity là IdentityConstants.ExternalScheme;
               // Properties là nó lấy từ authResult.properties khi call HandleRemoteAuthenticateAsync()
               // như trên đẫ nói nếu ta có bật SaveTokens = true thì khi call HandleRemoteAuthenticateAsync(), thì khi return nó có token được lưu lại trong properties.Items 
               // và nó thực hiện hàm await Context.SignInAsync(SignInScheme, Principal!, Properties); và tới đây là kết thúc phần OAuthHandler và nó chuyển tiép qua CookieAuthenticationHandler để thực hiện hàm Context.SignInAsync()
               // khi thực hiện hàm await Context.SignInAsync(SignInScheme, Principal!, Properties); nó sẽ truyền Properties vào hàm HandleSignInAsync() trong class CookieAuthenticationHandler
               // và hàm HandleSignInAsync() có nhiệm vụ là lưu cookies lên trình duyệt và nó lấy tên cookies là SignInScheme (SignInScheme là nó lấy tên của options.DefaultSignInScheme như mặc định của identity là IdentityConstants.ExternalScheme;)
               // khi nó lưu cookies thì nó lưu luôn properties vào cookies, properties đó là Properties khi mình truyền từ hàm await Context.SignInAsync(SignInScheme, Principal!, Properties); được call trong hàm HandleRequestAsync()
               // để lấy cookies thì ta dùng HttpContext.AuthenticateAsync(SignInScheme); và nó sẽ lấy ra cookies 
               // khi ta lấy cookies thì nó sẽ lấy ra properties trong cookies và trong đó có token trong properties.Items
               // và sau đó ta chỉ cần call auth.Properties là ta có thể lấy được token trong properties.Items
               googleOptions.SaveTokens = true;

               #endregion
           })
           .AddFacebook(facebookOptions =>
           {
               #region AddFacebook
               // hiện chỉ có account trong project dashboard mới đăng nhập được, account khác không đăng nhập được
               // để account khác đăng nhập được thì phải thêm người dùng thử nghiệm vào dashboard facebook
               // hoặc tạo người dùng thử nghiệm trong dashboard facebook

               // dashboard: https://developers.facebook.com/apps/
               // callback: https://localhost:7217/signin-facebook
               // JsonKey: https://developers.facebook.com/docs/graph-api/reference/user

               facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
               facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;

               // lưu ý: Facebook ClaimActions.MapAll(); giống y hệt của google, qua xem google để hiểu thêm
               // để lấy thông tin email thì phải bật scope email trong dashboard facebook, sau khi bật thì nó tự động truyền email về
               //facebookOptions.ClaimActions.MapAll(); // Map tất cả các thông tin từ facebook về, bật để xem các thông tin có
               //facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id", "string");
               //facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", "string");
               //facebookOptions.ClaimActions.MapJsonKey("sinhnhatne", "birthday", "string");

               // phải add Fields vào thì thông tin mới gữi về được
               // nếu không add Fields vào thì sẽ không có thông tin gữi về và MapJsonKey không được 
               // ví dụ birthday phải add Fields birthday vào thì MapJsonKey("sinhnhatne", "birthday", "string"); mới map được
               // sau khi add Fields vào thì phải bật scope liên quan đến Fields đó trong dashboard facebook thì nó mới gữi về được còn không thì không gữi về được
               // ví dụ birthday phải bật scope user_birthday trong dashboard facebook và Fields.Add("birthday");  thì nó mới gữi về được còn không thì không gữi về được
               //facebookOptions.Fields.Add("picture");
               //facebookOptions.Fields.Add("age_range");
               //facebookOptions.Fields.Add("birthday");
               //facebookOptions.Fields.Add("favorite_athletes");
               //facebookOptions.Fields.Add("favorite_teams");
               //facebookOptions.Fields.Add("gender");
               //facebookOptions.Fields.Add("hometown");
               //facebookOptions.Fields.Add("id_for_avatars");
               //facebookOptions.Fields.Add("inspirational_people");
               //facebookOptions.Fields.Add("link");
               //facebookOptions.Fields.Add("location");
               //facebookOptions.Fields.Add("meeting_for");
               //facebookOptions.Fields.Add("name_format");
               //facebookOptions.Fields.Add("quotes");
               //facebookOptions.Fields.Add("sports");
               //facebookOptions.Fields.Add("video_upload_limits");
               //facebookOptions.Fields.Add("likes");

               // lưu ý: mặc định scope của facebook là public_profile mà khi generate link thì là scope=email nên khi login sẽ bị lỗi
               // cách fix 1 là vô https://developers.facebook.com/apps/ bật scope email lên 
               // cách fix 2 là remove scope email như bên dưới
               //facebookOptions.Scope.Remove("email");

               // lưu ý: nếu muốn lấy thông tin email thì phải bật scope email trong https://developers.facebook.com/apps/ lên
               // nếu ađd scope nào như bên dưới thì phải bật scope đó lên trong https://developers.facebook.com/apps/
               // nếu không bật scope đó lên thì khi login sẽ bị lỗi scope
               // fix thì bật scope đó lên trong https://developers.facebook.com/apps/ hoặc remove scope đó đi
               //facebookOptions.Scope.Add("public_profile");
               //facebookOptions.Scope.Add("user_age_range");
               //facebookOptions.Scope.Add("user_birthday");
               //facebookOptions.Scope.Add("user_friends");
               //facebookOptions.Scope.Add("user_gender");
               //facebookOptions.Scope.Add("user_hometown");
               //facebookOptions.Scope.Add("user_likes");
               //facebookOptions.Scope.Add("user_link");
               //facebookOptions.Scope.Add("user_location");
               //facebookOptions.Scope.Add("user_photos");
               //facebookOptions.Scope.Add("user_posts");
               //facebookOptions.Scope.Add("user_videos");

               //facebookOptions.AccessDeniedPath = "/AccessDeniedPathInfo";

               // Cấu hình Url callback lại từ Facebook (không thiết lập thì mặc định là /signin-facebook)
               //facebookOptions.CallbackPath = "/dang-nhap-tu-facebook";

               #endregion
           })
           .AddMicrosoftAccount(microsoftOptions =>
           {
               #region AddMicrosoftAccount

               // dashboard: https://go.microsoft.com/fwlink/?linkid=2083908
               // callback: https://localhost:7217/signin-microsoft

               microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
               microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;

               // lưu ý: Microsoft ClaimActions.MapAll(); giống y hệt của google, qua xem google để hiểu thêm
               // lưu ý: field email của microsoft là 'mail' khác với google, twitter là email
               //microsoftOptions.ClaimActions.MapAll(); // Map tất cả các thông tin từ microsoft về, bật để xem các thông tin có
               //microsoftOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id", "string");
               //microsoftOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "mail", "string");

               // Cấu hình Url callback lại từ Microsoft (không thiết lập thì mặc định là /signin-microsoft)
               //microsoftOptions.CallbackPath = "/dang-nhap-tu-microsoft";

               #endregion
           })
           .AddTwitter(twitterOptions =>
           {
               #region AddTwitter

               // dashboard: https://developer.twitter.com/en/portal/dashboard
               // callback: https://localhost:7217/signin-twitter
               // jsonkey: https://developer.twitter.com/en/docs/twitter-api/v1/data-dictionary/object-model/user

               twitterOptions.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerAPIKey"]!;
               twitterOptions.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"]!;

               // RetrieveUserDetails false là chỉ lấy được id, username và không MapJsonKey các thông tin khác được
               // RetrieveUserDetails true là lấy được id, username và email và MapJsonKey các thông tin khác được
               // twitter cần RetrieveUserDetails true mới lấy được email hoặc các thông tin khác
               twitterOptions.RetrieveUserDetails = true;

               // có thể f12  _signInManager.GetExternalLoginInfoAsync(); để xem chi tiết hơn
               // nếu không bật MapAll() thì nó map vào ClaimTypes.NameIdentifier - id,  ClaimTypes.Name - username, ClaimTypes.Email - email
               // nếu bật MapAll() thì nó chỉ map ClaimTypes.NameIdentifier - id,  ClaimTypes.Name - username , chứ không map ClaimTypes.Email - email
               // cho nên nếu bật MapAll() và muốn map email vào claim thì phải MapJsonKey(ClaimTypes.Email, "email", "string");
               // nếu bật MapAll() thì nó vần map được ClaimTypes.NameIdentifier - id, không như google cho nên không bị lỗi _signInManager.GetExternalLoginInfoAsync(); == null
               //twitterOptions.ClaimActions.MapAll(); // Map tất cả các thông tin từ twitter về, bật để xem các thông tin có 
               //twitterOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", "string");

               //twitterOptions.ClaimActions.MapJsonKey("urn:twitter:name", "name", "string");
               //twitterOptions.ClaimActions.MapJsonKey("urn:twitter:screen_name", "screen_name", "string");
               //twitterOptions.ClaimActions.MapJsonKey("urn:twitter:created_at", "created_at", "string");
               //twitterOptions.ClaimActions.MapJsonKey("urn:twitter:profilepicture", "profile_image_url_https", ClaimTypes.Uri);

               //twitterOptions.CallbackPath = "/dang-nhap-tu-twitter";

               #endregion
           })
           .AddOAuth("Github", "Github", githubOptions =>
           {
               #region AddGithub

               githubOptions.ClientId = builder.Configuration["Authentication:Github:ClientId"]!;
               githubOptions.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"]!;

               githubOptions.CallbackPath = "/signin-github";

               githubOptions.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
               githubOptions.TokenEndpoint = "https://github.com/login/oauth/access_token";
               githubOptions.UserInformationEndpoint = "https://api.github.com/user";
               //githubOptions.UserInformationEndpoint = "https://api.github.com/user/emails";

               githubOptions.Scope.Add("user:email");
               githubOptions.Scope.Add("read:user");

               // lưu ý : github phải bật pulic email thì mới lấy được email
               // cách khác là có thể lấy bằng cách call https://api.github.com/user/emails để lấy email
               // nhưng mà chỉ có thể lấy được email thôi chứ không lấy được các thông tin khác nhứ id,...
               // nếu có 2 claims trùng tên thì nó lấy luôn cả 2
               // ví dụ như call https://api.github.com/user/emails thì nó trả về 2 email là email cá nhân và email private github map vào ClaimTypes.Email
               // nếu call https://api.github.com/user/emails thì phải có scope "user:email" mới lấy được email
               githubOptions.ClaimActions.MapAll();
               githubOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
               githubOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
               githubOptions.ClaimActions.MapJsonKey("urn:github:name", "name");
               githubOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
               githubOptions.ClaimActions.MapJsonKey("urn:github:url", "url");

               githubOptions.SaveTokens = true;

               githubOptions.Events = new OAuthEvents
               {
                   OnCreatingTicket = async context =>
                   {
                       #region cach 1
                       //var clien1 = new GitHubClient(new Octokit.ProductHeaderValue("login-github"));
                       //var tokenAuth = new Credentials(context.AccessToken);
                       //clien1.Credentials = tokenAuth;
                       //var user1 = await clien1.User.Current();
                       #endregion

                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

                       if (!response.IsSuccessStatusCode)
                       {
                           throw new HttpRequestException($"An error occurred when retrieving Yahoo user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Yahoo Account API is enabled.");
                       }

                       #region Cach 3 
                       // - context.RunClaimActions(payload.RootElement) sẽ map tất cả các thông tin từ github về nhưng nó không add vào claims
                       // để add vào claims thì dùng ClaimActions.MapJsonKey
                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           if (payload.RootElement.ValueKind == JsonValueKind.Array)
                           {
                               payload.RootElement.EnumerateArray().ToList().ForEach(x => context.RunClaimActions(x));
                           }
                           else
                           {
                               context.RunClaimActions(payload.RootElement);
                           }

                       }
                       #endregion

                       #region Cach 2
                       //var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                       //var identifier = user.Value<string>("id");
                       //if (!string.IsNullOrEmpty(identifier))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim(ClaimTypes.NameIdentifier, identifier,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var userName = user.Value<string>("login");
                       //if (!string.IsNullOrEmpty(userName))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim(ClaimTypes.Name, userName,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var email = user.Value<string>("email");
                       //if (!string.IsNullOrEmpty(email))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim(ClaimTypes.Email, email,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var name = user.Value<string>("name");
                       //if (!string.IsNullOrEmpty(name))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim(ClaimTypes.GivenName, name,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var avatar_url = user.Value<string>("avatar_url");
                       //if (!string.IsNullOrEmpty(avatar_url))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim("urn:github:avatar_url", avatar_url,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var url = user.Value<string>("url");
                       //if (!string.IsNullOrEmpty(url))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim("urn:github:url", url,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //}

                       //var html_url = user.Value<string>("html_url");
                       //if (!string.IsNullOrEmpty(html_url))
                       //{
                       //    context.Identity.AddClaim(
                       //        new Claim("urn:github:html_url", html_url,
                       //        ClaimValueTypes.String, context.Options.ClaimsIssuer));
                       //} 
                       #endregion

                   }
               };

               #endregion
           })
           .AddOAuth("Yahoo", "Yahoo", yahooOptions =>
           {
               #region Yahoo

               // https://developer.yahoo.com/oauth2/guide/
               // https://developer.yahoo.com/apps/

               yahooOptions.ClientId = builder.Configuration["Authentication:Yahoo:ClientId"]!;
               yahooOptions.ClientSecret = builder.Configuration["Authentication:Yahoo:ClientSecret"]!;

               yahooOptions.AuthorizationEndpoint = "https://api.login.yahoo.com/oauth2/request_auth";
               yahooOptions.TokenEndpoint = "https://api.login.yahoo.com/oauth2/get_token";
               yahooOptions.UserInformationEndpoint = "https://api.login.yahoo.com/openid/v1/userinfo";

               yahooOptions.CallbackPath = "/signin-yahoo";

               // MapAll() phải để trước các MapJsonKey vì MapAll() nó sẽ clear hết các claim rùi mới add lại claims
               // cho nên nếu để sau thì các MapJsonKey thì nó sẽ clear hết các claim rùi chỉ add lại các claim 
               //yahooOptions.ClaimActions.MapAll();
               yahooOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
               yahooOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
               yahooOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
               yahooOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
               yahooOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
               yahooOptions.ClaimActions.MapJsonKey("urn:yahoo:profile", "profile");
               yahooOptions.ClaimActions.MapJsonKey("urn:yahoo:name", "name");
               yahooOptions.ClaimActions.MapJsonKey("urn:yahoo:profile:profileUrl", "profile");
               yahooOptions.ClaimActions.MapJsonKey("urn:yahoo:profile:locale", "locale");
               yahooOptions.ClaimActions.MapJsonKey("urn:yahoo:profile:picture", "picture");

               yahooOptions.Events = new OAuthEvents()
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

                       if (!response.IsSuccessStatusCode)
                       {
                           throw new HttpRequestException($"An error occurred when retrieving Yahoo user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Yahoo Account API is enabled.");
                       }

                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion
           })
           .AddOAuth("Reddit", "Reddit", redditOptions =>
           {
               #region Reddit - loi access_token 401

               // https://www.reddit.com/dev/api/oauth/
               // https://www.reddit.com/prefs/apps/
               // https://github.com/reddit-archive/reddit/wiki/OAuth2

               redditOptions.ClientId = builder.Configuration["Authentication:Reddit:ClientId"]!;
               redditOptions.ClientSecret = builder.Configuration["Authentication:Reddit:ClientSecret"]!;

               redditOptions.AuthorizationEndpoint = "https://www.reddit.com/api/v1/authorize";
               redditOptions.TokenEndpoint = "https://www.reddit.com/api/v1/access_token";
               redditOptions.UserInformationEndpoint = "https://oauth.reddit.com/api/v1/me";

               redditOptions.CallbackPath = "/signin-reddit";

               redditOptions.Scope.Add("identity");
               redditOptions.Scope.Add("read");

               redditOptions.ClaimActions.MapAll();

               redditOptions.Events = new OAuthEvents()
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();

                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion
           })
           .AddOAuth("Linkedin", "Linkedin", linkedinOptions =>
           {
               #region Linkedin

               // https://developer.linkedin.com/
               // https://learn.microsoft.com/en-us/linkedin/consumer/
               // https://learn.microsoft.com/en-us/linkedin/?context=linkedin%2Fcontext&view=li-lms-2022-08

               linkedinOptions.ClientId = builder.Configuration["Authentication:Linkedin:ClientId"]!;
               linkedinOptions.ClientSecret = builder.Configuration["Authentication:Linkedin:ClientSecret"]!;

               linkedinOptions.CallbackPath = "/signin-linkedin";

               linkedinOptions.AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization";
               linkedinOptions.TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken";
               linkedinOptions.UserInformationEndpoint = "https://api.linkedin.com/v2/userinfo";

               linkedinOptions.Scope.Add("openid");
               linkedinOptions.Scope.Add("profile");
               linkedinOptions.Scope.Add("email");

               linkedinOptions.ClaimActions.MapAll();
               linkedinOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
               linkedinOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
               linkedinOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
               linkedinOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
               linkedinOptions.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");

               linkedinOptions.Events = new OAuthEvents()
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();

                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion

           })
           .AddOAuth("Pinterest", "Pinterest", pinterestOptions =>
           {
               #region Pinterest - chua test

               // https://developers.pinterest.com/docs/getting-started/scopes/#OpenAPI%20security%20definitions
               // https://developers.pinterest.com/
               // https://developers.pinterest.com/docs/api/v5/#operation/user_account/get
               // https://developers.pinterest.com/docs/api/v5/

               pinterestOptions.ClientId = builder.Configuration["Authentication:Pinterest:ClientId"]!;
               pinterestOptions.ClientSecret = builder.Configuration["Authentication:Pinterest:ClientSecret"]!;

               pinterestOptions.CallbackPath = "/signin-pinterest";

               pinterestOptions.AuthorizationEndpoint = "https://www.pinterest.com/oauth/";
               pinterestOptions.TokenEndpoint = "https://api.pinterest.com/v5/oauth/token";
               pinterestOptions.UserInformationEndpoint = "https://api.pinterest.com/v5/user_account";

               pinterestOptions.Scope.Add("user_accounts:read");

               pinterestOptions.ClaimActions.MapAll();
               pinterestOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
               pinterestOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
               pinterestOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);

               pinterestOptions.Events = new OAuthEvents()
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();

                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion

           })
           .AddOAuth("Bitbucket", "Bitbucket", bitbucketOptions =>
           {
               #region Bitbucket

               // https://developer.atlassian.com/
               // https://developer.atlassian.com/server/bitbucket/rest/v811/intro/#about
               // https://developer.atlassian.com/cloud/bitbucket/oauth-2/
               // https://developer.atlassian.com/server/bitbucket/rest/v811/intro/#about
               // https://developer.atlassian.com/cloud/bitbucket/rest/api-group-users/#api-users-selected-user-get

               bitbucketOptions.ClientId = builder.Configuration["Authentication:Bitbucket:ClientId"]!;
               bitbucketOptions.ClientSecret = builder.Configuration["Authentication:Bitbucket:ClientSecret"]!;

               bitbucketOptions.CallbackPath = "/signin-bitbucket";

               bitbucketOptions.AuthorizationEndpoint = "https://bitbucket.org/site/oauth2/authorize";
               bitbucketOptions.TokenEndpoint = "https://bitbucket.org/site/oauth2/access_token";
               bitbucketOptions.UserInformationEndpoint = "https://api.bitbucket.org/2.0/user";

               bitbucketOptions.Scope.Add("account");
               bitbucketOptions.Scope.Add("email");

               bitbucketOptions.ClaimActions.MapAll();
               bitbucketOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
               bitbucketOptions.ClaimActions.MapJsonKey(ClaimTypes.Sid, "uuid", ClaimValueTypes.Sid);
               bitbucketOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
               bitbucketOptions.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "display_name");
               bitbucketOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);

               bitbucketOptions.Events = new OAuthEvents
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();
                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }

                       request = new HttpRequestMessage(HttpMethod.Get, "https://api.bitbucket.org/2.0/user/emails");
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();
                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion

           })
           .AddOAuth("Apple-Custom", "Apple-Custom", appleOptions =>
           {
               #region Apple - chua test

               appleOptions.ClientId = builder.Configuration["Authentication:Apple:ClientId"]!;
               appleOptions.ClientSecret = builder.Configuration["Authentication:Apple:ClientSecret"]!;

               //appleOptions.CallbackPath = "/signin-apple";
               appleOptions.CallbackPath = "/signin-apple-apple-custom";

               appleOptions.AuthorizationEndpoint = "https://appleid.apple.com/auth/authorize";
               appleOptions.TokenEndpoint = "https://appleid.apple.com/auth/token";
               appleOptions.UserInformationEndpoint = "https://appleid.apple.com/auth/userinfo";
               // https://appleid.apple.com/.well-known/openid-configuration

               appleOptions.Scope.Add("openid");
               appleOptions.Scope.Add("name");
               appleOptions.Scope.Add("email");

               appleOptions.ClaimActions.MapAll();

               appleOptions.Events = new OAuthEvents
               {
                   OnCreatingTicket = async context =>
                   {
                       var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                       request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                       var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                       response.EnsureSuccessStatusCode();

                       using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                       {
                           context.RunClaimActions(payload.RootElement);
                       }
                   }
               };

               #endregion
           })
           .AddApple(appleOptions =>
           {
               #region Apple - chua test

               // https://developer.apple.com/account
               // https://appleid.apple.com/.well-known/openid-configuration
               // https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/blob/dev/docs/sign-in-with-apple.md
               // https://developer.apple.com/sign-in-with-apple/
               // https://developer.apple.com/documentation/sign_in_with_apple/request_an_authorization_to_the_sign_in_with_apple_server
               // https://developer.apple.com/design/human-interface-guidelines/sign-in-with-apple#sign-in-with-apple-buttons

               appleOptions.ClientId = builder.Configuration["Authentication:Apple:ClientId"]!;
               appleOptions.ClientSecret = builder.Configuration["Authentication:Apple:ClientSecret"]!;
               appleOptions.TeamId = builder.Configuration["Authentication:Apple:TeamId"]!;

               //appleOptions.CallbackPath = "/signin-apple";

               #endregion
           })
           .AddJwtBearer(options =>
           {
               #region AddJwtBearer

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration.GetSection("Authentication:Schemes:Bearer:SerectKey").Value!)),
                   ValidateIssuerSigningKey = true,

                   ClockSkew = TimeSpan.Zero,
                   ValidateLifetime = false,

                   ValidateIssuer = false,
                   ValidateAudience = false,

                   //ValidateIssuer = true,
                   //ValidIssuer = "",
                   //ValidateAudience = true,
                   //ValidAudience = "",

               };

               // cách lấy SaveToken
               // var auth = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
               // auth.Properties
               // cách khác là string accessToken = await HttpContext.GetTokenAsync("access_token");
               // nếu không truyền schema thì nó sẽ lấy schema mặc định là options.DefaultAuthenticateScheme
               // cho nên là phải truyền schema vào như này -> string accessToken = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
               options.SaveToken = true;

               options.RequireHttpsMetadata = false;
               options.HandleEvents();

               #endregion
           });
        #endregion

        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();
        builder.Services.AddDataProtection()
                  .PersistKeysToFileSystem(new DirectoryInfo(@"bin\debug\configuration"))
                  .ProtectKeysWithDpapi()
                  .SetDefaultKeyLifetime(TimeSpan.FromDays(100));// lifetime must be at least one week

        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            #region AddSwaggerGen

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.OperationFilter<SecurityRequirementsOperationFilter>(JwtBearerDefaults.AuthenticationScheme);

            #endregion
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnablePersistAuthorization();
                c.EnableDeepLinking();
            });
            #endregion
        }
        await app.UseInitialiseDatabaseAsync();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
