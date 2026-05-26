using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Utils;

namespace PersonalAccount.Controllers;

[Authorize]
public class CabinetController(IStudentService students) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var studentId = User.GetId();
        if (studentId == null) return RedirectToAction("Error", "Home");

        var student = await students.GetStudentByIdAsync(studentId.Value);
        if (student == null) return RedirectToAction("Error", "Home");

        return View(student);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var studentId = User.GetId();
        if (studentId == null) return RedirectToAction("Error", "Home");

        var student = await students.GetStudentByIdAsync(studentId.Value);
        if (student == null) return RedirectToAction("Error", "Home");

        return View(new StudentEditViewModel
        {
            FullName = student.FullName,
            GroupName = student.GroupName,
            PhotoUrl = student.PhotoUrl?.ToString()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentEditViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (model == null) return RedirectToAction("Error", "Home");

        var studentId = User.GetId();
        if (studentId == null) return RedirectToAction("Error", "Home");

        var success = await students.UpdateStudentAsync(studentId.Value, model);
        if (!success) return RedirectToAction("Error", "Home");

        return RedirectToAction("Index");
    }
}