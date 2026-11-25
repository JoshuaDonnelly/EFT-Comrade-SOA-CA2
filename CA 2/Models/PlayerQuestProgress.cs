namespace CA_2.Models;

public class PlayerQuestProgress
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    // Quest section that follows progress for a given user
    public int QuestId { get; set; }
    
    public string Status { get; set; } = "NotStarted";
    
    public DateTime? CompletedAt { get; set; }
    
    public Quest Quest { get; set; } = null!;
}