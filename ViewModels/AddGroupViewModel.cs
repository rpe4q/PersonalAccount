using System.ComponentModel.DataAnnotations;

namespace PersonalAccount;

public class AddGroupViewModel
{
    [Required(ErrorMessage = "Введите название группы")]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}
