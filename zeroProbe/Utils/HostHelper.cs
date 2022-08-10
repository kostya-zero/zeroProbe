namespace zeroProbe.Utils;

public class HostHelper
{
    public void ExecuteShellCommands(List<string> commands, bool ignoreExecErrors)
    {
        Messages.Work("Running shell commands...");
        foreach (var command in commands)
        {
            ScriptHandler shellScript = new ScriptHandler("tmp_shell_command.sh", $"{command}");
            Shell sh = new Shell();
            var setupResult = sh.Execute("/bin/sh", "tmp_shell_command.sh");
            if (setupResult.GotErrors && !ignoreExecErrors)
            {
                Messages.Fatal("Error occured while shell command. Test will be finished. Error:");
                Console.WriteLine(setupResult.Error);
                App.End(-1);
            }
            shellScript.Remove();
        }
    }
}