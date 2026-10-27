// Generated from FileSecurity.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;

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
        public Aow.Units.FixedArray_byte_51 ID__image;
        public Aow.Rtl.AnsiString ID { get => Aow.Rtl.Rtl.ReadShortString((ReadOnlySpan<byte>)ID__image); set => Aow.Rtl.Rtl.WriteShortString((Span<byte>)ID__image, value); }

        [FieldOffset(56)]
        public Aow.Units.FixedArray_byte_50 Reserved38;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct TSecurityFooter
    {
        [FieldOffset(0)]
        public int CRC;
        [FieldOffset(4)]
        public int Size;
        [FieldOffset(8)]
        public Aow.Units.FixedArray_byte_4 Signature;
    }

    public static partial class FileSecurity
    {
        // FileSecurity.pas:132
        // { FileSecurity, from AoWEPACK.dpl. Security trailers use a $6A-byte data record
//   and a $0C-byte footer containing CRC32, size and the SDID signature.
//   The four unnamed helpers follow PageTurnCtrl's finalization in code order. }
        public static bool GetFileSecurityID(Aow.Rtl.AnsiString FileName, ref Aow.Rtl.AnsiString ID)
        {
            bool Result = default;
            Aow.Units.FileSecurity.TSecurityData Data = default;
            if (Aow.Units.FileSecurity.FileSecurity.ReadSecurityData(FileName, ref Data))
            {
                var call__1 = Aow.Units.FileSecurity.FileSecurity.EncodeSecurityID(Data.ID, unchecked((byte)Data.Key));
                ID = call__1;
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
        public static bool SetFileSecurityID(Aow.Rtl.AnsiString FileName, Aow.Rtl.AnsiString ID)
        {
            bool Result = default;
            Aow.Units.FileSecurity.TSecurityData Data = default;
            byte Key = default;
            Data = default;
            Key = unchecked((byte)Aow.Rtl.Rtl.Random(256));
            Data.Key = unchecked((byte)Key);
            var call__2 = Aow.Units.FileSecurity.FileSecurity.EncodeSecurityID(ID, unchecked((byte)Key));
            Data.ID = (Aow.Rtl.AnsiString)call__2;
            Result = Aow.Units.FileSecurity.FileSecurity.WriteSecurityData(FileName, Data);
            return Result;
        }

        // FileSecurity.pas:160
        public static bool RemoveFileSecurityID(Aow.Rtl.AnsiString FileName)
        {
            bool Result = default;
            Result = Aow.Units.FileSecurity.FileSecurity.RemoveSecurityData(FileName);
            return Result;
        }

        // FileSecurity.pas:31
        public static Aow.Rtl.AnsiString EncodeSecurityID(Aow.Rtl.AnsiString Value, byte Key)
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 1;
                int end__3 = Value.Length;
                for (; I <= end__3; I += 1)
                {
                    Result = Result + AnsiString.FromBytes(unchecked((byte)(unchecked((byte)(((Value.Length + (I + 5)) + unchecked((byte)(Key + 0))) & 255)) ^ Value[I])));
                }
            }

            return Result;
        }

        // FileSecurity.pas:63
        public static bool WriteSecurityData(Aow.Rtl.AnsiString FileName, in Aow.Units.FileSecurity.TSecurityData Data)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.CreateFile */
            throw new System.NotImplementedException();
        }

        // FileSecurity.pas:99
        public static bool RemoveSecurityData(Aow.Rtl.AnsiString FileName)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.CreateFile */
            throw new System.NotImplementedException();
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
