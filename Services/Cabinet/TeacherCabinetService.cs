using PersonalAccount.Models;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services.Cabinet;

public class TeacherCabinetService(
    IGroupRepo groupRepo,
    ITeacherGroupDisciplineRepo teacherGroupDisciplineRepo,
    IDisciplineRepo disciplineRepo
) : ITeacherCabinetService
{
    public async Task<List<DisciplineModel>> GetAllDisciplinesAsync(int teacherAccountId)
    {
        var disciplines = new HashSet<DisciplineModel>();
        var teacherGroupDisciplines = await teacherGroupDisciplineRepo.GetAllByTeacherAccountIdAsync(teacherAccountId);
        foreach (var teacherGroupDiscipline in teacherGroupDisciplines)
        {
            var discipline = await disciplineRepo.GetByIdAsync(teacherGroupDiscipline.DisciplineId);
            if (discipline == null) throw new KeyNotFoundException();
            disciplines.Add(discipline);
        }

        return disciplines.ToList();
    }

    public async Task<Dictionary<int, List<GroupModel>>> GetAllGroupsByDisciplinesAsync(
        int teacherAccountId,
        List<int> disciplineIds
    )
    {
        var groupsByDisciplines = new Dictionary<int, List<GroupModel>>();
        foreach (var disciplineId in disciplineIds)
        {
            var teacherGroupDisciplines = await teacherGroupDisciplineRepo
                .GetAllByTeacherAccountIdAndDisciplineIdAsync(teacherAccountId, disciplineId);
            foreach (var teacherGroupDiscipline in teacherGroupDisciplines)
            {
                var group = await groupRepo.GetByIdAsync(teacherGroupDiscipline.GroupId);
                if (group == null) throw new KeyNotFoundException();
                if (!groupsByDisciplines.ContainsKey(group.Id))
                    groupsByDisciplines[group.Id] = [];
                groupsByDisciplines[group.Id].Add(group);
            }
        }

        return groupsByDisciplines;
    }
}