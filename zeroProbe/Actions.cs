using System.Text;
using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    private Parser Parser { get; }
    private HostHelper Helper { get; }
    private Project Project { get; set; }

    public Actions()
    {
        Parser = new Parser();
        Helper = new HostHelper();
        Project = new Project();
    }

    public void EnableDebug()
    {
        Parser.Debug = true;
    } 

    private void CheckForConfigFile()
    {
        if (!File.Exists("stages.pbc"))
        {
            Messages.Fatal("Looks like 'stages.pcf' not exists.");
            Messages.Hint("Try to write template configuration with 'writeconfig' action.");
            Environment.Exit(1);
        }
    }

    
    public void RunStages()
    {
        CheckForConfigFile();
        Parser.SetProject(Project);
        string[] lines = File.ReadAllLines("stages.pbc");
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
                
                if (Project.GetStageObject(stage).Directory != "")
                {
                    Messages.Info("Changing directory...");
                    shellCommand.Append($"cd {Project.GetStageObject(stage).Directory}\n");
                }
                
                foreach (var command in Project.GetStageObject(stage).Commands)
                {
                    shellCommand.Append($"{command}\n");
                }
                string commandToExecute = shellCommand.ToString();
                var res = Helper.ExecuteStage(stage, commandToExecute, Project.GetShell());
                if (!res.Error)
                {
                    if (Project.GetStageObject(stage).IgnoreErrors)
                    {
                        Messages.Info("Error occur but zeroProbe will ignore it.");
                    }
                    else
                    {
                        Messages.Fatal("Stage not passed due an error!");
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
        if (File.Exists("stages.pbc"))
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
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.Default);
                    break;
                case "2":
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.DefaultWithStages);
                    break;
                case "3":
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.TutorialConfig);
                    break;
                case "4":
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.DotNetBuildTest);      
                    break;
                case "5":
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.GccBuildTest);
                    break;
                case "6":
                    File.Create("stages.pbc").Close();
                    File.WriteAllText("stages.pbc", templates.ClangdBuildTest);
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
        Console.WriteLine($"Template config ready! It's called '{"stages.pbc"}'.");
        Console.WriteLine("If you got stuck, go to wiki on GitLab or GitHub and search what you want.");
        Environment.Exit(0);
    }

    public void RunStage(string name)
    {
        CheckForConfigFile();
        Parser.SetProject(Project);
        string[] lines = File.ReadAllLines("stages.pbc");
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
            if (!res.Error)
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