using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;
using PRN221.SalesManagement.Repo.SessionExtensions;

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
            var listProductRemove = new List<Product>();

            if (category != null)
            {
                Category = category;
                listProductRemove = _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.CategoryId.Equals(category.Id))
                    .ToList();

                //HttpContext.Session.SetObjectAsJson("listProductRemove", listProductRemove);
                //_context.RemoveRange(listProductRemove);

                foreach (var product in listProductRemove)
                {
                    foreach (var orderDetail in product.OrderDetails)
                    {
                        _context.OrderDetails.Remove(orderDetail);
                        _context.SaleOrders.Remove(orderDetail.SaleOrder);
                    }
                    _context.Products.Remove(product);
                }

                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
