namespace zeroProbe.Utils;

public class FuncV
{
    public static void ThrowError(string desc)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(" FATAL ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"]: {desc}");
        App.End(-1);
    }

    public static void ThrowWarning(string desc)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("WARNING");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"]: {desc}");
    }
}