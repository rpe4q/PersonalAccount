using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class ConfirmationTokenRepo(
    AppDbContext context,
    IMapper<ConfirmationTokenEntity, ConfirmationTokenModel> mapper
) : Repo<ConfirmationTokenEntity, ConfirmationTokenModel>(context, mapper, ctx => ctx.ConfirmationTokens),
    IConfirmationTokenRepo
{
    public async Task<List<ConfirmationTokenModel>> GetAllByAccountIdAsync(int accountId) =>
        await GetAllByAsync(entity => entity.AccountId == accountId);

    public async Task ConfirmByIdAsync(int id, DateTime confirmedAt) =>
        await UpdateByIdAsync(id, entity => entity.ConfirmedAt = confirmedAt);
}