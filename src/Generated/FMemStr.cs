// Generated from FMemStr.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.FMemStr
{
    // { The game's FMemStr unit (Enginep.dpl), written for Delphi 3.02 and compared with the original code
//   by tools/match.py. TFastMemoryStream is Delphi 3's TMemoryStream with a direct Position field, a
//   32/128-byte capacity granularity and one-, two- and four-byte fast paths in Read and Write. }
    // { FMemStr.TFastMemoryStream (instance size 20): +4 FMemory, +8 FSize, +$C FPosition, +$10 FCapacity }
    public partial class TFastMemoryStream : Aow.Rtl.TStream
    {
        internal Ptr<byte> FMemory;
        internal int FSize;
        internal int FPosition;
        internal int FCapacity;
        // FMemStr.pas:125
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity > 0)
            {
                if (FCapacity > 32)
                {
                    NewCapacity = (NewCapacity + 127) & ~127;
                }
                else
                {
                    NewCapacity = (NewCapacity + 31) & ~31;
                }
            }

            if (NewCapacity != FCapacity)
            {
                if (NewCapacity == 0)
                {
                    FMemory = default;
                    FMemory = default;
                }
                else
                {
                    if (FCapacity == 0)
                    {
                        var call__1 = Ptr<byte>.Alloc(NewCapacity);
                        FMemory = call__1;
                    }
                    else
                    {
                        var call__2 = FMemory.Realloc(NewCapacity);
                        FMemory = call__2;
                    }

                    if (FMemory.IsNull)
                    {
                        throw Aow.Rtl.EStreamError.Create((AnsiString)"Out of memory while expanding memory stream");
                    }
                }

                FCapacity = NewCapacity;
                if (FCapacity > FSize)
                {
                    FSize = FCapacity;
                }
            }
        }

        // FMemStr.pas:152
        public override void SetSize(int NewSize)
        {
            if (NewSize < FPosition)
            {
                FPosition = NewSize - 1;
            }

            SetCapacity(NewSize);
            FSize = NewSize;
        }

        // FMemStr.pas:90
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // FMemStr.pas:43
        public override int Read(Span<byte> Buffer)
        {
            int Count = Buffer.Length;
            int Result = default;
            Result = FSize - FPosition;
            if (Result > 0)
            {
                if (Result > Count)
                {
                    Result = Count;
                }

                if (Count == 4)
                {
                    {
                        int value__3 = MemoryMarshal.Read<int>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 4));
                        MemoryMarshal.Write<int>(Buffer, in value__3);
                    }
                }
                else
                {
                    if (Count == 1)
                    {
                        {
                            byte value__4 = MemoryMarshal.Read<byte>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 1));
                            MemoryMarshal.Write<byte>(Buffer, in value__4);
                        }
                    }
                    else
                    {
                        if (Count == 2)
                        {
                            {
                                ushort value__5 = MemoryMarshal.Read<ushort>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 2));
                                MemoryMarshal.Write<ushort>(Buffer, in value__5);
                            }
                        }
                        else
                        {
                            Aow.Rtl.Rtl.Move((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, Result), Buffer.Slice(0, Result), Result);
                        }
                    }
                }

                FPosition += Result;
                return Result;
            }

            Result = 0;
            return Result;
        }

        // FMemStr.pas:159
        public override int Write(ReadOnlySpan<byte> Buffer)
        {
            int Count = Buffer.Length;
            int Result = default;
            int Pos = default;
            if ((FPosition >= 0) && (Count >= 0))
            {
                Pos = FPosition + Count;
                if (Pos > 0)
                {
                    if (Pos > FSize)
                    {
                        if (Pos > FCapacity)
                        {
                            SetCapacity(Pos);
                        }

                        FSize = Pos;
                    }

                    if (Count == 4)
                    {
                        {
                            int value__6 = MemoryMarshal.Read<int>(Buffer);
                            MemoryMarshal.Write<int>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 4), in value__6);
                        }
                    }
                    else
                    {
                        if (Count == 1)
                        {
                            {
                                byte value__7 = MemoryMarshal.Read<byte>(Buffer);
                                MemoryMarshal.Write<byte>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 1), in value__7);
                            }
                        }
                        else
                        {
                            if (Count == 2)
                            {
                                {
                                    ushort value__8 = MemoryMarshal.Read<ushort>(Buffer);
                                    MemoryMarshal.Write<ushort>((FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, 2), in value__8);
                                }
                            }
                            else
                            {
                                Aow.Rtl.Rtl.Move(Buffer.Slice(0, Count), (FMemory + FPosition).Array.AsSpan((FMemory + FPosition).Index, Count), Count);
                            }
                        }
                    }

                    FPosition = Pos;
                    Result = Count;
                    return Result;
                }
            }

            Result = 0;
            return Result;
        }

        // FMemStr.pas:63
        public override int Seek(int Offset, int Origin)
        {
            int Result = default;
            switch (Origin)
            {
                case 0:
                    FPosition = Offset;
                    break;
                case 1:
                    FPosition += Offset;
                    break;
                case 2:
                    FPosition = FSize + Offset;
                    break;
            }

            Result = FPosition;
            return Result;
        }

        // FMemStr.pas:96
        public void Clear()
        {
            SetCapacity(0);
            FSize = 0;
            FPosition = 0;
        }

        // FMemStr.pas:103
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int Count = default;
            Stream.Position = 0;
            var call__9 = Stream.Size;
            Count = call__9;
            SetSize(Count);
            if (Count != 0)
            {
                Stream.ReadBuffer(FMemory.Array.AsSpan(FMemory.Index, Count));
            }
        }

        // FMemStr.pas:113
        public void LoadFromFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Rtl.TStream Stream = default;
            Stream = new Aow.Rtl.TFileStream(FileName, 0);
            try
            {
                LoadFromStream(Stream);
            }
            finally
            {
                Stream.Free();
            }
        }

        // FMemStr.pas:73
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            if (FSize != 0)
            {
                Stream.WriteBuffer(FMemory.Array.AsSpan(FMemory.Index, FSize));
            }
        }

        // FMemStr.pas:78
        public void SaveToFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Rtl.TStream Stream = default;
            Stream = new Aow.Rtl.TFileStream(FileName, 65535);
            try
            {
                SaveToStream(Stream);
            }
            finally
            {
                Stream.Free();
            }
        }

        public Ptr<byte> Memory { get => FMemory; }
        public new int Position { get => FPosition; set => FPosition = value; }
        public int Capacity { get => FCapacity; set => SetCapacity(value); }
    }

    public static partial class FMemStr
    {
    }
}
