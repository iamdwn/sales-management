using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.CategoryPage
{
    public class DeleteModel : PageModel
    {
        private readonly PRN221.SalesManagement.Repo.Persistences.SalesManagementContext _context;

        public DeleteModel(PRN221.SalesManagement.Repo.Persistences.SalesManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            var listRemove = new List<Product>();

            if (category != null)
            {
                Category = category;
                listRemove = _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId.Equals(category.Id))
                    .ToList();

                _context.RemoveRange(listRemove);
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
