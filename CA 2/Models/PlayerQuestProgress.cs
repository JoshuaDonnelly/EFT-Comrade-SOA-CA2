namespace CA_2.Models;

public class PlayerQuestProgress
{
    public int Id { get; set; }
    public int QuestId { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Quest Quest { get; set; } = null!;
}