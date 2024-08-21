using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221.SalesManagement.Repo.Dtos;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.SessionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PRN221.SalesManagement.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
                // Update quantity
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

            // Save the updated cart
            HttpContext.Session.SetObjectAsJson("Cart", CartItems);
            int cartItemCount = (int)CartItems.Sum(item => item.Quantity);
            HttpContext.Session.SetInt32("cartItemCount", cartItemCount);

            return RedirectToPage("/ProductPage/Index");
        }

        public IActionResult OnPostUpdateQuantity(int index, int quantity)
        {
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();

            if (index >= 0 && index < CartItems.Count)
            {
                CartItems[index].Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", CartItems);
            }
            int cartItemCount = (int)CartItems.Sum(item => item.Quantity);
            HttpContext.Session.SetInt32("cartItemCount", cartItemCount);

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

            int cartItemCount = (int)CartItems.Sum(item => item.Quantity);
            HttpContext.Session.SetInt32("cartItemCount", cartItemCount);
            CalculateTotals();
            return RedirectToPage();
        }

        public IActionResult OnPostProceedToCheckout(string customerName, string phone)
        {
            // Ensure the cart is not empty
            CartItems = HttpContext.Session.GetObjectFromJson<List<CartItems>>("Cart") ?? new List<CartItems>();
            Total = HttpContext.Session.GetInt32("Total") ?? 0;
            if (CartItems.Count == 0)
            {
                // Optionally, add some logic to handle empty carts
                return RedirectToPage("/Cart/Index");
            }

            // Create SaleOrder
            var saleOrder = new SaleOrder
            {
                CustomerName = customerName,
                Phone = phone,
                OrderDate = DateTime.Now,
                TotalAmount = (decimal)Total,
                Status = -1
            };

            _unitOfWork.SaleOrderRepository.Insert(saleOrder);
            _unitOfWork.Save();

            // Create OrderDetails
            foreach (var cartItem in CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = saleOrder.Id,
                    ProductId = cartItem.ProductId,
                    UnitPrice = (decimal)cartItem.Price,
                    Quantity = (int)cartItem.Quantity
                };

                _unitOfWork.OrderDetailRepository.Insert(orderDetail);
            }

            _unitOfWork.Save();

            // Clear the cart
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.SetInt32("cartItemCount", 0);

            // Redirect to confirmation page
            return RedirectToPage("/SaleOrderPage/Confirmation", new { id = saleOrder.Id });
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

            HttpContext.Session.SetInt32("Total", (int)Total);
        }
    }
}
