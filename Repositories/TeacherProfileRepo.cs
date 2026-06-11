using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class TeacherProfileRepo(AppDbContext context, IMapper<TeacherProfileEntity, TeacherProfileModel> mapper)
    : ProfileRepo<TeacherProfileEntity, TeacherProfileModel>(context, mapper, ctx => ctx.TeacherProfiles),
        ITeacherProfileRepo;