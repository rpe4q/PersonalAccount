namespace PersonalAccount.ViewModels;

public abstract class CabinetDisciplineViewModel : ViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}