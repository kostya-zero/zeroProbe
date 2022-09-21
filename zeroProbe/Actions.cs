using System.Text;
using zeroProbe.Enums;
using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    private List<ParserOptions> Options { get; }
    private Parser Parser { get; }
    private HostHelper Helper { get; }
    private Project Project { get; set; }
    public string FilePath { private get; set; }

    public Actions()
    {
        Options = new List<ParserOptions>();
        Parser = new Parser();
        Helper = new HostHelper();
        Project = new Project();
        FilePath = "";
    }

    private void CheckForConfigFile()
    {
        if (!File.Exists(FilePath))
        {
            Messages.Fatal($"Looks like '{FilePath}' not exists.");
            Messages.Hint("Try to write template configuration with 'writeconfig' action.");
            Environment.Exit(-1);
        }
    }

    public void AddOption(ParserOptions option, string optionName)
    {
        if (Options.Contains(option))
        {
            Messages.Fatal($"Option '{optionName}' already added.");
            Environment.Exit(0);
        }
        Options.Add(option);
    }
    
    public void RunStages()
    {
        CheckForConfigFile();
        Parser.SetProject(Project);
        Parser.ParsingOptions = Options;
        string[] lines = File.ReadAllLines(FilePath);
        Parser.ParseLines(lines);
        Project = Parser.GetProject();
        Messages.Info($"Running project: {Project.GetProjectName()}");

        if (Project.GetComponents().Count != 0)
        {
            Messages.Work("Checking for required components...");
            Helper.CheckComponents(Project.GetComponents());
        }

        if (Project.CountStages() == 0)
        {
            Messages.Info("No stages found, aborting...");
            Environment.Exit(0);
        }
        
        foreach (var stage in Project.GetStagesList())
        {
            if (Project.StagesContains(stage))
            {
                if (Project.GetStageObject(stage).Commands.Count == 0)
                {
                    Messages.Info($"Stage '{stage}' will be skipped. No command assigned.");
                    continue;
                }
                Messages.Info($"Running stage '{stage}'...");
                StringBuilder shellCommand = new StringBuilder();
                foreach (var command in Project.GetStageObject(stage).Commands)
                {
                    shellCommand.Append($"{command}\n");
                }
                string commandToExecute = shellCommand.ToString();
                var res = Helper.ExecuteStage(stage, commandToExecute, Project.GetShell());
                if (res.Error != "")
                {
                    if (Project.GetStageObject(stage).IgnoreErrors)
                    {
                        Messages.Info("Error occur but zeroProbe will ignore it.");
                    }
                    else
                    {
                        Messages.Fatal("Stage not passed due an error:");
                        Console.WriteLine(res.Error);
                        if (Project.GetStageObject(stage).OnError != "")
                        {
                            Messages.Work("Running stage undo command...");
                            Helper.ExecuteCommand(Project.GetStageObject(stage).OnError, "tmp_undo_script.sh", Project.GetShell());
                            Messages.Good("Undo complete.");
                        }

                        Environment.Exit(-1);
                    }
                }
                Messages.Good("Stage passed without errors!");
            }
        }
        Messages.Good("All good! Great job!");
    }

    public void WriteConfig()
    {
        if (File.Exists(FilePath))
        {
            Messages.Info("You already have configuration file.");
            Environment.Exit(0);
        }

        ConfigsTemplates templates = new ConfigsTemplates();
        
        Console.WriteLine("Welcome to zeroProbe Configuration Wizard!");
        Console.WriteLine("Here you can select template to use. Available templates:");
        Console.WriteLine("    (1) : Empty configuration.\n" +
                          "    (2) : Default config with stages.\n" +
                          "    (3) : Tutorial config (classic).\n" +
                          "    (4) : .NET Build Test.\n" +
                          "    (5) : GCC Build Test.\n" + 
                          "    (6) : Clangd Build Test.");
        
        while (true)
        {
            Console.Write("Your choice: ");
            var answer = Console.ReadLine();
            bool failed = false;
            switch (answer)
            {
                case "1":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.Default);
                    break;
                case "2":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.DefaultWithStages);
                    break;
                case "3":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.TutorialConfig);
                    break;
                case "4":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.DotNetBuildTest);      
                    break;
                case "5":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.GccBuildTest);
                    break;
                case "6":
                    File.Create(FilePath).Close();
                    File.WriteAllText(FilePath, templates.ClangdBuildTest);
                    break;
                default:
                    Console.WriteLine("Bad answer.");
                    failed = true;
                    break;
            }

            if (!failed)
            {
                break;
            }
        }
        Console.WriteLine($"Template config ready! It's called '{FilePath}'.");
        Console.WriteLine("If you got stuck, go to wiki on GitLab or GitHub and search what you want.");
        Environment.Exit(0);
    }

    public void RunStage(string name)
    {
        CheckForConfigFile();
        Parser.SetProject(Project);
        Parser.ParsingOptions = Options;
        string[] lines = File.ReadAllLines(FilePath);
        Parser.ParseLines(lines);
        Project = Parser.GetProject();
        Messages.Info($"Running stage of project: {Project.GetProjectName()}");

        if (Project.GetComponents().Count != 0)
        {
            Messages.Work("Checking for required components...");
            Helper.CheckComponents(Project.GetComponents());
        }
        
        if (Project.CountStages() == 0)
        {
            Messages.Info("No stages defined. Exiting...");
            Environment.Exit(0);
        }

        if (Project.StagesContains(name))
        {
            if (Project.GetStageObject(name).Commands.Count == 0)
            {
                Messages.Info($"Stage '{name}' will be skipped. No command assigned.");
                Environment.Exit(0);
            }
            Messages.Info($"Running stage '{name}'...");
            StringBuilder shellCommand = new StringBuilder();
            foreach (var command in Project.GetStageObject(name).Commands)
            {
                shellCommand.Append($"{command}\n");
            }
            string commandToExecute = shellCommand.ToString();
            var res = Helper.ExecuteStage(name, commandToExecute, Project.GetShell());
            if (res.Error != "")
            {
                if (Project.GetStageObject(name).IgnoreErrors)
                {
                    Messages.Info("Error occur but zeroProbe will ignore it.");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    if (Project.GetStageObject(name).OnError != "")
                    {
                        Messages.Work("Running stage undo command...");
                        Helper.ExecuteCommand(Project.GetStageObject(name).OnError, "tmp_undo_script.sh", Project.GetShell());
                        Messages.Good("Undo complete.");
                    }

                    Environment.Exit(-1);
                }
            }
            Messages.Good("Stage passed without errors!");
        }
        else
        {
            Messages.Fatal($"No stage found with name '{name}'. Maybe you forget to define it?");
        }
    }
}