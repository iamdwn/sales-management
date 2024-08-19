using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;

namespace PRN221.SalesManagement.Web.Pages.OrderDetailPage
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PRN221.SalesManagement.Repo.Persistences.SalesManagementContext _context;

        public IndexModel(IUnitOfWork unitOfWork, Repo.Persistences.SalesManagementContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IList<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

        // Pagination properties
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public string includeProperties = "SaleOrder,Product";

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

            //OrderDetail = _unitOfWork.OrderDetailRepository.Get(
            //    includeProperties: "SaleOrder, Product",
            //    pageIndex: PageIndex,
            //    pageSize: PageSize
            //).ToList();
            IQueryable<OrderDetail> query = _context.OrderDetails;

            // Include related entities
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply pagination
            query = query.Skip((pageIndex - 1) * PageSize).Take(PageSize);

            OrderDetail = query.ToList();
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
