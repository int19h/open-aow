
using Aow.Engine.Storage;

using Aow.Game;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

public static class EngineServices
{
    internal static int CopyStreamToClipboard(Stream stream, string format) => 0;

    internal static int PasteStreamFromClipboard(Stream stream, string format) => 0;

    internal const int MaxListSize = 134217727;

    internal const ControlStyleElement Stored = 0;

    internal const ControlStyleElement Shared = (ControlStyleElement)1;

    internal const ControlStyleElement Reserved2 = (ControlStyleElement)2;

    internal const ControlStyleElement KeepIndex = (ControlStyleElement)3;

    internal const StorageModeElement Text = (StorageModeElement)2;

    internal const StorageModeElement Partial = (StorageModeElement)3;

    internal static string ReplaceText(string S, string old, string newValue)
    {
        int P;
        string result = "";
        P = Pos(old, S);
        while (P != 0)
        {
            result = result + Copy(S, 1, P - 1) + newValue;
            S = Copy(S, P + old.Length, S.Length - P - old.Length + 1);
            P = Pos(old, S);
        }

        return result + S;
    }

    internal static Plugin GetPlugin(int index)
    {
        Plugin result;
        if (EngineHost.Plugins != null && index >= 0 && index < EngineHost.Plugins.Count)
        {
            result = EngineHost.Plugins[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    public static int GetCrc32(ReadOnlySpan<byte> buffer, int size)
    {
        if (EngineHost.Crc32Table.Length == 0) BuildCrc32Table();
        int crc = -1;
        foreach (byte value in buffer[..size])
            crc = EngineHost.Crc32Table[(value ^ crc) & 255] ^ (int)((uint)crc >> 8);
        return crc ^ -1;
    }

    internal static GameEngine GetEngine(int index)
    {
        GameEngine result;
        object P;
        if (EngineHost.Engines != null && index >= 0 && index < EngineHost.Engines.Count)
        {
            P = EngineHost.Engines[index];
            result = (GameEngine)P;
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static GameEngine FindEngine(int classId)
    {
        int I;
        bool found;
        found = false;
        GameEngine result = null;
        if (EngineHost.Engines != null)
        {
            I = 0;
            found = false;
            while (!found && I < EngineHost.Engines.Count)
            {
                found = classId == (EngineHost.Engines[I]).EngineClass.ClassId;
                I += 1;
            }

            if (found)
            {
                result = EngineHost.Engines[I - 1];
            }
        }

        if (!found)
        {
            throw new InvalidOperationException("Cannot access Engine");
        }

        return result;
    }

    internal static void StringToStream(string S, Stream stream)
    {
        int length;
        length = S.Length;
        stream.Write(Bytes.Of(ref length, 4));
        stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, length));
    }

    internal static void ExchangeLinkedStrings(DelphiList lists, int I, int J)
    {
        for (int K = lists.Count - 1; K >= 0; K--)
        {
            ((StringCollection)lists.List[K]).Exchange(I, J);
        }
    }

    internal static void BuildCrc32Table()
    {
        int C;
        int polynomial;
        if (EngineHost.Crc32Table.Length == 0)
        {
            EngineHost.Crc32Table = new int[256];
            polynomial = -306674912;
            for (int I = 0; I <= 255; I++)
            {
                C = I;
                for (int J = 7; J >= 0; J--)
                {
                    if ((C & 1) != 0)
                    {
                        C = ((int)(((uint)C) >> 1)) ^ polynomial;
                    }
                    else
                    {
                        C = (int)(((uint)C) >> 1);
                    }

                    EngineHost.Crc32Table[I] = C;
                }
            }
        }
    }

    internal static void FillLargeInteger(Span<LargeInteger> buffer, int count, LargeInteger value) => Fill(buffer, count, value);

    internal static void Fill<T>(Span<T> buffer, int count, T value)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative fill count");
        buffer[..count].Fill(value);
    }

}
