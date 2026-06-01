using PersonalAccount.Models;

namespace PersonalAccount.Services.Cabinet;

public interface IAdminCabinetService
{
    Task<List<AccountModel>> GetAllStudentAccountsAsync();
    Task<List<GroupModel>> GetAllGroupsAsync();
    Task<List<StudentProfileModel>> GetAllStudentProfilesAsync();
    Task AddStudentProfileAsync(string email, string fullName);
    Task AddTeacherProfileAsync(string email, string fullName);
}