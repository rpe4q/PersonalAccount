using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PersonalAccount.Constants;
using PersonalAccount.Models;
using PersonalAccount.Repositories;
using PersonalAccount.Types;

namespace PersonalAccount.Services.Db;

public class DbBootstrapService(
    IAccountRepo accountRepo,
    IGroupRepo groupRepo,
    IPasswordHasher<AccountModel> hasher,
    IOptions<DbBootstrapSettings> options)
{
    private readonly DbBootstrapSettings _settings = options.Value;

    public async Task SeedAsync()
    {
        await Task.WhenAll(
            InitAccountAsync(),
            InitGroupAsync()
        );
    }

    private async Task InitAccountAsync()
    {
        var hasAccounts = await accountRepo.AnyAsync();
        if (hasAccounts) return;

        var account = new AccountModel
        {
            Email = _settings.Email,
            Role = AccountRoles.Admin
        };
        account.PasswordHash = hasher.HashPassword(account, _settings.Password);
        await accountRepo.AddAsync(account);
    }

    private async Task InitGroupAsync()
    {
        var noGroup = await groupRepo.GetByIdAsync(GroupConstants.NoGroupId);
        if (noGroup != null) return;

        var group = new GroupModel
        {
            Id = GroupConstants.NoGroupId,
        };
        await groupRepo.AddAsync(group);
    }
}