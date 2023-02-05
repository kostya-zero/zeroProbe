using System.Diagnostics;
using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Runner
{
     private LexObject project { get; set; }

     public void CheckConfig()
     {
         if (!File.Exists("stages.pcf"))
         {
             Terminal.Fatal("Cannot find configuration file (stages.pcf).");
         }
     }

     private bool CheckExecutable(string name)
     {
         string[] path = Environment.GetEnvironmentVariable("PATH").Split(':');
         foreach (var dest in path)
         {
             if (File.Exists(dest + "/" + name))
             {
                 return true;
             }
         }

         return false;
     }

     private void CheckRequired()
     {
         int notFoundCount = 0;
         foreach (string component in project.Required)
         {
             bool componentFound = CheckExecutable(component);
             if (componentFound)
             {
                 Terminal.Good($"{component} found.");
             }

             if (!componentFound)
             {
                 Terminal.Fatal($"{component} not found.");
                 notFoundCount++;
             }
         }

         if (notFoundCount > 0)
         {
             Terminal.Fatal($"{notFoundCount.ToString()} components not found.");
             Terminal.Exit(2);
         }
     }

     private bool RunCommand(string command)
     {
         Process proc = new Process();
         proc.StartInfo = new ProcessStartInfo
         {
             FileName = project.Shell,
             Arguments = $"-c \"{command}\"",
             CreateNoWindow = true
         };
         Terminal.Work(command);
         proc.Start();
         proc.WaitForExit();
         if (proc.ExitCode != 0)
         {
             return false;
         }

         return true;
     }

     public void Run()
     {
         project = Lexer.Lex(File.ReadAllLines("stages.pcf"));
         Terminal.Info($"zeroProbe {VersionInfo.Version}");
         Terminal.Info($"Project: {project.Project}");
         Terminal.Info($"Author: {project.Author}");
         if (project.Required.Length != 0)
         {
             Terminal.Info("Checking for required components...");
             CheckRequired();
         }

         if (project.Stages.Count == 0)
         {
             Terminal.Info("No stage specified! Finishing.");
             Terminal.Exit(0);
         }

         foreach (string stage in project.Stages)
         {
             Terminal.Info($"Running stage - {project.StagesContainer[stage].Name}");

             if (!project.StagesContainer.ContainsKey(stage))
             {
                 Terminal.Fatal($"Object for {stage} stage not found!");
                 Terminal.Exit(4);
             }
             
             StageModel stageModel = project.StagesContainer[stage];
             if (stageModel.Commands.Count == 0)
             {
                 Terminal.Warning($"'{stage}' stage haven't any commands! Skipping...");
                 break;
             }

             foreach (string command in stageModel.Commands)
             {
                 bool executeResult = RunCommand(command);
                 if (!executeResult)
                 {
                     if (!stageModel.PredictFail)
                     {
                         Terminal.Fatal("Command executed with bad exit code. Ending tests...");
                         Terminal.Exit(4);
                     }
                 }

                 if (executeResult)
                 {
                     if (stageModel.PredictFail)
                     {
                         Terminal.Fatal("Command finished with good exit code, but we expected a bad one.");
                         Terminal.Exit(4);
                     }
                 }
             }
             
             Terminal.Good("Stage finished.");
         }
         
         Terminal.Good("Stages finished. No errors reported.");
     }
}
