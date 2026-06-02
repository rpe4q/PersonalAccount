using PersonalAccount.Data.Entities;
using PersonalAccount.Models.Students;

namespace PersonalAccount.Repositories.Mappers;

public class StudentModelMapper : IMapper<StudentEntity, StudentModel>
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
                PhotoUrl = model.PhotoUrl?.ToString(),
                PasswordHash = string.Empty
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
                PhotoUrl = entity.PhotoUrl is null ? null : new Uri(entity.PhotoUrl)
            };
}