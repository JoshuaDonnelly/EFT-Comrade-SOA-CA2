using Microsoft.EntityFrameworkCore;
using CA_2.Models;
namespace CA_2.Data;

public class EftContext(DbContextOptions<EftContext> options) : DbContext(options)
{
    public DbSet<Quest> Quests => Set<Quest>();
    
    public DbSet<QuestObjective> QuestObjectives => Set<QuestObjective>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quest>()
            .HasMany(q => q.Objectives)
            .WithOne(o => o.Quest)
            .HasForeignKey(o => o.QuestId);
        
        modelBuilder.Entity<Quest>()
            .HasIndex(q => q.GameId)
            .IsUnique();
    }

public DbSet<CA_2.Models.PlayerQuestProgress> PlayerQuestProgress { get; set; } = default!;

}