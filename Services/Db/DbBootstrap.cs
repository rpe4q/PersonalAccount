using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;
using PersonalAccount.Utils;

namespace PersonalAccount.Services.Db;

public class DbBootstrap(
    AppDbContext context,
    IPasswordHasher<AccountModel> hasher,
    IMapper<AccountEntity, AccountModel> accountMapper,
    IMapper<StudentProfileEntity, StudentProfileModel> studentProfileMapper,
    IOptions<DbBootstrapSettings> options)
{
    private readonly DbBootstrapSettings _settings = options.Value;

    public async Task SeedAsync()
    {
        await context.Database.MigrateAsync();
        var hasStudents = await context.StudentProfiles.AnyAsync();
        if (hasStudents) return;

        var account = new AccountModel
        {
            Email = _settings.Email
        };

        var accountEntity = accountMapper.ToEntity(account);
        accountEntity.PasswordHash = hasher.HashPassword(account, _settings.Password);

        await context.Accounts.AddAsync(accountEntity);
        await context.SaveChangesAsync();

        accountEntity = await context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Email == account.Email) ?? throw new InvalidOperationException();

        var studentProfile = new StudentProfileModel
        {
            AccountId = accountEntity.Id,
            FullName = "John Doe",
            GroupName = "PD-412",
            PhotoUrl = "https://masterpiecer-images.s3.yandex.net/5fd531dca6427c7:upscaled".ToUri(),
        };
        var studentProfileEntity = studentProfileMapper.ToEntity(studentProfile);
        await context.StudentProfiles.AddAsync(studentProfileEntity);
        await context.SaveChangesAsync();
    }
}