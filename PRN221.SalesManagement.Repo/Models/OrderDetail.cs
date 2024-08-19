using System;
using System.Collections.Generic;

namespace PRN221.SalesManagement.Repo.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public virtual SaleOrder SaleOrder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
