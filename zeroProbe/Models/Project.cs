namespace zeroProbe.Models;

public class Project
{
    // Base
    private string Name { get; set; } = "untitled";
    private string Shell { get; set; } = "/bin/sh";
    
    //Stages
    private List<string> StagesList { get; } = new List<string>();
    private Dictionary<string, StageModel> StagesModels { get; } = new Dictionary<string, StageModel>();

    // Components
    private List<string> Components { get; } = new List<string>();
    
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

    public bool StagesContains(string stageName)
    {
        return StagesModels.ContainsKey(stageName);
    }

    public void AddStage(string stageName, StageModel stage)
    {
        StagesModels.Add(stageName, stage);
    }

    public void AssignCommandToStage(string stage, string command)
    {
        StagesModels[stage].Commands.Add(command);
    }

    public List<string> GetComponents()
    {
        return Components;
    }

    public void AddComponent(string component)
    {
        Components.Add(component);
    }

    public void StageAddCommand(string stage, string arg)
    {
        StagesModels[stage].Commands.Add(arg);
    }
    
    public void StageSetOnError(string stage, string arg)
    {
        StagesModels[stage].OnError = arg;
    }
    
    public void StageSetIgnoreError(string stage, bool arg)
    {
        StagesModels[stage].IgnoreErrors = arg;
    }
    
    public void StageSetDirectory(string stage, string arg)
    {
        StagesModels[stage].Directory = arg;
    }
}