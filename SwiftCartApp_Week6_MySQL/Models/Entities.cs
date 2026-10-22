namespace SwiftCartApp.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Product> Products { get; set; } = new();
}

public class Seller
{
    public int Id { get; set; }
    public string StoreName { get; set; } = "";
    public string OwnerName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Product> Products { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Emoji { get; set; } = "📦";
    public string BgColor { get; set; } = "#eef1f7";

    // Expiry: only products with HasExpiry = true carry an ExpiryDate.
    public bool HasExpiry { get; set; }
    public DateOnly? ExpiryDate { get; set; }

    public bool IsActive { get; set; } = true;   // false = deleted by the seller (kept for order history)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime RowVersion { get; set; }   // stops two buyers taking the last item at once
}

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? Email { get; set; }
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string PaymentMethod { get; set; } = "Cash on delivery";
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Pending";   // Pending | Shipped | Delivered
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;
    public string ProductName { get; set; } = "";   // snapshot at purchase time
    public decimal UnitPrice { get; set; }          // snapshot at purchase time
    public int Quantity { get; set; }
}
