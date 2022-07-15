using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer
{
    public LexerObject Lex(string line)
    {
        /*
         * Functions type:
         * 0x11f - Comment
         * 0xc88 - Layout
         * 0x054 - Stages
         * 0x700 - Set command to stage
         */
        
        LexerObject lexerObject = new LexerObject();
        if (line.StartsWith("/*") && line.EndsWith("*/"))
        {
            lexerObject.FunctionType = "0x11f";
            return lexerObject;
        }

        if (!line.Contains(':'))
        {
            FuncV.ThrowError($"bad expression -> {line}");
        }
        
        string[] split = line.Split(":", 1);

        switch (split[0])
        {
            case "layout":
                lexerObject.FunctionType = "0xc88";
                lexerObject.FunctionName = split[0];
                lexerObject.
        }
    }
}