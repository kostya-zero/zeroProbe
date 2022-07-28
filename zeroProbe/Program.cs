using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        string ConfigFileName = "stages.conf";
        bool Dbg = false;

        if (args.Length == 0)
        {
            Console.WriteLine("zeroProbe: no argument provided.");
            App.End();
        }

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
                        Dbg = (splitStrings[1] == "1");
                        break;
                }
            }
        }

        Actions acts = new Actions(Dbg);
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
                Console.WriteLine($"zeroProbe: unknown argument -> {args[0]}");
                App.End();
                break;
        }
    }
}