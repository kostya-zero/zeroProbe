using zeroProbe.Utils;
namespace zeroProbe;

public class HelpMessages
{
    public static void Help()
    {
        Console.WriteLine("zeroProbe 1.0 // Advanced utility to create, run and manage tests. Powered by .NET 6.");
        Console.WriteLine("This is a help message. ");
        Console.WriteLine("usage: zeroProbe [action] [option,...]\n");
        Console.WriteLine(":: Actions");
        Console.WriteLine("help             - shows this message.");
        Console.WriteLine("version          - return version of zeroProbe.");
        Console.WriteLine("info             - information about environment.");
        Console.WriteLine("writeconfig      - writes template config.");
        Console.WriteLine("run              - runs all stages in stages.conf.");
        Console.WriteLine("inspect          - inspect configuration file.");
        Console.WriteLine("runstage [stage] - runs stage standalone.\n");
        Console.WriteLine(":: Options");
        Console.WriteLine("--debug=[0,1]              - Enables debug mode.");
        Console.WriteLine("--file=[path]              - Use file by given location.");
        Console.WriteLine("--skip-setup=[0,1]         - Skip setup scripts.");
        Console.WriteLine("--skip-shell-scripts=[0,1] - Skip shells scripts execution.");
        App.End();
    }

    public static void Version()
    {
        Console.WriteLine("1.0");
        App.End();
    }

    public static void Info()
    {
        Console.WriteLine("OS Is 64-bit: " + ((Environment.Is64BitOperatingSystem) ? "Yes" : "No"));
        App.End();
    }
}