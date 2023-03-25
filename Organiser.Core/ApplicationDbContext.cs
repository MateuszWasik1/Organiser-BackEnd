using Microsoft.EntityFrameworkCore;
using Organiser.Core.Entities;

namespace Organiser.Core
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<TestDB> TestDBs => Set<TestDB>();
    }
}
