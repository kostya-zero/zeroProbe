using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer {
    public static LexerObject Lex(string line)
    {
        /*
         * Functions type:
         * 0x11f - Comment
         * 0xc88 - Check for components
         * 0xa33 - Undo script
         * 0x805 - Shell command
         * 0xccf - Project name
         * 0x00f - Empty line
         *
         * 0x054 - Stages
         * 0x700 - Set command to stage
         * 0x5fc - Set command if error occur
         * 0x883 - Can ignore errors or not
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
        
        if (line.StartsWith("/*") && line.EndsWith("*/"))
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
            App.End(-1);
        }
        
        string[] split = line.Split(":", 2);

        if (split[0].StartsWith('!'))
        {
            if (!split[0].Contains('.'))
            {
                Messages.Fatal($"(NEW) Bad stage assign syntax. To assign command use: !{split[0]}.command: echo 'Your command'");
                App.End(-1);
            }
            string[] splitCommands = split[0].Trim('!').Split('.');
            switch (splitCommands[1])
            {
                case "command":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0x700",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    break;
                case "on_error":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0x5fc",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    break;
                case "ignore_errors":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0x883",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    break;
                default:
                    Messages.Fatal("Bad stage assign expression.");
                    App.End(-1);
                    break;
            }
        }
        else if (split[0].StartsWith("&"))
        {
            switch (split[0])
            {
                case "&check_for":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xc88",
                        Arguments = split[1]
                    };
                    break;
                case "&if_error":
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
                    App.End(-1);
                    break;
            }
        }    
        else if (split[0].StartsWith("@"))
        {
            lexerObject = new LexerObject
            {
                FunctionType = "0x805",
                Arguments = split[1]
            };
            return lexerObject;
        }
        return lexerObject;
    }
}