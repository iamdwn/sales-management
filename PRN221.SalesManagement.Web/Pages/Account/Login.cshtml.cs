using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.SessionExtensions;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PRN221.SalesManagement.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;


        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            UserDto user = null;
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            var customerEmail = _configuration["CustomerAccount:Email"];
            var customerPassword = _configuration["CustomerAccount:Password"];

            var existUser = HttpContext.Session.GetObjectFromJson<CustomerDto>($"{Input.Email}");

            if (ModelState.IsValid)
            {
                if (existUser != null)
                {
                    if (Input.Password.Equals(existUser.Password))
                    {
                        user = new UserDto
                        {
                            isAuthenticated = true,
                            FullName = existUser.FullName,
                            Email = existUser.Email,
                            isAdmin = false
                        };
                    } else
                    {
                        TempData["toast-error"] = "Invalid username or password!";
                        return Page();
                    }
                }

                if (Input.Email.Equals(adminEmail) && Input.Password.Equals(adminPassword))
                {
                    user = new UserDto
                    {
                        isAuthenticated = true,
                        FullName = "Admin",
                        Email = adminEmail,
                        isAdmin = true
                    };
                }

                if (Input.Email.Equals(customerEmail) && Input.Password.Equals(customerPassword))
                {
                    user = new UserDto
                    {
                        isAuthenticated = true,
                        FullName = "Customer",
                        Email = customerEmail,
                        isAdmin = false,
                    };
                }

                if (user == null)
                {
                    TempData["toast-error"] = "Invalid username or password!";
                    return Page();
                }

                TempData["toast-success"] = "Login success!";

                HttpContext.Session.SetObjectAsJson("User", user);
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["toast-error"] = "Login failed!";
                return Page();
            }
        }
    }
}
