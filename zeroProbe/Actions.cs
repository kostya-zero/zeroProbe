using System.Text;
using zeroProbe.Enums;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    private List<ParserOptions> Options { get; }
    private Parser Parser { get; }

    public Actions()
    {
        Options = new List<ParserOptions>();
        Parser = new Parser();
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
    
    public void RunStages(string filePath)
    {
        HostHelper helper = new HostHelper();
        string[] lines = File.ReadAllLines(filePath);
        Parser.ParseLines(lines);
        Messages.Info($"Running project: {Parser.ProjectName}");

        if (Parser.ComponentsToCheck.Count != 0)
        {
            Messages.Work("Checking for required components...");
            List<string> pathVar = Env.GetPath();
            int missingComponentsCount = 0;
            List<string> foundComponents = new List<string>();
            foreach (var component in Parser.ComponentsToCheck)
            {
                foreach (var path in pathVar)
                {
                    if (!foundComponents.Contains(component))
                    {
                        bool found = File.Exists($"{path}/{component}");
                        if (found)
                        {
                            foundComponents.Add(component);
                            Messages.Good($"Found {component}.");
                        }
                    }
                }
            }

            foreach (var component in Parser.ComponentsToCheck)
            {
                if (!foundComponents.Contains(component))
                {
                    missingComponentsCount++;
                }
            }
            
            if (missingComponentsCount > 0)
            {
                Messages.Fatal($"{missingComponentsCount.ToString()} are missing. Review a configuration to " +
                               "check which components missing.");
                App.End(-1);
            }
            else
            {
                Messages.Good("All components installed!");
            }
        }

        if (Parser.ShellCommands.Count != 0 && !Options.Contains(ParserOptions.SkipShellCommands))
        {
            helper.ExecuteShellCommands(Parser.ShellCommands, 
                Options.Contains(ParserOptions.SkipShellCommandsErrors));
        }
        
        foreach (var stage in Parser.StagesList)
        {
            if (Parser.StagesDict.ContainsKey(stage))
            {
                Messages.Info($"Running stage '{stage}'...");
                var res = helper.ExecuteStage(stage, Parser.StagesDict[stage].Command);

                if (res.Error != "")
                {
                    if (Parser.StagesDict[stage].IgnoreErrors)
                    {
                        Messages.Info("Error occur but zeroProbe will ignore it.");
                    }
                    else
                    {
                        Messages.Fatal("Stage not passed due an error:");
                        Console.WriteLine(res.Error);
                        if (Parser.StagesDict[stage].OnError != "")
                        {
                            Messages.Work("Running stage undo command...");
                            helper.RunUndoScript(Parser.StagesDict[stage].OnError);
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

    public void WriteConfig(string filePath)
    {
        if (File.Exists(filePath))
        {
            Console.WriteLine("You already have configuration file.");
            App.End();
        }
        
        Console.WriteLine("Writing new config file...");
        File.Create("stages.pbc").Close();
        File.WriteAllText("stages.pbc", @"/* This file was generated with zeroProbe 2.1 Emerging. */

/* Its a preview of how ProbeConfig file can be. */
/* Everything about syntax and parameters you can learn on zeroProbe wiki. */
/* GitLab wiki: https://gitlab.com/kostya-zero/zeroprobe/-/wikis/home */
/* GitHub wiki: https://github.com/kostya-zero/zeroProbe/wiki */

/* Project name are not necessary. */
/* zeroProbe automatically set your project name to 'unnamed'. */
&project: test

/* You can add shell commands to execute. */
/* &shell command can be used multiple times. */
/* All commands will be executed how you add it. */
&shell: echo 'This is a small template for screenshot!'

/* Stages are main feature of your config. */
/* To tell zeroProbe what to do you need to create stages. */
/* Stages include command that's must be executed. */
/* Every stage will be executed in order how you wrote him. */
&stages: restore, build, finish

/* !!! Updated in version 2.0 Emerging, read more on wiki. !!! */
/* Now you need to assign command to stage. */
/* Use '!' operator for it. */
/* After '!' enter stage name. */
/* Next, write '.command' and after double dots enter command to run. */
/* To make stage ignore errors write '.ignore_errors' and after double dots 1 or 0. */
/* To set command on error use '.on_error'. */
/* Learn more you can on official wiki on GitLab and GitHub. */
!restore.command: echo 'Doing some restore staff...'
!build.command: echo 'Doing some build staff...'
!finish.command: echo 'Finishing this deal...'");
        Console.WriteLine("Template config ready! It's called 'stages.pbc'.");
        Console.WriteLine("If you got stuck, go to wiki on GitLab or GitHub and search what you want.");
    }

    public void RunStage(string name, string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Cannot find file '{filePath}'.");
            App.End(-1);
        }

        HostHelper helper = new HostHelper();
        string[] allLines = File.ReadAllLines(filePath);
        foreach (var line in allLines)
        {
            Parser.ParseLine(line);
        }
        Messages.Info($"Running stage of project: {Parser.ProjectName}");

        if (Parser.ShellCommands.Count != 0 && !Options.Contains(ParserOptions.SkipShellCommands))
        {
            helper.ExecuteShellCommands(Parser.ShellCommands, 
                Options.Contains(ParserOptions.SkipShellCommandsErrors));
        }
        
        Messages.Info($"Running stage of project: {Parser.ProjectName}");
        if (Parser.StagesDict.ContainsKey(name))
        {
            Messages.Info($"Running stage '{name}'...");
            var res = helper.ExecuteStage(name, Parser.StagesDict[name].Command);

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
                        helper.RunUndoScript(Parser.StagesDict[name].OnError);
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