using Microsoft.EntityFrameworkCore;
namespace CA_2.Models;

public class EftContext(DbContextOptions<EftContext> options) : DbContext(options)
{
    public DbSet<EftItem> EftItems { get; set; } = null!;
}