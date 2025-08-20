using Application.Features.Auth.Commands;
using Application.Features.Auth.Contracts.Requests;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Presentation.Controllers;

public class AccountController(LoginCommand loginCommand) : ControllerBase
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return HandleResult<bool>(ExtractValidationErrors(ModelState));

        var result = await loginCommand.Handle(loginRequest);

        if (result.IsFailure)
            return HandleResult(result);

        var claims = new List<Claim> {
            new(ClaimTypes.Email,result.Value.Email),
            new(ClaimTypes.NameIdentifier,result.Value.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims,"CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        return HandleResult(result);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }
}