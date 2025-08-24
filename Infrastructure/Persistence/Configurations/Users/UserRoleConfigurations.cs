using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Users;

public class UserRoleConfigurations : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => new {x.UserId,x.RoleId});

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}
