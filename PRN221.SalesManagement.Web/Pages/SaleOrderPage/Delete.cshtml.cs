using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
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

            var saleorder = _unitOfWork.SaleOrderRepository.GetByID(id);
            var listOrderDetailRemove = new List<OrderDetail>();

            if (saleorder != null)
            {
                SaleOrder = saleorder;
                listOrderDetailRemove = _unitOfWork.OrderDetailRepository.Get(
                    includeProperties: "SaleOrder",
                    filter: o => o.SaleOrder.Id.Equals(saleorder.Id)
                    )
                    .ToList();

                foreach (var detail in listOrderDetailRemove)
                {
                    _unitOfWork.OrderDetailRepository.Delete(detail);
                }

                _unitOfWork.SaleOrderRepository.Delete(SaleOrder);
                _unitOfWork.Save();
            }

            return RedirectToPage("./Index");
        }
    }
}
