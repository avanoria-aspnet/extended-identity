using Application.Abstractions.Services;
using Application.Dtos.Members;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication.Register;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController(IMemberService service) : Controller
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
}
