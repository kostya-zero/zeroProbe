namespace zeroProbe.Utils;

public class Messages
{
    public static void Good(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"GOOD: {text}");
    }

    public static void Fatal(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"FATAL: {text}");
    }

    public static void Warning(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"WARN: {text}");
    }

    public static void Work(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"WORK: {text}");
    }
    
    public static void Info(string text)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"INFO: {text}");
    }
    
    public static void Debug(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"DEBUG: {text}");
    }

    public static void Hint(string text)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("** ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"HINT: {text}");
    }
}