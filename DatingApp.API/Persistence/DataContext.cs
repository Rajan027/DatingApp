using DatingApp.API.Core.Models;
using DatingApp.API.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add entity configurations here
            modelBuilder.ApplyConfiguration(new ValueConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}