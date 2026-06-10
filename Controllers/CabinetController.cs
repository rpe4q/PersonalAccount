using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Types;
using PersonalAccount.Utils;

namespace PersonalAccount.Controllers;

[Authorize]
public class CabinetController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var role = User.GetRole();
        if (role == null) return Forbid();

        var controllerName = role.Value switch
        {
            AccountRoles.Admin => "AdminCabinet",
            AccountRoles.Teacher => "TeacherCabinet",
            AccountRoles.Student => "StudentCabinet",
            _ => throw new InvalidOperationException($"Unknown role: {role.Value}")
        };

        return RedirectToAction("Index", controllerName);
    }
}