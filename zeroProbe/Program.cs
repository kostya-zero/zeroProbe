namespace zeroProbe;

internal class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("zeroProbe: no argument provided.");
            Environment.Exit(0);
        }

        Actions acts = new Actions();
        switch (args[0])
        {
            case "run":
                acts.RunStages();
                break;
            case "writeconfig":
                acts.WriteConfig();
                break;
            default:
                Console.WriteLine($"zeroProbe: unknown argument -> {args[0]}");
                Environment.Exit(0);
                break;
        }
    }
}