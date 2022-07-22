namespace zeroProbe.Utils;

public class FuncV
{
    public static void ThrowError(string desc)
    {
        Console.WriteLine($"[ FATAL ]: {desc}");
        App.End(-1);
    }

    public static void ThrowWarning(string desc)
    {
        Console.WriteLine($"[WARNING]: {desc}");
    }
}