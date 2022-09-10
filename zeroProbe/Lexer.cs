using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer {
    public static LexerObject Lex(string line, int lineNumber)
    {
        /*
         * Functions type:
         * 0x11f - Comment.
         * 0xc88 - Check for components.
         * 0xa33 - Undo script.
         * 0x805 - Set shell.
         * 0x6b8 - Project name.
         * 0x00f - Empty line.
         *
         * 0x054 - Stages.
         * 0x700 - Add command to stage.
         * 0x5fc - Set command if error occur.
         * 0x883 - Can ignore errors or not.
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
            Messages.Fatal("Bad line expression.");
            Messages.TraceBack(line, lineNumber);
            Messages.Hint("Every line must have command and argument divided by double dots (:).");
            App.End(-1);
        }
        
        string[] split = line.Split(":", 2);

        if (split[0].StartsWith('!'))
        {
            if (!split[0].Contains('.'))
            {
                Messages.Fatal("Bad stage assign syntax.");
                Messages.TraceBack(line, lineNumber);
                Messages.Hint($"To assign command use: !{split[0].TrimStart('!')}.command: echo 'Your command'"); 
                App.End(-1);
            }
            string[] splitCommands = split[0].Trim('!').Split('.');
            switch (splitCommands[1])
            {
                case "add_command":
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
                default:
                    Messages.Fatal("Unknown stage property.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Learn more about stages property at zeroProbe wiki.");
                    App.End(-1);
                    break;
            }
        }
        switch (split[0])
        {
            case "check_for":
                lexerObject = new LexerObject
                {
                    FunctionType = "0xc88",
                    Arguments = split[1]
                };
                break;
            case "stages":
                lexerObject = new LexerObject
                {
                    FunctionType = "0x054",
                    Arguments = split[1]
                };
                break;
            case "shell":
                lexerObject = new LexerObject
                {
                    FunctionType = "0x805",
                    Arguments = split[1]
                };
                break;
            case "project":
                lexerObject = new LexerObject
                {
                    FunctionType = "0x6b8",
                    Arguments = split[1]
                };
                break;
            default:
                Messages.Fatal("Unknown command called.");
                Messages.TraceBack(line, lineNumber);
                Messages.Hint("Learn about available commands at zeroProbe wiki.");
                App.End(-1);
                break;
        }
        
        return lexerObject;
    }
}