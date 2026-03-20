using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

[Route("account")]
public class AccountController(IMemberService service) : Controller
{
    [HttpGet("my")]
    public IActionResult My()
    {
        return View();
    }

    
    [HttpPost]
    public async Task<IActionResult> RemoveAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await service.DeleteMemberAsync(userId);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        return RedirectToAction(nameof(My));

    }
}
