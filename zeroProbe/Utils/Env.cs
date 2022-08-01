namespace zeroProbe.Utils;

public class Env
{
    public static List<string> GetPath()
    {
        var envPath = Environment.GetEnvironmentVariable("PATH")?.Split(':');
        List<string> pathVar;
        if (envPath == null)
        {
            pathVar = new List<string>();
            return pathVar;
        }
        pathVar = new List<string>(envPath);
        return pathVar;
    }
}