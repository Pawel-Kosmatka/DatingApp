
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.EntityConfigurations
{
    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(k => k.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(k => k.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}