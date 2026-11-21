#nullable disable

using System.Collections.Immutable;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public static class EngineServices
{
    internal const int MaxListSize = 134_217_727;

    internal const ControlStyles Stored = ControlStyles.SaveLoad;

    internal const ControlStyles Shared = ControlStyles.FixedChild;

    internal const ControlStyles Reserved2 = ControlStyles.VerifyFixedChildren;

    internal const ControlStyles KeepIndex = ControlStyles.ArrayMode;

    // The CRC-32 table of the reflected polynomial 0xEDB88320, built once.
    private static readonly ImmutableArray<int> s_crc32Table = BuildCrc32Table();

    public static int GetCrc32(ReadOnlySpan<byte> buffer, int size)
    {
        int crc = -1;
        foreach (byte value in buffer[..size])
        {
            crc = s_crc32Table[(value ^ crc) & 0xFF] ^ (crc >>> 8);
        }

        return crc ^ -1;
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
        const int polynomial = -306_674_912;
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
