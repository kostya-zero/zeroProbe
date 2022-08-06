namespace zeroProbe.Models;

public class LexerObject
{
    public string FunctionType { get; set; }
    public string Arguments { get; set; }
    public StageObject StageObject { get; set; }

    public LexerObject()
    {
        FunctionType = "";
        Arguments = "";
    }
}