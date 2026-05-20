using ItAssetPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ItAssetPortal.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Asset> Assets => Set<Asset>();
}
