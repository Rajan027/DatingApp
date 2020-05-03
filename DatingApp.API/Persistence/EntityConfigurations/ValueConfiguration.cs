using DatingApp.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatingApp.API.Persistence.EntityConfigurations
{
    public class ValueConfiguration : IEntityTypeConfiguration<Value>
    {
        public void Configure(EntityTypeBuilder<Value> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(255);
        }
    }
}