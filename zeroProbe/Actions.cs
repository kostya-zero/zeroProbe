using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;
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
        Handlers hdl = new Handlers();
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
                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = "/bin/sh";
                procInfo.Arguments = $"tmp_stage_{stage}.sh";
                procInfo.RedirectStandardError = true;
                procInfo.RedirectStandardOutput = true;
                procInfo.RedirectStandardInput = true;
                proc.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
                proc.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
                procInfo.CreateNoWindow = true;
                Console.WriteLine($"* Running stage: {stage}");
                proc.StartInfo = procInfo;
                proc.Start();
                proc.BeginOutputReadLine();
                string errs = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                if (errs == "")
                {
                    Console.WriteLine("** GOOD: No errors provided. Stage passed.");
                    File.Delete($"tmp_stage_{stage}.sh");
                }
                else
                {
                    Console.WriteLine("** FATAL: Stage not passed due an error:");
                    Console.WriteLine(errs);
                    Environment.Exit(0);
                }

            }
        }
        Console.WriteLine("** GOOD: Test passed.");
    }
}