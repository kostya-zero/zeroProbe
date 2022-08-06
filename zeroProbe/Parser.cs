using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, StageModel> StagesDict { get; }  = new();
    private bool Comments { get; set; }
    public bool Debug { get; init; }
    private bool SetProjectFirstTime { get; set; } = true;
    public string ProjectName { get; private set; }  = "unnamed";
    public string ScriptIfError { get; private set; } = "";
    public List<string> StagesList  { get; } = new();
    public List<string> ShellCommands { get; } = new();
    public List<string> ComponentsToCheck { get; } = new();

    public void DebugInstruction(string instruction)
    {
        Messages.Debug($"Calling instruction {instruction}.");
    }

    public void ParseLines(string[] lines)
    {
        foreach (string line in lines)
        {
            ParseLine(line);
        }
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
                    if (StagesList.Contains(stages.Trim()))
                    {
                        Messages.Fatal($"Stage already defined -> {stages.Trim()}");
                        App.End(-1);
                    }
                    StagesList.Add(stages.Trim());
                    StagesDict.Add(stages.Trim(), new StageModel());
                }
                else
                {
                    foreach (var stage in split)
                    {
                        if (StagesList.Contains(stage.Trim()))
                        {
                            Messages.Fatal($"Stage already defined -> {stage.Trim()}");
                            App.End(-1);
                        }
                        StagesList.Add(stage.Trim());
                        StagesDict.Add(stage.Trim(), new StageModel());
                    } 
                }
                break;
            case "0x700":
                if (Debug) { DebugInstruction("0x700"); }
                string stg = obj.StageObject.StageName;
                if (!StagesList.Contains(stg))
                {
                    Messages.Fatal($"Stage '{stg}' not defined.");
                    App.End(-1);
                }
                
                if (StagesDict.ContainsKey(stg))
                {
                    Messages.Fatal($"Stage already assigned -> {stg}");
                    App.End(-1);
                }
                StagesDict[stg].Command = obj.StageObject.StageCommand;
                break;
            case "0x5fc":
                if (Debug) { DebugInstruction("0x5fc"); }
                string stageName = obj.StageObject.StageName;
                if (!StagesList.Contains(stageName))
                {
                    Messages.Fatal($"Stage '{stageName}' not defined.");
                    App.End(-1);
                }
                StagesDict[stageName].OnError = obj.StageObject.StageCommand;
                break;
            case "0x883":
                if (Debug) { DebugInstruction("0x5fc"); }
                string anotherStageName = obj.StageObject.StageName;
                if (!StagesList.Contains(anotherStageName))
                {
                    Messages.Fatal($"Stage '{anotherStageName}' not defined.");
                    App.End(-1);
                }

                switch (obj.StageObject.StageCommand.Trim())
                {
                    case "1":
                        StagesDict[anotherStageName].IgnoreErrors = true;
                        break;
                    case "0":
                        StagesDict[anotherStageName].IgnoreErrors = false;
                        break;
                    default:
                        Messages.Fatal("Bad syntax. You can set only 1 or 0.");
                        App.End(-1);
                        break;
                }
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
