using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Constants;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Utils;
using PersonalAccount.ViewModels;

namespace PersonalAccount.Controllers;

[Authorize(Roles = AccountRoleConstants.Student)]
public class StudentCabinetController(
    IStudentCabinetService cabinetService,
    IConfirmationTokenService confirmationTokenService
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accountId = User.GetId();
        var accountEmail = User.GetEmail();
        if (accountId == null || accountEmail == null) return RedirectToAction("Error", "Home");

        var student = await cabinetService.GetByAccountIdAsync(accountId.Value);
        if (student == null) return RedirectToAction("Error", "Home");

        var isEmailConfirmed = await confirmationTokenService.HasAnyConfirmedTokenAsync(accountId.Value);
        var group = await cabinetService.GetStudentGroup(student.GroupId);
        if (group == null) return RedirectToAction("Error", "Home");

        return View(new StudentCabinetViewModel
        {
            Email = accountEmail,
            FullName = student.FullName,
            GroupName = group.Name,
            PhotoUrl = student.PhotoUrl?.ToString(),
            IsEmailConfirmed = isEmailConfirmed
        });
    }
}