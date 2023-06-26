using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Organiser.Cores.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace Organiser.Cores
{
    public class DataContext : IdentityDbContext<Users>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<TestDB> TestDBs => Set<TestDB>();
        public DbSet<Categories> Categories => Set<Categories>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            );
            builder.Entity<Categories>().Property(f => f.CID).ValueGeneratedOnAdd();
        }
    }
}
