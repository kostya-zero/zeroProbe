namespace zeroProbe.Models;

public class ExecuteResult
{
    public bool Executed { get; init; }
    public bool Error { get; init; }
    public bool GotErrors { get; init; }
    public List<string> Output { get; set; } = new List<string>();

}