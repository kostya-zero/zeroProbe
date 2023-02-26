using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer
{
    public static LexObject Lex(string[] lines)
    {
        /*
         * ;; Example of Probe Instructions (ProbeConfig v2).
         * project: test
         * author: zero
         * required: npm nodejs
         * stages: build run
         * 
         * @build.command: npm init
         * @build.predictFail: 0
         *
         * @run.command: npm run
         * @run.predictFail: true
         */

        LexObject lexObject = new LexObject();

        foreach (string line in lines)
        {
            if (line.StartsWith(";;") || line.Trim() == "")
            {
                continue;
            }

            if (!line.Contains(':'))
            {
                Terminal.Fatal($"Bad expression: {line}");
                Terminal.Exit(2);
            }

            var splitLine = line.Split(':', 2);
            var command = new LexerLineObject
            {
                Name = splitLine[0].Trim(),
                Argument = splitLine[1].Trim(),
                IsStage = splitLine[0].StartsWith('@')
            };

            if (!command.IsStage)
            {
                switch (command.Name)
                {
                    case "project":
                        lexObject.Project = command.Argument;
                        break;
                    case "author":
                        lexObject.Author = command.Argument;
                        break;
                    case "shell":
                        lexObject.Shell = command.Argument;
                        break;
                    case "required":
                        lexObject.Required = command.Argument.Split(' ');
                        break;
                    case "stages":
                        foreach (string stage in command.Argument.Split(' '))
                        {
                            if (!stage.StartsWith('!'))
                            {
                                lexObject.Stages.Add(stage);
                            }
                        }
                        break;
                }
            }

            if (command.IsStage)
            {
                if (!command.Name.Contains('.'))
                {
                    Terminal.Fatal($"Bad expression: {line}");
                    Terminal.Exit(2);
                }
                
                string[] stageSplit = command.Name.Split('.');
                string stageName = stageSplit[0].Trim().TrimStart('@');
                string stageAction = stageSplit[1].Trim();

                if (!lexObject.StagesContainer.ContainsKey(stageName))
                {
                    lexObject.StagesContainer.Add(stageName, new StageModel());
                    lexObject.StagesContainer[stageName].Name = stageName;
                }

                switch (stageAction)
                {
                    case "name":
                        lexObject.StagesContainer[stageName].Name = command.Argument.Trim();
                        break;
                    case "command":
                        lexObject.StagesContainer[stageName].Commands.Add(command.Argument);
                        break;
                    case "predictFail":
                        lexObject.StagesContainer[stageName].PredictFail = Convert.ToBoolean(command.Argument);
                        break;
                    case "showOnlyErrors":
                        lexObject.StagesContainer[stageName].PredictFail = Convert.ToBoolean(command.Argument); 
                        break;
                }
            }
        }

        return lexObject;
    }
}