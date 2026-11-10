using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Saves;

public static class SaveSecurity
{
    public static bool GetFileSecurityID(string FileName, ref string ID)
    {
        bool Result;
        SecurityData Data = default;
        if (ReadSecurityData(FileName, ref Data))
        {
            ID = EncodeSecurityID(Data.ID, Data.Key);
            Result = true;
        }
        else
        {
            ID = "";
            Result = false;
        }

        return Result;
    }

    internal static bool SetFileSecurityID(string FileName, string ID)
    {
        SecurityData Data;
        byte Key;
        Data = default;
        Key = (byte)Random(256);
        Data.Key = Key;
        var encodeSecurityID = EncodeSecurityID(ID, Key);
        Data.ID = encodeSecurityID;
        return WriteSecurityData(FileName, Data);
    }

    internal static bool RemoveFileSecurityID(string FileName) => RemoveSecurityData(FileName);

    public static string EncodeSecurityID(string Value, byte Key)
    {
        string Result = "";
        for (int I = 1; I <= Value.Length; I++)
        {
            Result = Result + Cp1252.Text((byte)((byte)((Value.Length + (I + 5) + (byte)(Key + 0)) & 255) ^ Cp1252.Byte(Value[I - 1])));
        }

        return Result;
    }

    internal static bool WriteSecurityData(string FileName, in SecurityData Data)
    {
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        throw new NotImplementedException();
    }

    internal static bool RemoveSecurityData(string FileName)
    {
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        throw new NotImplementedException();
    }

    public static bool ReadSecurityData(string fileName, ref SecurityData data)
    {
        data = default;
        try
        {
            using var stream = Files.Open(fileName, fmOpenRead | fmShareDenyWrite);
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
            return EngineServices.GetCRC32(bytes, size) == BinaryPrimitives.ReadInt32LittleEndian(footer);
        }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
    }

}
