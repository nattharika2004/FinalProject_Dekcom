using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class ManageUsersModel : PageModel
{
    private readonly UserManager<FinalProjectUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ManageUsersModel(UserManager<FinalProjectUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<FinalProjectUser> Users { get; set; }

    public async Task OnGetAsync()
    {
        Users = _userManager.Users.ToList();
    }

    public async Task<IActionResult> OnPostDeleteUserAsync(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // ????????????????????????????????????????????? (Admin)
        if (user.UserName == User.Identity.Name)
        {
            ModelState.AddModelError(string.Empty, "Cannot delete the logged-in user.");
            return RedirectToPage();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToPage();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return RedirectToPage();
    }
}
