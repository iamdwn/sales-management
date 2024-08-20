using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Impl;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;
using PRN221.SalesManagement.Repo.SessionExtensions;

namespace PRN221.SalesManagement.Web.Pages.CategoryPage
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  _unitOfWork.CategoryRepository.GetByID(id);

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

            var category = _unitOfWork.CategoryRepository.GetByID(id);
            var listProductRemove = new List<Product>();

            if (category != null)
            {
                Category = category;
                listProductRemove = _unitOfWork.ProductRepository.Get(
                    includeProperties: "Category",
                    filter: p => p.CategoryId.Equals(category.Id)
                    )
                    .ToList();

                //HttpContext.Session.SetObjectAsJson("listProductRemove", listProductRemove);
                //_context.RemoveRange(listProductRemove);

                foreach (var product in listProductRemove)
                {
                    foreach (var orderDetail in product.OrderDetails)
                    {
                        _unitOfWork.OrderDetailRepository.Delete(orderDetail);
                        _unitOfWork.SaleOrderRepository.Delete(orderDetail.SaleOrder);
                    }
                    _unitOfWork.ProductRepository.Delete(product);
                }

                _unitOfWork.CategoryRepository.Delete(Category);
                _unitOfWork.Save();
            }

            return RedirectToPage("./Index");
        }
    }
}
