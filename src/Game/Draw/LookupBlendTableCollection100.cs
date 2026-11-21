#nullable disable warnings
// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// FTables, Lookup (101 tables, indexed by percentage)
public sealed class LookupBlendTableCollection100 : IDisposable
{

    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public BlendTable[] Lookup { get; } = new BlendTable[101];

    // GFXE.pas:9687
    public BlendTable GetLookupTables(int index) => Lookup[index];

    // GFXE.pas:9674
    public LookupBlendTableCollection100() => Tables = new BlendTableList();

    // GFXE.pas:9680
    public void Dispose()
    {
        Clear();
        Tables?.Dispose();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        Tables.Clear();
        Array.Clear(Lookup);
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int bits, Action<BlendTable?, float> proc, int count)
    {
        Clear();
        if (bits == 5 && count > 0)
        {
            for (int i = 0; i <= count - 1; i++)
            {
                BlendTable? t = bits switch
                {
                    5 => new BlendTable5Bit(),
                    _ => null,
                };
                float v = count != 1 ? (float)((double)i / (count - 1)) : (float)0.5;

                proc(t, v);
                Tables.Add(t);
            }

            for (int i = 0; i <= 100; i++)
            {
                int j = i * count / 100;
                if (j == count)
                {
                    j = count - 1;
                }

                BlendTable blendTable = Tables.Tables[j];
                Lookup[i] = blendTable;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public BlendTableList Tables { get; }

    public sealed class LookupTablesAccessor(LookupBlendTableCollection100 target)
    {
        public BlendTable this[int index] => target.GetLookupTables(index);
    }

    public LookupTablesAccessor LookupTables => new(this);
}
