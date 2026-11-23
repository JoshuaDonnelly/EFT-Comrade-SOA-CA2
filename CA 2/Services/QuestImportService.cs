using System.Text.Json;
using CA_2.Data;
using CA_2.Models;
using Microsoft.EntityFrameworkCore;

namespace CA_2.Services;

public class QuestImportService
{
    private readonly EftContext _context;
    public QuestImportService(EftContext context)
    {
        _context = context;
    }

    public async Task ImportQuestsAsync(string path)
    {
        var json = await File.ReadAllTextAsync(path);
        
        using var document = JsonDocument.Parse(json);
        
        var root = document.RootElement;
        
        var existingIds = (await _context.Quests
            .Select(q => q.Id)
            .ToListAsync()).ToHashSet();
        
        var quests = root.ValueKind == JsonValueKind.Array
            ? root 
            : root.GetProperty("quests");

        foreach (var questJson in quests.EnumerateArray())
        {
            int id = questJson.GetProperty("id").GetInt32();
            
            // Duplicate quest check
            if (!existingIds.Add(id))
            {
                continue;
            }
            
            // Create new quest object, casting to strongly typed variables form the quest json entries
            var quest = new Quest
            {
                Id = id,
                GameId = questJson.TryGetProperty("gameId", out var gameId)
                         && gameId.ValueKind == JsonValueKind.String
                         && !string.IsNullOrWhiteSpace(gameId.GetString())
                    ? gameId.GetString()
                    : null,
                Title = questJson.TryGetProperty("title", out var title )? title.ToString() : "",
                TraderId = questJson.GetProperty("giver").GetInt32(),
                Experience = questJson.GetProperty("exp").GetInt32(),
                RequiredLevel = questJson.TryGetProperty("require", out var require)
                                && require.ValueKind == JsonValueKind.Object
                                && require.TryGetProperty("level", out var level)
                                && level.ValueKind == JsonValueKind.Number
                                && level.TryGetInt32(out var requiredLevel)
                    ? requiredLevel
                    : 1
            };
            foreach (var objectiveJson in questJson.GetProperty("objectives").EnumerateArray())
            {
                quest.Objectives.Add(new QuestObjective
                {
                    Type = objectiveJson.GetProperty("type").GetString() ?? "",
                    Target = objectiveJson.TryGetProperty("target", out var target )? target.ToString() : "",
                    RequiredAmount = objectiveJson.TryGetProperty("number", out var number) ? number.GetInt32() : 1
                });
            }
            _context.Quests.Add(quest);
        }
        
        await  _context.SaveChangesAsync();
    }
}