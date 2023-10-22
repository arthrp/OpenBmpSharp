namespace OpenBmpSharp.Pds;

public record BmpImage
{
    public uint Size { get; set; } = 0;
    public byte[] Reserved { get; set; }
    public uint DataOffset { get; set; }
    public uint HeaderSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public ushort ColorPlanes { get; set; }
    public ushort BitsPerPixel { get; set; }
    public uint CompressionMethodRaw { get; set; }

    public CompressionMethod CompressionMethod
    {
        get
        {
            return (CompressionMethod) CompressionMethodRaw;
        }
    }

    public uint ImageSize { get; set; }
    public int HorizontalRes { get; set; }
    public int VerticalRes { get; set; }
    public uint ColorsInPaletteCount { get; set; }
    public uint ImportantColorCount { get; set; }
    public byte[]? RestOfHeader { get; set; }
    public List<byte[]> DataRows { get; set; }
}