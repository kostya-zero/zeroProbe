using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    private Project Project { get; set; } = new Project();

    public bool Debug { get; set; } = false;
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
        if (Debug) { DebugInstruction(obj.FunctionType); }
        Console.WriteLine(obj.FunctionType);
        switch (obj.FunctionType)
        {
            case "0xc88":
                if (obj.Arguments.Trim().Contains(','))
                {
                    string[] splitComponents = obj.Arguments.Trim().Split();
                    foreach (string component in splitComponents)
                    {
                        Project.AddComponent(component.Trim());
                    }
                }
                else
                {
                    Project.AddComponent(obj.Arguments.Trim());
                }
                break;
            case "0xs54":
                var split = obj.Arguments.Trim().Split();
                foreach (string stage in split)
                {
                    SpellChecker.CheckStageName(stage);
                    Project.GetStagesList().Add(stage);
                    Project.AddStage(stage, new StageModel());
                }
                break;
            case "0xs00":
                if (!Project.GetStagesList().Contains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Check your config. Maybe you haven't defined this stage."); 
                    Environment.Exit(-1);
                }
                Project.AssignCommandToStage(obj.StageObject.StageName, obj.StageObject.StageCommand);
                break;
            case "0xs34":
                if (!Project.StagesContains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Check your config. Maybe you haven't defined this stage."); 
                    Environment.Exit(-1);
                }
                Project.StageSetOnError(obj.StageObject.StageName, obj.StageObject.StageCommand);
                break;
            
            case "0xs55":
                if (!Project.StagesContains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Check your config. Maybe you haven't defined this stage."); 
                    Environment.Exit(-1);
                }
                Project.StageSetDirectory(obj.StageObject.StageName, obj.Arguments.Trim());
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
                            Environment.Exit(-1);
                        }
                        break;
                    case "32":
                        if (Environment.Is64BitOperatingSystem)
                        {
                            Messages.Fatal("This config can be used only on 32 bit systems.");
                            Environment.Exit(-1);
                        }
                        break;
                    default:
                        Messages.Fatal("Bad value for 'arch'.");
                        Messages.TraceBack(line, lineNumber);
                        Messages.Hint("You can set only 64 or 32.");
                        Environment.Exit(-1);
                        break;
                }
                break;
            case "0xs83":
                if (!Project.StagesContains(obj.StageObject.StageName))
                {
                    Messages.Fatal($"Stage '{obj.StageObject.StageName}' not defined. Or, you entered wrong name.");
                    Environment.Exit(-1);
                }
                switch (obj.StageObject.StageCommand.Trim())
                {
                    case "1":
                        Project.StageSetIgnoreError(obj.StageObject.StageName, true);
                        break;
                    case "0":
                        Project.StageSetIgnoreError(obj.StageObject.StageName, false);
                        break;
                    default:
                        Messages.Fatal("Bad syntax. You can set only 1 or 0 for 'ignore_errors'.");
                        Messages.TraceBack(line, lineNumber);
                        Environment.Exit(-1);
                        break;
                }
                break;
            case "0x6b8":
                Project.SetProjectName(obj.Arguments.Trim());
                break;
            case "0xa55":
                Project.SetProjectName(obj.Arguments.Trim());
                break;
            case "0x00f":
                break;
            default:
                Messages.Fatal($"Illegal instruction called -> {obj.FunctionType}");
                Messages.TraceBack(line, lineNumber);
                Environment.Exit(-1);
                break;
        }
    }

    
}
