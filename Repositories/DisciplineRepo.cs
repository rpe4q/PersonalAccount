using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class DisciplineRepo(AppDbContext context, IMapper<DisciplineEntity, DisciplineModel> mapper)
    : Repo<DisciplineEntity, DisciplineModel>(context, mapper, ctx => ctx.Disciplines),
        IDisciplineRepo;