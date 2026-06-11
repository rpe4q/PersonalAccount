using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public interface IConfirmationTokenRepo : IRepo<ConfirmationTokenModel>
{
    Task<List<ConfirmationTokenModel>> GetAllByAccountIdAsync(int accountId);
    Task ConfirmByIdAsync(int id, DateTime confirmedAt);
}