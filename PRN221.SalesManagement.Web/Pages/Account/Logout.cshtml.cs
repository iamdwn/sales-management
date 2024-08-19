using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221.SalesManagement.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            //HttpContext.Session.Clear;
            HttpContext.Session.Remove("User");

            return RedirectToPage("/Index");
        }
    }
}
