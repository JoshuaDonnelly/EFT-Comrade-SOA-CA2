namespace CA_2.Models;

public class Quest
{
    public int Id { get; set; }
    public string GameId { get; set; } = "";
    public string Title { get; set; } = "";
    
    public int TraderId { get; set; }
    public int RequiredLevel { get; set; }
    public int Experience { get; set; }

    public List<QuestObjective> Objectives { get; set; } = new();
}