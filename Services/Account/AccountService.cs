using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Account
{
    public class AccountService(IAccountRepo accountRepo, IPasswordHasher<AccountModel> hasher)
        : IAccountService
    {
        public async Task<AccountModel?> ValidateCredentialsAsync(string email, string password)
        {
            var account = await accountRepo.GetByEmailAsync(email);
            if (account is null) return null;

            var result = hasher.VerifyHashedPassword(account, account.PasswordHash, password);
            return result == PasswordVerificationResult.Failed ? null : account;
        }

        public async Task SignInAsync(HttpContext ctx, AccountModel account)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new(ClaimTypes.Email, account.Email),
                new(ClaimTypes.Role, account.Role.ToString()),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOutAsync(HttpContext ctx) =>
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        public async Task<string> RegisterAsync(string email, AccountRoles role)
        {
            var account = new AccountModel
            {
                Email = email,
                Role = role,
            };
            var password = Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
            account.PasswordHash = hasher.HashPassword(account, password);
            await accountRepo.AddAsync(account);

            return password;
        }

        public async Task<bool> IsRegisteredAsync(string email)
        {
            var account = await accountRepo.GetByEmailAsync(email);
            return account != null;
        }
    }
}