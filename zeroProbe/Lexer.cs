using zeroProbe.Models;
using zeroProbe.Utils;

namespace zeroProbe;

public class Lexer
{
    public static LexObject Lex(string[] lines)
    {
        // project: test
        // @build.command: npm init

        /*
         * ;; Example of Probe Instruction (ProbeConfig v2).
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
                break;
            }

            if (!line.Contains(':'))
            {
                // TODO: Throw error.
            }

            var splitLine = line.Split(':');
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
                        lexObject.Stages = command.Argument.Split(' ');
                        break;
                }
            }

            if (command.IsStage)
            {
                string[] stageSplit = command.Name.Split('.');
                string stageName = stageSplit[0].Trim().TrimStart('@');
                string stageAction = stageSplit[1].Trim();

                if (!lexObject.StagesContainer.ContainsKey(stageName))
                {
                    lexObject.StagesContainer.Add(stageName, new StageModel());
                }

                switch (stageAction)
                {
                    case "command":
                        lexObject.StagesContainer[stageName].Commands.Add(command.Argument);
                        break;
                    case "predictFail":
                        lexObject.StagesContainer[stageName].PredictFail = Convert.ToBoolean(command.Argument);
                        break;
                }
            }
        }

        return lexObject;
    }
}