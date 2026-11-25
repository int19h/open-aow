#nullable disable warnings
using System.Collections;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

/// <summary>
/// The targets of an AI group control target list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the targets. This collection reads them
/// typed and sorts them. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct AIGroupControlTargetCollection : IReadOnlyList<AIGroupControlTarget>
{
    internal AIGroupControlTargetCollection(AIGroupControlTargetList list) => Holder = list;

    /// <summary>The list that holds the targets.</summary>
    public AIGroupControlTargetList Holder { get; }

    public int Count => Holder.GetCount();

    // The target in slot index, read as the original reads the list, without a range check. The
    // list admits only control targets.
    public AIGroupControlTarget this[int index] => Holder.GetChild<AIGroupControlTarget>(index);

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
        DelphiAssert.Assert(index >= 0 && index < Count, message: "", file: "AIGroup.pas", line: 687);
        return this[index];
    }

    // AIGroup.pas:696
    public void SortOnPriority() => Holder.SortChildren<AIGroupControlTarget>(ComparePriority);

    // AIGroup.pas:706
    public void SortOnDataB() => Holder.SortChildren<AIGroupControlTarget>(CompareDataB);

    // AIGroup.pas:739
    // Location is a field. FindLastIndex keeps the last slot, the first hit of the backward walk.
    public int IndexOfMapCoordinate(MapCoordinate location) => this.FindLastIndex(target => location == target.Location);

    private static int ComparePriority(AIGroupControlTarget item1, AIGroupControlTarget item2) => item2.Data.Priority - item1.Data.Priority;

    private static int CompareDataB(AIGroupControlTarget item1, AIGroupControlTarget item2) => item2.DataB - item1.DataB;
}
