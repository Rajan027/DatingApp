using DatingApp.API.Data.EntityConfigurations;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){ }

        public DbSet<Value> Values{get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Add entity configurations here
            modelBuilder.ApplyConfiguration(new ValueConfiguration());
        }
    }
}