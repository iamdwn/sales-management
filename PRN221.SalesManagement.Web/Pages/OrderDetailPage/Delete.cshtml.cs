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

namespace PRN221.SalesManagement.Web.Pages.OrderDetailPage
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;
        public string IncludeProperties { get; set; } = "SaleOrder,Product";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = _unitOfWork.OrderDetailRepository.Get(
                includeProperties: IncludeProperties,
                filter: o => o.Id == id
                )
                .FirstOrDefault();

            if (orderdetail == null)
            {
                return NotFound();
            }
            else
            {
                OrderDetail = orderdetail;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = _unitOfWork.OrderDetailRepository.Get(
                includeProperties: IncludeProperties,
                filter: o => o.Id == id)
                .FirstOrDefault();


            //var orderdetailRemove = _unitOfWork.OrderDetailRepository.Get(
            //    includeProperties: IncludeProperties,
            //    filter: o => o.Id == orderdetail.SaleOrder.Id)
            //    .ToList();


            if (orderdetail != null)
            {
                OrderDetail = orderdetail;

                //foreach ( var item in orderdetailRemove )
                //{
                //    _unitOfWork.OrderDetailRepository.Delete(item);
                //}

                _unitOfWork.OrderDetailRepository.Delete(orderdetail);

                //_unitOfWork.SaleOrderRepository.Delete(OrderDetail.SaleOrder);

                _unitOfWork.Save();
            }

            return RedirectToPage("./Index");
        }
    }
}
