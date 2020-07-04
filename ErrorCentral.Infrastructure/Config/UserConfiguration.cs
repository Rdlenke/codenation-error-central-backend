using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ErrorCentral.Infrastructure.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", schema: "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").HasDefaultValue(DateTimeOffset.UtcNow).IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").HasDefaultValue(DateTimeOffset.UtcNow).IsRequired();

            builder.Property(p => p.Removed).HasColumnName("removed").HasDefaultValue(false).IsRequired();
            builder.HasQueryFilter(p => !p.Removed);
        }
    }
}
