namespace PersonalAccount.Data.Entities;

public class TeacherGroupDisciplineEntity : Entity
{
    public int GroupId { get; set; }
    public GroupEntity? Group { get; set; }
    
    public int DisciplineId { get; set; }
    public DisciplineEntity? Discipline { get; set; }
    
    public int TeacherAccountId { get; set; }
    public AccountEntity? TeacherAccount { get; set; }
}