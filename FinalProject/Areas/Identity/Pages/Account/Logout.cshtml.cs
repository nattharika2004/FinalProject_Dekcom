using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FinalProject.Areas.Identity.Data;

namespace FinalProject.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<FinalProjectUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<FinalProjectUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Perform the logout action
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // If no return URL is provided, redirect to the home page (Layout page)
            returnUrl = returnUrl ?? "/"; // Redirect to the home page or the default Layout page

            // Redirect to the return URL or home page (Layout)
            return LocalRedirect(returnUrl);
        }
    }
}
