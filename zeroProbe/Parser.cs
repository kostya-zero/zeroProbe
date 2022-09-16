using zeroProbe.Enums;
using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public List<ParserOptions> ParsingOptions { private get; set; } = new List<ParserOptions>();
    private Project Project { get; set; }

    private void DebugInstruction(string instruction)
    {
        Messages.Debug($"Calling instruction {instruction}.");
    }

    public void SetProject(Project newProject)
    {
        Project = newProject;
    }
    
    public Project GetProject()
    {
        return Project;
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
                        Project.Components.Add(component.Trim());
                    }
                }
                else
                {
                    Project.Components.Add(obj.Arguments.Trim());
                }
                break;
            case "0x054":
                var split = obj.Arguments.Trim().Split();
                foreach (string stage in split)
                {
                    SpellChecker.CheckStageName(stage);
                    Project.StagesList.Add(stage);
                    Project.StagesModels.Add(stage, new StageModel());
                }
                break;
            case "0x700":
                if (!Project.StagesList.Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Check your config. Maybe you haven't defined this stage."); 
                    App.End(-1);
                }
                Project.StagesModels[obj.StageObject.StageName].Commands.Add(obj.StageObject.StageCommand);
                break;
            case "0x5fc":
                string stageName = obj.StageObject.StageName;
                if (!Project.StagesList.Contains(stageName))
                {
                    Messages.Fatal($"Stage '{stageName}' not defined.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Check your config. Maybe you haven't defined this stage."); 
                    App.End(-1);
                }
                Project.StagesModels[stageName].OnError = obj.StageObject.StageCommand;
                break;
            case "0x805":
                Project.SetShell(obj.Arguments);
                break;
            case "0xa58":
                switch (obj.Arguments)
                {
                    case "64":
                        if (!Environment.Is64BitOperatingSystem)
                        {
                            Messages.Fatal("This config can be used only on 64 bit systems.");
                            App.End(-1);
                        }
                        break;
                    case "32":
                        if (Environment.Is64BitOperatingSystem)
                        {
                            Messages.Fatal("This config can be used only on 32 bit systems.");
                            App.End(-1);
                        }
                        break;
                    default:
                        Messages.Fatal("Bad value for 'arch'.");
                        Messages.TraceBack(line, lineNumber);
                        Messages.Hint("You can set only 64 or 32.");
                        App.End(-1);
                        break;
                }
                break;
            case "0x883":
                if (!Project.StagesList.Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined. Or, you entered wrong name.");
                    App.End(-1);
                }
                switch (obj.StageObject.StageCommand.Trim())
                {
                    case "1":
                        if (Project.StagesModels[obj.StageObject.StageName].OnError != "")
                        {
                            Messages.Fatal("You can't set ignore errors if you set an error command.");
                            Messages.TraceBack(line, lineNumber);
                            App.End(-1);
                        }
                        Project.StagesModels[obj.StageObject.StageName].IgnoreErrors = true;
                        break;
                    case "0":
                        if (Project.StagesModels[obj.StageObject.StageName].OnError != "")
                        {
                            Messages.Fatal("You can't set ignore errors if you set an error command.");
                            Messages.TraceBack(line, lineNumber);
                            App.End(-1);
                        }
                        Project.StagesModels[obj.StageObject.StageName].IgnoreErrors = false;
                        break;
                    default:
                        Messages.Fatal("Bad syntax. You can set only 1 or 0 for 'ignore_errors'.");
                        Messages.TraceBack(line, lineNumber);
                        App.End(-1);
                        break;
                }
                break;
            case "0x6b8":
                Project.SetProjectName(obj.Arguments.Trim());
                break;
            case "0x00f":
                break;
            default:
                Messages.Fatal($"Illegal instruction called -> {obj.FunctionType}");
                Messages.TraceBack(line, lineNumber);
                App.End(-1);
                break;
        }
    }

    
}
