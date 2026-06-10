using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalAccount;

public class AddDisciplineViewModel
{
    [Required(ErrorMessage = "Введите название дисциплины")]
    public string Name { get; set; } = string.Empty;
}
