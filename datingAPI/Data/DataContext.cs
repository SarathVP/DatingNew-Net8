
using datingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace datingAPI.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}