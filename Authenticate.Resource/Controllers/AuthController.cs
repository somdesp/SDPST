using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Auth.Service.Interface;
using Services.Auth.Service.Service;
using Services.Helpers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Authenticate.Resource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        private static readonly HttpClient Client = new HttpClient();

        public AuthController(IAuthService authService, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _authService = authService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

        }


        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await _authService.GetClaimsIdentity(login);
            if (identity == null)
            {
                //HttpContext.Response.ContentType = "application/json";
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            //HttpContext.Response.ContentType = "application/json";
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, login.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return Ok(jwt);
        }
    }
}