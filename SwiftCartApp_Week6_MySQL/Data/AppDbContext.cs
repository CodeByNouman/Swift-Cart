using Microsoft.EntityFrameworkCore;
using SwiftCartApp.Models;

namespace SwiftCartApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Seller> Sellers => Set<Seller>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Category>().Property(c => c.Name).HasMaxLength(100);
        b.Entity<Category>().HasIndex(c => c.Name).IsUnique();

        b.Entity<Seller>(e =>
        {
            e.Property(s => s.Email).HasMaxLength(100);
            e.HasIndex(s => s.Email).IsUnique();
            e.HasOne(s => s.Category).WithMany().HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Product>(e =>
        {
            e.Property(p => p.Price).HasPrecision(10, 2);
            e.Property(p => p.RowVersion).IsRowVersion();
            e.HasOne(p => p.Seller).WithMany(s => s.Products).HasForeignKey(p => p.SellerId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Products_Price", "Price > 0");
                t.HasCheckConstraint("CK_Products_Stock", "Stock >= 0");
                // A product has an expiry date if and only if HasExpiry is true.
                t.HasCheckConstraint("CK_Products_Expiry",
                    "(HasExpiry = 0 AND ExpiryDate IS NULL) OR (HasExpiry = 1 AND ExpiryDate IS NOT NULL)");
            });
        });

        b.Entity<Order>(e =>
        {
            e.Property(o => o.OrderNumber).HasMaxLength(20);
            e.HasIndex(o => o.OrderNumber).IsUnique();
            e.Property(o => o.Subtotal).HasPrecision(12, 2);
            e.Property(o => o.Tax).HasPrecision(12, 2);
            e.Property(o => o.Shipping).HasPrecision(12, 2);
            e.Property(o => o.Total).HasPrecision(12, 2);
            e.ToTable(t => t.HasCheckConstraint("CK_Orders_Status", "Status IN ('Pending','Shipped','Delivered')"));
        });

        b.Entity<OrderItem>(e =>
        {
            e.Property(i => i.UnitPrice).HasPrecision(10, 2);
            e.HasOne(i => i.Order).WithMany(o => o.Items).HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(i => i.Seller).WithMany().HasForeignKey(i => i.SellerId).OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t => t.HasCheckConstraint("CK_OrderItems_Quantity", "Quantity > 0"));
        });
    }
}
