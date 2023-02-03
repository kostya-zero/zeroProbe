namespace zeroProbe.Models;

public class StageModel
{
    public string Name { get; set; }
    public List<string> Commands { get; set; }
    public bool PredictFail { get; set; }
}