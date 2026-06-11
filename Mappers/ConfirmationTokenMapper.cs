using PersonalAccount.Data.Entities;
using PersonalAccount.Models;

namespace PersonalAccount.Mappers;

public class ConfirmationTokenMapper : Mapper<ConfirmationTokenEntity, ConfirmationTokenModel>
{
    public override ConfirmationTokenEntity ToEntity(ConfirmationTokenModel model)
    {
        var entity = base.ToEntity(model);
        entity.AccountId = model.AccountId;
        entity.TokenHash = model.TokenHash;
        entity.ExpiresAt = model.ExpiresAt;
        entity.ConfirmedAt = model.ConfirmedAt;
        return entity;
    }

    public override ConfirmationTokenModel ToModel(ConfirmationTokenEntity entity)
    {
        var model = base.ToModel(entity);
        model.AccountId = entity.AccountId;
        model.TokenHash = entity.TokenHash;
        model.ExpiresAt = entity.ExpiresAt;
        model.ConfirmedAt = entity.ConfirmedAt;
        return model;
    }
}