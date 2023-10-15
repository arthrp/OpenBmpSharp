using OpenBmpSharp;

namespace OpenBmpSharpConsole;
class Program
{
    static void Main(string[] args)
    {
        var img = new BmpParser().Parse(args[0]);
    }
}
