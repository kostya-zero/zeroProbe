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
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true,
            CreateNoWindow = true
        };
        proc.OutputDataReceived += (s, e) => Console.WriteLine(e.Data);
        proc.ErrorDataReceived += (s, e) => Console.WriteLine(e.Data);
        proc.StartInfo = procInfo;
        proc.Start();
        proc.BeginOutputReadLine();
        string errs = proc.StandardError.ReadToEnd();
        proc.WaitForExit();
        ExecuteResult execRes = new ExecuteResult
        {
            Executed = true,
            GotErrors = (errs != ""),
            Error = errs,
            ExitCode = proc.ExitCode
        };
        return execRes;
    }
}