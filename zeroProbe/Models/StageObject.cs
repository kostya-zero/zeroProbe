namespace zeroProbe.Models;

public class StageObject
{
    public string StageName { get; set; }
    public string StageCommand { get; set; }
    public string Directory { get; set; }

    public StageObject()
    {
        StageCommand = "";
        StageName = "";
        Directory = "";
    }
}