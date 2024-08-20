using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.Impl;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.SessionExtensions;
using System.Collections.Generic;
using System.Linq;

namespace PRN221.SalesManagement.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public List<CartItems> CartItems { get; set; }

        public double Subtotal => CartItems.Sum(x => x.Price * x.Quantity);
        public double Tax => Subtotal * 0.1;
        public double Shipping => 5.00; 
        public double Total => Subtotal + Tax + Shipping;
        public IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();
        }

        public IActionResult OnPost(int ProductId, int Quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();
            var product = _unitOfWork.ProductRepository.GetByID(ProductId);

            var cartItem = cart.FirstOrDefault(x => x.ProductId == ProductId);
            if (cartItem == null)
            {
                cart.Add(new CartItems { ProductId = ProductId, ProductName = product.Name, Price = (double) product.Price, Quantity = Quantity });
            }
            else
            {
                cartItem.Quantity += Quantity;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPostRemoveItem(int index)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart");
            if (cart != null && index >= 0 && index < cart.Count)
            {
                cart.RemoveAt(index);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateQuantity(int index, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart");
            if (cart != null && index >= 0 && index < cart.Count)
            {
                cart[index].Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToPage();
        }
    }
}
