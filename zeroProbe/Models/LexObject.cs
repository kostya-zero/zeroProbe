namespace zeroProbe.Models;

public class LexObject
{
    /// <summary>
    /// Contains name of the project.
    /// </summary>
    /// <example> project: Test Project</example>
    public string Project { get; set; }
    
    /// <summary>
    /// Author of this configuration.
    /// </summary>
    /// <example> author: Kostya Zero </example>
    public string Author { get; set; }
    
    /// <summary>
    /// Tools required for testing.
    /// </summary>
    /// <example> required: git yarn npm nodejs </example>
    public string[] Required { get; set; }
    
    /// <summary>
    /// Which shell to use.
    /// </summary>
    /// <example> shell: /usr/bin/zsh </example>
    public string Shell { get; set; }
    
    /// <summary>
    /// List of stages to do.
    /// </summary>
    /// <example> stages: prepare test deploy </example>
    public string[] Stages { get; set; }
   
    /// <summary>
    /// Container with stage models.
    /// </summary>
    public Dictionary<string, StageModel> StagesContainer { get; set; }

}