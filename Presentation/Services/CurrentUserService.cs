using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Presentation.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
{
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var userIdStr = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }

    public bool IsAdmin => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) == "Admin";

    public string? Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
}