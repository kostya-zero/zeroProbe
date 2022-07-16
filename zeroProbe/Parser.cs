using System.Net.Http.Headers;
using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, string> StagesDict = new Dictionary<string, string>();
    public string LayoutType;
    public List<string> Stages = new List<string>();

    public void ParseLine(string line)
    {
        var obj = Lexer.Lex(line);
        Console.WriteLine(obj.FunctionType);
        switch (obj.FunctionType)
        {
            case "0x11f":
                break;
            case "0xc88":
                string layout = obj.Arguments.Trim();
                switch (layout)
                {
                    case "std":
                        LayoutType = "std";
                        break;
                    default:
                        FuncV.ThrowError($"unknown layout -> {layout}");
                        break;
                }
                break;
            case "0x054":
                string stages = obj.Arguments.Trim();
                var split = stages.Split(",");
                foreach (var stage in split)
                {
                    Stages.Add(stage.Trim());
                }
                break;
            case "0x700":
                string stg = obj.FunctionName;
                stg = stg.TrimStart('!');
                if (Stages.Contains(stg))
                {
                    StagesDict.Add(stg, obj.Arguments.Trim());
                }
                else
                {
                    FuncV.ThrowError($"stage '{stg}' not defined.");
                }
                break;
            default:
                FuncV.ThrowError($"Illegal instruction called -> {obj.FunctionType}");
                break;
        }
    }
}