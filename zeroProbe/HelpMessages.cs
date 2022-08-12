using zeroProbe.Utils;
namespace zeroProbe;

public class HelpMessages
{
    public static void Help()
    {
        Console.WriteLine("zeroProbe 2.0 // Advanced utility to create, run and manage tests. Powered by .NET 6.");
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
        Console.WriteLine("--debug=[0,1]                      - Enables debug mode.");
        Console.WriteLine("--file=[path]                      - Use file by given location.");
        Console.WriteLine("--skip-shell-commands=[0,1]        - Skip shells scripts execution.");
        Console.WriteLine("--skip-shell-commands-errors=[0,1] - Ignores shell commands execution errors.\n");
        App.End();
    }

    public static void Version()
    {
        Console.WriteLine("Version:  2.0");
        Console.WriteLine("Codename: Emerging");
        App.End();
    }

    public static void IgnoreExecErrors()
    {
        Console.WriteLine("--ignore-exec-errors was deprecated in version 2.0.");
        Console.WriteLine(" Use 'ignore_errors' for each stage.");
        App.End();
    }
}