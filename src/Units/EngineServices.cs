using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Storage;
using Aow.Game.Saves;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Nodes;

public static class EngineServices
{
    internal static int CopyStreamToClipboard(Stream stream, string format) => 0;

    internal static int PasteStreamFromClipboard(Stream stream, string format) => 0;

    internal const int MaxListSize = 134217727;

    internal const EControlStyleElement csStored = 0;

    internal const EControlStyleElement csShared = (EControlStyleElement)1;

    internal const EControlStyleElement csReserved2 = (EControlStyleElement)2;

    internal const EControlStyleElement csKeepIndex = (EControlStyleElement)3;

    internal const StorageModeElement smText = (StorageModeElement)2;

    internal const StorageModeElement smPartial = (StorageModeElement)3;

    internal static string ReplaceText(string S, string Old, string New)
    {
        int P;
        string Result = "";
        P = Pos(Old, S);
        while (P != 0)
        {
            Result = Result + Copy(S, 1, P - 1) + New;
            S = Copy(S, P + Old.Length, S.Length - P - Old.Length + 1);
            P = Pos(Old, S);
        }

        return Result + S;
    }

    internal static Aow.Game.Engine.PlugIn GetPlugIn(int Index)
    {
        Aow.Game.Engine.PlugIn Result;
        if (Aow.Game.Engine.Nodes.EngineHost.PlugIns != null && Index >= 0 && Index < Aow.Game.Engine.Nodes.EngineHost.PlugIns.Count)
        {
            Result = (Aow.Game.Engine.PlugIn)Aow.Game.Engine.Nodes.EngineHost.PlugIns[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public static int GetCRC32(ReadOnlySpan<byte> buffer, int size)
    {
        if (Aow.Game.Engine.Nodes.EngineHost.CRC32Table.Length == 0) Aow.Game.Engine.Nodes.EngineServices.BuildCRC32Table();
        int crc = -1;
        foreach (byte value in buffer[..size])
            crc = Aow.Game.Engine.Nodes.EngineHost.CRC32Table[(value ^ crc) & 255] ^ (int)((uint)crc >> 8);
        return crc ^ -1;
    }

    internal static global::Aow.Game.Engine.Engine GetEngine(int Index)
    {
        global::Aow.Game.Engine.Engine Result;
        object P;
        if (Aow.Game.Engine.Nodes.EngineHost.Engines != null && Index >= 0 && Index < Aow.Game.Engine.Nodes.EngineHost.Engines.Count)
        {
            P = Aow.Game.Engine.Nodes.EngineHost.Engines[Index];
            Result = (global::Aow.Game.Engine.Engine)P;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static global::Aow.Game.Engine.Engine FindEngine(int AClassID)
    {
        int I;
        bool Found;
        Found = false;
        global::Aow.Game.Engine.Engine Result = null;
        if (Aow.Game.Engine.Nodes.EngineHost.Engines != null)
        {
            I = 0;
            Found = false;
            while (!Found && I < Aow.Game.Engine.Nodes.EngineHost.Engines.Count)
            {
                Found = AClassID == ((EObject)Aow.Game.Engine.Nodes.EngineHost.Engines[I]).EClass.ClassID;
                I += 1;
            }

            if (Found)
            {
                Result = (global::Aow.Game.Engine.Engine)Aow.Game.Engine.Nodes.EngineHost.Engines[I - 1];
            }
        }

        if (!Found)
        {
            throw new InvalidOperationException("Cannot access Engine");
        }

        return Result;
    }

    internal static void StringToStream(string S, Stream Stream)
    {
        int Len;
        Len = S.Length;
        Stream.Write(Bytes.Of(ref Len, 4));
        Stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, Len));
    }

    internal static void ExchangeLinkedStrings(DelphiList Lists, int I, int J)
    {
        for (int K = Lists.Count - 1; K >= 0; K--)
        {
            ((StringCollection)Lists.List[K]).Exchange(I, J);
        }
    }

    internal static void BuildCRC32Table()
    {
        int C;
        int Polynomial;
        if (Aow.Game.Engine.Nodes.EngineHost.CRC32Table.Length == 0)
        {
            Aow.Game.Engine.Nodes.EngineHost.CRC32Table = new int[256];
            Polynomial = -306674912;
            for (int I = 0; I <= 255; I++)
            {
                C = I;
                for (int J = 7; J >= 0; J--)
                {
                    if ((C & 1) != 0)
                    {
                        C = ((int)(((uint)C) >> 1)) ^ Polynomial;
                    }
                    else
                    {
                        C = (int)(((uint)C) >> 1);
                    }

                    Aow.Game.Engine.Nodes.EngineHost.CRC32Table[I] = C;
                }
            }
        }
    }

    internal static void FillLargeInteger(Span<LargeInteger> buffer, int count, LargeInteger value) => Aow.Game.Engine.Nodes.EngineServices.Fill(buffer, count, value);

    internal static void Fill<T>(Span<T> buffer, int count, T value)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative fill count");
        buffer[..count].Fill(value);
    }

}
