using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication.Register;

public class RegisterEmailForm
{
    [Required(ErrorMessage = "Email address is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email address must be valid")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;
}
