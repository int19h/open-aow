#nullable disable warnings
using System.Collections;
using Aow.Game.Magic.Abilities;
using Aow.Rtl;

namespace Aow.Game.AI;

/// <summary>
/// The recruitment groups of a recruitment group list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the groups. This collection reads them
/// typed and sorts them. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct AIRecruitmentGroupCollection : IReadOnlyList<AIRecruitmentGroup>
{
    internal AIRecruitmentGroupCollection(AIRecruitmentGroupList list) => Holder = list;

    /// <summary>The list that holds the recruitment groups.</summary>
    public AIRecruitmentGroupList Holder { get; }

    public int Count => Holder.GetCount();

    // The group in slot index, read as the original reads the list, without a range check. The
    // list admits only recruitment groups.
    public AIRecruitmentGroup this[int index] => Holder.GetChild<AIRecruitmentGroup>(index);

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
        DelphiAssert.Assert(index >= 0 && index < Count, message: "", file: "AIGroup.pas", line: 802);
        return this[index];
    }

    // AIGroup.pas:811
    public void SortOnMovePoints() => Holder.SortChildren<AIRecruitmentGroup>(CompareMovePoints);

    // AIGroup.pas:861
    // IndexOf reads the unit list. FindLastIndex keeps the backward walk.
    public bool UnitExists(Troop troop) => this.FindLastIndex(group => group.Group.Units.IndexOf(troop) != -1) != -1;

    // AIGroup.pas:874. FindLast keeps the last slot, which is the first hit of the backward walk.
    public AIRecruitmentGroup? FindUnit(Troop troop) => this.FindLast(group => group.Group.Units.IndexOf(troop) != -1);

    private static int CompareMovePoints(AIRecruitmentGroup item1, AIRecruitmentGroup item2) => item2.MovePoints - item1.MovePoints;
}
