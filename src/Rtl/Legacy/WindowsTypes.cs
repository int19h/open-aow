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

public static partial class RtlUnit
{
    public static bool Succeeded(int result) => result >= 0;
    public static bool Failed(int result) => result < 0;
}
