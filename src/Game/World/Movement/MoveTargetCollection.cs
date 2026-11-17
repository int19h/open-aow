#nullable disable
using System.Collections;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

/// <summary>
/// The move targets of a move target list, in the list's child slots and their stored order. The
/// list holds references and does not dispose the targets. This collection reads them typed and
/// sorts them. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct MoveTargetCollection : IReadOnlyList<MoveTarget>
{
    internal MoveTargetCollection(MoveTargetList list) => Holder = list;

    /// <summary>The list that holds the targets.</summary>
    public MoveTargetList Holder { get; }

    public int Count => Holder.GetCount();

    // The target in slot index, read as the original reads the list, without a range check. The
    // list admits only move targets, so the cast never fails.
    public MoveTarget this[int index] => Holder.GetChild<MoveTarget>(index);

    public IEnumerator<MoveTarget> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:4240
    public MoveTarget GetTarget(int index) => this[index];

    // AIGroup.pas:4245
    public int IndexOfMapCoordinate(MapCoordinate location)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].Location == location)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:4275
    public void SortOnMovePoints() => Holder.SortChildren<MoveTarget>(CompareMoveTargets);

    private static int CompareMoveTargets(MoveTarget item1, MoveTarget item2) => item1.MovePoints - item2.MovePoints;
}
