using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Constants;
using PersonalAccount.Services.Account;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Email;
using PersonalAccount.Types;
using PersonalAccount.ViewModels;

namespace PersonalAccount.Controllers;

[Authorize(Roles = AccountRoleConstants.Admin)]
public class AdminCabinetController(
    IAdminCabinetService cabinetService,
    IAccountService accountService,
    IEmailSenderService emailSenderService
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await cabinetService.GetAllStudentAndTeacherAccountsAsync();
        var accountsDictionary = accounts.ToDictionary(account => account.Id);
        var studentProfiles = await cabinetService.GetAllStudentProfilesAsync();
        var teacherProfiles = await cabinetService.GetAllTeacherProfilesAsync();
        var groups = await cabinetService.GetAllGroupsAsync();
        var groupsDictionary = groups.ToDictionary(group => group.Id);

        return View(new AdminCabinetViewModel
        {
            Teachers = teacherProfiles
                .OrderBy(teacherProfile => teacherProfile.FullName)
                .Select(teacherProfile =>
                    new AdminCabinetTeacherViewModel
                    {
                        FullName = teacherProfile.FullName,
                        Email = accountsDictionary[teacherProfile.AccountId].Email,
                        PhotoUrl = teacherProfile.PhotoUrl?.ToString()
                    }).ToList(),
            Students = studentProfiles.Select(studentProfile => new AdminCabinetStudentViewModel
                {
                    FullName = studentProfile.FullName,
                    Email = accountsDictionary[studentProfile.AccountId].Email,
                    GroupName = groupsDictionary[studentProfile.GroupId].Name,
                    PhotoUrl = studentProfile.PhotoUrl?.ToString()
                }).OrderBy(student => student.GroupName)
                .ThenBy(student => student.FullName)
                .ToList(),
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

        var isAlreadyRegistered = await accountService.IsRegisteredAsync(model.Email);
        if (isAlreadyRegistered)
        {
            ModelState.AddModelError(string.Empty, "Email is already registered");
            return View(model);
        }

        var password = await accountService.RegisterAsync(model.Email, AccountRoles.Student);
        await cabinetService.AddStudentProfileAsync(model.Email, model.FullName);
        await emailSenderService.SendEmailAsync(model.ContactEmail, "Данные для входа в личный кабинет",
            $"""
             <head></head>
             <body>
             <p>{model.Email}</p>
             <p>{password}</p>
             </body>
             """);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddTeacher()
    {
        return View(new AddTeacherViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddTeacher(AddTeacherViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var isAlreadyRegistered = await accountService.IsRegisteredAsync(model.Email);
        if (isAlreadyRegistered)
        {
            ModelState.AddModelError(string.Empty, "Email is already registered");
            return View(model);
        }

        var password = await accountService.RegisterAsync(model.Email, AccountRoles.Teacher);
        await cabinetService.AddTeacherProfileAsync(model.Email, model.FullName);
        await emailSenderService.SendEmailAsync(model.ContactEmail, "Данные для входа в личный кабинет",
            $"""
             <head></head>
             <body>
             <p>{model.Email}</p>
             <p>{password}</p>
             </body>
             """);

        return RedirectToAction("Index");
    }
}