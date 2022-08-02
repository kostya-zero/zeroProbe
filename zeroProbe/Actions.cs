using System.Text;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    public bool Debug { get; set; }
    public bool IgnoreShellCommands { get; set; }
    public bool IgnoreExecErrors { get; set; }

    public Actions()
    {
        Debug = false;
        IgnoreShellCommands = false;
        IgnoreExecErrors = false;
    }
    
    public void RunStages(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Messages.Fatal($"No {filePath} file found.");
            App.End();
        }

        string[] lines = File.ReadAllLines(filePath);
        Parser pr = new Parser
        {
            Debug = Debug
        };
        
        foreach (var line in lines)
        {
            pr.ParseLine(line);
        }
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
                        bool found = FSHelper.IsExists($"{path}/{component}");
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
                Messages.Fatal($"{missingComponentsCount.ToString()} are missing. Components:");
                App.End(-1);
            }
            else
            {
                Messages.Good("All components installed!");
            }
        }

        if (pr.ShellCommands.Count != 0 && !IgnoreShellCommands)
        {
            Messages.Work("Running shell commands...");
            foreach (var command in pr.ShellCommands)
            {
                ScriptHandler shellScript = new ScriptHandler
                {
                    ScriptPath = "tmp_shell_command.sh",
                    ScriptContent = $"#!/bin/sh\n{command}"
                };
                shellScript.GenScript();
                Shell sh = new Shell();
                var setupResult = sh.Execute("/bin/sh", "tmp_shell_command.sh");
                if (setupResult.GotErrors && !IgnoreExecErrors)
                {
                    Messages.Fatal("Error occured while shell command. Test will be finished. Error:");
                    Console.WriteLine(setupResult.Error);
                    App.End(-1);
                }
                shellScript.Remove();
            }
        }
        
        foreach (var stage in pr.Stages)
        {
            if (pr.StagesDict.ContainsKey(stage))
            {
                var cmd = pr.StagesDict[stage];
                ScriptHandler script = new ScriptHandler
                {
                    ScriptPath = $"tmp_stage_{stage}.sh",
                    ScriptContent = $"#!/bin/sh\n{cmd}"
                };
                script.GenScript();
                Messages.Work($"Running stage: {stage}");
                Shell sh = new Shell();
                var res = sh.Execute("/bin/sh", $"tmp_stage_{stage}.sh");
                
                if (!res.GotErrors || IgnoreExecErrors)
                {
                    Messages.Good("No errors provided. Stage passed.");
                    File.Delete($"tmp_stage_{stage}.sh");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    if (pr.ScriptIfError != "")
                    {
                        Messages.Work("Running undo script...");
                        ScriptHandler undoScript = new ScriptHandler
                        {
                            ScriptPath = "tmp_undo_script.sh",
                            ScriptContent = $"#!/bin/sh\n{pr.ScriptIfError}"
                        };
                        undoScript.GenScript();
                        Shell undoSh = new Shell();
                        undoSh.Execute("/bin/sh", "tmp_undo_script.sh");
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
        File.Create("stages.conf").Close();
        File.WriteAllText("stages.conf", @"/* Small example of configuration file */
/* Test of building zeroProbe */
&project: test
&shell: echo 'This is a small template for screenshot!'
&stages: restore, build, finish

!restore: echo 'Doing some restore staff...'
!build: echo 'Doing some build staff...'
!finish: echo 'Finishing this deal...'");
        Console.WriteLine("Template config ready!");
    }

    public void InspectStages(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Cannot find {filePath} file for inspect.");
            App.End();
        }
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines(filePath);
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
        }
        
        List<string> inspectStages = pr.Stages;
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
        Console.WriteLine("Shell commands: " + (pr.ShellCommands.Count == 0 ? "None" : pr.ShellCommands.Count.ToString()));
        StringBuilder reqBuilder = new StringBuilder();
        foreach (string component in pr.ComponentsToCheck)
        {
            reqBuilder.Append($"{component} ");
        }
        Console.WriteLine("Required components: " + reqBuilder.ToString().Trim() == "" ? "None" : reqBuilder.ToString().Trim() );
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
            Console.WriteLine($"Cannot find {filePath} file for inspect.");
            App.End(-1);
        }
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines(filePath);
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
        }
        Messages.Info($"Running stage of project: {pr.ProjectName}");

        if (pr.ShellCommands.Count != 0)
        {
            Messages.Work("Running shell commands...");
            foreach (var command in pr.ShellCommands)
            {
                ScriptHandler shellScript = new ScriptHandler
                {
                    ScriptPath = "tmp_shell_command.sh",
                    ScriptContent = $"#!/bin/sh\n{command}"
                };
                shellScript.GenScript();
                Shell sh = new Shell();
                var setupResult = sh.Execute("/bin/sh", "tmp_setup.sh");
                if (setupResult.GotErrors)
                {
                    Messages.Fatal("Error occured while shell command. Test will be finished. Error:");
                    Console.WriteLine(setupResult.Error);
                    App.End(-1);
                }
                shellScript.Remove();
            }
        }
        
        Messages.Info($"Running stage of project: {pr.ProjectName}");
        if (pr.StagesDict.ContainsKey(name))
        {
            var cmd = pr.StagesDict[name];
            ScriptHandler script = new ScriptHandler
            {
                ScriptPath = $"tmp_stage_{name}.sh",
                ScriptContent = $"#!/bin/sh\n{cmd}"
            };
            script.GenScript();
            Messages.Work($"Running stage: {name}");
            Shell sh = new Shell();
            var res = sh.Execute("/bin/sh", $"tmp_stage_{name}.sh");
                
            if (!res.GotErrors || IgnoreExecErrors)
            {
                Messages.Good("No errors provided. Stage passed.");
                File.Delete($"tmp_stage_{name}.sh");
            }
            else
            {
                Messages.Fatal("Stage not passed due an error:");
                Console.WriteLine(res.Error);
                if (pr.ScriptIfError != "")
                {
                    Messages.Work("Running undo script...");
                    ScriptHandler undoScript = new ScriptHandler
                    {
                        ScriptPath = "tmp_undo_script.sh",
                        ScriptContent = $"#!/bin/sh\n{pr.ScriptIfError}"
                    };
                    undoScript.GenScript();
                    Shell undoSh = new Shell();
                    undoSh.Execute("/bin/sh", "tmp_undo_script.sh");
                    Messages.Good("Undo complete.");
                }
                App.End(-1);
            }
        }
        else
        {
            Messages.Fatal($"No stage found with name '{name}'.");
        }
    }
}