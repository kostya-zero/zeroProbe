using zeroProbe.Enums;
using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;
        string configFileName = "stages.pbc";

        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided. Use 'help' argument to see what zeroProbe can do.");
            App.End();
        }
        Actions acts = new Actions();
        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                if (!arg.Contains('='))
                {
                    Messages.Fatal($"Argument syntax error: {arg}.");
                    Messages.Hint("Arguments must looks like this: argument=value");
                    App.End(-1);
                }
                string[] splitStrings = arg.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                if (splitStrings.Length == 1)
                {
                    Messages.Fatal($"Nothing provided after equals character: {arg}.");
                    Messages.Hint("Arguments value mustn't be empty!");
                    App.End(-1);
                }
                switch (splitStrings[0])
                {
                    case "--file":
                        if (!File.Exists(splitStrings[1]))
                        {
                            Messages.Fatal($"Looks like '{splitStrings[1]}' not exists.");
                            Messages.Hint("Try to write template configuration with 'writeconfig' action.");
                            App.End(-1);
                        }

                        Console.WriteLine(Path.GetExtension(splitStrings[1]));
                        if (Path.GetExtension(splitStrings[1]) != "pbc")
                        {
                            Messages.Fatal("File not associate with ProbeConfig.");
                            Messages.Hint("Set file extension to '.pbc'.");
                            App.End(-1);
                        }
                        configFileName = splitStrings[1];
                        break;
                    case "--debug":
                        acts.AddOption(ParserOptions.Debug, "--debug");
                        break;
                    case "--skip-shell-commands":
                        acts.AddOption(ParserOptions.Debug, "--skip-shell-commands");
                        break;
                    case "--skip-shell-commands-errors":
                        acts.AddOption(ParserOptions.SkipShellCommandsErrors,
                            "--skip-shell-commands-errors");
                        break;
                    default:
                        Messages.Fatal($"Unknown argument: {splitStrings[0]}.");
                        Messages.Hint("Run zeroProbe with command 'help' to get list of arguments.");
                        App.End(-1);
                        break;
                }
            }
        }

        switch (args[0])
        {
            case "run":
                if (!File.Exists(configFileName))
                {
                    Messages.Fatal($"Looks like '{configFileName}' not exists.");
                    Messages.Hint("Try to write template configuration with 'writeconfig'.");
                    App.End();
                }
                acts.RunStages(configFileName);
                break;
            case "writeconfig":
                acts.WriteConfig(configFileName);
                break;
            case "asciiart":
                HelpMessages.AsciiArt();
                break;
            case "runstage":
                if (!File.Exists(configFileName))
                {
                    Messages.Fatal($"Looks like '{configFileName}' not exists.");
                    Messages.Hint("Try to write template configuration with 'writeconfig'.");
                    App.End();
                }
                if (args.Length != 2)
                {
                    Console.WriteLine("To run stage you need to enter name of stage you want to run.");
                    Console.WriteLine("Example: zeroProbe runstage build (where 'build' - name of stage).");
                    App.End();
                }
                acts.RunStage(args[1], configFileName);
                break;
            case "help":
                HelpMessages.Help();
                break;
            case "wiki":
                HelpMessages.Wiki();
                break;
            case "version":
                HelpMessages.Version();
                break;
            default:
                Messages.Fatal($"Unknown argument '{args[0]}'.");
                Messages.Hint("Run zeroProbe with command 'help' to get list of arguments and actions.");
                App.End(-1);
                break;
        }
    }
}
