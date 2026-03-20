using Application.Abstractions.Identity;
using Application.Abstractions.Services;
using Application.Dtos.Members;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication.Register;
using Presentation.WebApp.Models.Authentication.SignIn;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController(IIdentityService auth, IMemberService service) : Controller
{
    private const string EmailSessionKey = "EmailSessionKey";

    [HttpGet("sign-up")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("sign-up")]
    public IActionResult SignUp(RegisterEmailForm form)
    {
        if(!ModelState.IsValid)
            return View(form);

        HttpContext.Session.SetString(EmailSessionKey, form.Email);
        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString(EmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        return View();
    }

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword(SetPasswordForm form, CancellationToken ct = default)
    {
        var email = HttpContext.Session.GetString(EmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        if (!ModelState.IsValid)
            return View(form);

        var request = new RegisterMemberRequest(email, form.Password);
        var result = await service.CreateMemberAsync(request, ct);

        if (!result.Succeeded)
        {
            ViewData["ErrorMessage"] = result.Errors.FirstOrDefault();
            return View(form);
        }

        return RedirectToAction(nameof(SignIn));
    }

    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInForm form)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ErrorMessage"] = "Incorrect email or password";
            return View(form);
        }

        var loggedIn = await auth.LoginAsync(form.Email, form.Password, false, false);
        if (!loggedIn)
        {
            ViewData["ErrorMessage"] = "Incorrect email or password";
            return View(form);
        }

        return RedirectToAction("My", "Account");
    }

    [HttpPost]
    public new async Task<IActionResult> SignOut()
    {
        await auth.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}
