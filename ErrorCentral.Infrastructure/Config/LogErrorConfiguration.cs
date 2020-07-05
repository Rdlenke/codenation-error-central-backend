using ErrorCentral.Domain.AggregatesModel.LogErrorAggregate;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorCentral.Infrastructure.Config
{
    public class LogErrorConfiguration : IEntityTypeConfiguration<LogError>
    {
        public void Configure(EntityTypeBuilder<LogError> builder)
        {
            builder.ToTable("log_errors", schema: "dbo");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id").ValueGeneratedOnAdd();

            builder.Property(t => t.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
            builder.Property(t => t.Details).HasColumnName("details").HasMaxLength(2000);
            builder.Property(t => t.Source).HasColumnName("source").HasMaxLength(300).IsRequired();
            builder.Property(p => p.Filed).HasColumnName("filed").HasDefaultValue(false).IsRequired();
            builder.Property(p => p.Level).HasColumnName("e_level").IsRequired();
            builder.Property(p => p.Environment).HasColumnName("e_environment").IsRequired();

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();

            builder.Property(p => p.Removed).HasColumnName("removed").HasDefaultValue(false).IsRequired();
            builder.HasQueryFilter(p => !p.Removed);

            builder.Property(p => p.UserId).HasColumnName("id_user").IsRequired();
            builder.HasOne<User>(p => p.User)
                .WithMany(p => p.LogErrors)
                .HasForeignKey(p => p.UserId);
        }
    }
}
