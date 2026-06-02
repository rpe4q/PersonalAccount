using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.ViewModels;

public class AddStudentViewModel
{
    [Required(ErrorMessage = "Name is required")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "GroupName is required")]
    public string GroupName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "ContactEmail is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string ContactEmail { get; set; } = string.Empty;
}