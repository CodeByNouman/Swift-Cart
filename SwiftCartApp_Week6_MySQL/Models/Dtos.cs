using System.ComponentModel.DataAnnotations;

namespace SwiftCartApp.Models;

public class RegisterDto
{
    [Required, StringLength(40, MinimumLength = 3, ErrorMessage = "Store name must be 3-40 characters.")]
    public string StoreName { get; set; } = "";

    [Required, StringLength(50, MinimumLength = 3), RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Owner name can only contain letters and spaces.")]
    public string OwnerName { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    public string Email { get; set; } = "";

    [Required, RegularExpression(@"^03\d{2}-?\d{7}$", ErrorMessage = "Use a Pakistani mobile number, like 0300-1234567.")]
    public string Phone { get; set; } = "";

    [Required(ErrorMessage = "Choose a category.")]
    public string Category { get; set; } = "";

    [Required, StringLength(30)]
    public string City { get; set; } = "";

    [Required, RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$", ErrorMessage = "Password needs at least 8 characters with a letter and a number.")]
    public string Password { get; set; } = "";
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
}

public record ProductDto(
    int Id, string Name, string Category, decimal Price, int Stock,
    string Emoji, string BgColor, string Description, bool HasExpiry, DateOnly? ExpiryDate);
