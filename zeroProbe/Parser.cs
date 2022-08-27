using zeroProbe.Enums;
using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, StageModel> StagesDict { get; }  = new();
    private bool SetProjectFirstTime { get; set; } = true;
    public string ProjectName { get; private set; }  = "unnamed";
    public List<string> StagesList  { get; } = new();
    public List<string> ShellCommands { get; } = new();
    public List<string> ComponentsToCheck { get; } = new();
    public List<ParserOptions> ParsingOptions { private get; set; } = new List<ParserOptions>();

    public void DebugInstruction(string instruction)
    {
        Messages.Debug($"Calling instruction {instruction}.");
    }

    public void ParseLines(string[] lines)
    {
        int lineNumber = 0;
        foreach (string line in lines)
        {
            lineNumber++;
            ParseLine(line, lineNumber);
        }
    }

    private void ParseLine(string line, int lineNumber)
    {
        var obj = Lexer.Lex(line, lineNumber);
        if (ParsingOptions.Contains(ParserOptions.Debug)) { DebugInstruction(obj.FunctionType); }
        switch (obj.FunctionType)
        {
            case "0x11f":
                break;
            case "0xc88":
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
                var split = obj.Arguments.Trim().Split();
                foreach (string stage in split)
                {
                    SpellChecker.CheckStageName(stage);
                    StagesList.Add(stage);
                    StagesDict.Add(stage, new StageModel());
                }
                break;
            case "0x700":
                if (!StagesList.Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined. " +
                                   "Or, you entered wrong name.");
                    App.End(-1);
                }
                StagesDict[obj.StageObject.StageName].Commands.Add(obj.StageObject.StageCommand);
                break;
            case "0x5fc":
                string stageName = obj.StageObject.StageName;
                if (!StagesList.Contains(stageName))
                {
                    Messages.Fatal($"Stage '{stageName}' not defined. Or, you entered wrong name.");
                    App.End(-1);
                }
                StagesDict[stageName].OnError = obj.StageObject.StageCommand;
                break;
            case "0x883":
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
                ShellCommands.Add(obj.Arguments);
                break;
            case "0x6b8":
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
