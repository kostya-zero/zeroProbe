using zeroProbe.Enums;
using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (args.Length == 0)
            {
                Messages.Info("No arguments provided.");
                Messages.Hint("Use 'help' argument to see what zeroProbe can do.");
                App.End();
            }

            Actions acts = new Actions
            {
                FilePath = "stages.pbc"
            };
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
                            acts.FilePath = splitStrings[1];
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
                    if (!File.Exists(acts.FilePath))
                    {
                        Messages.Fatal($"Looks like '{acts.FilePath}' not exists.");
                        Messages.Hint("Try to write template configuration with 'writeconfig'.");
                        App.End();
                    }

                    acts.RunStages();
                    break;
                case "writeconfig":
                    acts.WriteConfig();
                    break;
                case "asciiart":
                    HelpMessages.AsciiArt();
                    break;
                case "runstage":
                    if (!File.Exists(acts.FilePath))
                    {
                        Messages.Fatal($"Looks like '{acts.FilePath}' not exists.");
                        Messages.Hint("Try to write template configuration with 'writeconfig'.");
                        App.End();
                    }

                    if (args.Length != 2)
                    {
                        Console.WriteLine("To run stage you need to enter name of stage you want to run.");
                        Console.WriteLine("Example: zeroProbe runstage build (where 'build' - name of stage).");
                        App.End();
                    }

                    acts.RunStage(args[1]);
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
        catch (InvalidOperationException ex)
        {
            Messages.Fatal("zeroProbe got an exception and was immediately stopped.");
            Messages.Debug(ex.Message);
            Messages.Debug(ex.StackTrace ?? "");
            Messages.Info("Please, report this accident to developers to fix it.");
        }
        catch (Exception ex)
        {
            Messages.Fatal("zeroProbe got an exception and was immediately stopped.");
            Messages.Debug(ex.Message);
            Messages.Info("Please, report this accident to developers to fix it.");
        }
    }
}
