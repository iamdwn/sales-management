using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Impl;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public SaleOrder SaleOrder { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleorder = _unitOfWork.SaleOrderRepository.GetByID(id);
            if (saleorder == null)
            {
                return NotFound();
            }
            SaleOrder = saleorder;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(SaleOrder saleOrder)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Attach(SaleOrder).State = EntityState.Modified;

            try
            {
                _unitOfWork.SaleOrderRepository.Update(saleOrder);
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleOrderExists(SaleOrder.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SaleOrderExists(int id)
        {
            return _unitOfWork.SaleOrderRepository.GetByID(id) != null ? true : false;
        }
    }
}
