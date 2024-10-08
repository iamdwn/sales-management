using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.SessionExtensions;

namespace PRN221.SalesManagement.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public CustomerDto Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
                var existUser = HttpContext.Session.GetObjectFromJson<CustomerDto>($"{Input.Email}");

                if (existUser != null)
                {
                    if (existUser.Email.Equals(Input.Email))
                    {
                        TempData["toast-error"] = "Email is exist!";
                        return Page();
                    }
                }

                if (!Input.Password.Equals(Input.ConfirmPassword))
                {
                    TempData["toast-error"] = "Password is not match!";
                    return Page();
                }

                var user = new CustomerDto
                {
                    FullName = Input.FullName,
                    Email = Input.Email,
                    Phone = Input.Phone,
                    Password = Input.Password,
                    ConfirmPassword = Input.ConfirmPassword
                };

                HttpContext.Session.SetObjectAsJson($"{Input.Email}", user);
                TempData["toast-success"] = "Register success!";
                return RedirectToPage("/Account/Login");
        }
    }
}
