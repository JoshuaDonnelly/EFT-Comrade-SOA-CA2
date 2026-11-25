using CA_2.Data;
using CA_2.Models;
using Microsoft.EntityFrameworkCore;

namespace CA_2.Repositories;

public class QuestProgressRepository(EftContext context) : IQuestProgressRepository
{
    // Get specific progress for a given player
    public async Task<PlayerQuestProgress?> GetAsync(string userId, int questId)
    {
        return await context.PlayerQuestProgress
            // So we can access specific fields for Quests
            .Include(p => p.Quest)
            .FirstOrDefaultAsync(p =>
                p.UserId == userId &&
                p.QuestId == questId
            );
    }
    
    // Get all quests for a given user
    public async Task<List<PlayerQuestProgress>> GetAllAsync(string userId)
    {
        // Include a quest for this player, convert it to a list
        return await context.PlayerQuestProgress
            // Suitable for read queries, tells EF no need to tracked for updates, lowers memory usage
            .AsNoTracking()
            .Include(p => p.Quest)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
    
    // Add a new task for a given player
    public async Task AddAsync(PlayerQuestProgress progress)
    {
        // Tells EF Core the entity needs to be inserted
        await context.PlayerQuestProgress.AddAsync(progress);
        await context.SaveChangesAsync(); // saves the changes to Postgres
    }

    public async Task UpdateAsync(PlayerQuestProgress progress)
    {
        context.PlayerQuestProgress.Update(progress);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PlayerQuestProgress progress)
    {
        context.PlayerQuestProgress.Remove(progress);
        await context.SaveChangesAsync();
    }
}