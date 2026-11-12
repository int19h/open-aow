using System.Runtime.InteropServices;

namespace Aow.Rtl;

public struct GUID
{
    public int D1;
    public ushort D2, D3;
    public byte[] D4;

    public static implicit operator GUID(string value)
    {
        var bytes = Guid.Parse(value).ToByteArray();
        return new GUID { D1 = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes), D2 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(4)), D3 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6)), D4 = bytes[8..] };
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

public static partial class RtlUnit
{
    public static bool Succeeded(int result) => result >= 0;
    public static bool Failed(int result) => result < 0;
}
