using zeroProbe.Utils;

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("zeroProbe: no argument provided.");
            App.End();
        }

        Actions acts = new Actions();
        switch (args[0])
        {
            case "run":
                acts.RunStages();
                break;
            case "writeconfig":
                acts.WriteConfig();
                break;
            case "inspect":
                acts.InspectStages();
                break;
            case "runstage":
                if (args.Length != 2)
                {
                    Console.WriteLine("To run stage you need to enter name of stage you want to run.");
                    Console.WriteLine("Example: zeroProbe runstage build (where 'build' - name of stage).");
                    App.End();
                }
                acts.RunStage(args[1]);
                break;
            default:
                Console.WriteLine($"zeroProbe: unknown argument -> {args[0]}");
                App.End();
                break;
            case "help":
                HelpMessages.Help();
                break;
            case "version":
                HelpMessages.Version();
                break;
        }
    }
}