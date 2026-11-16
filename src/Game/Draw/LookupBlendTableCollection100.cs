#nullable disable
// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// FTables, Lookup (101 tables, indexed by percentage)
public partial class LookupBlendTableCollection100 : IDisposable
{
    private readonly BlendTableList _tables;

    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public BlendTable[] Lookup { get; } = new BlendTable[101];

    // GFXE.pas:9687
    public BlendTable GetLookupTables(int index) => Lookup[index];

    // GFXE.pas:9674
    public LookupBlendTableCollection100() => _tables = new BlendTableList();

    // GFXE.pas:9680
    public virtual void Dispose()
    {
        Clear();
        _tables?.Dispose();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        _tables.Clear();
        Array.Clear(Lookup);
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int bits, GenerateBlendTableProc proc, int count)
    {
        bool result;
        BlendTable t;
        float v;
        int j;
        Clear();
        if (bits == 5 && count > 0)
        {
            for (int i = 0; i <= count - 1; i++)
            {
                switch (bits)
                {
                    case 5:
                        t = new BlendTable5Bit();
                        break;
                    default:
                        t = null;
                        break;
                }

                v = count != 1 ? (float)((double)i / (count - 1)) : (float)0.5;

                proc(t, v);
                _tables.Add(t);
            }

            for (int i = 0; i <= 100; i++)
            {
                j = i * count / 100;
                if (j == count)
                {
                    j = count - 1;
                }

                BlendTable blendTable = _tables.Tables[j];
                Lookup[i] = blendTable;
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public BlendTableList Tables => _tables;

    public sealed class LookupTablesAccessor(LookupBlendTableCollection100 target)
    {
        public BlendTable this[int index] => target.GetLookupTables(index);
    }

    public LookupTablesAccessor LookupTables => new(this);

    public delegate void GenerateBlendTableProc(BlendTable Table, float Value);

}
