using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.SalesManagement.Repo.Dtos
{
    public class CartItems
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string Category { get; set; }
    }

    public class Cart
    {
        public List<CartItems> Items { get; set; } = new List<CartItems>();
    }
}
