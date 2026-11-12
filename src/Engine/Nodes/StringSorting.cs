
using Aow.Engine.Collections;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

internal static class StringSorting
{
    internal static void SortStrings(StringCollection strings, StringsSortCompare compare)
    {
        SortStringsContext context = default;
        if (strings.Count != 0)
        {
            context.Strings = strings;
            context.Compare = compare;
            context.ExchangeProc = null;
            var count = strings.Count;
            SortStringsContext.QuickSortStrings(ref context, 0, count - 1);
        }
    }

    internal static void SortStrings(StringCollection strings, StringsSortCompare compare, ReadOnlySpan<StringCollection> linkedStrings)
    {
        DelphiList lists = default;
        SortStringsContext context = default;
        if (strings.Count != 0)
        {
            strings.BeginUpdate();
            context.Strings = strings;
            context.Compare = compare;
            // { Original unsigned test also takes an empty array. }
            if ((uint)(linkedStrings.Length - 1) >= 0)
            {
                lists = new DelphiList();
                for (int I = 0, last = linkedStrings.Length - 1; I <= last; I++)
                {
                    linkedStrings[I].BeginUpdate();
                    lists.Add(linkedStrings[I]);
                }

                context.ExchangeProc = EngineServices.ExchangeLinkedStrings;
                context.ExchangeData = lists;
            }
            else
            {
                lists = null;
            }

            var count = strings.Count;
            SortStringsContext.QuickSortStrings(ref context, 0, count - 1);
            if (lists != null)
            {
                for (int I = 0, last = linkedStrings.Length - 1; I <= last; I++)
                {
                    linkedStrings[I].EndUpdate();
                }

                lists?.Dispose();
            }

            strings.EndUpdate();
        }
    }

    internal static void SortStringsAscending(StringCollection strings, ReadOnlySpan<StringCollection> linkedStrings)
    {
        SortStrings(strings, CompareAscending, linkedStrings);
    }

    internal static void SortStringsDescending(StringCollection strings, ReadOnlySpan<StringCollection> linkedStrings)
    {
        SortStrings(strings, CompareDescending, linkedStrings);
    }

    internal static void SortStringsNumericalAscending(StringCollection strings, ReadOnlySpan<StringCollection> linkedStrings)
    {
        SortStrings(strings, CompareNumericalAscending, linkedStrings);
    }

    internal static void SortStringsNumericalDescending(StringCollection strings, ReadOnlySpan<StringCollection> linkedStrings)
    {
        SortStrings(strings, CompareNumericalDescending, linkedStrings);
    }

    internal static int CompareAscending(string s1, string s2, object o1, object o2)
    {
        int result = CompareText(s1, s2);
        if (result == 0)
        {
            result = s1.Length - s2.Length;
        }

        return result;
    }

    internal static int CompareDescending(string s1, string s2, object o1, object o2)
    {
        int result = CompareText(s2, s1);
        if (result == 0)
        {
            result = s2.Length - s1.Length;
        }

        return result;
    }

    internal static int CompareNumericalAscending(string s1, string s2, object o1, object o2)
    {
        int v1;
        int v2;
        v1 = StrToIntDef(s1, -1);
        v2 = StrToIntDef(s2, -1);
        return v1 - v2;
    }

    internal static int CompareNumericalDescending(string s1, string s2, object o1, object o2)
    {
        int v1;
        int v2;
        v1 = StrToIntDef(s1, -1);
        v2 = StrToIntDef(s2, -1);
        return v2 - v1;
    }

}
