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

namespace PRN221.SalesManagement.Web.Pages.ProductPage
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.GetByID(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.ProductRepository.GetByID(id);
            var listRemove = new List<OrderDetail>();

            if (product != null)
            {
                Product = product;
                listRemove = _unitOfWork.OrderDetailRepository.Get(
                    includeProperties: "SaleOrder",
                    filter: o => o.Product.Id.Equals(product.Id)
                    )
                    .ToList();

                foreach (var orderDetail in product.OrderDetails)
                {
                    _unitOfWork.OrderDetailRepository.Delete(orderDetail);
                    _unitOfWork.SaleOrderRepository.Delete(orderDetail.SaleOrder);
                }

                _unitOfWork.ProductRepository.Delete(Product);
                _unitOfWork.Save();
            }

            return RedirectToPage("./Index");
        }
    }
}
