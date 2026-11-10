// Translated from FileSecurity.pas.
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Saves;

[StructLayout(LayoutKind.Explicit, Size = 106)]
public struct TSecurityData
{
    [FieldOffset(0)]
    public int Reserved00;
    [FieldOffset(4)]
    public byte Key;
    [FieldOffset(5)]
    public FixedArray_byte_51 ID__image;

    public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

    [FieldOffset(56)]
    public FixedArray_byte_50 Reserved38;
}

[StructLayout(LayoutKind.Explicit, Size = 12)]
public struct TSecurityFooter
{
    [FieldOffset(0)]
    public int CRC;
    [FieldOffset(4)]
    public int Size;
    [FieldOffset(8)]
    public FixedArray_byte_4 Signature;
}

public static partial class FileSecurityUnit
{
    // FileSecurity.pas:132
    // { FileSecurity, from AoWEPACK.dpl. Security trailers use a $6A-byte data record
    //   and a $0C-byte footer containing CRC32, size and the SDID signature.
    //   The four unnamed helpers follow PageTurnCtrl's finalization in code order. }
    public static bool GetFileSecurityID(string FileName, ref string ID)
    {
        bool Result;
        TSecurityData Data = default;
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

    // FileSecurity.pas:148
    public static bool SetFileSecurityID(string FileName, string ID)
    {
        TSecurityData Data;
        byte Key;
        Data = default;
        Key = (byte)Random(256);
        Data.Key = Key;
        var encodeSecurityID = EncodeSecurityID(ID, Key);
        Data.ID = encodeSecurityID;
        return WriteSecurityData(FileName, Data);
    }

    // FileSecurity.pas:160
    public static bool RemoveFileSecurityID(string FileName) => RemoveSecurityData(FileName);

    // FileSecurity.pas:31
    public static string EncodeSecurityID(string Value, byte Key)
    {
        string Result = "";
        for (int I = 1; I <= Value.Length; I++)
        {
            Result = Result + Cp1252.Text((byte)((byte)((Value.Length + (I + 5) + (byte)(Key + 0)) & 255) ^ Cp1252.Byte(Value[I - 1])));
        }

        return Result;
    }

    // FileSecurity.pas:63
    public static bool WriteSecurityData(string FileName, in TSecurityData Data)
    {
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        throw new NotImplementedException();
    }

    // FileSecurity.pas:99
    public static bool RemoveSecurityData(string FileName)
    {
        /* TODO: Implement Windows.CreateFile in the platform or editor service. */
        throw new NotImplementedException();
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }

    public static bool ReadSecurityData(string fileName, ref TSecurityData data)
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
            return EngineUnit.GetCRC32(bytes, size) == BinaryPrimitives.ReadInt32LittleEndian(footer);
        }
        catch (IOException) { return false; }
        catch (UnauthorizedAccessException) { return false; }
    }
}
