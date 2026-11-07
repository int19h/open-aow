using System.Runtime.InteropServices;

namespace Aow.Rtl;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TRGBQuad
{
    public byte rgbBlue, rgbGreen, rgbRed, rgbReserved;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TBitmapInfoHeader
{
    public int biSize, biWidth, biHeight;
    public ushort biPlanes, biBitCount;
    public int biCompression, biSizeImage, biXPelsPerMeter, biYPelsPerMeter, biClrUsed, biClrImportant;
}

public struct TGUID
{
    public int D1;
    public ushort D2, D3;
    public byte[] D4;

    public static implicit operator TGUID(string value)
    {
        var bytes = Guid.Parse(value).ToByteArray();
        return new TGUID { D1 = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes), D2 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(4)), D3 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6)), D4 = bytes[8..] };
    }
}

[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct WordRec
{
    [FieldOffset(0)] public byte Lo;
    [FieldOffset(1)] public byte Hi;
}

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct LongRec
{
    [FieldOffset(0)] public ushort Lo;
    [FieldOffset(2)] public ushort Hi;
}

public struct TMessage { public int Msg, WParam, LParam, Result; }

public struct TWMKey { public int Msg; public ushort CharCode, Unused; public int KeyData, Result; }

public struct TCreateParams { public int Style, ExStyle, X, Y, Width, Height, WndParent, Param; }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TPaletteEntry { public byte peRed, peGreen, peBlue, peFlags; }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TWaveFormatEx
{
    public ushort wFormatTag, nChannels;
    public int nSamplesPerSec, nAvgBytesPerSec;
    public ushort nBlockAlign, wBitsPerSample, cbSize;
}

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct TLargeInteger
{
    [FieldOffset(0)] public uint LowPart;
    [FieldOffset(4)] public int HighPart;
    [FieldOffset(0)] public long QuadPart;
}

public static partial class RtlUnit
{
    public static bool Succeeded(int result) => result >= 0;
    public static bool Failed(int result) => result < 0;
}
