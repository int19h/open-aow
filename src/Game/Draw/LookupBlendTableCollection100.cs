// Translated from GFXE.pas.

namespace Aow.Game.Draw;

// FTables, Lookup (101 tables, indexed by percentage)
public partial class LookupBlendTableCollection100 : IDisposable
{
    internal BlendTableList FTables;
    // { ImageLib's blitters read Lookup directly; adding lines here moves the Asserts below }
    public FixedArray_Aow_Units_GFXE_TBlendTable_101 Lookup;

    // GFXE.pas:9687
    public BlendTable GetLookupTables(int Index) => Lookup[Index];

    // GFXE.pas:9674
    public LookupBlendTableCollection100()
    {
        FTables = new BlendTableList();
    }

    // GFXE.pas:9680
    public virtual void Dispose()
    {
        Clear();
        FTables?.Dispose();
    }

    // GFXE.pas:9692
    public void Clear()
    {
        FTables.Clear();
        Lookup = default;
    }

    // GFXE.pas:9698
    public bool GenerateLookupTables(int Bits, GenerateBlendTableProc Proc, int Count)
    {
        bool Result;
        BlendTable T;
        float V;
        int J;
        Clear();
        if (Bits == 5 && Count > 0)
        {
            for (int I = 0; I <= Count - 1; I++)
            {
                switch (Bits)
                {
                    case 5:
                        T = new BlendTable5Bit();
                        break;
                    default:
                        T = null;
                        break;
                }

                if (Count != 1)
                {
                    V = (float)((double)I / (Count - 1));
                }
                else
                {
                    V = (float)0.5;
                }

                Proc(T, V);
                FTables.Add(T);
            }

            for (int I = 0; I <= 100; I++)
            {
                J = I * Count / 100;
                if (J == Count)
                {
                    J = Count - 1;
                }

                var blendTable = FTables.Tables[J];
                Lookup[I] = blendTable;
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public BlendTableList Tables { get => FTables; }

    public sealed class LookupTablesAccessor(LookupBlendTableCollection100 target)
    {
        public BlendTable this[int Index] { get => target.GetLookupTables(Index); }
    }

    public LookupTablesAccessor LookupTables => new LookupTablesAccessor(this);

    public delegate void GenerateBlendTableProc(BlendTable Table, float Value);

}
