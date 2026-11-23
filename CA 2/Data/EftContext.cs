using Microsoft.EntityFrameworkCore;
using CA_2.Models;
namespace CA_2.Models;

public class EftContext(DbContextOptions<EftContext> options) : DbContext(options)
{
    public DbSet<Quest> EftItems { get; set; } = null!;

public DbSet<QuestObjective> EftItemTasks { get; set; } = null!;
}