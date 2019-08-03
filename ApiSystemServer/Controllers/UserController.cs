using ApiSystemServer.Model;
using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Validations;
using System;
using System.Threading.Tasks;

namespace ApiSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize(Policy = "ApiUser")]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse("error", ModelState));
            }

            try
            {
                bool retun = await _userService.CreateUserAsync(user, new ModelStateWrapper(ModelState));

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse("error", ModelState));
                }

                return Ok(new ApiResponse("OK", "Usuário cadastrado com sucesso"));


            }
            catch (Exception e)
            {
                HttpContext.Response.ContentType = "application/json";
                return StatusCode(500, "Erro interno do servidor" + e.Message);
            }

        }
    }
}