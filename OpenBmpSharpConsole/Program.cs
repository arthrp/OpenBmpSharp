using OpenBmpSharp;

namespace OpenBmpSharpConsole;
class Program
{
    static void Main(string[] args)
    {
        var b = new BmpParser();
        var img = b.Parse(args[0]);
        b.Write(img, args[1]);
        
        Console.WriteLine("Done");
    }
}
