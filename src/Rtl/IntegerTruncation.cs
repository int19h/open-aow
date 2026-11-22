namespace Aow.Rtl;

// A deliberate narrowing: the value a store into a Delphi Shortint, Byte, Smallint or Word kept.
// Each method returns the narrow type itself.
public static class IntegerTruncation
{
    extension(int value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public short TruncateToInt16() => unchecked((short)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(uint value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public short TruncateToInt16() => unchecked((short)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(long value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public short TruncateToInt16() => unchecked((short)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(ulong value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public short TruncateToInt16() => unchecked((short)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(short value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(ushort value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);

        public byte TruncateToByte() => unchecked((byte)value);

        public short TruncateToInt16() => unchecked((short)value);
    }

    extension(sbyte value)
    {
        public byte TruncateToByte() => unchecked((byte)value);

        public ushort TruncateToUInt16() => unchecked((ushort)value);
    }

    extension(byte value)
    {
        public sbyte TruncateToSByte() => unchecked((sbyte)value);
    }

    extension(char value)
    {
        // Through ushort: the strings check forbids a direct cast from a char to a byte.
        public sbyte TruncateToSByte() => unchecked((sbyte)(ushort)value);

        public byte TruncateToByte() => unchecked((byte)(ushort)value);

        public short TruncateToInt16() => unchecked((short)value);
    }
}
