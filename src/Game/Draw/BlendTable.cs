// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FBits, FTable (the bytes of the table, which the original addresses with a pointer into the
// derived class's own field)
public partial class BlendTable
{
    internal int _bits;
    internal byte[] _table;

    // GFXE.pas:741
    public virtual BlendTable Copy()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:742
    public virtual void Assign(BlendTable source)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:9601
    public void Initialize(int bits, byte[] table)
    {
        _table = table;
        _bits = bits;
    }

    public int Bits { get => _bits; }
    public byte[] Table { get => _table; }

    internal static void GenerateAlphaBlendTable(BlendTable table, float alpha)
    {
        if (table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    ((BlendTable5Bit)table).Table[I][J] = (byte)Round((double)((double)I * alpha) + (double)((double)1 - alpha) * J);
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
        int V;
        float F;
        if (table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    if (I == 0)
                    {
                        F = 0;
                    }
                    else
                    {
                        F = (float)((double)((double)((double)I * intensity) * 1.75) / (I + J));
                    }

                    V = Round((double)I * F) + J;
                    if (V > 31)
                    {
                        V = 31;
                    }

                    ((BlendTable5Bit)table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateShadowBlendTable(BlendTable table, float shadow)
    {
        int V;
        float F;
        if (table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    F = (float)((double)((double)(31 - I) * ((double)1 / 31)) * shadow);
                    V = Round((double)((double)1 - F) * J);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    if (V > J)
                    {
                        V = J;
                    }

                    ((BlendTable5Bit)table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateBrightenBlendTable(BlendTable table, float brighten)
    {
        int V;
        if (table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    V = Round((double)((double)I * brighten) + (double)((double)1 - brighten) * J);
                    if (V < J)
                    {
                        V = J;
                    }

                    ((BlendTable5Bit)table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateLinearAlphaBlendTable(BlendTable table, float alpha)
    {
        float F;
        if (table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                if ((double)alpha > 0)
                {
                    F = (float)((double)I / ((double)31 * alpha));
                }
                else
                {
                    F = 1;
                }

                if ((double)F > 1)
                {
                    F = 1;
                }

                for (int J = 0; J <= 31; J++)
                {
                    ((BlendTable5Bit)table).Table[I][J] = (byte)Round((double)((double)I * F) + (double)((double)1 - F) * J);
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

}
