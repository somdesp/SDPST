using Authenticate.Resource.Models;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.AuthRes;
using Services.Helpers;
using Services.IAuthRes;
using Services.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authenticate.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly FacebookAuthSettings _fbAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _userService;

        private static readonly HttpClient Client = new HttpClient();

        public AuthController(IAuthService authService, IOptions<FacebookAuthSettings> fbAuthSettingsAccessor, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _authService = authService;
            _fbAuthSettings = fbAuthSettingsAccessor.Value; 
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]User login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await _authService.GetClaimsIdentity(login);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, login.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        public async Task<IActionResult> Facebook([FromBody]FacebookAuthViewModel model)
        {
            // 1.generate an app access token
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            // 2. validate the user access token
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid facebook token.", ModelState));
            }

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var user = await _authService.LoginFacebook(userInfo.Email);

            if (user == null)
            {
                var appUser = new User
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    FacebookId = userInfo.Id,
                    Email = userInfo.Email,
                    Name = userInfo.Email,
                    PictureUrl = userInfo.Picture.Data.Url
                };

                var result = await _userService.CreateUserFacebookAsync(appUser);

                //if (!result) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            }

            // generate the jwt for the local user...
            var localUser = await _authService.LoginFacebook(userInfo.Email);

            if (localUser == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(_jwtFactory.GenerateClaimsIdentity(localUser.Name, localUser.Id.ToString()),
              _jwtFactory, localUser.Name, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

           return new OkObjectResult(jwt);
        }   



    }
}