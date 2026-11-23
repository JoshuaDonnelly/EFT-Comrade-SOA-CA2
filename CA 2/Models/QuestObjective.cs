namespace CA_2.Models;

public class QuestObjective
{
    public int Id { get; set; }
    public int QuestId { get; set; }

    public string Type { get; set; } = "";
    public string Target { get; set; } = "";
    public int RequiredAmount { get; set; }

    public Quest Quest { get; set; } = null!;
}