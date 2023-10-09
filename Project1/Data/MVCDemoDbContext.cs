using Microsoft.EntityFrameworkCore;
using Project1.Models.Domain;

namespace Project1.Data
{
    public class MVCDemoDbContext : DbContext
    {
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }

    }
}