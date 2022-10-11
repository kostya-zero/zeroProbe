using System.Diagnostics;
using zeroProbe.Enums;
using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Shell
{
    
    private void OutputHandler(object s, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
    }
    
    private void ErrorHandler(object s, DataReceivedEventArgs e)
    {
        Console.WriteLine(e.Data);
    }
    
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
        proc.OutputDataReceived += OutputHandler;
        proc.ErrorDataReceived += ErrorHandler;
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
        proc.BeginOutputReadLine();
        string errs = proc.StandardError.ReadToEnd();
        proc.WaitForExit();
        ExecuteResult execRes = new ExecuteResult
        {
            Executed = true,
            GotErrors = errs != "",
            Error = errs
        };
        return execRes;
    }
}