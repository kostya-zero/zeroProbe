using zeroProbe.Utils;

namespace zeroProbe;

public class Parser
{
    public Dictionary<string, string> StagesDict { get; }
    public List<string> Stages  { get; }
    public string LayoutType  { get; private set; }
    private bool Comments { get; set; }
    public string ProjectName { get; set;  }
    public bool Debug { get; set; }

    public Parser()
    {
        StagesDict = new Dictionary<string, string>();
        Stages = new List<string>();
        ProjectName = "unnamed";
        Debug = false;
    }

    public void DebugInstruction(string instruction)
    {
        Messages.Debug($"Calling instruction {instruction}.");
    }

    public void ParseLine(string line)
    {
        var obj = Lexer.Lex(line);
        switch (obj.FunctionType)
        {
            case "0x11f":
                if (Debug) DebugInstruction("0x11f");
                if (!Comments)
                {
                    FuncV.ThrowWarning("Use comments less. It slows zeroProbe.");
                    Comments = true;
                }
                break;
            case "0xc88":
                if (Debug) DebugInstruction("0xc88");
                string layout = obj.Arguments.Trim();
                switch (layout)
                {
                    case "std":
                        LayoutType = "std";
                        break;
                    default:
                        FuncV.ThrowError($"unknown layout -> {layout}");
                        break;
                }
                break;
            case "0x054":
                if (Debug) DebugInstruction("0x054");
                string stages = obj.Arguments.Trim();
                var split = stages.Split(",");
                if (!stages.Contains(','))
                {
                    Stages.Add(stages.Trim());
                }
                else
                {
                    foreach (var stage in split)
                    {
                        Stages.Add(stage.Trim());
                    } 
                }
                break;
            case "0x700":
                if (Debug) DebugInstruction("0x700");
                string stg = obj.FunctionName;
                stg = stg.TrimStart('!');
                if (Stages.Contains(stg))
                {
                    StagesDict.Add(stg, obj.Arguments.Trim());
                }
                else
                {
                    FuncV.ThrowError($"stage '{stg}' not defined.");
                }
                break;
            case "0xa33":
                if (Debug) DebugInstruction("0xa33");
                string cmd = obj.Arguments;
                cmd = cmd.Trim();
                ScriptHandler script = new ScriptHandler
                {
                    ScriptPath = "tmp_setup_script.sh",
                    ScriptContent = $"#!/bin/sh\n{cmd}"
                };
                script.GenScript();
                Messages.Work("Setting up...");
                Shell sh = new Shell();
                var res = sh.Execute("/bin/sh", "tmp_setup_script.sh");
                if (res.GotErrors)
                {
                    Messages.Fatal("Setup failed. Error:");
                    Console.WriteLine(res.Error);
                    Environment.Exit(0);
                }

                Messages.Good("Setup complete.");
                break;
            case "0x805":
                if (Debug) DebugInstruction("0x805");
                string shell = obj.Arguments;
                shell = shell.Trim();
                ScriptHandler shellScript = new ScriptHandler
                {
                    ScriptPath = "tmp_shell_script.sh",
                    ScriptContent = $"#!/bin/sh\n{shell}"
                };
                shellScript.GenScript();
                Shell sh1 = new Shell();
                var res1 = sh1.Execute("/bin/sh", "tmp_shell_script.sh");
                if (res1.GotErrors)
                {
                    Messages.Fatal("Failed to call shell command. Error:");
                    Console.WriteLine(res1.Error);
                }
                break;
            case "0xccf":
                if (Debug) DebugInstruction("0xccf");
                ProjectName = obj.Arguments;
                break;
            default:
                FuncV.ThrowError($"Illegal instruction called -> {obj.FunctionType}");
                break;
        }
    }

    
}