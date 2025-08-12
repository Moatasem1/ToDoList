using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class TaskConfigurations : IEntityTypeConfiguration<Domain.Entities.Task.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task.Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.Name).IsUnique();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(t=>t.UserAssigments)
                .WithOne()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(f => f.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}