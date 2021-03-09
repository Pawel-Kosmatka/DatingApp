using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.EntityConfigurations
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder
                .HasOne(u => u.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder
                .HasOne(r => r.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(r => r.RoleId)
                .IsRequired();
        }
    }
}