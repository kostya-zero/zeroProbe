namespace zeroProbe.Utils;

public class Env
{
    public static string[] GetPath()
    {
        string[] pathVar = Environment.GetEnvironmentVariable("PATH").Split(':');
        return pathVar;
    }
}