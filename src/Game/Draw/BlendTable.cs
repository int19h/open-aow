// Translated from GFXE.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FBits, FTable (the bytes of the table, which the original addresses with a pointer into the
// derived class's own field)
public partial class BlendTable
{
    internal int FBits;
    internal byte[] FTable;

    // GFXE.pas:741
    public virtual BlendTable Copy()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:742
    public virtual void Assign(BlendTable Source)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // GFXE.pas:9601
    public void Initialize(int ABits, byte[] ATable)
    {
        FTable = ATable;
        FBits = ABits;
    }

    public int Bits { get => FBits; }
    public byte[] Table { get => FTable; }

    internal static void GenerateAlphaBlendTable(BlendTable Table, float Alpha)
    {
        if (Table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    ((BlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * Alpha) + (double)((double)1 - Alpha) * J);
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

    internal static void GenerateIntensityBlendTable(BlendTable Table, float Intensity)
    {
        int V;
        float F;
        if (Table is BlendTable5Bit)
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
                        F = (float)((double)((double)((double)I * Intensity) * 1.75) / (I + J));
                    }

                    V = Round((double)I * F) + J;
                    if (V > 31)
                    {
                        V = 31;
                    }

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateShadowBlendTable(BlendTable Table, float Shadow)
    {
        int V;
        float F;
        if (Table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    F = (float)((double)((double)(31 - I) * ((double)1 / 31)) * Shadow);
                    V = Round((double)((double)1 - F) * J);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    if (V > J)
                    {
                        V = J;
                    }

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateBrightenBlendTable(BlendTable Table, float Brighten)
    {
        int V;
        if (Table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                for (int J = 0; J <= 31; J++)
                {
                    V = Round((double)((double)I * Brighten) + (double)((double)1 - Brighten) * J);
                    if (V < J)
                    {
                        V = J;
                    }

                    ((BlendTable5Bit)Table).Table[I][J] = (byte)V;
                }
            }
        }
    }

    internal static void GenerateLinearAlphaBlendTable(BlendTable Table, float Alpha)
    {
        float F;
        if (Table is BlendTable5Bit)
        {
            for (int I = 0; I <= 31; I++)
            {
                if ((double)Alpha > 0)
                {
                    F = (float)((double)I / ((double)31 * Alpha));
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
                    ((BlendTable5Bit)Table).Table[I][J] = (byte)Round((double)((double)I * F) + (double)((double)1 - F) * J);
                }
            }
        }
        else
        {
            throw new NotSupportedException("BlendTable format not supported");
        }
    }

}
