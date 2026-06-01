using PersonalAccount.Models;

namespace PersonalAccount.Services.Cabinet;

public interface ITeacherCabinetService
{
    Task<List<DisciplineModel>> GetAllDisciplinesAsync(int teacherAccountId);

    Task<Dictionary<int, List<GroupModel>>> GetAllGroupsByDisciplinesAsync(
        int teacherAccountId,
        List<int> disciplineIds
    );
}