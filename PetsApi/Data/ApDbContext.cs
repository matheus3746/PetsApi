using Microsoft.EntityFrameworkCore;
using PetsApi.Model;

namespace PetsApi.Data;

public class ApDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Animal> Pets { get; set; } = null!;
    }
}
