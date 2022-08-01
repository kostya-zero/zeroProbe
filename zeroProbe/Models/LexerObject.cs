namespace zeroProbe.Models;

public class LexerObject
{
    public string FunctionType { get; set; }
    public string Arguments { get; set; }
    public string FunctionName { get; set; }

    public LexerObject()
    {
        FunctionName = "";
        FunctionType = "";
        Arguments = "";
    }
}