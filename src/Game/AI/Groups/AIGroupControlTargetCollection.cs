#nullable disable
using System.Collections;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

/// <summary>
/// The targets of an AI group control target list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the targets. This collection reads them
/// typed and sorts them. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct AIGroupControlTargetCollection : IReadOnlyList<AIGroupControlTarget>
{
    private readonly AIGroupControlTargetList _list;

    internal AIGroupControlTargetCollection(AIGroupControlTargetList list) => _list = list;

    /// <summary>The list that holds the targets.</summary>
    public AIGroupControlTargetList Holder => _list;

    public int Count => _list.GetCount();

    // The target in slot index, read as the original reads the list, without a range check. The
    // list admits only control targets.
    public AIGroupControlTarget this[int index] => _list.GetChild<AIGroupControlTarget>(index);

    public IEnumerator<AIGroupControlTarget> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:684
    public AIGroupControlTarget GetTargets(int index)
    {
        Rtl.RtlUnit.Assert(index >= 0 && index < Count, message: "", file: "AIGroup.pas", line: 687);
        return this[index];
    }

    // AIGroup.pas:696
    public void SortOnPriority() => _list.SortChildren<AIGroupControlTarget>(ComparePriority);

    // AIGroup.pas:706
    public void SortOnDataB() => _list.SortChildren<AIGroupControlTarget>(CompareDataB);

    // AIGroup.pas:739
    public int IndexOfMapCoordinate(MapCoordinate location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (location == this[result].Location)
            {
                return result;
            }
        }

        return -1;
    }

    private static int ComparePriority(AIGroupControlTarget item1, AIGroupControlTarget item2) => item2.Data.Priority - item1.Data.Priority;

    private static int CompareDataB(AIGroupControlTarget item1, AIGroupControlTarget item2) => item2.DataB - item1.DataB;
}
