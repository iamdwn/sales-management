using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.SessionExtensions;
using System.Collections.Generic;
using System.Linq;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class HistoryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<SaleOrder> SaleOrders { get; set; }

        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public string UserFullName { get; set; }

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;

            var user = HttpContext.Session.GetObjectFromJson<UserDto>("User");

            var totalItems = _unitOfWork.SaleOrderRepository.Get(
                filter: order => order.CustomerName.Equals(user.FullName),
                orderBy: q => q.OrderByDescending(order => order.OrderDate)
            ).Count();

            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            UserFullName = user.FullName;

            SaleOrders = _unitOfWork.SaleOrderRepository.Get(
                filter: order => order.CustomerName.Equals(user.FullName),
                orderBy: q => q.OrderByDescending(order => order.OrderDate),
                pageIndex: PageIndex,
                pageSize: PageSize
            ).ToList();

        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
