namespace Aow.Rtl;

public struct GUID
{
    public int D1 { get; init; }
    public ushort D2 { get; init; }
    public ushort D3 { get; init; }
    public byte[] D4 { get; init; }

    public static implicit operator GUID(string value)
    {
        byte[] bytes = Guid.Parse(value).ToByteArray();
        return new GUID { D1 = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes), D2 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(4)), D3 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6)), D4 = bytes[8..] };
    }
}

public static partial class RtlUnit
{
    public static bool Succeeded(int result) => result >= 0;
    public static bool Failed(int result) => result < 0;
}
