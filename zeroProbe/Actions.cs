using System.Text;
using zeroProbe.Enums;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    private List<ParserOptions> Options { get; }

    public Actions()
    {
        Options = new List<ParserOptions>();
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
        Parser pr = new Parser
        {
            Debug = Options.Contains(ParserOptions.Debug)
        };
        string[] lines = File.ReadAllLines(filePath);
        pr.ParseLines(lines);
        Messages.Info($"Running project: {pr.ProjectName}");

        if (pr.ComponentsToCheck.Count != 0)
        {
            Messages.Work("Checking for required components...");
            List<string> pathVar = Env.GetPath();
            int missingComponentsCount = 0;
            List<string> foundComponents = new List<string>();
            foreach (var component in pr.ComponentsToCheck)
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

            foreach (var component in pr.ComponentsToCheck)
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

        if (pr.ShellCommands.Count != 0 && !Options.Contains(ParserOptions.SkipShellCommands))
        {
            helper.ExecuteShellCommands(pr.ShellCommands, Options.Contains(ParserOptions.SkipExecutionErrors));
        }
        
        foreach (var stage in pr.StagesList)
        {
            if (pr.StagesDict.ContainsKey(stage))
            {
                var res = helper.ExecuteStage(stage, pr.StagesDict[stage].Command);
                
                if (pr.StagesDict[stage].IgnoreErrors || Options.Contains(ParserOptions.SkipExecutionErrors))
                {
                    Messages.Info("Error occur but zeroProbe will ignore it.");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    if (pr.ScriptIfError != "")
                    {
                        Messages.Work("Running undo script...");
                        helper.RunUndoScript(pr.ScriptIfError);
                        Messages.Good("Undo complete.");
                    }
                    App.End(-1);
                }
            }
        }
        Messages.Good("All good!");
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
        File.WriteAllText("stages.pbc", @"/* This file was generated with zeroProbe. */

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

/* Now you need to assign command to stage. */
/* Use '!' operator for it. */
/* After '!' enter stage name. */
/* Next, after double dots write command to run. */
/* Learn more you can on official wiki on GitLab and GitHub. */
!restore: echo 'Doing some restore staff...'
!build: echo 'Doing some build staff...'
!finish: echo 'Finishing this deal...'");
        Console.WriteLine("Template config ready! It's called 'stages.pbc'.");
        Console.WriteLine("If you got stuck, go to wiki on GitLab or GitHub and search what you want.");
    }

    public void InspectStages(string filePath)
    {
        
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines(filePath);
        pr.ParseLines(allLines);
        
        List<string> inspectStages = pr.StagesList;
        string inspectStagesCount = inspectStages.Count.ToString();
        bool inspectNoStages = false;
        string strStages;
        
        if (inspectStages.Count == 0)
        {
            inspectNoStages = true;
        }
        
        Console.WriteLine(":::: Inspection results");
        Console.WriteLine(":: Project info");
        Console.WriteLine($"Project name: {pr.ProjectName}");
        Console.WriteLine("Shell commands: " 
                          + (pr.ShellCommands.Count == 0 ? "None" : pr.ShellCommands.Count.ToString()));
        StringBuilder reqBuilder = new StringBuilder();
        foreach (string component in pr.ComponentsToCheck)
        {
            reqBuilder.Append($"{component} ");
        }
        Console.WriteLine("Required components: " +
            reqBuilder.ToString().Trim() == "" ? "None" : reqBuilder.ToString().Trim());
        if (inspectNoStages)
        {
            Console.WriteLine(":: Stages");
            Console.WriteLine("No stages in this configuration!");
            App.End();
        }
        
        StringBuilder stringBuilder = new StringBuilder();
        if (inspectStages.Count == 1)
        {
            strStages = inspectStages[0];
        }
        else
        {
            foreach (var stage in inspectStages)
            {
                stringBuilder.Append($"{stage}, ");
            }

            strStages = stringBuilder.ToString().TrimEnd(',');
        }
        Console.WriteLine(":: Stages");
        Console.WriteLine($"Stages: {strStages}");
        Console.WriteLine($"Count:  {inspectStagesCount}\n");
        Console.WriteLine(":: Stages commands");
        foreach (var stage in inspectStages)
        {
            Console.WriteLine($"{stage}: {pr.StagesDict[stage]}");
        }        
    }

    public void RunStage(string name, string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Cannot find file '{filePath}'.");
            App.End(-1);
        }

        HostHelper helper = new HostHelper();
        Parser pr = new Parser
        {
            Debug = Options.Contains(ParserOptions.Debug)
        };
        string[] allLines = File.ReadAllLines(filePath);
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
        }
        Messages.Info($"Running stage of project: {pr.ProjectName}");

        if (pr.ShellCommands.Count != 0 && !Options.Contains(ParserOptions.SkipShellCommands))
        {
            helper.ExecuteShellCommands(pr.ShellCommands, Options.Contains(ParserOptions.SkipExecutionErrors));
        }
        
        Messages.Info($"Running stage of project: {pr.ProjectName}");
        if (pr.StagesDict.ContainsKey(name))
        {
            var res = helper.ExecuteStage(name, pr.StagesDict[name].Command);
            
            if (!res.GotErrors)
            {
                Messages.Good("No errors provided. Stage passed.");
            }
            else
            {
                if (pr.StagesDict[name].IgnoreErrors || Options.Contains(ParserOptions.SkipExecutionErrors))
                {
                    Messages.Info("Error occur but zeroProbe will ignore it.");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    if (pr.ScriptIfError != "")
                    {
                        Messages.Work("Running undo script...");
                        helper.RunUndoScript(pr.ScriptIfError);
                        Messages.Good("Undo complete.");
                    }
                    App.End(-1);
                }
            }
        }
        else
        {
            Messages.Fatal($"No stage found with name '{name}'. Maybe you forget to define it?");
        }
    }
}