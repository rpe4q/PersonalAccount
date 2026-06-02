using System.ComponentModel.DataAnnotations;

namespace PersonalAccount.ViewModels;

public class ConfirmEmailViewModel
{
    [Required] public int AccountId { get; set; }
    [Required] public string Token { get; set; } = string.Empty;
}