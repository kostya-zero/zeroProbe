namespace zeroProbe.Utils;

public class SpellChecker
{
    public static void CheckStageName(string name)
    {
        if (name.Contains('.'))
        {
            Messages.Fatal($"Stage '{name}' has a bad name.");
            Messages.Hint("Remove dots from name. It might confuse zeroProbe while parsing properties.");
            App.End(-1);
        }
        
        if (name.Contains(':'))
        {
            Messages.Fatal($"Stage '{name}' has a bad name.");
            Messages.Hint("Remove double dots from name. It might confuse zeroProbe while parsing lines.");
            App.End(-1);
        }
    }
}