using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.ProductPage
{
    public class CreateModel : PageModel
    {
        private readonly PRN221.SalesManagement.Repo.Persistences.SalesManagementContext _context;

        public CreateModel(PRN221.SalesManagement.Repo.Persistences.SalesManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var activeCategories = _context.Categories
                                           .Where(c => c.Status == true)
                                           .ToList();

            ViewData["CategoryId"] = new SelectList(activeCategories, "Id", "Name");
            return Page();
        }


        [BindProperty]
        public Product Product { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
