using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer {
    public static LexerObject Lex(string line, int lineNumber)
    {
        /*
         * Functions type:
         * 0x00f - Comment or empty line.
         * 0xc88 - Check for components.
         * 0xa33 - Undo script.
         * 0x805 - Set shell.
         * 0x6b8 - Project name.
         * 0xa58 - Architecture
         *
         * 0xs54 - Stages.
         * 0xs00 - Add command to stage.
         * 0xs34 - Set command if error occur.
         * 0xs83 - Can ignore errors or not.
         * 0xs55 - Change directory.
         */
        
        LexerObject lexerObject = new LexerObject();
        
        if (line.StartsWith("//") || line == "")
        {
            lexerObject = new LexerObject
            {
                FunctionType = "0x00f"
            };
            return lexerObject;
        }

        if (!line.Contains(':'))
        {
            Messages.Fatal("Bad line expression.");
            Messages.TraceBack(line, lineNumber);
            Messages.Hint("Every line must have command and argument divided by double dots (:).");
            Environment.Exit(-1);
        }
        
        string[] split = line.Split(":", 2);

        if (split[0].StartsWith(';'))
        {
            if (!split[0].Contains('.'))
            {
                Messages.Fatal("Bad stage assign syntax.");
                Messages.TraceBack(line, lineNumber);
                Messages.Hint($"To assign command use: !{split[0].TrimStart(';')}.command: echo 'Your command'"); 
                Environment.Exit(-1);
            }
            string[] splitCommands = split[0].Trim(';').Split('.', 2);
            Console.WriteLine(splitCommands[0]);
            Console.WriteLine(splitCommands[1]);
            switch (splitCommands[1])
            {
                case "add_command":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xs00",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    return lexerObject;
                case "ignore_errors":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xs83",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    return lexerObject;
                case "on_error":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xs34",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    return lexerObject;
                case "dir":
                    lexerObject = new LexerObject
                    {
                        FunctionType = "0xs55",
                        StageObject = new StageObject
                        {
                            StageName = splitCommands[0],
                            StageCommand = split[1].Trim()
                        }
                    };
                    return lexerObject;
                default:
                    Messages.Fatal("Unknown stage property.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Learn more about stages property at zeroProbe wiki.");
                    Environment.Exit(-1);
                    break;
            }
        }
        switch (split[0])
        {
            case "check_for":
                lexerObject = new LexerObject
                {
                    FunctionType = "0xc88",
                    Arguments = split[1].Trim()
                };
                break;
            case "stages":
                lexerObject = new LexerObject
                {
                    FunctionType = "0xs54",
                    Arguments = split[1].Trim()
                };
                break;
            case "shell":
                lexerObject = new LexerObject
                {
                    FunctionType = "0x805",
                    Arguments = split[1].Trim()
                };
                break;
            case "project":
                lexerObject = new LexerObject
                {
                    FunctionType = "0x6b8",
                    Arguments = split[1].Trim()
                };
                break;
            case "arch":
                lexerObject = new LexerObject
                {
                    FunctionType = "0xa58",
                    Arguments = split[1].Trim()
                };
                break;
            default:
                if (!split[0].StartsWith("!"))
                {
                    Messages.Fatal("Unknown command called.");
                    Messages.TraceBack(line, lineNumber);
                    Messages.Hint("Learn about available commands at zeroProbe wiki.");
                    Environment.Exit(-1);
                }
                break;
        }
        
        return lexerObject;
    }
}