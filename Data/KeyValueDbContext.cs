using Assignment_1_DotNet.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1_DotNet.Data
{
    public class KeyValueDbContext : DbContext
    {
        public KeyValueDbContext(DbContextOptions<KeyValueDbContext> options) : base(options) { }

        public DbSet<KeyValue> KeyValuePairs { get; set; }
    }
}
