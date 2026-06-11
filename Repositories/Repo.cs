using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public abstract class Repo<TEntity, TModel>(
    AppDbContext context,
    IMapper<TEntity, TModel> mapper,
    Func<AppDbContext, DbSet<TEntity>> tableSelector)
    : IRepo<TModel>
    where TModel : Model, new()
    where TEntity : Entity, new()
{
    protected AppDbContext Context { get; } = context;
    protected IMapper<TEntity, TModel> Mapper { get; } = mapper;

    protected DbSet<TEntity> Table => tableSelector(Context);

    public async Task AddAsync(TModel model)
    {
        await Table.AddAsync(Mapper.ToEntity(model));
        await Context.SaveChangesAsync();
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        var entity = await Table.FindAsync(id);
        return entity == null ? null : Mapper.ToModel(entity);
    }

    public async Task<List<TModel>> GetAllAsync()
    {
        return await Table.AsNoTracking()
            .Select(entity => Mapper.ToModel(entity))
            .ToListAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await Table.FindAsync(id) ?? throw new KeyNotFoundException();
        Table.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> AnyAsync() => await Table.AnyAsync();

    protected async Task<TModel?> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await Table
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);

        return entity == null ? null : Mapper.ToModel(entity);
    }

    protected async Task<List<TModel>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate) =>
        await Table
            .AsNoTracking()
            .Where(predicate)
            .Select(entity => Mapper.ToModel(entity))
            .ToListAsync();

    protected async Task UpdateByIdAsync(int id, Action<TEntity> updateAction)
    {
        var entity = await Table.FindAsync(id) ?? throw new KeyNotFoundException();
        updateAction(entity);
        await Context.SaveChangesAsync();
    }
}