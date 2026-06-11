namespace PersonalAccount.ViewModels;

public abstract class CabinetGroupViewModel : ViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}