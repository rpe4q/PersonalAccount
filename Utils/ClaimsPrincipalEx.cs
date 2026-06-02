using System.Security.Claims;
using PersonalAccount.Types;

namespace PersonalAccount.Utils;

public static class ClaimsPrincipalEx
{
    public static int? GetId(this ClaimsPrincipal user) =>
        int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public static string? GetEmail(this ClaimsPrincipal user) => user.FindFirstValue(ClaimTypes.Email);

    public static AccountRoles? GetRole(this ClaimsPrincipal user) =>
        Enum.TryParse<AccountRoles>(user.FindFirstValue(ClaimTypes.Role), out var role) ? role : null;
}