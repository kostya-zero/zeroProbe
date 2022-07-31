namespace zeroProbe.Utils;

public class FSHelper
{
    public static bool IsExists(string path)
    {
        return File.Exists(path);
    }
}