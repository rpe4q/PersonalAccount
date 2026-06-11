using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Models;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Services.Email;
using PersonalAccount.Utils;
using PersonalAccount.ViewModels;

namespace PersonalAccount.Controllers;

public class EmailConfirmationController(
    IConfirmationTokenService confirmationTokenService,
    IEmailSenderService emailSenderService
) : Controller
{
    [HttpGet]
    public IActionResult Index(int accountId, string token) => View(new ConfirmEmailViewModel
    {
        AccountId = accountId,
        Token = token
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
    {
        var confirmed = await confirmationTokenService.ValidateTokenAsync(model.AccountId, model.Token);
        if (!confirmed)
            return RedirectToAction("Error", "Home");
        return RedirectToAction("Index", "Cabinet");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendEmailConfirmation()
    {
        var accountId = User.GetId();
        var accountEmail = User.GetEmail();
        if (accountId == null || accountEmail == null) return RedirectToAction("Error", "Home");
        var token = await confirmationTokenService.GenerateTokenAsync(accountId.Value);
        var confirmationUrl = Url.Action("Index", "EmailConfirmation", new
        {
            accountId, token
        }, Request.Scheme);

        await emailSenderService.SendEmailAsync(accountEmail, "Подтверждение почты", $"""
             <head></head>
             <body>
             <p>{confirmationUrl}</p>
             </body>
             """);
        return RedirectToAction("Index", "Cabinet");
    }
}