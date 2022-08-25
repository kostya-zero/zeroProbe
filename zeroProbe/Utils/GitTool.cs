using zeroProbe.Enums;

namespace zeroProbe.Utils;

public class GitTool
{
    public string GetRemoteUrl()
    {
        if (!Directory.Exists(".git"))
        {
            Messages.Fatal("Cannot find .git directory.");
            App.End(-1);
        }

        string remote = "";
        string[] lines = File.ReadAllLines(".git/config");
        foreach (string line in lines)
        {
            string stringLine = line.Trim();
            if (!stringLine.StartsWith("[") && !stringLine.EndsWith("]"))
            {
                string[] split = stringLine.Split('=', 2);
                if (split[0].Trim() == "url")
                {
                    remote = split[1].Trim();
                }
            }
        }
        return remote;
    }
}