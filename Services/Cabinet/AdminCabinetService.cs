using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Services.Confirmation;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Cabinet;

public class AdminCabinetService(
    IAccountRepo accounts,
    IStudentProfileRepo studentProfiles,
    IConfirmationTokenService confirmationService) : IAdminCabinetService
{
    public async Task<Dictionary<int, AccountModel>> GetAllStudentAccounts()
    {
        var studentAccounts = await accounts.GetAllByRoleAsync(AccountRoles.Student);
        return studentAccounts.ToDictionary(account => account.Id);
    }

    public async Task<List<StudentProfileModel>> GetAllStudentProfiles() => await studentProfiles.GetAllAsync();

    public async Task ConfirmStudentEmailAsync(int id)
    {
        var isAlreadyConfirmed = await confirmationService.HasAnyConfirmedTokenAsync(id);

        if (!isAlreadyConfirmed)
        {
            var token = await confirmationService.GenerateTokenAsync(id);
            await confirmationService.ValidateTokenAsync(id, token);
        }
    }
}