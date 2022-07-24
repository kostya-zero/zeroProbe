namespace zeroProbe.Models;

public class ExecuteResult
{
    public bool Executed { get; set; }
    public string Error { get; set; }
    public bool GotErrors { get; set; }
    public int ExitCode { get; set; }

    public ExecuteResult()
    {
        Executed = false;
        Error = "";
        GotErrors = false;
        ExitCode = 0;
    }
}