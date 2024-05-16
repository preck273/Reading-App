using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace bookAppApi.Entities
{
    public class databaseContext : DbContext
    {
        public databaseContext(DbContextOptions<databaseContext> options)
            : base(options)
        {
        }

        public DbSet<Users> User { get; set; }

    }
}
