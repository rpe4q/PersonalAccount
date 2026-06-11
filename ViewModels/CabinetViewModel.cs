namespace PersonalAccount.ViewModels;

public abstract class CabinetViewModel : ViewModel
{
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
}