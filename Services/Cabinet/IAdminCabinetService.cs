using PersonalAccount.Models;

namespace PersonalAccount.Services.Cabinet;

public interface IAdminCabinetService
{
    Task<Dictionary<int, AccountModel>> GetAllStudentAccounts();
    Task<List<StudentProfileModel>> GetAllStudentProfiles();
    Task ConfirmStudentEmailAsync(int id);
}