using AuthService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthService.Repositories.Interface
{
    public interface IActionRepository
    {
        Task<IEnumerable<ActionViewModel>> GetActions();
        Task<IEnumerable<ActionViewModel>> GetActionsByModuleId(int moduleId);
        Task<IEnumerable<ActionViewModel>> GetActionsByRoleId(string roleId);
        void CreateAction(string nameAction, string description, int moduleId, string roleId);
        void DeleteAction(string nameAction, string description, int moduleId);
        Task<List<string>> GeneratePermissionsForRole(string roleId);
    }
}
