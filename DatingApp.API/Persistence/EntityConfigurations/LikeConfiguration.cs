using DatingApp.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.API.Persistence.EntityConfigurations
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(p => new{p.LikerId, p.LikeeId});

            builder.HasOne(x => x.Likee)
            .WithMany(x => x.Likers)
            .HasForeignKey(x => x.LikeeId)
            .OnDelete(DeleteBehavior.Restrict);

             builder.HasOne(x => x.Liker)
            .WithMany(x => x.Likees)
            .HasForeignKey(x => x.LikerId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}