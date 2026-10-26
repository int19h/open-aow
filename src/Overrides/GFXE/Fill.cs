using System.Buffers.Binary;
using Aow.Rtl;

namespace Aow.Units.GFXE;

public static partial class GFXE
{
    // GFXE.pas:2281. REP STOSD consumes an unsigned count.
    public static void FillInt(Span<byte> destination, int count, int value)
    {
        for (uint i = 0; i < unchecked((uint)count); i++)
        {
            if (i >= (uint)(destination.Length / 4)) throw new EAccessViolation("FillInt crossed the buffer");
            BinaryPrimitives.WriteInt32LittleEndian(destination.Slice((int)i * 4, 4), value);
        }
    }

    // GFXE.pas:2309. The wrapping ADD preserves the original high-word carry.
    public static void FillWord(Span<byte> destination, int count, int value)
    {
        uint pair = unchecked(((uint)value << 16) + (uint)value);
        uint pairs = unchecked((uint)count) >> 1;
        for (uint i = 0; i < pairs; i++)
        {
            if (i >= (uint)(destination.Length / 4)) throw new EAccessViolation("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice((int)i * 4, 4), pair);
        }
        if ((count & 1) != 0)
        {
            int offset = checked((int)pairs * 4);
            if (offset > destination.Length - 2) throw new EAccessViolation("FillWord crossed the buffer");
            BinaryPrimitives.WriteUInt16LittleEndian(destination.Slice(offset, 2), unchecked((ushort)pair));
        }
    }

    // GFXE.pas:2291. Count zero enters the loop and faults after writing the available prefix.
    public static void FillTrippleByte(Span<byte> destination, int count, int value)
    {
        int offset = 0;
        do
        {
            if (offset > destination.Length - 3) throw new EAccessViolation("FillTrippleByte crossed the buffer");
            destination[offset] = unchecked((byte)value);
            destination[offset + 1] = unchecked((byte)(value >> 8));
            destination[offset + 2] = unchecked((byte)(value >> 16));
            offset += 3;
            count = unchecked(count - 1);
        } while (count != 0);
    }

    public static int RDTSC() => 0;
    public static bool CPUIDSupported() => false;
    public static bool MMXsupport() => false;
}
