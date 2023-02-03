using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Runner
{
     private Project project { get; set; }
     private HostHelper helper = new HostHelper();
     private Parser parser = new Parser();

     public void SetProject(Project newProject)
     {
          project = newProject;
     }

     public bool CheckConfig()
     {
          return File.Exists("stages.pcf");
     }
     

}