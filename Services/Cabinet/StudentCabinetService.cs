using PersonalAccount.Models;
using PersonalAccount.Repositories;

namespace PersonalAccount.Services.Cabinet;

public class StudentCabinetService(IStudentProfileRepo studentProfiles) : IStudentCabinetService
{
    public Task<StudentProfileModel?> GetByAccountIdAsync(int accountId) =>
        studentProfiles.GetByAccountIdAsync(accountId);
}