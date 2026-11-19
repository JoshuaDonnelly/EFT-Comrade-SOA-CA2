namespace CA_2.Models;

public class EftItemTasks
{
    public int id { get; set; }
    public List<String> require { get; set; }
    public int giver { get; set; }
    public string title { get; set; }
    public int exp { get; set; }
}