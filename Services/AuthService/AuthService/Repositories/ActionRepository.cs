using AuthService.Infrastructure.Data.Context;
using AuthService.Infrastructure.Data.Identity;
using AuthService.Models;
using AuthService.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Repositories
{
    public class ActionRepository : IActionRepository
    {
        private readonly AppIdentityDbContext _dbContext;
        public ActionRepository(AppIdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateAction(string nameModule, string description, int moduleId, string roleId)
        {
            _dbContext.Actions.Add(
                    new Action 
                    { 
                        Name = nameModule, 
                        Description = description, 
                        ModuleId = moduleId == 0 ? null : moduleId, 
                        RoleId = roleId }
                );
            _dbContext.SaveChanges();
        }

        public void DeleteAction(string nameModule, string description, int moduleId)
        {
            _dbContext.Actions.Remove(new Action { Name = nameModule, ModuleId = moduleId });
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<ActionViewModel>> GetActions()
        {
            return await _dbContext.Actions.Select(s => new ActionViewModel { Id = s.Id, Name = s.Name }).ToListAsync();
        }

        public async Task<IEnumerable<ActionViewModel>> GetActionsByRoleId(string roleId)
        {
            return await _dbContext.Actions.Where(s => s.RoleId == roleId)
                            .Select(s => new ActionViewModel { Id = s.Id, Name = s.Name, Description = s.Description })
                            .ToListAsync();
        }

        public async Task<IEnumerable<ActionViewModel>> GetActionsByModuleId(int moduleId)
        {
            return await _dbContext.Actions.Where(s => s.ModuleId == moduleId)
                            .Select(s => new ActionViewModel { Id = s.Id, Name = s.Name, Description = s.Description })
                            .ToListAsync();
        }

        public async Task<List<string>> GeneratePermissionsForRole(string roleId)
        {
            List<string> permissionsForRole = new List<string>();
            var actions = _dbContext.Actions.Where(s => s.RoleId == roleId);
            foreach (var action in actions)
            {
                if (action.ModuleId != null)
                {
                    var module = await _dbContext.Modules.FirstAsync(s => s.Id == action.ModuleId);
                    if (module != null)
                    {
                        permissionsForRole.Add($"Permissions.{module.Name}.{action.Name}");
                    } else
                    {
                        permissionsForRole.Add($"Permissions.{action.Name}");
                    }
                } else
                {
                    permissionsForRole.Add($"Permissions.{action.Name}");
                }
            }
            return permissionsForRole;
        }
    }
}
