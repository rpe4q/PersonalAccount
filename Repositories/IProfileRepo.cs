using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public interface IProfileRepo<TProfileModel> : IRepo<TProfileModel>
    where TProfileModel : ProfileModel, new()
{
    Task<TProfileModel?> GetByAccountIdAsync(int accountId);
}