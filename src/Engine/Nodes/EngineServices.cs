
using System.Collections.Immutable;

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

    // The CRC-32 table of the reflected polynomial 0xEDB88320, built once.
    private static readonly ImmutableArray<int> s_crc32Table = BuildCrc32Table();

    public static int GetCrc32(ReadOnlySpan<byte> buffer, int size)
    {
        int crc = -1;
        foreach (byte value in buffer[..size])
            crc = s_crc32Table[(value ^ crc) & 255] ^ (int)((uint)crc >> 8);
        return crc ^ -1;
    }

    internal static void StringToStream(string S, Stream stream)
    {
        int length;
        length = S.Length;
        stream.Write(Bytes.Of(ref length, 4));
        stream.Write(Cp1252.Encode(S).AsSpan().Slice(0, length));
    }

    internal static void ExchangeLinkedStrings(DelphiList<StringCollection> lists, int I, int J)
    {
        for (int K = lists.Count - 1; K >= 0; K--)
        {
            lists.List[K].Exchange(I, J);
        }
    }

    private static ImmutableArray<int> BuildCrc32Table()
    {
        const int polynomial = -306674912;
        var table = new int[256];
        for (int I = 0; I <= 255; I++)
        {
            int C = I;
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
            }

            table[I] = C;
        }

        return ImmutableArray.Create(table);
    }

    internal static void FillLargeInteger(Span<LargeInteger> buffer, int count, LargeInteger value) => Fill(buffer, count, value);

    internal static void Fill<T>(Span<T> buffer, int count, T value)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative fill count");
        buffer[..count].Fill(value);
    }

}
