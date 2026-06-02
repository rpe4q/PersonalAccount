using PersonalAccount.Data.Entities;
using PersonalAccount.Models.Students;
using PersonalAccount.Utils;

namespace PersonalAccount.Repositories.Mappers;

public class StudentMapper : IMapper<StudentEntity, StudentModel>
{
    public StudentEntity? ToEntity(StudentModel? model) =>
        model is null
            ? null
            : new StudentEntity
            {
                Id = model.Id,
                Email = model.Email,
                GroupName = model.GroupName,
                FullName = model.FullName,
                PhotoUrl = model.PhotoUrl?.ToString()
            };

    public StudentModel? ToModel(StudentEntity? entity) =>
        entity is null
            ? null
            : new StudentModel
            {
                Id = entity.Id,
                Email = entity.Email,
                GroupName = entity.GroupName,
                FullName = entity.FullName,
                PhotoUrl = entity.PhotoUrl?.ToUri()
            };
}