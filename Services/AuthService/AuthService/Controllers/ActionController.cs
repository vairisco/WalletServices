using AuthService.Models;
using AuthService.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IActionRepository _actionRepository;
        public ActionController(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }

        //public async Task<ActionResult> Index()
        //{
        //    var listModule = await _moduleRepository.GetModules();
        //    return View(listModule);
        //}

        [HttpGet("{moduleId}")]
        public async Task<IEnumerable<ActionViewModel>> GetActionsByModuleId(int moduleId)
        {
            if (moduleId != null)
            {
                return await _actionRepository.GetActionsByModuleId(moduleId);
            }
            return null;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult AddAction([FromBody]AddActionViewModel addActionViewModel)
        {
            if (addActionViewModel != null)
            {
                _actionRepository.CreateAction(addActionViewModel.Name, addActionViewModel.Description, addActionViewModel.ModuleId, addActionViewModel.RoleId);
            }
            return Ok();
        }
    }
}
