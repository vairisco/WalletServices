using AuthService.Constants;
using AuthService.Helpers;
using AuthService.Models;
using AuthService.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionManagement.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IModuleRepository _moduleRepository;
        private readonly IActionRepository _actionRepository;

        public PermissionController(
            RoleManager<IdentityRole> roleManager, 
            IModuleRepository moduleRepository,
            IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
            _roleManager = roleManager;
            _moduleRepository = moduleRepository;
        }
        [HttpGet("{roleId}")]
        public async Task<ActionResult<PermissionViewModel>> GetPermissions(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();

            var allPermissionsFromDB = new List<string>();
            var modules = await _moduleRepository.GetModules();

            var actions = await _actionRepository.GeneratePermissionsForRole(roleId);
            foreach (var item in actions)
            {
                allPermissions.Add(new RoleClaimsViewModel() { Selected = false, Type = "Permissions", Value = item });
            }
            //if (modules != null)
            //{
            //    foreach (var module in modules)
            //    {
            //        var actionsInModule = await _actionRepository.GetActionsByModuleId(module.Id);
            //        if (actionsInModule.Count() > 0)
            //        {
            //            foreach (var item in Permissions.GeneratePermissionsForModule(module.Name, actionsInModule.Select(s => s.Name).ToList()))
            //            {
            //                allPermissions.Add(new RoleClaimsViewModel() { Selected = false, Type = "Permissions", Value = item });
            //            }
            //        }
            //        else
            //        {
            //            foreach (var item in Permissions.GeneratePermissionsForModule(module.Name, null))
            //            {
            //                allPermissions.Add(new RoleClaimsViewModel() { Selected = false, Type = "Permissions", Value = item });
            //            }
            //        }
            //    }
            //}
            //else
            //{

            //}
            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return model;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Update([FromBody]PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return Ok();
        }
    }
}
