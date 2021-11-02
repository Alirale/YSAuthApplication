using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Token> tokens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureTables();
        }
    }
}
