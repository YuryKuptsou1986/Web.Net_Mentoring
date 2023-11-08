using Homework.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        UserManager<UserIdentity> _manager;

        public UsersController(UserManager<UserIdentity> manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {

            return View(_manager.Users);
        }
    }
}
