﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;

namespace PRN221.SalesManagement.Web.Pages.ProductPage
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Product> Product { get; set; } = new List<Product>();
        public IList<CartItems> cartList { get; set; } = new List<CartItems>();

        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;

            var totalItems = _unitOfWork.ProductRepository
                .Get(
                filter: p => p.Category.Status == true,
                includeProperties: "Category")
                .Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            Product = _unitOfWork.ProductRepository.Get(
                filter: p => p.Category.Status == true,
                includeProperties: "Category",
                pageIndex: PageIndex,
                pageSize: PageSize
            ).ToList();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

    }
}
