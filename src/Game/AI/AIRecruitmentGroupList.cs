// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.AI;

// children
public class AIRecruitmentGroupList(CustomNode owner) : ReferenceNode(owner), IChildList<AIRecruitmentGroup>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIRecruitmentGroup, so the cast never fails.
    public AIRecruitmentGroup this[int index] => Groups[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIRecruitmentGroup;

    int IChildSink<AIRecruitmentGroup>.AddChild(AIRecruitmentGroup child) => AddChild(child);

    /// <summary>The recruitment groups of this list, in their stored order.</summary>
    public AIRecruitmentGroupCollection Groups => new(this);

    // AIGroup.pas:800
    public AIRecruitmentGroup GetRecruitmentGroups(int index) => Groups.GetRecruitmentGroups(index);

    // AIGroup.pas:811
    public void SortOnMovePoints() => Groups.SortOnMovePoints();

    // AIGroup.pas:816
    public void OutputDebugInfo()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            AIRecruitmentGroup group = GetRecruitmentGroups(i);
            string text = $"{group.Group.GetType().Name}(ID:{group.Group.GroupId.ToString(CultureInfo.InvariantCulture)})";
            text = $"{text}/Loc:({((int)group.Location.X).ToString(CultureInfo.InvariantCulture)},{((int)group.Location.Y).ToString(CultureInfo.InvariantCulture)},{((int)group.Location.MapLevel).ToString(CultureInfo.InvariantCulture)})";
            text = $"{text}/MP:{group.MovePoints.ToString(CultureInfo.InvariantCulture)}";
            text = $"{text}/MPBFS:{group.PathStrength.ToString(CultureInfo.InvariantCulture)}";
            GetEngine().OutputDebugMessage(DebugChannel.AI, S: text);
        }
    }

    // AIGroup.pas:834
    public void Add(AIRecruitmentGroup group) => AddChild(group);

    // AIGroup.pas:839
    public void Delete(int index) => RemoveChild(GetRecruitmentGroups(index));

    // AIGroup.pas:844
    public bool Validate(AIGroupControl control)
    {
        bool result = true;
        for (int i = Count - 1; i >= 0; i--)
        {
            AIRecruitmentGroup group = this[i];
            if (!group.Validate(control))
            {
                _ = RemoveChild(group);
                result = false;
            }
        }

        return result;
    }

    // AIGroup.pas:861
    public bool UnitExists(Troop troop) => Groups.UnitExists(troop);

    // AIGroup.pas:874
    public AIRecruitmentGroup FindUnit(Troop troop) => Groups.FindUnit(troop);
}
