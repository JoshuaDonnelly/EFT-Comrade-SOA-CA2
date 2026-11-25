using Microsoft.EntityFrameworkCore;
using CA_2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CA_2.Data;

public class EftContext(DbContextOptions<EftContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Quest> Quests => Set<Quest>();
    
    public DbSet<QuestObjective> QuestObjectives => Set<QuestObjective>();
    
    public DbSet<PlayerQuestProgress> PlayerQuestProgress => Set<PlayerQuestProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Used to configure tables for items below

        modelBuilder.Entity<Quest>()
            .Property(q => q.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Quest>()
            .HasMany(q => q.Objectives)
            .WithOne(o => o.Quest)
            .HasForeignKey(o => o.QuestId);

        modelBuilder.Entity<Quest>()
            .HasIndex(q => q.GameId);
        
        modelBuilder.Entity<PlayerQuestProgress>()
            .HasOne(p => p.Quest)
            .WithMany()
            .HasForeignKey(p => p.QuestId);

        modelBuilder.Entity<PlayerQuestProgress>()
            .HasIndex(p => new { p.UserId, p.QuestId }) // Ensures player's cannot have duplicate progress for one task
            .IsUnique();
    }
}