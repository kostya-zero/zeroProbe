using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer {
    public static LexerObject Lex(string line)
    {
        /*
         * Functions type:
         * 0x11f - Comment
         * 0xc88 - Layout
         * 0x054 - Stages
         * 0x700 - Set command to stage
         */
        
        LexerObject lexerObject = new LexerObject();
        if ((line.StartsWith("/*") && line.EndsWith("*/")) || line == "")
        {
            lexerObject.FunctionType = "0x11f";
            return lexerObject;
        }

        if (!line.Contains(':'))
        {
            FuncV.ThrowError($"bad expression -> {line}");
        }
        
        string[] split = line.Split(":", 2);

        if (split[0].StartsWith('!'))
        {
            lexerObject.FunctionType = "0x700";
            lexerObject.FunctionName = split[0];
            lexerObject.Arguments = split[1];
        }
        else
        {
            switch (split[0])
            {
                case "layout":
                    lexerObject.FunctionType = "0xc88";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                case "stages":
                    lexerObject.FunctionType = "0x054";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                default:
                    FuncV.ThrowError($"Unknown expression called -> {split[0]}");
                    break;
            }
        }
        
        return lexerObject;
    }
}