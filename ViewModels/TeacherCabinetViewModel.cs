namespace PersonalAccount.ViewModels;

public class TeacherCabinetGroupViewModel : CabinetGroupViewModel;

public class TeacherCabinetDisciplineViewModel : ViewModel
{
    public string Name { get; set; } = string.Empty;
}

public class TeacherCabinetViewModel : CabinetViewModel
{
    public List<int> DisciplineIdsOrder { get; set; } = [];
    public Dictionary<int, TeacherCabinetDisciplineViewModel> Disciplines { get; set; } = [];
    public Dictionary<int, List<TeacherCabinetGroupViewModel>> Groups { get; set; } = [];
}