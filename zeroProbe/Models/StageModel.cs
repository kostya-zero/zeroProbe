namespace zeroProbe.Models;

public class StageModel
{
    public string Name { get; set; }
    public List<string> Commands { get; set; } = new List<string>();
    public bool PredictFail { get; set; }
}