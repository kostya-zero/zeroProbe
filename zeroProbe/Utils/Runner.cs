using zeroProbe.Models;

namespace zeroProbe.Utils;

public class Runner
{
     private LexObject project { get; set; }
     private HostHelper helper = new HostHelper();

     public bool CheckConfig()
     {
          return File.Exists("stages.pcf");
     }

     public void Run()
     {
         project = Lexer.Lex(File.ReadAllLines("stages.pcf"));
         Terminal.Info($"zeroProbe {VersionInfo.Version}");
         Terminal.Info($"Project: {project.Project}");
     }
}
