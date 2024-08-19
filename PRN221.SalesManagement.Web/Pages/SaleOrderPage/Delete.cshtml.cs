using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class DeleteModel : PageModel
    {
        private readonly PRN221.SalesManagement.Repo.Persistences.SalesManagementContext _context;

        public DeleteModel(PRN221.SalesManagement.Repo.Persistences.SalesManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SaleOrder SaleOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleorder = await _context.SaleOrders.FirstOrDefaultAsync(m => m.Id == id);

            if (saleorder == null)
            {
                return NotFound();
            }
            else
            {
                SaleOrder = saleorder;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleorder = await _context.SaleOrders.FindAsync(id);
            var listRemove = new List<OrderDetail>();

            if (saleorder != null)
            {
                SaleOrder = saleorder;
                listRemove = _context.OrderDetails
                    .Include(o => o.SaleOrder)
                    .Where(o => o.SaleOrder.Id.Equals(saleorder.Id))
                    .ToList();

                _context.RemoveRange(listRemove);


                _context.SaleOrders.Remove(SaleOrder);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
