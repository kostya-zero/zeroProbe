using zeroProbe.Models;

namespace zeroProbe.Utils;

public class HostHelper
{
    public void ExecuteShellCommands(List<string> commands, bool ignoreExecErrors)
    {
        Messages.Work("Running shell commands...");
        foreach (var command in commands)
        {
            ExecuteResult executeResult = ExecuteCommand(command, "tmp_shell_command.sh");
            if (executeResult.GotErrors && !ignoreExecErrors)
            {
                Messages.Fatal("Error occured while shell command. Test will be finished. Error:");
                Console.WriteLine(executeResult.Error);
                App.End(-1);
            }
        }
    }

    public void CheckComponents(List<string> components)
    {
        List<string> pathVariable = Env.GetPath();
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
            App.End();
        }
    }

    public ExecuteResult ExecuteCommand(string commandToExecute, string fileName)
    {
        ScriptHandler script = new ScriptHandler(fileName, commandToExecute);
        Shell sh = new Shell();
        var res = sh.Execute("/bin/sh", fileName);
        script.Remove();
        return res;
    }

    public ExecuteResult ExecuteStage(string stageName, string command)
    {
        var res = ExecuteCommand(command, $"tmp_stage_{stageName}.sh");
        return res;
    }
}