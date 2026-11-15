
using System.Collections.Immutable;

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

    internal static string ReplaceText(string S, string old, string newValue)
    {
        int p;
        string result = "";
        p = Pos(old, S);
        while (p != 0)
        {
            result = result + Copy(S, 1, p - 1) + newValue;
            S = Copy(S, p + old.Length, S.Length - p - old.Length + 1);
            p = Pos(old, S);
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
        for (int k = lists.Count - 1; k >= 0; k--)
        {
            lists.List[k].Exchange(I, J);
        }
    }

    private static ImmutableArray<int> BuildCrc32Table()
    {
        const int polynomial = -306674912;
        var table = new int[256];
        for (int i = 0; i <= 255; i++)
        {
            int c = i;
            for (int j = 7; j >= 0; j--)
            {
                if ((c & 1) != 0)
                {
                    c = ((int)(((uint)c) >> 1)) ^ polynomial;
                }
                else
                {
                    c = (int)(((uint)c) >> 1);
                }
            }

            table[i] = c;
        }

        return ImmutableArray.Create(table);
    }

    internal static void Fill<T>(Span<T> buffer, int count, T value)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(paramName: null, "Negative fill count");
        buffer[..count].Fill(value);
    }

}
