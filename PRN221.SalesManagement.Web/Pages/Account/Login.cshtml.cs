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

            if (ModelState.IsValid)
            {
                if (Input.Email.Equals(adminEmail) && Input.Password.Equals(adminPassword))
                {
                    user = new UserDto
                    {
                        isAuthenticated = true,
                        Username = "Admin",
                        Email = adminEmail,
                        isAdmin = true
                    };
                }

                if (Input.Email.Equals(customerEmail) && Input.Password.Equals(customerPassword))
                {
                    user = new UserDto
                    {
                        isAuthenticated = true,
                        Username = "Customer",
                        Email = customerEmail,
                        isAdmin = false,
                    };
                }

                if (user == null) return Page();

                TempData["toast-success"] = "Login success!";

                HttpContext.Session.SetObjectAsJson("User", user);;
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
