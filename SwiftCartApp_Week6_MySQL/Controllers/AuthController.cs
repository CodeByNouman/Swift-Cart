using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwiftCartApp.Data;
using SwiftCartApp.Models;

namespace SwiftCartApp.Controllers;

[ApiController, Route("api/auth")]
public class AuthController(AppDbContext db) : ControllerBase
{
    private static readonly PasswordHasher<Seller> Hasher = new();

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        if (await db.Sellers.AnyAsync(s => s.Email == email))
            return Conflict(new { message = "An account with this email already exists." });

        var category = await db.Categories.FirstOrDefaultAsync(c => c.Name == dto.Category);
        if (category is null)
            return BadRequest(new { message = "Choose a valid category." });

        var seller = new Seller
        {
            StoreName = dto.StoreName.Trim(),
            OwnerName = dto.OwnerName.Trim(),
            Email = email,
            Phone = dto.Phone.Trim(),
            City = dto.City.Trim(),
            CategoryId = category.Id
        };
        seller.PasswordHash = Hasher.HashPassword(seller, dto.Password);

        db.Sellers.Add(seller);
        await db.SaveChangesAsync();
        return Ok(new { message = "Seller account created." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var seller = await db.Sellers.FirstOrDefaultAsync(s => s.Email == email);

        if (seller is null || Hasher.VerifyHashedPassword(seller, seller.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Incorrect email or password." });

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, seller.Id.ToString()),
            new(ClaimTypes.Name, seller.StoreName),
            new(ClaimTypes.Email, seller.Email)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = dto.RememberMe });

        return Ok(new { store = seller.StoreName, email = seller.Email });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [Authorize, HttpGet("me")]
    public IActionResult Me() =>
        Ok(new { store = User.Identity!.Name, email = User.FindFirstValue(ClaimTypes.Email) });
}
