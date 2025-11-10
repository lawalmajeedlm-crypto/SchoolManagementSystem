using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }
            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        // POST: Toggle Role
                [HttpPost]
        public async Task<IActionResult> ToggleRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            if (await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}