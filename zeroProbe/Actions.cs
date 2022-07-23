using System.Diagnostics;
using System.Text;
using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    public void RunStages()
    {
        if (!File.Exists("stages.conf"))
        {
            FuncV.ThrowError("no stages.conf file found.");
        }

        string[] lines = File.ReadAllLines("stages.conf");
        Parser pr = new Parser();
        foreach (var line in lines)
        {
            pr.ParseLine(line);
        }

        foreach (var stage in pr.Stages)
        {
            if (pr.StagesDict.ContainsKey(stage))
            {
                var cmd = pr.StagesDict[stage];
                File.Create($"tmp_stage_{stage}.sh").Close();
                File.WriteAllText($"tmp_stage_{stage}.sh",$"#!/bin/sh\n{cmd}");
                Process proc = new Process();
                ProcessStartInfo procInfo = new ProcessStartInfo
                {
                    FileName = "/bin/sh",
                    Arguments = $"tmp_stage_{stage}.sh",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                };
                proc.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                proc.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("* ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Running stage: {stage}");
                proc.StartInfo = procInfo;
                proc.Start();
                proc.BeginOutputReadLine();
                string errs = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                if (errs == "")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("** ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("GOOD: No errors provided. Stage passed.");
                    File.Delete($"tmp_stage_{stage}.sh");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("** ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("FATAL: Stage not passed due an error:");
                    Console.WriteLine(errs);
                    App.End();
                }
            }
        }
        Console.WriteLine("** GOOD: Test passed.");
    }

    public void WriteConfig()
    {
        if (File.Exists("stages.conf"))
        {
            Console.WriteLine("You already have configuration file.");
            App.End();
        }
        
        Console.WriteLine("Writing new config file...");
        File.Create("stages.conf").Close();
        File.WriteAllText("stages.conf", @"/* Layouts: */
/* std - Standart for most projects */
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

    public void InspectStages()
    {
        if (!File.Exists("stages.conf"))
        {
            Console.WriteLine("Cannot find stages.conf file for inspect.");
            App.End();
        }
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines("stages.conf");
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
        }
        
        // // Inspecting our details
        // Preparing variables
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

    public void RunStage(string name)
    {
        if (!File.Exists("stages.conf"))
        {
            Console.WriteLine("Cannot find stages.conf file for inspect.");
            App.End();
        }
        Parser pr = new Parser();
        string[] allLines = File.ReadAllLines("stages.conf");
        foreach (var line in allLines)
        {
            pr.ParseLine(line);
        }
        
        if (pr.StagesDict.ContainsKey(name))
        {
            var cmd = pr.StagesDict[name];
            File.Create($"tmp_stage_{name}.sh").Close();
            File.WriteAllText($"tmp_stage_{name}.sh",$"#!/bin/sh\n{cmd}");
            Process proc = new Process();
            ProcessStartInfo procInfo = new ProcessStartInfo();
            procInfo.FileName = "/bin/sh";
            procInfo.Arguments = $"tmp_stage_{name}.sh";
            procInfo.RedirectStandardError = true;
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardInput = true;
            proc.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
            proc.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
            procInfo.CreateNoWindow = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("* ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Running stage: {name}");
            proc.StartInfo = procInfo;
            proc.Start();
            proc.BeginOutputReadLine();
            string errs = proc.StandardError.ReadToEnd();
            proc.WaitForExit();
            if (errs == "")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("** ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("GOOD: No errors provided. Stage passed.");
                File.Delete($"tmp_stage_{name}.sh");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("** ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("FATAL: Stage not passed due an error:");
                Console.WriteLine(errs);
                App.End();
            }
        }
        else
        {
            Console.WriteLine($"No stage found with name '{name}'.");
        }
    }
}