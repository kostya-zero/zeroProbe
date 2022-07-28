using System.Text;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    public bool Debug { get; set; }

    public Actions(bool dbg)
    {
        Debug = dbg;
    }
    
    public void RunStages(string filePath)
    {
        if (!File.Exists(filePath))
        {
            FuncV.ThrowError($"no {filePath} file found.");
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
                
                if (res.GotErrors == false)
                {
                    Messages.Good("No errors provided. Stage passed.");
                    File.Delete($"tmp_stage_{stage}.sh");
                }
                else
                {
                    Messages.Fatal("Stage not passed due an error:");
                    Console.WriteLine(res.Error);
                    App.End();
                }
            }
        }
        Messages.Good("Test passed.");
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
        File.WriteAllText("stages.conf", @"/* Layouts: */
/* std - Standard for most projects */
layout: std

/* Stages */
/* Warning: stage will be executed in queue that you typed after 'stages' */
stages: restore, build

/* Setting commands for stages */
/* To setup command to execute for stage you must start line with '!' operator. */
!restore: echo 'Restore'
!build: echo 'Build'");
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
        
        string inspectLayout = pr.LayoutType;
        List<string> inspectStages = pr.Stages;
        string inspectStagesCount = inspectStages.Count.ToString();
        bool inspectNoStages = false;
        string strStages = "";
        
        if (inspectStages.Count == 0)
        {
            inspectNoStages = true;
        }
        
        
        Console.WriteLine(":::: Inspection results");
        Console.WriteLine(":: Basic");
        Console.WriteLine($"Layout: {inspectLayout}\n");
        if (inspectNoStages)
        {
            Console.WriteLine(":: Stages");
            Console.WriteLine($"No stages in this configuration!");
            App.End();
        }
        if (inspectStages.Count == 1)
        {
            strStages = inspectStages[0];
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var stage in inspectStages)
            {
                stringBuilder.Append($"{stage}, ");
            }
            strStages = stringBuilder.ToString();
            strStages = strStages.TrimEnd();
            strStages = strStages.TrimEnd(',');
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
            App.End();
        }
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines(filePath);
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
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
                
            if (res.GotErrors == false)
            {
                Messages.Good("No errors provided. Stage passed.");
                File.Delete($"tmp_stage_{name}.sh");
            }
            else
            {
                Messages.Fatal("Stage not passed due an error:");
                Console.WriteLine(res.Error);
                App.End();
            }
        }
        else
        {
            Messages.Fatal($"No stage found with name '{name}'.");
        }
    }
}