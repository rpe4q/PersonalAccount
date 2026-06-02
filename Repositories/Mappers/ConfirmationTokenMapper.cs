using PersonalAccount.Data.Entities;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories.Mappers;

public class ConfirmationTokenMapper : IMapper<ConfirmationTokenEntity, ConfirmationTokenModel>
{
    public ConfirmationTokenEntity? ToEntity(ConfirmationTokenModel? model) =>
        model is null
            ? null
            : new ConfirmationTokenEntity
            {
                Id = model.Id,
                StudentId = model.StudentId,
                TokenHash = model.TokenHash,
                ExpiresAt = model.ExpiresAt,
                ConfirmedAt = model.ConfirmedAt,
            };

    public ConfirmationTokenModel? ToModel(ConfirmationTokenEntity? entity) =>
        entity is null
            ? null
            : new ConfirmationTokenModel
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                TokenHash = entity.TokenHash,
                ExpiresAt = entity.ExpiresAt,
                ConfirmedAt = entity.ConfirmedAt,
            };
}