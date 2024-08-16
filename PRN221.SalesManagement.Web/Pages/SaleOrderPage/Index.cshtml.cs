using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<SaleOrder> SaleOrder { get; set; } = new List<SaleOrder>();
 
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync(int pageIndex = 1)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;

            var totalItems = _unitOfWork.SaleOrderRepository.Get().Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            SaleOrder = _unitOfWork.SaleOrderRepository.Get(
                pageIndex: PageIndex,
                pageSize: PageSize
            ).ToList();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
