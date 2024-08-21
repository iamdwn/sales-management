//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PRN221.SalesManagement.Repo.Interfaces;
//using PRN221.SalesManagement.Repo.Models;

//namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
//{
//    public class ConfirmationModel : PageModel
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public ConfirmationModel(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public SaleOrder SaleOrder { get; set; } = default!;
//        public List<OrderDetail> OrderDetails { get; set; } = default!;

//        public IActionResult OnGet(int id)
//        {
//            SaleOrder = _unitOfWork.SaleOrderRepository.GetByID(id);
//            if (SaleOrder == null)
//            {
//                return NotFound();
//            }

//            OrderDetails = _unitOfWork.OrderDetailRepository.Get()
//                .Where(od => od.OrderId == id)
//                .ToList();

//            return Page();
//        }
//    }
//}
