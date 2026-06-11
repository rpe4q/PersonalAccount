using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class GroupRepo(AppDbContext context, IMapper<GroupEntity, GroupModel> mapper)
    : Repo<GroupEntity, GroupModel>(context, mapper, ctx => ctx.Groups), IGroupRepo;