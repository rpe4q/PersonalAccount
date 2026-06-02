using PersonalAccount.Data.Entities;
using PersonalAccount.Models;
using PersonalAccount.Utils;

namespace PersonalAccount.Mappers;

public class StudentProfileMapper : IMapper<StudentProfileEntity, StudentProfileModel>
{
    public StudentProfileEntity ToEntity(StudentProfileModel model) =>
        new()
        {
            Id = model.Id,
            AccountId = model.AccountId,
            GroupName = model.GroupName,
            FullName = model.FullName,
            PhotoUrl = model.PhotoUrl?.ToString()
        };

    public StudentProfileModel ToModel(StudentProfileEntity entity) =>
        new()
        {
            Id = entity.Id,
            AccountId = entity.AccountId,
            GroupName = entity.GroupName,
            FullName = entity.FullName,
            PhotoUrl = entity.PhotoUrl?.ToUri()
        };
}