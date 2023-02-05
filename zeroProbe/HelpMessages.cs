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
       \/    \/                                         \/     \/  5.0
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
        Environment.Exit(0);
    }

    public static void Version()
    {
        Console.WriteLine($"{VersionInfo.Version}");
        Terminal.Exit(0);
    }
}