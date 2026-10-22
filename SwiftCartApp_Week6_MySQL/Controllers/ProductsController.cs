using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftCartApp.Data;
using SwiftCartApp.Models;

namespace SwiftCartApp.Controllers;

[ApiController, Route("api")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    /// <summary>Sprint 2: products for the homepage. Expired products are hidden.</summary>
    [HttpGet("products")]
    public async Task<List<ProductDto>> List()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return await db.Products
            .Where(p => p.IsActive && (!p.HasExpiry || p.ExpiryDate >= today))
            .OrderBy(p => p.Id)
            .Select(p => new ProductDto(p.Id, p.Name, p.Category.Name, p.Price, p.Stock,
                                        p.Emoji, p.BgColor, p.Description, p.HasExpiry, p.ExpiryDate))
            .ToListAsync();
    }
}
