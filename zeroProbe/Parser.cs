using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, string> StagesDict { get; }
    public List<string> Stages  { get; }
    private bool Comments { get; set; }
    public string ProjectName { get; set;  }
    public string ScriptIfError { get; set; }
    public bool Debug { get; set; }
    public List<string> ShellCommands { get; set; }
    public List<string> ComponentsToCheck { get; set; }
    public bool SetProjectFirstTime { get; set; }
    

    public Parser()
    {
        StagesDict = new Dictionary<string, string>();
        Stages = new List<string>();
        ProjectName = "unnamed";
        Debug = false;
        ShellCommands = new List<string>();
        ComponentsToCheck = new List<string>();
        ScriptIfError = "";
        SetProjectFirstTime = true;
    }

    public void DebugInstruction(string instruction)
    {
        Messages.Debug($"Calling instruction {instruction}.");
    }

    public void ParseLine(string line)
    {
        var obj = Lexer.Lex(line);
        switch (obj.FunctionType)
        {
            case "0x11f":
                if (Debug) { DebugInstruction("0x11f"); }
                if (!Comments)
                {
                    Messages.Warning("Use comments less. It slows zeroProbe.");
                    Comments = true;
                }
                break;
            case "0xa33":
                if (Debug) { DebugInstruction("0xa33"); }
                ScriptIfError = obj.Arguments.Trim();
                break;
            case "0xc88":
                if (Debug) { DebugInstruction("0xc88"); }
                string components = obj.Arguments.Trim();
                if (components.Contains(','))
                {
                    string[] splitComponents = components.Split();
                    foreach (string component in splitComponents)
                    {
                        ComponentsToCheck.Add(component.Trim());
                    }
                }
                else
                {
                    ComponentsToCheck.Add(components);
                }
                break;
            case "0x054":
                if (Debug) { DebugInstruction("0x054"); }
                string stages = obj.Arguments.Trim();
                var split = stages.Split(",");
                if (!stages.Contains(','))
                {
                    if (Stages.Contains(stages.Trim()))
                    {
                        Messages.Fatal($"Stage already defined -> {stages.Trim()}");
                        App.End(-1);
                    }
                    Stages.Add(stages.Trim());
                }
                else
                {
                    foreach (var stage in split)
                    {
                        if (Stages.Contains(stage.Trim()))
                        {
                            Messages.Fatal($"Stage already defined -> {stage.Trim()}");
                            App.End(-1);
                        }
                        Stages.Add(stage.Trim());
                    } 
                }
                break;
            case "0x700":
                if (Debug) { DebugInstruction("0x700"); }
                string stg = obj.StageObject.StageName;
                if (!Stages.Contains(stg))
                {
                    Messages.Fatal($"Stage '{stg}' not defined.");
                    App.End(-1);
                }
                
                if (StagesDict.ContainsKey(stg))
                {
                    Messages.Fatal($"Stage already assigned -> {stg}");
                    App.End(-1);
                }
                StagesDict.Add(stg, obj.StageObject.StageCommand);
                break;
            case "0x805":
                if (Debug) { DebugInstruction("0x805"); }
                ShellCommands.Add(obj.Arguments);
                break;
            case "0xccf":
                if (Debug) { DebugInstruction("0xccf"); }
                if (!SetProjectFirstTime)
                {
                    Messages.Warning("Project name already assigned.");
                }
                SetProjectFirstTime = false;
                ProjectName = obj.Arguments.Trim();
                break;
            case "0x00f":
                break;
            default:
                Messages.Fatal($"Illegal instruction called -> {obj.FunctionType}");
                App.End(-1);
                break;
        }
    }

    
}