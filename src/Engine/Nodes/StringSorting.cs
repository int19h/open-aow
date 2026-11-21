#nullable disable warnings

using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Engine.Nodes;

internal static class StringSorting
{
    internal static void SortStrings(this DelphiStringCollection strings, Func<string, string, object?, object?, int> compare, ReadOnlySpan<DelphiStringCollection> linkedStrings)
    {
        SortStringsContext context = default;
        if (strings.Count != 0)
        {
            strings.BeginUpdate();
            context.Strings = strings;
            context.Compare = compare;
            // { Original unsigned test also takes an empty array. }
            DelphiList<DelphiStringCollection>? lists;
            if ((uint)(linkedStrings.Length - 1) >= 0)
            {
                lists = new DelphiList<DelphiStringCollection>();
                for (int i = 0, last = linkedStrings.Length - 1; i <= last; i++)
                {
                    linkedStrings[i].BeginUpdate();
                    _ = lists.Add(linkedStrings[i]);
                }

                context.ExchangeProc = EngineServices.ExchangeLinkedStrings;
                context.ExchangeData = lists;
            }
            else
            {
                lists = null;
            }

            int count = strings.Count;
            SortStringsContext.QuickSortStrings(ref context, L: 0, R: count - 1);
            if (lists is not null)
            {
                for (int i = 0, last = linkedStrings.Length - 1; i <= last; i++)
                {
                    linkedStrings[i].EndUpdate();
                }

                lists?.Dispose();
            }

            strings.EndUpdate();
        }
    }

    internal static void SortStringsAscending(this DelphiStringCollection strings, ReadOnlySpan<DelphiStringCollection> linkedStrings) => strings.SortStrings(compare: CompareAscending, linkedStrings: linkedStrings);

    internal static void SortStringsDescending(this DelphiStringCollection strings, ReadOnlySpan<DelphiStringCollection> linkedStrings) => strings.SortStrings(compare: CompareDescending, linkedStrings: linkedStrings);

    internal static void SortStringsNumericalAscending(this DelphiStringCollection strings, ReadOnlySpan<DelphiStringCollection> linkedStrings) => strings.SortStrings(compare: CompareNumericalAscending, linkedStrings: linkedStrings);

    internal static void SortStringsNumericalDescending(this DelphiStringCollection strings, ReadOnlySpan<DelphiStringCollection> linkedStrings) => strings.SortStrings(compare: CompareNumericalDescending, linkedStrings: linkedStrings);

    internal static int CompareAscending(string s1, string s2, object? o1, object? o2)
    {
        int result = CompareText(s1, s2);
        return result == 0 ? s1.Length - s2.Length : result;
    }

    internal static int CompareDescending(string s1, string s2, object? o1, object? o2)
    {
        int result = CompareText(s2, s1);
        return result == 0 ? s2.Length - s1.Length : result;
    }

    internal static int CompareNumericalAscending(string s1, string s2, object? o1, object? o2)
    {
        int v1 = StrToIntDef(s1, fallback: -1);
        int v2 = StrToIntDef(s2, fallback: -1);
        return v1 - v2;
    }

    internal static int CompareNumericalDescending(string s1, string s2, object? o1, object? o2)
    {
        int v1 = StrToIntDef(s1, fallback: -1);
        int v2 = StrToIntDef(s2, fallback: -1);
        return v2 - v1;
    }
}
