using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class TaskStatusConfigurations : IEntityTypeConfiguration<Domain.Entities.Task.TaskStatus>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Task.TaskStatus> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            Enumeration.GetAll<Domain.Entities.Task.TaskStatus>()
            .ToList()
            );
    }
}