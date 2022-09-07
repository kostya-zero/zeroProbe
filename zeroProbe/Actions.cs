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
    public string FilePath { get; set; }

    public Actions()
    {
        Options = new List<ParserOptions>();
        Parser = new Parser();
        Helper = new HostHelper();
        Project = new Project();
        FilePath = "";
    }

    public void AddOption(ParserOptions option, string optionName)
    {
        if (Options.Contains(option))
        {
            Messages.Fatal($"Option '{optionName}' already added.");
            App.End();
        }
        Options.Add(option);
    }
    
    public void RunStages()
    {
        Parser.SetProject(Project);
        Parser.ParsingOptions = Options;
        string[] lines = File.ReadAllLines(FilePath);
        Parser.ParseLines(lines);
        Project = Parser.GetProject();
        Messages.Info($"Running project: {Project.Name}");

        if (Project.Components.Count != 0)
        {
            Messages.Work("Checking for required components...");
            Helper.CheckComponents(Project.Components);
        }

        if (Project.StagesList.Count == 0)
        {
            Messages.Info("No stages found, aborting...");
            App.End();
        }
        
        foreach (var stage in Project.StagesList)
        {
            if (Project.StagesModels.ContainsKey(stage))
            {
                Messages.Info($"Running stage '{stage}'...");
                StringBuilder shellCommand = new StringBuilder();
                foreach (var command in Project.StagesModels[stage].Commands)
                {
                    shellCommand.Append($"{command}\n");
                }
                string commandToExecute = shellCommand.ToString();
                var res = Helper.ExecuteStage(stage, commandToExecute);
                if (res.Error != "")
                {
                    if (Project.StagesModels[stage].IgnoreErrors)
                    {
                        Messages.Info("Error occur but zeroProbe will ignore it.");
                    }
                    else
                    {
                        Messages.Fatal("Stage not passed due an error:");
                        Console.WriteLine(res.Error);
                        if (Project.StagesModels[stage].OnError != "")
                        {
                            Messages.Work("Running stage undo command...");
                            Helper.ExecuteCommand(Project.StagesModels[stage].OnError, "tmp_undo_script.sh");
                            Messages.Good("Undo complete.");
                        }

                        App.End(-1);
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
            App.End();
        }
        
        Console.WriteLine("Writing new config file...");
        File.Create(FilePath).Close();
        File.WriteAllText(FilePath, @"/* This file was generated with zeroProbe 3.0 Rebirth. */

/* Its a preview of how ProbeConfig file can be. */
/* Everything about syntax and parameters you can learn on zeroProbe wiki. */
/* GitLab wiki: https://gitlab.com/kostya-zero/zeroprobe/-/wikis/home */
/* GitHub wiki: https://github.com/kostya-zero/zeroProbe/wiki */

/* Project name are not necessary. */
/* zeroProbe automatically set your project name to 'unnamed'. */
&project: test

/* Stages are main feature of your config. */
/* To tell zeroProbe what to do you need to create stages. */
/* Stages include command that's must be executed. */
/* Every stage will be executed in order how you wrote him. */
&stages: restore build finish

/* Now you need to assign command to stage. */
/* Use '!' operator for it. */
/* After '!' enter stage name. */
/* Next, write '.add_command' and after double dots enter command to add. */
/* zeroProbe will execute it one by one.
/* To make stage ignore errors write '.ignore_errors' and after double dots 1 or 0. */
/* To set command on error use '.on_error'. */
/* Learn more you can on official wiki on GitLab and GitHub. */
!restore.add_command: echo 'Doing some restore staff...'
!build.add_command: echo 'Doing some build staff...'
!finish.add_command: echo 'Finishing this deal...'");
        Console.WriteLine($"Template config ready! It's called '{FilePath}'.");
        Console.WriteLine("If you got stuck, go to wiki on GitLab or GitHub and search what you want.");
    }

    public void RunStage(string name)
    {
        if (!File.Exists(FilePath))
        {
            Console.WriteLine($"Cannot find file '{FilePath}'.");
            App.End(-1);
        }

        HostHelper helper = new HostHelper();
        string[] allLines = File.ReadAllLines(FilePath);
        Parser.ParseLines(allLines);

        Messages.Info($"Running stage of project: {Parser.ProjectName}");

        if (Parser.ComponentsToCheck.Count != 0)
        {
            Messages.Work("Checking for required components...");
            helper.CheckComponents(Parser.ComponentsToCheck);
        }
        
        Messages.Info($"Running stage of project: {Parser.ProjectName}");
        if (Parser.StagesDict.ContainsKey(name))
        {
            Messages.Info($"Running stage '{name}'...");
            StringBuilder shellCommand = new StringBuilder();
            foreach (var command in Parser.StagesDict[name].Commands)
            {
                shellCommand.Append($"{command}\n");
            }
            string commandToExecute = shellCommand.ToString();
            var res = helper.ExecuteStage(name, commandToExecute);
            if (res.Error != "")
            {
                if (Parser.StagesDict[name].IgnoreErrors)
                {
                    Messages.Info("Error occur but zeroProbe will ignore it.");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    if (Parser.StagesDict[name].OnError != "")
                    {
                        Messages.Work("Running stage undo command...");
                        helper.ExecuteCommand(Parser.StagesDict[name].OnError, "tmp_undo_script.sh");
                        Messages.Good("Undo complete.");
                    }

                    App.End(-1);
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