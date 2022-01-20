using AuthService.Models;
using AuthService.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _moduleRepository;
        public ModuleController(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        //public async Task<ActionResult> Index()
        //{
        //    var listModule = await _moduleRepository.GetModules();
        //    return View(listModule);
        //}

        [HttpPost]
        [Route("[action]")]
        public ActionResult AddModule([FromBody] AddModuleViewModel addModuleViewModel)
        {
            if (addModuleViewModel != null)
            {
                _moduleRepository.CreateModule(addModuleViewModel.Name, addModuleViewModel.Description, addModuleViewModel.RoleId);
            }
            return Ok();
        }
    }
}
