namespace zeroProbe.Models;

public class Project
{
    // Base
    public string Name { get; set; } = "untitled";
    public string Shell { get; set; } = "/bin/sh";
    
    //Stages
    public List<string> StagesList { get; } = new List<string>();
    public Dictionary<string, StageModel> StagesModels { get; } = new Dictionary<string, StageModel>();

    // Components
    public List<string> Components { get; } = new List<string>();
}