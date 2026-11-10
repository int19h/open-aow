using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Storage;
using Aow.Game.Saves;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Nodes;

internal static class StringSorting
{
    internal static void SortStrings(TStrings Strings, TStringsSortCompare Compare)
    {
        TSortStringsContext Ctx = default;
        if (Strings.Count != 0)
        {
            Ctx.Strings = Strings;
            Ctx.Compare = Compare;
            Ctx.ExchangeProc = null;
            var count = Strings.Count;
            Aow.Game.Engine.Collections.TSortStringsContext.QuickSortStrings(ref Ctx, 0, count - 1);
        }
    }

    internal static void SortStringsEx(TStrings Strings, TStringsSortCompare Compare, ReadOnlySpan<TStrings> LinkedStrings)
    {
        TList Lists = default;
        TSortStringsContext Ctx = default;
        if (Strings.Count != 0)
        {
            Strings.BeginUpdate();
            Ctx.Strings = Strings;
            Ctx.Compare = Compare;
            // { Original unsigned test also takes an empty array. }
            if ((uint)(LinkedStrings.Length - 1) >= 0)
            {
                Lists = new TList();
                for (int I = 0, last = LinkedStrings.Length - 1; I <= last; I++)
                {
                    LinkedStrings[I].BeginUpdate();
                    Lists.Add(LinkedStrings[I]);
                }

                Ctx.ExchangeProc = Aow.Game.Engine.Nodes.EngineServices.ExchangeLinkedStrings;
                Ctx.ExchangeData = Lists;
            }
            else
            {
                Lists = null;
            }

            var count = Strings.Count;
            Aow.Game.Engine.Collections.TSortStringsContext.QuickSortStrings(ref Ctx, 0, count - 1);
            if (Lists != null)
            {
                for (int I = 0, last = LinkedStrings.Length - 1; I <= last; I++)
                {
                    LinkedStrings[I].EndUpdate();
                }

                Lists?.Dispose();
            }

            Strings.EndUpdate();
        }
    }

    internal static void SortStringsAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(Strings, Aow.Game.Engine.Nodes.StringSorting.CompareAscending, LinkedStrings);
    }

    internal static void SortStringsDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(Strings, Aow.Game.Engine.Nodes.StringSorting.CompareDescending, LinkedStrings);
    }

    internal static void SortStringsNumericalAscending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(Strings, Aow.Game.Engine.Nodes.StringSorting.CompareNumericalAscending, LinkedStrings);
    }

    internal static void SortStringsNumericalDescending(TStrings Strings, ReadOnlySpan<TStrings> LinkedStrings)
    {
        Aow.Game.Engine.Nodes.StringSorting.SortStringsEx(Strings, Aow.Game.Engine.Nodes.StringSorting.CompareNumericalDescending, LinkedStrings);
    }

    internal static int CompareAscending(string S1, string S2, object O1, object O2)
    {
        int Result = CompareText(S1, S2);
        if (Result == 0)
        {
            Result = S1.Length - S2.Length;
        }

        return Result;
    }

    internal static int CompareDescending(string S1, string S2, object O1, object O2)
    {
        int Result = CompareText(S2, S1);
        if (Result == 0)
        {
            Result = S2.Length - S1.Length;
        }

        return Result;
    }

    internal static int CompareNumericalAscending(string S1, string S2, object O1, object O2)
    {
        int V1;
        int V2;
        V1 = StrToIntDef(S1, -1);
        V2 = StrToIntDef(S2, -1);
        return V1 - V2;
    }

    internal static int CompareNumericalDescending(string S1, string S2, object O1, object O2)
    {
        int V1;
        int V2;
        V1 = StrToIntDef(S1, -1);
        V2 = StrToIntDef(S2, -1);
        return V2 - V1;
    }

}
