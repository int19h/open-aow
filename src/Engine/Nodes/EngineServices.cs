#nullable disable

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Engine.Nodes;

public static class EngineServices
{
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi clipboard stub keeps stream and format in its signature.")]
    internal static int CopyStreamToClipboard(Stream stream, string format) => 0;

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi clipboard stub keeps stream and format in its signature.")]
    internal static int PasteStreamFromClipboard(Stream stream, string format) => 0;

    internal const int MaxListSize = 134217727;

    internal const ControlStyleElement Stored = 0;

    internal const ControlStyleElement Shared = (ControlStyleElement)1;

    internal const ControlStyleElement Reserved2 = (ControlStyleElement)2;

    internal const ControlStyleElement KeepIndex = (ControlStyleElement)3;

    internal static string ReplaceText(string S, string old, string newValue)
    {
        string result = "";
        int p = Pos(old, haystack: S);
        while (p != 0)
        {
            result = result + Copy(S, index: 1, count: p - 1) + newValue;
            S = Copy(S, index: p + old.Length, count: S.Length - p - old.Length + 1);
            p = Pos(old, haystack: S);
        }

        return result + S;
    }

    // The CRC-32 table of the reflected polynomial 0xEDB88320, built once.
    private static readonly ImmutableArray<int> s_crc32Table = BuildCrc32Table();

    public static int GetCrc32(ReadOnlySpan<byte> buffer, int size)
    {
        int crc = -1;
        foreach (byte value in buffer[..size])
        {
            crc = s_crc32Table[(value ^ crc) & 255] ^ (crc >>> 8);
        }

        return crc ^ -1;
    }

    internal static void StringToStream(string S, Stream stream)
    {
        int length = S.Length;
        stream.Write(Bytes.Of(ref length, count: 4));
        stream.Write(Cp1252.Encode(S).AsSpan()[..length]);
    }

    internal static void ExchangeLinkedStrings(DelphiList<DelphiStringCollection> lists, int I, int J)
    {
        for (int k = lists.Count - 1; k >= 0; k--)
        {
            lists.List[k].Exchange(I, second: J);
        }
    }

    private static ImmutableArray<int> BuildCrc32Table()
    {
        const int polynomial = -306674912;
        int[] table = new int[256];
        for (int i = 0; i <= 255; i++)
        {
            int c = i;
            for (int j = 7; j >= 0; j--)
            {
                c = (c & 1) != 0 ? (c >>> 1) ^ polynomial : c >>> 1;
            }

            table[i] = c;
        }

        return ImmutableArray.Create(table);
    }

    internal static void Fill<T>(Span<T> buffer, int count, T value)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(paramName: null, message: "Negative fill count");
        }

        buffer[..count].Fill(value);
    }
}
