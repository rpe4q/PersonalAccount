using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.Models;

public class ConfirmEmailViewModel
{
    [Required] public int StudentId { get; set; }
    [Required] public string Token { get; set; } = string.Empty;
}