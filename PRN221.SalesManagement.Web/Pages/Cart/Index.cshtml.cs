using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace PRN221.SalesManagement.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private const string CartSessionKey = "CartItems";

        public List<CartItems> CartItems { get; set; } = new List<CartItems>();
        [BindProperty]
        public CartItems NewItem { get; set; } = new CartItems();
        public double Subtotal { get; private set; }
        public double Tax { get; private set; }
        public double Shipping { get; private set; } = 5.99;
        public double Total { get; private set; }

        public void OnGet()
        {
            // Retrieve cart from session or initialize a new list
            CartItems = HttpContext.Session.Get<List<CartItems>>(CartSessionKey) ?? new List<CartItems>();
            CalculateTotals();
        }

        public IActionResult OnPostAddItem()
        {
            if (ModelState.IsValid)
            {
                CartItems = HttpContext.Session.Get<List<CartItems>>(CartSessionKey) ?? new List<CartItems>();
                CartItems.Add(NewItem);
                HttpContext.Session.Set(CartSessionKey, CartItems);
                NewItem = new CartItems(); // Clear form after adding
                CalculateTotals();
            }
            return Page();
        }

        public IActionResult OnPostUpdateQuantity(int index, double quantity)
        {
            if (index >= 0 && index < CartItems.Count)
            {
                CartItems = HttpContext.Session.Get<List<CartItems>>(CartSessionKey) ?? new List<CartItems>();
                CartItems[index].Quantity = quantity;
                HttpContext.Session.Set(CartSessionKey, CartItems);
                CalculateTotals();
            }
            return Page();
        }

        public IActionResult OnPostRemoveItem(int index)
        {
            if (index >= 0 && index < CartItems.Count)
            {
                CartItems = HttpContext.Session.Get<List<CartItems>>(CartSessionKey) ?? new List<CartItems>();
                CartItems.RemoveAt(index);
                HttpContext.Session.Set(CartSessionKey, CartItems);
                CalculateTotals();
            }
            return Page();
        }

        private void CalculateTotals()
        {
            Subtotal = CartItems.Sum(item => item.Price * item.Quantity);   
            Tax = Subtotal * 0.1; // Example tax calculation (10%)
            Total = Subtotal + Tax + Shipping;
        }
    }

    public class CartItems
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}
