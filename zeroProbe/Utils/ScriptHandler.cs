namespace zeroProbe.Utils;

public class ScriptHandler
{
    public string ScriptPath { get; init; } = "";
    public string ScriptContent { get; init; } = "";

    public void GenScript()
    {
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