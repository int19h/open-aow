// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

public struct SortStringsContext
{
    public StringCollection Strings;
    public StringsSortCompare Compare;
    public ExchangeProc ExchangeProc;
    public DelphiList ExchangeData;

    internal static void QuickSortStrings(ref SortStringsContext Ctx, int L, int R)
    {
        int I;
        int J;
        int M;
        object PO;
        string P;
        do
        {
            I = L;
            J = R;
            M = (int)(((uint)(L + R)) >> 1);
            P = Ctx.Strings[M];
            PO = Ctx.Strings.Objects[M];
            do
            {
                while (true)
                {
                    var item = Ctx.Strings.Objects[I];
                    var stringsItem = Ctx.Strings[I];
                    if (!(Ctx.Compare(stringsItem, P, item, PO) < 0))
                        break;
                    I += 1;
                }

                while (true)
                {
                    var item = Ctx.Strings.Objects[J];
                    var stringsItem = Ctx.Strings[J];
                    if (!(Ctx.Compare(stringsItem, P, item, PO) > 0))
                        break;
                    J -= 1;
                }

                if (I <= J)
                {
                    Ctx.Strings.Exchange(I, J);
                    if (Ctx.ExchangeProc != null)
                    {
                        Ctx.ExchangeProc(Ctx.ExchangeData, I, J);
                    }

                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortStrings(ref Ctx, L, J);
            }

            L = I;
        }
        while (!(I >= R));
    }

}
