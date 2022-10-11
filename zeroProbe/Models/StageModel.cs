namespace zeroProbe.Models;

public class StageModel
{
    public List<string> Commands { get; set; } = new List<string>();
    public string OnError { get; set; } = "";
    public bool IgnoreErrors { get; set; }
    public string Directory { get; set; } = "";
}