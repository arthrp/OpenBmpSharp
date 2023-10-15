using System.Text;
using OpenBmpSharp.Pds;

namespace OpenBmpSharp;

public class BmpParser
{
    private const string BMP_HEADER_STR = "BM";
    public BmpImage Parse(string filePath)
    {
        if (!File.Exists(filePath)) throw new ArgumentException("File doesn't exist!");
        using var stream = new FileStream(filePath, FileMode.Open);

        return Parse(stream);
    }

    public BmpImage Parse(Stream input)
    {
        using var binaryReader = new BinaryReader(input);
        
        var b = new BmpImage();
        var type = Encoding.ASCII.GetString(binaryReader.ReadBytes(2));
        if (type != BMP_HEADER_STR) throw new ArgumentException("Not a bmp file!");
        
        b.Size = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        _ = binaryReader.ReadBytes(4); //Reserved
        b.Offset = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        var headerSize = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        if (headerSize != 124) throw new ArgumentException("Only BITMAPV5HEADER is supported");
        
        b.Width = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.Height = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.ColorPlanes = BitConverter.ToUInt16(binaryReader.ReadBytes(2));
        b.BitsPerPixel = BitConverter.ToUInt16(binaryReader.ReadBytes(2));
        var compressionMethod = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.CompressionMethod = (CompressionMethod) compressionMethod;
        b.ImageSize = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.HorizontalRes = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.VerticalRes = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.ColorsInPaletteCount = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.ImportantColorCount = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        binaryReader.BaseStream.Position = b.Offset;
        return b;
    }
}