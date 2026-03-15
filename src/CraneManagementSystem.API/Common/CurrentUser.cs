using System.Security.Claims;

namespace CraneManagementSystem.API.Common;

public static class CurrentUser
{
    public static Guid? GetUserId(this ClaimsPrincipal? user)
    {
        var sub = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? user?.FindFirstValue("sub");
        return Guid.TryParse(sub, out var id) ? id : null;
    }

    public static Guid? GetCompanyId(this ClaimsPrincipal? user)
    {
        var companyId = user?.FindFirstValue("companyId");
        return Guid.TryParse(companyId, out var id) ? id : null;
    }

    public static string? GetRole(this ClaimsPrincipal? user) =>
        user?.FindFirstValue(ClaimTypes.Role) ?? user?.FindFirstValue("role");
}
