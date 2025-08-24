using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Users;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
       builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(245);

        builder.HasMany<UserTaskAssignment>()
            .WithOne()
            .HasForeignKey(x => x.UserId);

        builder.HasMany<Domain.Entities.Task.Task>()
            .WithOne()
            .HasForeignKey(x=>x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(x => x.UserId);
    }
}
