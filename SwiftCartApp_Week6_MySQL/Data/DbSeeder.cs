using Microsoft.AspNetCore.Identity;
using SwiftCartApp.Models;

namespace SwiftCartApp.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (!db.Categories.Any())
        {
            db.Categories.AddRange(new[] { "Electronics", "Clothes", "Home", "Beauty" }.Select(n => new Category { Name = n }));
            db.SaveChanges();
        }

        if (db.Sellers.Any()) return;

        var cat = db.Categories.ToDictionary(c => c.Name, c => c.Id);
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Demo login:  demo@swiftcart.pk  /  Demo@1234
        var seller = new Seller
        {
            StoreName = "SwiftCart Demo Store",
            OwnerName = "Demo Seller",
            Email = "demo@swiftcart.pk",
            Phone = "0300-1234567",
            City = "Lahore",
            CategoryId = cat["Electronics"]
        };
        seller.PasswordHash = new PasswordHasher<Seller>().HashPassword(seller, "Demo@1234");
        db.Sellers.Add(seller);
        db.SaveChanges();

        Product P(string name, string category, decimal price, int stock, string emoji, string bg, string desc, int? expiresInDays = null) => new()
        {
            SellerId = seller.Id,
            CategoryId = cat[category],
            Name = name,
            Price = price,
            Stock = stock,
            Emoji = emoji,
            BgColor = bg,
            Description = desc,
            HasExpiry = expiresInDays is not null,
            ExpiryDate = expiresInDays is null ? null : today.AddDays(expiresInDays.Value)
        };

        var products = new List<Product>
        {
            P("Aero Wireless Earbuds", "Electronics", 7999, 14, "🎧", "#dfe6ff", "Bluetooth 5.3 earbuds with 28-hour battery life and a pocket charging case."),
            P("Pulse Smart Watch", "Electronics", 12499, 3, "⌚", "#e3f7ee", "AMOLED display, heart-rate tracking and 7-day battery."),
            P("Everyday Cotton Hoodie", "Clothes", 3499, 30, "🧥", "#fff0d1", "Heavyweight 400gsm cotton hoodie in a relaxed fit."),
            P("Street Canvas Sneakers", "Clothes", 5299, 0, "👟", "#ffe1e8", "Lightweight canvas sneakers with a cushioned insole."),
            P("Ceramic Pour-Over Set", "Home", 4150, 9, "☕", "#efe6ff", "Hand-glazed dripper, server and two cups."),
            P("Desk Lamp Halo", "Home", 3899, 21, "💡", "#fff6c9", "Dimmable LED lamp with three colour temperatures."),
            P("Glow Vitamin C Serum", "Beauty", 2299, 40, "🧴", "#ffe9d6", "15% vitamin C serum for an even, bright complexion.", expiresInDays: 270),
            P("Hydrating Face Cream", "Beauty", 1899, 25, "🧼", "#fde2f3", "Light daily moisturiser with hyaluronic acid.", expiresInDays: 20),
            P("Portable Power Bank 20K", "Electronics", 6499, 6, "🔋", "#d9f1ff", "20,000 mAh with 65W fast charging and a digital readout.")
        };
        db.Products.AddRange(products);
        db.SaveChanges();

        Order O(string no, string name, string status, Product p, int qty)
        {
            var sub = p.Price * qty;
            var tax = Math.Round(sub * 0.10m, 0);
            var ship = sub >= 15000 ? 0m : 250m;
            return new Order
            {
                OrderNumber = no, CustomerName = name, Phone = "0301-5550101", Address = "House 12, Street 4, Gulberg",
                City = "Lahore", PostalCode = "54000", Status = status, Subtotal = sub, Tax = tax, Shipping = ship, Total = sub + tax + ship,
                Items = { new OrderItem { ProductId = p.Id, SellerId = seller.Id, ProductName = p.Name, UnitPrice = p.Price, Quantity = qty } }
            };
        }

        db.Orders.AddRange(
            O("SC-48213", "Ayesha Khan", "Pending", products[0], 2),
            O("SC-48190", "Bilal Ahmed", "Shipped", products[3], 1),
            O("SC-48122", "Sana Malik", "Delivered", products[1], 1));
        db.SaveChanges();
    }
}
