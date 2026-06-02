using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Models;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Services.Email;
using PersonalAccount.Utils;

namespace PersonalAccount.Controllers;

public class EmailConfirmationController(IConfirmationTokenService confirmation, IEmailSender emailSender) : Controller
{
    [HttpGet]
    public IActionResult Index(int studentId, string token) => View(new ConfirmEmailViewModel
    {
        StudentId = studentId,
        Token = token
    });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
    {
        var confirmed = await confirmation.ValidateTokenAsync(model.StudentId, model.Token);
        if (!confirmed)
            return RedirectToAction("Error", "Home");
        return RedirectToAction("Index", "Cabinet");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendEmailConfirmation()
    {
        var studentId = User.GetId();
        var studentEmail = User.GetEmail();
        if (studentId == null) return RedirectToAction("Error", "Home");
        var token = confirmation.GenerateTokenAsync(studentId.Value);
        var confirmationUrl = Url.Action("Index", "EmailConfirmation", new
        {
            studentId, token
        }, Request.Scheme);

        await emailSender.SendEmailAsync(studentEmail!, "Подтверждение почты", $"""
                                                                               <head></head>
                                                                               <body>
                                                                               <p>{confirmationUrl}</p>
                                                                               </body>
                                                                               """);
        return RedirectToAction("Index", "Cabinet");
    }
}