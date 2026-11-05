// Translated from FileSecurity.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;

namespace Aow.Units.FileSecurity
{
    [StructLayout(LayoutKind.Explicit, Size = 106)]
    public struct TSecurityData
    {
        [FieldOffset(0)]
        public int Reserved00;
        [FieldOffset(4)]
        public byte Key;
        [FieldOffset(5)]
        public FixedArray_byte_51 ID__image;
        public AnsiString ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

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

    public static partial class FileSecurity
    {
        // FileSecurity.pas:132
        // { FileSecurity, from AoWEPACK.dpl. Security trailers use a $6A-byte data record
//   and a $0C-byte footer containing CRC32, size and the SDID signature.
//   The four unnamed helpers follow PageTurnCtrl's finalization in code order. }
        public static bool GetFileSecurityID(AnsiString FileName, ref AnsiString ID)
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
                ID = (AnsiString)"";
                Result = false;
            }

            return Result;
        }

        // FileSecurity.pas:148
        public static bool SetFileSecurityID(AnsiString FileName, AnsiString ID)
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
        public static bool RemoveFileSecurityID(AnsiString FileName) => RemoveSecurityData(FileName);

        // FileSecurity.pas:31
        public static AnsiString EncodeSecurityID(AnsiString Value, byte Key)
        {
            AnsiString Result = (AnsiString)"";
            for (int I = 1; I <= Value.Length; I++)
            {
                Result = Result + AnsiString.FromBytes((byte)((byte)((Value.Length + (I + 5) + (byte)(Key + 0)) & 255) ^ Value[I]));
            }

            return Result;
        }

        // FileSecurity.pas:63
        public static bool WriteSecurityData(AnsiString FileName, in TSecurityData Data)
        {
            /* TODO: Implement Windows.CreateFile in the platform or editor service. */
            throw new NotImplementedException();
        }

        // FileSecurity.pas:99
        public static bool RemoveSecurityData(AnsiString FileName)
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
    }
}
