using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Units.FileSecurity;

public static partial class FileSecurity
{
    public static bool ReadSecurityData(AnsiString fileName, ref TSecurityData data)
    {
        data = default;
        try
        {
            using var stream = Aow.Rtl.Rtl.Files.Open(fileName.ToString(), Aow.Rtl.Rtl.fmOpenRead | Aow.Rtl.Rtl.fmShareDenyWrite);
            if (stream.Length < 12) return false;
            stream.Seek(-12, SeekOrigin.End);
            Span<byte> footer = stackalloc byte[12];
            stream.ReadExactly(footer);
            if (!footer[8..].SequenceEqual("SDID"u8)) return false;
            int size = BinaryPrimitives.ReadInt32LittleEndian(footer[4..]);
            // The original overflows a fixed stack buffer for a corrupt oversized footer.
            if (size < 0 || size > 106 || size > stream.Length - 12) return false;
            stream.Seek(-size - 12, SeekOrigin.End);
            byte[] bytes = new byte[size];
            stream.ReadExactly(bytes);
            bytes.CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref data, 1)));
            return Engine.Engine.GetCRC32(bytes, size) == BinaryPrimitives.ReadInt32LittleEndian(footer);
        }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
    }
}
