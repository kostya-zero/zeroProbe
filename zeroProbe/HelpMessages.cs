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
        Console.WriteLine("writeconfig      - writes template config.");
        Console.WriteLine("run              - runs all stages in stages.conf.");
        Console.WriteLine("inspect          - inspect configuration file.");
        Console.WriteLine("runstage [stage] - runs stage standalone.\n");
        
        Console.WriteLine(":: Options");
        Console.WriteLine("--debug=[0,1]               - Enables debug mode.");
        Console.WriteLine("--file=[path]               - Use file by given location.");
        Console.WriteLine("--skip-shell-scripts=[0,1]  - Skip shells scripts execution.");
        Console.WriteLine("--ignore-exec-errors=[0,1]  - Ignores execution errors.\n");
        App.End();
    }

    public static void Version()
    {
        Console.WriteLine("Version:  1.0");
        Console.WriteLine("Codename: Anxious");
        App.End();
    }
}