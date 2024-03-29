﻿using zeroProbe.Utils;

[assembly: System.Reflection.AssemblyVersion("4.0.*")]

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
                Environment.Exit(0);
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
                        Environment.Exit(-1);
                    }

                    string[] splitStrings = arg.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                    if (splitStrings.Length == 1)
                    {
                        Messages.Fatal($"Nothing provided after equals character: {arg}.");
                        Messages.Hint("Arguments value mustn't be empty!");
                        Environment.Exit(-1);
                    }

                    switch (splitStrings[0])
                    {
                        case "--debug":
                            acts.EnableDebug();
                            break;
                        default:
                            Messages.Fatal($"Unknown argument: {splitStrings[0]}.");
                            Messages.Hint("Run zeroProbe with command 'help' to get list of arguments.");
                            Environment.Exit(-1);
                            break;
                    }
                }
            }

            switch (args[0])
            {
                case "run":
                    acts.RunStages();
                    break;
                case "writeconfig":
                    acts.WriteConfig();
                    break;
                case "asciiart":
                    HelpMessages.AsciiArt();
                    break;
                case "runstage":
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
                    Environment.Exit(-1);
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
