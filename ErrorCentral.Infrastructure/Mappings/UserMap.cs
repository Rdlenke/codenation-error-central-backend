using ErrorCentral.Domain.AggregatesModel.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorCentral.Infrastructure.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", schema: "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();

            builder.Property(p => p.Removed).HasColumnName("removed").HasDefaultValue(false);
            builder.HasQueryFilter(p => !p.Removed);
        }
    }
}
