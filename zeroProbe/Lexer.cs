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
         * 0xa33 - Setup
         * 0x805 - Shell command
         * 0xccf - Project name
         * 0x00f - Empty line
         */
        
        LexerObject lexerObject = new LexerObject();
        if (line == "")
        {
            lexerObject.FunctionType = "0x00f";
            return lexerObject;
        }
        
        if ((line.StartsWith("/*") && line.EndsWith("*/")))
        {
            lexerObject.FunctionType = "0x11f";
            return lexerObject;
        }

        if (!line.Contains(':'))
        {
            Messages.Fatal($"Bad expression -> {line}");
            App.End();
        }
        
        string[] split = line.Split(":", 2);

        if (split[0].StartsWith('!'))
        {
            lexerObject.FunctionType = "0x700";
            lexerObject.FunctionName = split[0];
            lexerObject.Arguments = split[1];
        }
        else if (split[0].StartsWith("&"))
        {
            switch (split[0])
            {
                case "&msg":
                    lexerObject.FunctionType = "0xc88";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                case "&stages":
                    lexerObject.FunctionType = "0x054";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                case "&shell":
                    lexerObject.FunctionType = "0x805";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                case "&project":
                    lexerObject.FunctionType = "0xccf";
                    lexerObject.FunctionName = split[0];
                    lexerObject.Arguments = split[1];
                    break;
                default:
                    Messages.Fatal($"Unknown expression called -> {split[0]}");
                    App.End();
                    break;
            }
        }    
        else if (split[0].StartsWith("@"))
        {
            lexerObject.FunctionType = "0x805";
            lexerObject.FunctionName = split[0];
            lexerObject.Arguments = split[1];
            return lexerObject;
        }
        
        return lexerObject;
    }
}