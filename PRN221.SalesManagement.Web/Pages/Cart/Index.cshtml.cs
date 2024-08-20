using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.SessionExtensions;

namespace PRN221.SalesManagement.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public List<CartItems> CartItems { get; set; } = new List<CartItems>();

        public double Subtotal { get; private set; }
        public double Tax { get; private set; }
        public double Shipping { get; private set; }
        public double Total { get; private set; }

        public void OnGet()
        {
            // Load cart from session
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            CalculateTotals();
        }

        public IActionResult OnPostAddToCart(int productId, string productName, string description, double price, double quantity)
        {
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            // Check if the item already exists in the cart
            var existingItem = CartItems.Find(item => item.ProductId == productId);

            if (existingItem != null)
            {
                // Update quantity if item exists
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add new item to cart
                CartItems.Add(new CartItems
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = (double)price,
                    Quantity = quantity
                });
            }

            // Save the updated cart back to session
            HttpContext.Session.SetObjectAsJson("Cart", CartItems);

            return RedirectToPage("/Cart/Index");
        }


        public IActionResult OnPostUpdateQuantity(int index, int quantity)
        {
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            if (index >= 0 && index < CartItems.Count)
            {
                CartItems[index].Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", CartItems);
            }

            CalculateTotals();
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveItem(int index)
        {
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            if (index >= 0 && index < CartItems.Count)
            {
                CartItems.RemoveAt(index);
                HttpContext.Session.SetObjectAsJson("Cart", CartItems);
            }

            CalculateTotals();
            return RedirectToPage();
        }

        private void CalculateTotals()
        {
            Subtotal = 0;
            foreach (var item in CartItems)
            {
                Subtotal += item.Price * item.Quantity;
            }

            Tax = Subtotal * 0.1;
            Shipping = 5.00; 
            Total = Subtotal + Tax + Shipping;
        }
    }
}
