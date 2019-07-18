using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System;
using System.Threading.Tasks;

namespace ApiSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]

    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("InserirUsuario")]
        [Authorize(Roles = "CRIAR_USUARIO")]
        public async Task<dynamic> InserirUsuario(User user)
        {
            try
            {
                return _userService.InserirUsuario(user);

                // return Created("/api/ManagerUsers/ObterUsuarioId", jusuario);
            }
            catch (Exception e)
            {
                return BadRequest("Error " + e.Message);
            }

        }
    }
}