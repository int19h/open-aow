#nullable disable
// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FBits, FTable (the bytes of the table, which the original addresses with a pointer into the
// derived class's own field)
public partial class BlendTable
{
    private int _bits;
    private byte[] _table;

    // GFXE.pas:741
    public virtual BlendTable Copy() => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:742
    public virtual void Assign(BlendTable source) => throw AbstractErrorException.Create("Abstract Error");

    // GFXE.pas:9601
    public void Initialize(int bits, byte[] table)
    {
        _table = table;
        _bits = bits;
    }

    public int Bits => _bits;
    public byte[] Table => _table;

    // The bytes of the table in one span, as the blenders read them.
    public ReadOnlySpan<byte> Bytes => Table;

    internal static void GenerateAlphaBlendTable(BlendTable table, float alpha)
    {
        if (table is BlendTable5Bit fiveBit)
        {
            for (int i = 0; i <= 31; i++)
            {
                for (int j = 0; j <= 31; j++)
                {
                    fiveBit.RowView[i][j] = (byte)Round((double)((double)i * alpha) + ((double)((double)1 - alpha) * j));
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

    internal static void GenerateIntensityBlendTable(BlendTable table, float intensity)
    {
        int v;
        float f;
        if (table is BlendTable5Bit fiveBit)
        {
            for (int i = 0; i <= 31; i++)
            {
                for (int j = 0; j <= 31; j++)
                {
                    if (i == 0)
                    {
                        f = 0;
                    }
                    else
                    {
                        f = (float)((double)((double)((double)i * intensity) * 1.75) / (i + j));
                    }

                    v = Round((double)i * f) + j;
                    if (v > 31)
                    {
                        v = 31;
                    }

                    fiveBit.RowView[i][j] = (byte)v;
                }
            }
        }
    }

    internal static void GenerateShadowBlendTable(BlendTable table, float shadow)
    {
        int v;
        float f;
        if (table is BlendTable5Bit fiveBit)
        {
            for (int i = 0; i <= 31; i++)
            {
                for (int j = 0; j <= 31; j++)
                {
                    f = (float)((double)((31 - i) * ((double)1 / 31)) * shadow);
                    v = Round((double)((double)1 - f) * j);
                    if (v < 0)
                    {
                        v = 0;
                    }

                    if (v > j)
                    {
                        v = j;
                    }

                    fiveBit.RowView[i][j] = (byte)v;
                }
            }
        }
    }

    internal static void GenerateBrightenBlendTable(BlendTable table, float brighten)
    {
        int v;
        if (table is BlendTable5Bit fiveBit)
        {
            for (int i = 0; i <= 31; i++)
            {
                for (int j = 0; j <= 31; j++)
                {
                    v = Round((double)((double)i * brighten) + ((double)((double)1 - brighten) * j));
                    if (v < j)
                    {
                        v = j;
                    }

                    fiveBit.RowView[i][j] = (byte)v;
                }
            }
        }
    }

    internal static void GenerateLinearAlphaBlendTable(BlendTable table, float alpha)
    {
        float f;
        if (table is BlendTable5Bit fiveBit)
        {
            for (int i = 0; i <= 31; i++)
            {
                if ((double)alpha > 0)
                {
                    f = (float)(i / ((double)31 * alpha));
                }
                else
                {
                    f = 1;
                }

                if ((double)f > 1)
                {
                    f = 1;
                }

                for (int j = 0; j <= 31; j++)
                {
                    fiveBit.RowView[i][j] = (byte)Round((double)((double)i * f) + ((double)((double)1 - f) * j));
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

}
