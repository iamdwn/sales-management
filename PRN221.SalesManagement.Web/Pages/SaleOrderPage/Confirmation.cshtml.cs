using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PRN221.SalesManagement.Web.Pages.SaleOrderPage
{
    public class ConfirmationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int SaleOrderId { get; set; }

        public void OnGet(int id)
        {
            SaleOrderId = id;
        }
    }
}
