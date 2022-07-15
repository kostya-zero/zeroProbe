using System.Diagnostics;

namespace zeroProbe.Utils;

public class Handlers
{
    public static void ProccessOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Console.Write(e.Data);
    }
}