using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Cabinet;

public class AdminCabinetService(
    IAccountRepo accountRepo,
    IGroupRepo groupRepo,
    IStudentProfileRepo studentProfileRepo,
    ITeacherProfileRepo teacherProfileRepo,
    ITeacherGroupDisciplineRepo teacherGroupDisciplineRepo,
    IDisciplineRepo disciplineRepo
) : IAdminCabinetService
{
    public async Task<List<AccountModel>> GetAllStudentAndTeacherAccountsAsync() =>
        await accountRepo.GetAllByRoleAsync(AccountRoles.Student | AccountRoles.Teacher);

    public async Task<List<GroupModel>> GetAllGroupsAsync() => await groupRepo.GetAllAsync();

    public async Task<List<StudentProfileModel>> GetAllStudentProfilesAsync() =>
        await studentProfileRepo.GetAllAsync();
    public async Task<List<TeacherProfileModel>> GetAllTeacherProfilesAsync() =>
        await teacherProfileRepo.GetAllAsync();

    public async Task AddStudentProfileAsync(string email, string fullName) =>
        await AddProfileAsync(studentProfileRepo, email, fullName);

    public async Task AddTeacherProfileAsync(string email, string fullName) =>
        await AddProfileAsync(teacherProfileRepo, email, fullName);

    public async Task AddTeacherGroupDisciplineAsync(int teacherAccountId, int groupId, int disciplineId) =>
        await teacherGroupDisciplineRepo.AddAsync(new TeacherGroupDisciplineModel
        {
            DisciplineId = disciplineId,
            GroupId = groupId,
            TeacherAccountId = teacherAccountId
        });

    private async Task AddProfileAsync<TProfileModel>(
        IProfileRepo<TProfileModel> profileRepo,
        string email,
        string fullName
    ) where TProfileModel : ProfileModel, new()
    {
        var account = await accountRepo.GetByEmailAsync(email);
        if (account == null) return;

        await profileRepo.AddAsync(new TProfileModel
        {
            FullName = fullName,
            AccountId = account.Id
        });
    }

    public Task AddGroupAsync(string name, string description, string? imageUrl) =>
        groupRepo.AddAsync(new GroupModel
        {
            Name = name,
            Description = description,
            ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : new Uri(imageUrl)
        });

    public Task AddDisciplineAsync(string name) =>
        disciplineRepo.AddAsync(new DisciplineModel{Name = name});

    public Task<List<DisciplineModel>> GetAllDisciplinesAsync() => disciplineRepo.GetAllAsync();

    public Task ChangeStudentGroupAsync(int studentAccountId, int groupId) =>
        studentProfileRepo.UpdateGroupByAccountIdAsync(studentAccountId, groupId);

    public Task DeleteGroupAsync(int groupId) => groupRepo.DeleteByIdAsync(groupId);

    public Task DeleteDisciplineAsync(int disciplineId) =>
        disciplineRepo.DeleteByIdAsync(disciplineId);

    public Task DeleteStudentAsync(int accountId) =>
        studentProfileRepo.DeleteByIdAsync(accountId);

    public Task DeleteTeacherAsync(int accountId) =>
        teacherProfileRepo.DeleteByIdAsync(accountId);
}