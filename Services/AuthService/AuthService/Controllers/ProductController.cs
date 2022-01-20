using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Constants;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Permissions.LinkBank.Link")]
        public IActionResult Create()
        {
            return Ok("Permissions.LinkBank.Link");
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Permissions.LinkBank.UnLink")]
        public IActionResult Approve()
        {
            return Ok("Permissions.LinkBank.UnLink");
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Permissions.Initialize.PayInMoney")]
        public IActionResult Reject()
        {
            return Ok("Permissions.Initialize.PayInMoney");
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Permissions.Report")]
        public IActionResult ActionView()
        {
            return Ok("Permissions.Report");
        }
        //[HttpGet]
        //[Route("[action]")]
        //[Authorize(Policy = "Permissions.Products.Delete")]
        //public IActionResult Delete()
        //{
        //    return Ok();
        //}
    }
}
