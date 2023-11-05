using System.Text;
using OpenBmpSharp.Pds;

namespace OpenBmpSharp;

public class BmpParser
{
    private readonly byte[] BMP_FILE_ID = new byte[] { 0x42, 0x4D }; //BM
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
        var h = binaryReader.ReadBytes(2);
        if (h[0] != BMP_FILE_ID[0] || h[1] != BMP_FILE_ID[1]) throw new ArgumentException("Not a bmp file!");
        
        b.Size = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        b.Reserved = binaryReader.ReadBytes(4); //Reserved
        b.DataOffset = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        b.HeaderSize = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        if (b.HeaderSize != 124) throw new ArgumentException("Only BITMAPV5HEADER is supported");
        
        b.Width = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.Height = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.ColorPlanes = BitConverter.ToUInt16(binaryReader.ReadBytes(2));
        b.BitsPerPixel = BitConverter.ToUInt16(binaryReader.ReadBytes(2));
        b.CompressionMethodRaw = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.ImageSize = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.HorizontalRes = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.VerticalRes = BitConverter.ToInt32(binaryReader.ReadBytes(4));
        b.ColorsInPaletteCount = BitConverter.ToUInt32(binaryReader.ReadBytes(4));
        b.ImportantColorCount = BitConverter.ToUInt32(binaryReader.ReadBytes(4));

        var remainingHeaderBytes = b.DataOffset - binaryReader.BaseStream.Position;
        if (remainingHeaderBytes > 0) b.RestOfHeader = binaryReader.ReadBytes((int)remainingHeaderBytes);

        // if (b.CompressionMethod == CompressionMethod.BI_BITFIELDS)
        // {
        //     var redBitMask = binaryReader.ReadBytes(4);
        //     var greenBitMask = binaryReader.ReadBytes(4);
        //     var blueBitMask = binaryReader.ReadBytes(4);
        //     var alphaBitMask = binaryReader.ReadBytes(4);
        //     var colorSpace = Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
        // }

        binaryReader.BaseStream.Position = b.DataOffset;
        b.DataRows = ReadRgbaData(binaryReader, b.Height, b.Width);
        return b;
    }

    public void Write(BmpImage b, string filePath)
    {
        using var stream = File.OpenWrite(filePath);
        using var binaryWriter = new BinaryWriter(stream);
        
        binaryWriter.Write(BMP_FILE_ID);
        binaryWriter.Write(b.Size);
        binaryWriter.Write(b.Reserved);
        binaryWriter.Write(b.DataOffset);
        binaryWriter.Write(b.HeaderSize);
        binaryWriter.Write(b.Width);
        binaryWriter.Write(b.Height);
        binaryWriter.Write(b.ColorPlanes);
        binaryWriter.Write(b.BitsPerPixel);
        binaryWriter.Write(b.CompressionMethodRaw);
        binaryWriter.Write(b.ImageSize);
        binaryWriter.Write(b.HorizontalRes);
        binaryWriter.Write(b.VerticalRes);
        binaryWriter.Write(b.ColorsInPaletteCount);
        binaryWriter.Write(b.ImportantColorCount);
        if(b.RestOfHeader != null && b.RestOfHeader.Length > 0) binaryWriter.Write(b.RestOfHeader);
        WriteRgbaData(binaryWriter, b);
    }

    private List<byte[]> ReadRgbaData(BinaryReader binaryReader, int height, int width)
    {
        var list = new List<byte[]>();
        for (int i = 0; i < height; i++)
        {
            var bytes = binaryReader.ReadBytes(4 * width);
            list.Add(bytes);
        }

        return list;
    }

    private void WriteRgbaData(BinaryWriter binaryWriter, BmpImage b)
    {
        foreach (var dataRow in b.DataRows)
        {
            binaryWriter.Write(dataRow);
        }
    }
}