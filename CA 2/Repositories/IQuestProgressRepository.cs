using CA_2.Models;
namespace CA_2.Repositories;

// Defines the operations in relation to our data
public interface IQuestProgressRepository
{
    Task<PlayerQuestProgress?> GetAsync (
        string userId, int questId
    );

    Task<List<PlayerQuestProgress>> GetAllAsync(
        string userId);
    
    Task AddAsync(PlayerQuestProgress progress);
    Task UpdateAsync(PlayerQuestProgress progress);
    Task DeleteAsync(PlayerQuestProgress progress);
}