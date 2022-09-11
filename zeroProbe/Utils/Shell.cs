using System.Diagnostics;
using zeroProbe.Enums;
using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Shell
{
    private bool noSendOutput { get; set; } 
    
    private void OutputHandler(object s, DataReceivedEventArgs e)
    {
        if (!noSendOutput)
        {
            Console.WriteLine(e.Data);
        }
    }
    
    private void ErrorHandler(object s, DataReceivedEventArgs e)
    {
        if (!noSendOutput)
        {
            Console.WriteLine(e.Data);
        }
    }
    
    public ExecuteResult Execute(string path, string arguments, List<ExecutionOptions> optionsList)
    {
        noSendOutput = optionsList.Contains(ExecutionOptions.NoOutputToConsole);
        Process proc = new Process();
        ProcessStartInfo procInfo = new ProcessStartInfo
        {
            FileName = path,
            Arguments = arguments,
            RedirectStandardError = true,
            RedirectStandardOutput = !noSendOutput,
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
            App.End(-1);
        }
        proc.BeginOutputReadLine();
        string errs = proc.StandardError.ReadToEnd();
        proc.WaitForExit();
        List<string> outputLines = new List<string>();
        if (optionsList.Contains(ExecutionOptions.ReturnOutput))
        {
            while (!proc.StandardOutput.EndOfStream)
            {
                outputLines.Add(proc.StandardOutput.ReadLine() ?? "");
            }
        }
        ExecuteResult execRes = new ExecuteResult
        {
            Executed = true,
            GotErrors = errs != "",
            Error = errs,
            Output = outputLines
        };
        return execRes;
    }
}