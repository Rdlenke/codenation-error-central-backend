using ErrorCentral.Domain.AggregatesModel.LogError;
using ErrorCentral.Domain.AggregatesModel.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ErrorCentral.Infrastructure.Mappings
{
    public class LogErrorMap : IEntityTypeConfiguration<LogError>
    {
        public void Configure(EntityTypeBuilder<LogError> builder)
        {
            builder.ToTable("log_errors", schema: "dbo");
            builder.HasKey(p => p.Id).HasName("id");
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Title).HasColumnName("title").HasMaxLength(500).IsRequired();
            builder.Property(t => t.Details).HasColumnName("details").HasMaxLength(2000);
            builder.Property(t => t.Source).HasColumnName("source").HasMaxLength(300).IsRequired();
            builder.Property(p => p.Filed).HasColumnName("filed").HasDefaultValue(false).IsRequired();
            builder.Property(p => p.Level).HasColumnName("e_level").IsRequired();
            builder.Property(p => p.Environment).HasColumnName("e_environment").IsRequired();

            builder.Property(p => p.UserId).HasColumnName("id_user").IsRequired();

            builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();

            builder.Property(p => p.Removed).HasColumnName("removed");
            builder.HasQueryFilter(p => !p.Removed);

            builder.HasOne<User>()
                .WithMany(u => u.LogErrors)
                .IsRequired(false)
                .HasForeignKey("id_user");
        }
    }
}
