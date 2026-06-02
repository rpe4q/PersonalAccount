using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;
using PersonalAccount.Types;

namespace PersonalAccount.Repositories;

public class AccountRepo(AppDbContext context, IMapper<AccountEntity, AccountModel> mapper) : IAccountRepo
{
    private DbSet<AccountEntity> Accounts => context.Accounts;

    public async Task<AccountModel?> GetByEmailAsync(string email)
    {
        var entity = await Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Email == email);
        return entity == null ? null : mapper.ToModel(entity);
    }

    public async Task<List<AccountModel>> GetAllByRoleAsync(AccountRoles role)
    {
        return await Accounts
            .AsNoTracking()
            .Where(entity => entity.Role == role)
            .Select(entity => mapper.ToModel(entity))
            .ToListAsync();
    }

    public async Task AddAsync(AccountModel account)
    {
        await Accounts.AddAsync(mapper.ToEntity(account));
        await context.SaveChangesAsync();
    }
}