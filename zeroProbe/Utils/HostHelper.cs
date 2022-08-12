using zeroProbe.Models;

namespace zeroProbe.Utils;

public class HostHelper
{
    /*
     * > NOTICE
     * ignoreExecErrors will be deprecated and removed with "--ignore-exec-errors" argument.
     */
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

    public void CheckComponents()
    {
        /*
         * > NOTICE
         * Make components to check script (or copy from Actions.cs).
         */
    }

    public ExecuteResult ExecuteCommand(string commandToExecute, string fileName)
    {
        ScriptHandler script = new ScriptHandler(fileName, commandToExecute);
        Shell sh = new Shell();
        var res = sh.Execute("/bin/sh", fileName);
        script.Remove();
        return res;
    }

    public void RunUndoScript(string commandToExecute)
    {
        ExecuteCommand(commandToExecute, "tmp_undo_script.sh");
    }

    public ExecuteResult ExecuteStage(string stageName, string command)
    {
        var res = ExecuteCommand(command, $"tmp_stage_{stageName}.sh");
        return res;
    }
}