using DatingApp.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.API.Persistence.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Username)
                .HasMaxLength(255)
                .IsRequired();
            
            builder.Property(x => x.PasswordHash)
                .IsRequired();
            
            builder.Property(x => x.PasswordSalt)
                .IsRequired();
        }
    }
}