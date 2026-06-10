using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Types;
using PersonalAccount.Utils;
using PersonalAccount.ViewModels;
using static PersonalAccount.Types.AccountRoles;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Confirmation;

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
    [HttpGet]
    public async Task<IActionResult> Admin(
        [FromServices] IStudentCabinetService cabinetService,
        [FromServices] IConfirmationTokenService confirmationService)
    {
        var role = User.GetRole();
        if (role != Administrator)
            return Forbid();

        var accountId = User.GetId();
        var accountEmail = User.GetEmail();
        if (accountId == null || accountEmail == null)
            return RedirectToAction("Error", "Home");

        var student = await cabinetService
            .GetByAccountIdAsync(accountId.Value);
        if (student == null)
            return RedirectToAction("Error", "Home");

        var isEmailConfirmed = await confirmationService
            .HasAnyConfirmedTokenAsync(accountId.Value);

        return View(new AdminCabinetStudentViewModel
        {
            AccountId = accountId.Value,
            Email = accountEmail,
            FullName = student.FullName,
            GroupName = student.GroupName,
            PhotoUrl = student.PhotoUrl?.ToString(),
            IsEmailConfirmed = isEmailConfirmed
        });
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmStudentEmail(
        int id,
        [FromServices] IAdminCabinetService cabinetService)
    {
        await cabinetService.ConfirmStudentEmailAsync(id);

        return RedirectToAction("Admin");
    }
}