using zeroProbe.Utils;

namespace zeroProbe.Internal;

public class DecreesResolver
{
    public void Resolve(string decree)
    {
        switch (decree)
        {
            case "@clearconsole":
                Decrees.ClearConsole();
                break;
            default:
                FuncV.ThrowError($"Unknown internal decree -> {decree}.");
                break;
        }
    }
}