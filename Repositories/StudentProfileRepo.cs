using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public class StudentProfileRepo(AppDbContext context, IMapper<StudentProfileEntity, StudentProfileModel> mapper)
    : ProfileRepo<StudentProfileEntity, StudentProfileModel>(context, mapper, ctx => ctx.StudentProfiles),
        IStudentProfileRepo
{
    public async Task UpdateGroupByAccountIdAsync(int accountId, int groupId)
    {
        var entity = await Table.FirstOrDefaultAsync(e => e.AccountId == accountId)
            ?? throw new KeyNotFoundException();

        entity.GroupId = groupId;
        await Context.SaveChangesAsync();
    }
}