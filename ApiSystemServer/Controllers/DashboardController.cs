using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace ApiSystemServer.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]/[action]")]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;

        public DashboardController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Home(int id)
        {

            var user = await _userService.GetUser(Convert.ToInt32(id));

            return new OkObjectResult(new
            {
                Message = "This is secure API and user data!",
                user.FirstName,
                user.LastName,
                user.PictureUrl,
                user.FacebookId,
                user.Name,
                user.Email,
                user.DateRegister
            });
        }
    }
}