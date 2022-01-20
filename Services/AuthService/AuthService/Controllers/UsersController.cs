//using AuthServer.Infrastructure.Data.Identity;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AuthService.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    [Authorize]
//    public class UsersController : Controller
//    {
//        private readonly UserManager<User> _userManager;
//        public UsersController(UserManager<User> userManager)
//        {
//            _userManager = userManager;
//        }
//        [Route("[action]")]
//        public async Task<IActionResult> Index()
//        {
//            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
//            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
//            return View(allUsersExceptCurrentUser);
//        }
//    }
//}
