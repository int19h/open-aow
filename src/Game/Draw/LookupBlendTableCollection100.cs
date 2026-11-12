// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// FTables, Lookup (101 tables, indexed by percentage)
public partial class LookupBlendTableCollection100 : IDisposable
{
    private BlendTableList _tables;
    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public BlendTableArray101 Lookup;

    // GFXE.pas:9687
    public BlendTable GetLookupTables(int index) => Lookup[index];

    // GFXE.pas:9674
    public LookupBlendTableCollection100()
    {
        _tables = new BlendTableList();
    }

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
        Lookup = default;
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int bits, GenerateBlendTableProc proc, int count)
    {
        bool result;
        BlendTable T;
        float V;
        int J;
        Clear();
        if (bits == 5 && count > 0)
        {
            for (int I = 0; I <= count - 1; I++)
            {
                switch (bits)
                {
                    case 5:
                        T = new BlendTable5Bit();
                        break;
                    default:
                        T = null;
                        break;
                }

                if (count != 1)
                {
                    V = (float)((double)I / (count - 1));
                }
                else
                {
                    V = (float)0.5;
                }

                proc(T, V);
                _tables.Add(T);
            }

            for (int I = 0; I <= 100; I++)
            {
                J = I * count / 100;
                if (J == count)
                {
                    J = count - 1;
                }

                var blendTable = _tables.Tables[J];
                Lookup[I] = blendTable;
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public BlendTableList Tables { get => _tables; }

    public sealed class LookupTablesAccessor(LookupBlendTableCollection100 target)
    {
        public BlendTable this[int index] { get => target.GetLookupTables(index); }
    }

    public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

    public delegate void GenerateBlendTableProc(BlendTable Table, float Value);

}
