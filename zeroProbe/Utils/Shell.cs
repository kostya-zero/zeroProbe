using System.Diagnostics;
using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Shell
{
    public ExecuteResult Execute(string path, string arguments)
    {
        Process proc = new Process();
        ProcessStartInfo procInfo = new ProcessStartInfo
        {
            FileName = path,
            Arguments = arguments,
            CreateNoWindow = true
        };
        proc.StartInfo = procInfo;
        try
        {
            proc.Start();
        }
        catch (InvalidOperationException)
        {
            Messages.Fatal("Cannot start shell process due to error.");
            Messages.Hint("Check if selected shell are exists in your system.");
            Environment.Exit(-1);
        }
        proc.WaitForExit();
        ExecuteResult execRes = new ExecuteResult
        {
            Executed = true,
            Error = (proc.ExitCode != 0)
        };
        return execRes;
    }
}