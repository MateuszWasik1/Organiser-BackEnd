using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Organiser.Cores.Entities;

namespace Organiser.Cores
{
    public class DataContext : IdentityDbContext<Users> //można zamienić po prostu na DBCONTEXT ???
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> User => Set<User>();
        public DbSet<Roles> AppRoles => Set<Roles>();
        public DbSet<Categories> Categories => Set<Categories>();
        public DbSet<Tasks> Tasks => Set<Tasks>();
        public DbSet<TasksNotes> TasksNotes => Set<TasksNotes>();
        public DbSet<Savings> Savings => Set<Savings>();
        public DbSet<Bugs> Bugs => Set<Bugs>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "User",
            //        NormalizedName = "USER"
            //    },
            //    new IdentityRole
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "Administrator",
            //        NormalizedName = "ADMINISTRATOR"
            //    }
            //);
            builder.Entity<Categories>().Property(f => f.CID).ValueGeneratedOnAdd();
        }
    }
}
