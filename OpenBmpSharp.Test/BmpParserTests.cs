using OpenBmpSharp.Pds;

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

    [Test]
    public void ParsingBitmapFile_ProducesRealData()
    {
        var bmpParser = new BmpParser();
        var b = bmpParser.Parse("test.bmp");
        
        Assert.That(b.Size, Is.EqualTo(154));
        Assert.That(b.Height, Is.EqualTo(1));
        Assert.That(b.Width, Is.EqualTo(4));
        Assert.That(b.ColorPlanes, Is.EqualTo(1));
        Assert.That(b.BitsPerPixel, Is.EqualTo((short)32));
        Assert.That(b.CompressionMethod, Is.EqualTo(CompressionMethod.BI_BITFIELDS));
    }
}