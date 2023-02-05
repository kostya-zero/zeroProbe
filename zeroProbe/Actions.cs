using zeroProbe.Utils;

namespace zeroProbe;

public class Actions
{
    public void MakeConfig()
    {
        if (File.Exists("stages.pcf"))
        {
            Terminal.Info("Configuration file (stages.pcf) already exists at this directory.");
            Terminal.Exit(0);
        }

        File.Create("stages.pcf").Close();
        File.WriteAllText("stages.pcf", "project: Example" +
                                        "author: zeroProbe" +
                                        "stages: example" +
                                        "@example.command: echo \"Hello from zeroProbe!\"");
        Terminal.Info("Configuration file generated.");
    }
}