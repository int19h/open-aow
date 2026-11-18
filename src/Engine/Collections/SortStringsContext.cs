#nullable disable
// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Collections;

public struct SortStringsContext
{
    public DelphiStringCollection Strings { get; set; }
    public StringsSortCompare Compare { get; set; }
    public ExchangeProc ExchangeProc { get; set; }
    public DelphiList<DelphiStringCollection> ExchangeData { get; set; }

    internal static void QuickSortStrings(ref SortStringsContext context, int L, int R)
    {
        int i;
        int j;
        int m;
        object po;
        string p;
        do
        {
            i = L;
            j = R;
            m = (int)(((uint)(L + R)) >> 1);
            p = context.Strings[m];
            po = context.Strings.Objects[m];
            do
            {
                while (true)
                {
                    object item = context.Strings.Objects[i];
                    string stringsItem = context.Strings[i];
                    if (!(context.Compare(stringsItem, p, item, po) < 0))
                    {
                        break;
                    }

                    i += 1;
                }

                while (true)
                {
                    object item = context.Strings.Objects[j];
                    string stringsItem = context.Strings[j];
                    if (!(context.Compare(stringsItem, p, item, po) > 0))
                    {
                        break;
                    }

                    j -= 1;
                }

                if (i <= j)
                {
                    context.Strings.Exchange(i, j);
                    context.ExchangeProc?.Invoke(context.ExchangeData, i, j);

                    i += 1;
                    j -= 1;
                }
            }
            while (!(i > j));
            if (L < j)
            {
                QuickSortStrings(ref context, L, j);
            }

            L = i;
        }
        while (!(i >= R));
    }

}
