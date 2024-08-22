using Microsoft.EntityFrameworkCore;

class VinylDb : DbContext
{
  public VinylDb(DbContextOptions<VinylDb> options)
      : base(options) { }

  public DbSet<Vinyl> Vinyls => Set<Vinyl>();
}
