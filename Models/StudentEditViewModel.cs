using System.ComponentModel.DataAnnotations;

public class StudentEditViewModel
{
    [Required(ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Group name is required")]
    public string GroupName { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
}