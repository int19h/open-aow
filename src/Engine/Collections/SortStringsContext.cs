// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

public struct SortStringsContext
{
    public StringCollection Strings { get; set; }
    public StringsSortCompare Compare { get; set; }
    public ExchangeProc ExchangeProc { get; set; }
    public DelphiList<StringCollection> ExchangeData { get; set; }

    internal static void QuickSortStrings(ref SortStringsContext context, int L, int R)
    {
        int I;
        int J;
        int M;
        object po;
        string P;
        do
        {
            I = L;
            J = R;
            M = (int)(((uint)(L + R)) >> 1);
            P = context.Strings[M];
            po = context.Strings.Objects[M];
            do
            {
                while (true)
                {
                    var item = context.Strings.Objects[I];
                    var stringsItem = context.Strings[I];
                    if (!(context.Compare(stringsItem, P, item, po) < 0))
                        break;
                    I += 1;
                }

                while (true)
                {
                    var item = context.Strings.Objects[J];
                    var stringsItem = context.Strings[J];
                    if (!(context.Compare(stringsItem, P, item, po) > 0))
                        break;
                    J -= 1;
                }

                if (I <= J)
                {
                    context.Strings.Exchange(I, J);
                    if (context.ExchangeProc != null)
                    {
                        context.ExchangeProc(context.ExchangeData, I, J);
                    }

                    I += 1;
                    J -= 1;
                }
            }
            while (!(I > J));
            if (L < J)
            {
                QuickSortStrings(ref context, L, J);
            }

            L = I;
        }
        while (!(I >= R));
    }

}
