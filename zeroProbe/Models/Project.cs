namespace zeroProbe.Models;

public class Project
{
    // Base
    private string Name { get; set; } = "untitled";
    private string Shell { get; set; } = "/bin/sh";
    
    //Stages
    public List<string> StagesList { get; } = new List<string>();
    public Dictionary<string, StageModel> StagesModels { get; } = new Dictionary<string, StageModel>();

    // Components
    public List<string> Components { get; } = new List<string>();
    
    public void SetProjectName(string name)
    {
        Name = name;
    }

    public string GetProjectName()
    {
        return Name;
    }

    public void SetShell(string shell)
    {
        Shell = shell;
    }

    public string GetShell()
    {
        return Shell;
    }

    public int CountStages()
    {
        return StagesList.Count;
    }

    public List<string> GetStagesList()
    {
        return StagesList;
    }

    public void AddStageToList(string stageName)
    {
        StagesList.Add(stageName);
    }

    public StageModel GetStageObject(string stageName)
    {
        if (!StagesModels.ContainsKey(stageName))
        {
            throw new Exception($"Stage with name '{stageName}' not exists.");
        }

        return StagesModels[stageName];
    }
}