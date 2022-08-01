using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer {
    public static LexerObject? Lex(string line)
    {
        /*
         * Functions type:
         * 0x11f - Comment
         * 0xc88 - Check for components
         * 0x054 - Stages
         * 0x700 - Set command to stage
         * 0xa33 - Undo script
         * 0x805 - Shell command
         * 0xccf - Project name
         * 0x00f - Empty line
         */
        
        LexerObject lexerObject = new LexerObject();
        if (line == "")
        {
            lexerObject = new LexerObject
            {
                FunctionType = "0x00f"
            };
            return lexerObject;
        }
        
        if ((line.StartsWith("/*") && line.EndsWith("*/")))
        {
            lexerObject = new LexerObject
            {
                FunctionType = "0x11f"
            };
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
            lexerObject = new LexerObject
            {
                FunctionType = "0x700",
                Arguments = split[1],
                FunctionName = split[0]
            };
        }
        else if (split[0].StartsWith("&"))
        {
            switch (split[0])
            {
                case "&checkfor":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xc88",
                        Arguments = split[1]
                    };
                    break;
                case "&iferror":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xa33",
                        Arguments = split[1]
                    };
                    break;
                case "&stages":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0x054",
                        Arguments = split[1]
                    };
                    break;
                case "&shell":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0x805",
                        Arguments = split[1]
                    };
                    break;
                case "&project":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xccf",
                        Arguments = split[1]
                    };
                    break;
                default:
                    Messages.Fatal($"Unknown expression called -> {split[0]}");
                    App.End();
                    break;
            }
        }    
        else if (split[0].StartsWith("@"))
        {
            lexerObject = new LexerObject
            {
                FunctionType = "0x805",
                Arguments = split[1], 
                FunctionName = split[0]
            };
            return lexerObject;
        }
        
        return lexerObject;
    }
}