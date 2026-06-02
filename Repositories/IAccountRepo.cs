using PersonalAccount.Models;
using PersonalAccount.Types;

namespace PersonalAccount.Repositories
{
    public interface IAccountRepo
    {
        public Task<AccountModel?> GetByEmailAsync(string email);
        public Task<List<AccountModel>> GetAllByRoleAsync(AccountRoles role);
        public Task AddAsync(AccountModel account);
    }
}
