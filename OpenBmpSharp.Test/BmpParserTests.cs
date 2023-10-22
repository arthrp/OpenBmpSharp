namespace OpenBmpSharp.Test;

public class BmpParserTests
{
    [Test]
    public void ReadingNonBitmapStream_ThrowsArgumentException()
    {
        var bmpParser = new BmpParser();
        var fileStream = new MemoryStream(new byte[] {0xFF, 0xD8});

        Assert.Throws<ArgumentException>(() => bmpParser.Parse(fileStream));
    }
}