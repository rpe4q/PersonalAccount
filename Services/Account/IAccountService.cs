using PersonalAccount.Models;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Account
{
    public interface IAccountService
    {
        Task<AccountModel?> ValidateCredentialsAsync(string email, string password);
        Task SignInAsync(HttpContext ctx, AccountModel account);
        Task SignOutAsync(HttpContext ctx);
        Task<string> RegisterAsync(string email, AccountRoles role);
    }
}