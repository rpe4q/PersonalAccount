using PersonalAccount.Constants;
using PersonalAccount.Data.Entities;
using PersonalAccount.Models;
using PersonalAccount.Utils;

namespace PersonalAccount.Mappers;

public class StudentProfileMapper : ProfileMapper<StudentProfileEntity, StudentProfileModel>
{
    public override StudentProfileEntity ToEntity(StudentProfileModel model)
    {
        var entity = base.ToEntity(model);
        entity.GroupId = model.GroupId == GroupConstants.NoGroupId ? null : model.GroupId;
        return entity;
    }

    public override StudentProfileModel ToModel(StudentProfileEntity entity)
    {
        var model = base.ToModel(entity);
        model.GroupId = entity.GroupId ?? GroupConstants.NoGroupId;
        return model;
    }
}