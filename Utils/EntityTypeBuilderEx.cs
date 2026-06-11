using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalAccount.Data.Entities;

namespace PersonalAccount.Utils;

public static class EntityTypeBuilderEx
{
    public static void BuildEntity<TEntity>(this EntityTypeBuilder<TEntity> entity, string tableName)
        where TEntity : Entity
    {
        entity.ToTable(tableName);
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
    }

    public static void BuildProfileEntity<TProfileEntity>(
        this EntityTypeBuilder<TProfileEntity> entity,
        string tableName,
        Expression<Func<AccountEntity, TProfileEntity?>> profileSelector
    ) where TProfileEntity : ProfileEntity
    {
        entity.BuildEntity(tableName);
        entity.HasIndex(e => e.AccountId).IsUnique();

        entity.Property(e => e.AccountId)
            .HasColumnName("account_id")
            .IsRequired();

        entity.Property(e => e.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(255)
            .IsRequired();

        entity.Property(e => e.PhotoUrl)
            .HasColumnName("photo_url");

        entity.HasOne(e => e.Account)
            .WithOne(profileSelector)
            .HasForeignKey<TProfileEntity>(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}