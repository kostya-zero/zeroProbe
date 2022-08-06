using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
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
                    Messages.Fatal($"Argument syntax error -> {arg}.");
                    App.End(-1);
                }
                string[] splitStrings = arg.Split("=", 2, StringSplitOptions.RemoveEmptyEntries);
                if (splitStrings[1] == "")
                {
                    Messages.Fatal($"Nothing provided after equals character -> {arg}");
                    App.End(-1);
                }
                switch (splitStrings[0])
                {
                    case "--file":
                        if (!File.Exists(splitStrings[1]))
                        {
                            Messages.Fatal($"Cannot find file by given path: {splitStrings[1]}");
                            App.End(-1);
                        }

                        Console.WriteLine(Path.GetExtension(splitStrings[1]));
                        if (Path.GetExtension(splitStrings[1]) != "pbc")
                        {
                            Messages.Fatal("File not associate with ProbeConfig. Set file extension to '.pbc'.");
                            App.End(-1);
                        }
                        configFileName = splitStrings[1];
                        break;
                    case "--debug":
                        acts.Debug = splitStrings[1] == "1";
                        break;
                    case "--skip-shell-commands":
                        acts.IgnoreShellCommands = splitStrings[1] == "1";
                        break;
                    case "--ignore-exec-errors":
                        acts.IgnoreExecErrors = splitStrings[1] == "1";
                        break;
                    default:
                        Messages.Fatal($"Unknown argument give -> {splitStrings[0]}");
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
                    Console.WriteLine($"Cannot find {configFileName} file for inspect.");
                    App.End();
                }
                acts.RunStages(configFileName);
                break;
            case "writeconfig":
                acts.WriteConfig(configFileName);
                break;
            case "inspect":
                acts.InspectStages(configFileName);
                break;
            case "runstage":
                if (!File.Exists(configFileName))
                {
                    Console.WriteLine($"Cannot find {configFileName} file for inspect.");
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
            case "version":
                HelpMessages.Version();
                break;
            default:
                Messages.Fatal($"Unknown argument -> {args[0]}");
                App.End(-1);
                break;
        }
    }
}
