using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, string> StagesDict { get; }
    public List<string> Stages  { get; }
    private bool Comments { get; set; }
    public string ProjectName { get; set;  }
    public bool Debug { get; set; }
    public string SetupCommand { get; set; }
    public List<string> ShellCommands { get; set; }
    

    public Parser()
    {
        StagesDict = new Dictionary<string, string>();
        Stages = new List<string>();
        ProjectName = "unnamed";
        Debug = false;
        ShellCommands = new List<string>();
        SetupCommand = "";
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
            case "0xc88":
                if (Debug) { DebugInstruction("0xc88"); }
                Messages.Info(obj.Arguments.Trim());
                break;
            case "0x054":
                if (Debug) { DebugInstruction("0x054"); }
                string stages = obj.Arguments.Trim();
                var split = stages.Split(",");
                if (!stages.Contains(','))
                {
                    Stages.Add(stages.Trim());
                }
                else
                {
                    foreach (var stage in split)
                    {
                        Stages.Add(stage.Trim());
                    } 
                }
                break;
            case "0x700":
                if (Debug) { DebugInstruction("0x700"); }
                string stg = obj.FunctionName;
                stg = stg.TrimStart('!');
                if (Stages.Contains(stg))
                {
                    StagesDict.Add(stg, obj.Arguments.Trim());
                }
                else
                {
                    Messages.Fatal($"Stage '{stg}' not defined.");
                    App.End();
                }
                break;
            case "0xa33":
                if (Debug) { DebugInstruction("0xa33"); }
                SetupCommand = obj.Arguments;
                break;
            case "0x805":
                if (Debug) { DebugInstruction("0x805"); }
                ShellCommands.Add(obj.Arguments);
                break;
            case "0xccf":
                if (Debug) { DebugInstruction("0xccf"); }
                ProjectName = obj.Arguments.Trim();
                break;
            case "0x00f":
                break;
            default:
                Messages.Fatal($"Illegal instruction called -> {obj.FunctionType}");
                App.End();
                break;
        }
    }

    
}