namespace zeroProbe.Utils;

public static class Terminal
{
    public static void Good(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[ GOOD ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }

    public static void Fatal(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[ FATAL ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }

    public static void Warning(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[ WARN ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }

    public static void Work(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[ WORK ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }
    
    public static void Info(string text)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("[ INFO ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }
    
    public static void Debug(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.Write("[ DEBUG ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }

    public static void Hint(string text)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("[ HINT ]");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" : {text}");
    }
}