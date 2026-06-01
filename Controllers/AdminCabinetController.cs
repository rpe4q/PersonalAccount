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
        var accounts = (await cabinetService.GetAllStudentAccountsAsync())
            .ToDictionary(account => account.Id);
        var profiles = await cabinetService.GetAllStudentProfilesAsync();
        var groups = await cabinetService.GetAllGroupsAsync();
        var groupsDictionary = groups.ToDictionary(group => group.Id);

        return View(new AdminCabinetViewModel
        {
            // GroupIdsOrder = groups.OrderBy(group => group.Name).Select(group => group.Id).ToList(),
            // Groups = groupsDictionary.Select(group => (group.Key, new AdminCabinetGroupViewModel
            // {
            //     Name = group.Value.Name,
            //     ImageUrl = group.Value.ImageUrl?.ToString(),
            //     Description = group.Value.Description
            // })).ToDictionary(),
            // Students = profiles.GroupBy(profile => profile.GroupId)
            //     .ToDictionary(group => group.Key, group =>
            //         group.Select(profile => new AdminCabinetStudentViewModel
            //         {
            //             Email = accounts[profile.AccountId].Email,
            //             FullName = profile.FullName,
            //             PhotoUrl = profile.PhotoUrl?.ToString()
            //         }).ToList())
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
}