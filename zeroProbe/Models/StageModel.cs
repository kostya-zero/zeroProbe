namespace zeroProbe.Models;

public class StageModel
{
    public List<string> Commands { get; set; }
    public string OnError { get; set; } = "";
    public bool IgnoreErrors { get; set; }

    public StageModel()
    {
        Commands = new List<string>();
    }
}