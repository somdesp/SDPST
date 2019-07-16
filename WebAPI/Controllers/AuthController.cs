using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]User login)
        {
            var token = await _authService.Authenticate(login);


            if (token == null)
                return BadRequest(new { message = "Usuario ou senha incorreto" });

            return Ok(token);
        }
    }
}