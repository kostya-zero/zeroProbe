using zeroProbe.Enums;
using zeroProbe.Models;

namespace zeroProbe.Utils;

public class HostHelper
{
    public void CheckComponents(List<string> components)
    {
        List<string> pathVariable = new List<string>();
        var envPath = Environment.GetEnvironmentVariable("PATH")?.Split(':');
        if (envPath != null)
        {
            pathVariable = new List<string>(envPath);
        }
        List<string> notFoundComponents = new List<string>();
        bool foundComponent = false;
        foreach (var component in components)
        {
            foreach (var path in pathVariable)
            {
                foundComponent = File.Exists($"{path}/{component}");
                
                if (foundComponent)
                {
                    break;
                }
            }
            
            if (foundComponent)
            {
                Messages.Good($"{component} was found!");
                break;
            }

            if (!foundComponent)
            {
                notFoundComponents.Add(component);
                Messages.Warning($"{component} not found!");
            }
        }

        if (notFoundComponents.Count > 0)
        {
            Messages.Fatal($"{notFoundComponents.Count.ToString()} not found! Cannot continue.");
            Environment.Exit(-1);
        }
    }

    public ExecuteResult ExecuteCommand(string commandToExecute, string fileName, string shell)
    {
        ScriptHandler script = new ScriptHandler(fileName, commandToExecute);
        Shell sh = new Shell();
        var res = sh.Execute(shell, fileName);
        script.Remove();
        return res;
    }

    public ExecuteResult ExecuteStage(string stageName, string command, string shell)
    {
        if (shell == "")
        {
            shell = "/bin/sh";
        }
        var res = ExecuteCommand(command, $"tmp_stage_{stageName}.sh", shell);
        return res;
    }
}