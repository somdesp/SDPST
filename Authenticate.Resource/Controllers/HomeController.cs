using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authenticate.Resource.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return Content("Running...");
        }

        [Route("api/home/isAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            return new ObjectResult(User.Identity.IsAuthenticated);
        }

        [Route("api/home/fail")]
        public IActionResult Fail()
        {
            return Unauthorized();
        }

        [Route("api/home/name")]
        [Authorize]
        public IActionResult Name()
        {
            var claimsPrincial = (ClaimsPrincipal)User;
            var givenName = claimsPrincial.FindFirst(ClaimTypes.GivenName).Value;
            return Ok(givenName);
        }

        [Route("/home/[action]")]
        public IActionResult Denied()
        {
            return Content("You need to allow this application access in Google order to be able to login");
        }
    }
}