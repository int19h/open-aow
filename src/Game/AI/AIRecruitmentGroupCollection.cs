#nullable disable
using System.Collections;

using Aow.Engine.Nodes;

using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.AI;

/// <summary>
/// The recruitment groups of a recruitment group list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the groups. This collection reads them
/// typed and sorts them. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct AIRecruitmentGroupCollection : IReadOnlyList<AIRecruitmentGroup>
{
    private readonly AIRecruitmentGroupList _list;

    internal AIRecruitmentGroupCollection(AIRecruitmentGroupList list) => _list = list;

    /// <summary>The list that holds the recruitment groups.</summary>
    public AIRecruitmentGroupList Holder => _list;

    public int Count => _list.GetCount();

    // The group in slot index, read as the original reads the list, without a range check. The
    // list admits only recruitment groups, so the cast never fails.
    public AIRecruitmentGroup this[int index] => (AIRecruitmentGroup)_list.GetChild(index);

    public IEnumerator<AIRecruitmentGroup> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:800
    public AIRecruitmentGroup GetRecruitmentGroups(int index)
    {
        Aow.Rtl.RtlUnit.Assert(index >= 0 && index < Count, message: "", file: "AIGroup.pas", line: 802);
        return this[index];
    }

    // AIGroup.pas:811
    public void SortOnMovePoints()
    {
        _list.SortChildren<AIRecruitmentGroup>(CompareMovePoints);
    }

    // AIGroup.pas:861
    public bool UnitExists(Troop troop)
    {
        int index;
        bool result = false;
        index = Count;
        while (!result && index != 0)
        {
            index -= 1;
            result = this[index].Group.Units.IndexOf(troop) != -1;
        }

        return result;
    }

    // AIGroup.pas:874
    public AIRecruitmentGroup FindUnit(Troop troop)
    {
        int index;
        AIRecruitmentGroup result = null;
        index = Count;
        while (result == null && index != 0)
        {
            index -= 1;
            if (this[index].Group.Units.IndexOf(troop) != -1)
            {
                result = this[index];
            }
        }

        return result;
    }

    private static int CompareMovePoints(AIRecruitmentGroup item1, AIRecruitmentGroup item2)
    {
        return item2.MovePoints - item1.MovePoints;
    }
}
