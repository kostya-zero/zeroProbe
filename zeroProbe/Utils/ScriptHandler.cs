namespace zeroProbe.Utils;

public class ScriptHandler
{
    private string ScriptPath { get; init; }
    private string ScriptContent { get; init; }

    public ScriptHandler(string scriptPath, string scriptContent)
    {
        ScriptPath = scriptPath;
        ScriptContent = scriptContent;
        if (File.Exists(ScriptPath))
        {
            File.Delete(ScriptPath);
        }
        
        File.Create(ScriptPath).Close();
        File.WriteAllText(ScriptPath, ScriptContent);
    }

    public void Remove()
    {
        File.Delete(ScriptPath);
    }
}