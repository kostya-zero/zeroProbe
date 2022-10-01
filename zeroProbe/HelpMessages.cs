using System.Diagnostics;
using zeroProbe.Utils;
namespace zeroProbe;

public class HelpMessages
{
    public static void Help()
    {
        
        List<string> firstWords = new List<string>
        {
           "Powerful",
           "Advanced",
           "Automate"
        };
        List<string> secondWords = new List<string>
        {
            "easy-to-use",
            "free",
            "open-source"
        };

        Random rand = new Random();
        string firstWord = firstWords[rand.Next(0, 3)];
        string secondWord = secondWords[rand.Next(0, 3)];
        
        Console.WriteLine(@"
                           __________              ___.           
 ________ ___________  ____\______   \_______  ____\_ |__   ____  
 \___   // __ \_  __ \/  _ \|     ___/\_  __ \/  _ \| __ \_/ __ \ 
  /    /\  ___/|  | \(  <_> )    |     |  | \(  <_> ) \_\ \  ___/ 
 /_____ \\___  >__|   \____/|____|     |__|   \____/|___  /\___  >
       \/    \/                                         \/     \/  4.1
");
        Console.WriteLine($"\x1B[4m{firstWord}\x1B[24m and \x1B[4m{secondWord}\x1B[24m utility" +
                          " to create, run and manage tests.");
        Console.WriteLine("This is a help message. ");
        Console.WriteLine("usage: zeroProbe [action]\n");
        Console.WriteLine("   or: zeroProbe [action] [option,...]\n");
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(":: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Actions");
        Console.WriteLine("help             - shows this message.");
        Console.WriteLine("version          - return version of zeroProbe.");
        Console.WriteLine("writeconfig      - writes template config.");
        Console.WriteLine("run              - runs all stages in stages.conf.");
        Console.WriteLine("asciiart         - shows zeroProbe ASCII art.");
        Console.WriteLine("wiki             - opens zeroProbe wiki page in browser.");
        Console.WriteLine("runstage [stage] - runs stage standalone.\n");
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(":: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Options");
        Console.WriteLine("--debug=[0,1] - Enables debug mode.");
        Console.WriteLine("--file=[path] - Use file by given location.\n");
        Environment.Exit(0);
    }

    public static void Version()
    {
        Console.WriteLine("Information about zeroProbe:");
        Console.WriteLine("     Version:      4.1");
        Console.WriteLine("     Codename:     Voyage");
        Console.WriteLine("     .NET Version: 6.0.108");
        Console.WriteLine("     Build Date:   22.09.2022");
        Console.WriteLine("     Build Number: 129");
        Console.WriteLine("     Build Engine: 17.0.0\n");
        Console.WriteLine("Environment:");
        Console.WriteLine($"     System:  {Environment.OSVersion}");
        Console.WriteLine($"     Version: {Environment.Version}");
        Console.WriteLine($"     Domain:  {Environment.UserDomainName}");
        Environment.Exit(0);
    }

    public static void AsciiArt()
    {
        Console.WriteLine(@"
                           __________              ___.           
 ________ ___________  ____\______   \_______  ____\_ |__   ____  
 \___   // __ \_  __ \/  _ \|     ___/\_  __ \/  _ \| __ \_/ __ \ 
  /    /\  ___/|  | \(  <_> )    |     |  | \(  <_> ) \_\ \  ___/ 
 /_____ \\___  >__|   \____/|____|     |__|   \____/|___  /\___  >
       \/    \/                                         \/     \/  4.1
"); 
        Environment.Exit(0);
    }

    public static void Wiki()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/kostya-zero/zeroProbe/wiki",
            UseShellExecute = true
        });
    }
}