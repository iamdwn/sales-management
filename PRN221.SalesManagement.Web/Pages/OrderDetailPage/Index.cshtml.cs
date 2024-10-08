﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;

namespace PRN221.SalesManagement.Web.Pages.OrderDetailPage
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

        // Pagination properties
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public string IncludeProperties { get; set; } = "SaleOrder,Product";

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10; // Default page size

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;

            var totalItems = _unitOfWork.OrderDetailRepository.Get().Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            var orderDetailsQuery = _unitOfWork.OrderDetailRepository.Get(
                includeProperties: IncludeProperties
            );

            // Apply pagination
            OrderDetail = orderDetailsQuery
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
