using System.ComponentModel.DataAnnotations;

namespace motorcycle_sales.Web.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    // TODO implements this validation
    // [Remote(action: "IsEmailInUse", controller: "Account")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password",
        ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = "You must provide a phone number")]
    [Display(Name = "Phone number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Region { get; set; }
}
