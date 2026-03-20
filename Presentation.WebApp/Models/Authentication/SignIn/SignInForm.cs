using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication.SignIn;

public class SignInForm
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;
}
