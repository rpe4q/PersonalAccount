using PersonalAccount.Models;
using PersonalAccount.Types;

namespace PersonalAccount.Repositories
{
    public interface IAccountRepo : IRepo<AccountModel>
    {
        public Task<AccountModel?> GetByEmailAsync(string email);
        public Task<List<AccountModel>> GetAllByRoleAsync(AccountRoles role);
    }
}
