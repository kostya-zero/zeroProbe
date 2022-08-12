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
            case "0xc88":
                if (Debug) { DebugInstruction("0xc88"); }
                if (obj.Arguments.Trim().Contains(','))
                {
                    string[] splitComponents = obj.Arguments.Trim().Split();
                    foreach (string component in splitComponents)
                    {
                        ComponentsToCheck.Add(component.Trim());
                    }
                }
                else
                {
                    ComponentsToCheck.Add(obj.Arguments.Trim());
                }
                break;
            case "0x054":
                if (Debug) { DebugInstruction("0x054"); }
                var split = obj.Arguments.Trim().Split(",");
                if (!obj.Arguments.Trim().Contains(','))
                {
                    if (StagesList.Contains(obj.Arguments.Trim().Trim()))
                    {
                        Messages.Fatal($"Stage '{obj.Arguments.Trim().Trim()}' already defined.");
                        App.End(-1);
                    }
                    StagesList.Add(obj.Arguments.Trim().Trim());
                    StagesDict.Add(obj.Arguments.Trim().Trim(), new StageModel());
                }
                else
                {
                    foreach (var stage in split)
                    {
                        if (StagesList.Contains(stage.Trim()))
                        {
                            Messages.Fatal($"Stage '{stage.Trim()}' already defined");
                            App.End(-1);
                        }
                        StagesList.Add(stage.Trim());
                        StagesDict.Add(stage.Trim(), new StageModel());
                    } 
                }
                break;
            case "0x700":
                if (Debug) { DebugInstruction("0x700"); }
                if (!StagesList.Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined. " +
                                   "Or, you entered wrong name.");
                    App.End(-1);
                }
                StagesDict[obj.StageObject.StageName].Command = obj.StageObject.StageCommand;
                break;
            case "0x5fc":
                if (Debug) { DebugInstruction("0x5fc"); }
                string stageName = obj.StageObject.StageName;
                if (!StagesList.Contains(stageName))
                {
                    Messages.Fatal($"Stage '{stageName}' not defined. Or, you entered wrong name.");
                    App.End(-1);
                }
                StagesDict[stageName].OnError = obj.StageObject.StageCommand;
                break;
            case "0x883":
                if (Debug) { DebugInstruction("0x5fc"); }
                if (!StagesList.Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined. Or, you entered wrong name.");
                    App.End(-1);
                }
                switch (obj.StageObject.StageCommand.Trim())
                {
                    case "1":
                        if (StagesDict[obj.StageObject.StageName].OnError != "")
                        {
                            Messages.Fatal("You can't set ignore errors if you set an error command.");
                            App.End(-1);
                        }
                        StagesDict[obj.StageObject.StageName].IgnoreErrors = true;
                        break;
                    case "0":
                        if (StagesDict[obj.StageObject.StageName].OnError != "")
                        {
                            Messages.Fatal("You can't set ignore errors if you set an error command.");
                            App.End(-1);
                        }
                        StagesDict[obj.StageObject.StageName].IgnoreErrors = false;
                        break;
                    default:
                        Messages.Fatal("Bad syntax. You can set only 1 or 0 for 'ignore_errors'.");
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
