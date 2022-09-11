namespace zeroProbe.Utils;

public class ConfigsTemplates
{
    public string Default = "project: Empty config";

    public string DefaultWithStages = "project: Default config with stages\n" +
                                      "stages: lights camera action\n\n" +
                                      "!lights.add_command: echo 'Lights...'\n" +
                                      "!camera.add_command: echo 'Camera...'\n" +
                                      "!action.add_command: echo 'Action!'\n";

    public string TutorialConfig = @"/* This file was generated with zeroProbe 4.0 Voyage. */

/* Its a preview of how ProbeConfig file can be. */
/* Everything about syntax and parameters you can learn on zeroProbe wiki. */
/* GitLab wiki: https://gitlab.com/kostya-zero/zeroprobe/-/wikis/home */
/* GitHub wiki: https://github.com/kostya-zero/zeroProbe/wiki */

/* Project name are not necessary. */
/* zeroProbe automatically set your project name to 'unnamed'. */
project: test

/* Stages are main feature of your config. */
/* To tell zeroProbe what to do you need to create stages. */
/* Stages include command that's must be executed. */
/* Every stage will be executed in order how you wrote him. */
stages: restore build finish

/* Now you need to assign command to stage. */
/* Use '!' operator for it. */
/* After '!' enter stage name. */
/* Next, write '.add_command' and after double dots enter command to add. */
/* zeroProbe will execute it one by one.
/* To make stage ignore errors write '.ignore_errors' and after double dots 1 or 0. */
/* To set command on error use '.on_error'. */
/* Learn more you can on official wiki on GitLab and GitHub. */
!restore.add_command: echo 'Doing some restore staff...'
!build.add_command: echo 'Doing some build staff...'
!finish.add_command: echo 'Finishing this deal...'";

    public string DotNetBuildTest = "project: .NET build test\n" +
                                    "stages: restore build \n\n" +
                                    "!restore.add_command: dotnet restore\n" +
                                    "!build.add_command: dotnet build --no-restore";
    
    public string GccBuildTest = "project: GCC build test\n" +
                                    "stages: build test \n" +
                                    "check_for: gcc\n\n" +
                                    "!build.add_command: gcc main.cpp -o main\n" +
                                    "!test.add_command: ./main";
    
    public string ClangdBuildTest = "project: GCC build test\n" +
                                 "stages: build test \n" +
                                 "check_for: clangd\n\n" +
                                 "!build.add_command: clangd main.cpp -o main\n" +
                                 "!test.add_command: ./main";
    
}