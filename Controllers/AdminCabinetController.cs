using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Types;
using PersonalAccount.ViewModels;

namespace PersonalAccount.Controllers;

[Authorize(Roles = AccountRoleConstants.Administrator)]
public class AdminCabinetController(
    IAdminCabinetService cabinet,
    IConfirmationTokenService confirmationService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await cabinet.GetAllStudentAccounts();
        var profiles = await cabinet.GetAllStudentProfiles();

        var students = new List<AdminCabinetStudentViewModel>();
        foreach (var profile in profiles)
        {
            var isEmailConfirmed = await confirmationService
                .HasAnyConfirmedTokenAsync(profile.AccountId);

            students.Add(new AdminCabinetStudentViewModel
            {
                AccountId = profile.AccountId,
                Email = accounts[profile.AccountId].Email,
                FullName = profile.FullName,
                GroupName = profile.GroupName,
                PhotoUrl = profile.PhotoUrl?.ToString(),
                IsEmailConfirmed = isEmailConfirmed,
            });
        }

        return View(new AdminCabinetViewModel
        {
            Students = students
        });
    }

    [HttpGet]
    public IActionResult AddStudent()
    {
        return View(new AddStudentViewModel());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStudent(AddStudentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        return RedirectToAction("Index");
    }
}