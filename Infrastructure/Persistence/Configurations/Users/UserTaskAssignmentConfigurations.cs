using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Users;

public class UserTaskAssignmentConfigurations : IEntityTypeConfiguration<UserTaskAssignment>
{
    public void Configure(EntityTypeBuilder<UserTaskAssignment> builder)
    {
        builder.HasKey(x => new { x.UserId, x.TaskId});

        builder.Property(x => x.StatusId)
            .IsRequired();

        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId);
    }
}