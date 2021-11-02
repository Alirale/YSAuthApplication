using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public static class ConfigureDb
    {
        public static void ConfigureTables(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(p => p.tokens).WithOne(p => p.User);
        }
    }
}
