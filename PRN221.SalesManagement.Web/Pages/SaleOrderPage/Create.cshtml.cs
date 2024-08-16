using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
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
            return Page();
        }

        [BindProperty]
        public SaleOrder SaleOrder { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SaleOrders.Add(SaleOrder);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
