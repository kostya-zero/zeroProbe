using zeroProbe.Utils;

[assembly: System.Reflection.AssemblyVersion("4.0.*")]

namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Terminal.Info("No arguments provided.");
            Terminal.Hint("Use 'help' argument to see what zeroProbe can do.");
            Environment.Exit(0);
        }

        Actions acts = new Actions();
        switch (args[0])
        {
            case "run":
                Runner runner = new Runner();
                runner.CheckConfig();
                runner.Run();
                break;
            case "writeconfig":
                break;
            case "runstage":
                break;
            case "help":
                break;
            case "version":
                break;
            default:
                Terminal.Fatal($"Unknown argument '{args[0]}'.");
                Terminal.Hint("Run zeroProbe with command 'help' to get list of arguments and actions.");
                Environment.Exit(-1);
                break;
        }
    }
}
