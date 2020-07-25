using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorCentral.Infrastructure.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", schema: "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();


            builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(500).IsRequired();
            builder.Property(p => p.Password).HasColumnName("password").IsRequired();
            builder.Property(p => p.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();


            builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();

            builder.HasMany<LogError>(p => p.LogErrors)
                .WithOne(p => p.User);

            builder.Property(p => p.Removed).HasColumnName("removed").HasDefaultValue(false).IsRequired();
            builder.HasQueryFilter(p => !p.Removed);
        }
    }
}
