using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        string ConfigFileName = "stages.conf";

        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided. Use 'help' argument to see what zeroProbe can do.");
            App.End();
        }
        Actions acts = new Actions();
        foreach (var arg in args)
        {
            string[] splitStrings = arg.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
            if (splitStrings[0].StartsWith("--"))
            {
                switch (splitStrings[0])
                {
                    case "--file":
                        if (!File.Exists(splitStrings[1]))
                        {
                            Messages.Fatal($"Cannot find file by given path: {splitStrings[1]}");
                            Environment.Exit(0);
                        }
                        ConfigFileName = splitStrings[1];
                        break;
                    case "--debug":
                        acts.Debug = (splitStrings[1] == "1");
                        break;
                    case "--skip-setup":
                        acts.IgnoreSetup = (splitStrings[1] == "1");
                        break;
                    case "--skip-shell-commands":
                        acts.IgnoreShellCommands = (splitStrings[1] == "1");
                        break;
                    case "--ignore-exec-errors":
                        acts.IgnoreExecErrors = (splitStrings[1] == "1");
                        break;
                    case "--ignore-setup-errors":
                        acts.IgnoreSetupErrors = (splitStrings[1] == "1");
                        break;
                    default:
                        Messages.Fatal($"Unknown argument give -> {splitStrings[0]}");
                        App.End();
                        break;
                }
            }
        }

        switch (args[0])
        {
            case "run":
                acts.RunStages(ConfigFileName);
                break;
            case "writeconfig":
                acts.WriteConfig(ConfigFileName);
                break;
            case "inspect":
                acts.InspectStages(ConfigFileName);
                break;
            case "runstage":
                if (args.Length != 2)
                {
                    Console.WriteLine("To run stage you need to enter name of stage you want to run.");
                    Console.WriteLine("Example: zeroProbe runstage build (where 'build' - name of stage).");
                    App.End();
                }
                acts.RunStage(args[1], ConfigFileName);
                break;
            case "help":
                HelpMessages.Help();
                break;
            case "version":
                HelpMessages.Version();
                break;
            case "info":
                HelpMessages.Info();
                break;
            default:
                Messages.Fatal($"Unknown argument -> {args[0]}");
                App.End();
                break;
        }
    }
}