using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class TaskConfigurations : IEntityTypeConfiguration<Domain.Entities.Task.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task.Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany<Domain.Entities.User.UserTaskAssignment>()
                .WithOne()
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}