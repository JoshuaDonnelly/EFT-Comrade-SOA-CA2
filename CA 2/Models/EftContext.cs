using Microsoft.EntityFrameworkCore;
using CA_2.Models;
namespace CA_2.Models;

public class EftContext(DbContextOptions<EftContext> options) : DbContext(options)
{
    public DbSet<EftItem> EftItems { get; set; } = null!;

public DbSet<EftItemTasks> EftItemTasks { get; set; } = null!;
}