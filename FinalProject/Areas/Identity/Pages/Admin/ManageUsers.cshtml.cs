using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinalProject.Areas.Identity.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Pages.Admin
{
    // ??????????????????????? Admin ???????????????????????????????
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<FinalProjectUser> _userManager;

        public ManageUsersModel(UserManager<FinalProjectUser> userManager)
        {
            _userManager = userManager;
        }

        // ?????????????????????????????? Manage Users
        public List<UserRolesViewModel> Users { get; set; }

        // ??????????????????????????????????????
public async Task OnGetAsync()
{
    // ??????????????????????????????????
    var users = _userManager.Users.ToList();
    var userRolesList = new List<UserRolesViewModel>();

    // ???????????????????????????????
    foreach (var user in users)
    {
        var roles = await _userManager.GetRolesAsync(user);
        userRolesList.Add(new UserRolesViewModel
        {
            UserId = user.Id,  // ??????? UserId ???? ? ????????????
            UserName = "Admin", // ?????????????????? Admin
            Password = "Admin_12345", // ???????????????? Admin_12345
            Roles = roles.ToList()  // ??????? IList<string> ???? List<string>
        });
    }

    // ?????????????????????????????????
    Users = userRolesList;
}


        // ??????????????????????
        public async Task<IActionResult> OnGetDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // ????????????????????????????????????????????????????
            if (user == null)
            {
                return NotFound(); // ??????????????
            }

            // ?????????????????????? Admin ???????????????????
            if (userId == User.Identity.Name)
            {
                ModelState.AddModelError(string.Empty, "You cannot delete your own account.");
                return Page();
            }

            // ????????
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToPage(); // ????????????????????????????
            }

            // ????????????????? ?????????????????
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page(); // ????????????????????
        }
    }

    // ViewModel ??????????????????????????????
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }  // ?????????????????? "Admin"
        public string Password { get; set; }  // ???????????????? "Admin_12345"
        public List<string> Roles { get; set; }
    }
}
