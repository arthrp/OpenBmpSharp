namespace OpenBmpSharp.Pds;

public record BmpImage
{
    public uint Size { get; set; } = 0;
    public uint Offset { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public ushort ColorPlanes { get; set; }
    public ushort BitsPerPixel { get; set; }
    public CompressionMethod CompressionMethod { get; set; }
    public uint ImageSize { get; set; }
    public int HorizontalRes { get; set; }
    public int VerticalRes { get; set; }
    public uint ColorsInPaletteCount { get; set; }
    public uint ImportantColorCount { get; set; }
}