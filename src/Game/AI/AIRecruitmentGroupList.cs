// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// children
public partial class AIRecruitmentGroupList : EReferenceNode, IChildList<AIRecruitmentGroup>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIRecruitmentGroup, so the cast never fails.
    public AIRecruitmentGroup this[int index] => (AIRecruitmentGroup)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIRecruitmentGroup;

    int IChildSink<AIRecruitmentGroup>.AddChild(AIRecruitmentGroup child) => AddChild(child);

    public AIRecruitmentGroupList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:800
    public AIRecruitmentGroup GetRecruitmentGroups(int index)
    {
        Assert(index >= 0 && index < _children.Count, "", "AIGroup.pas", 802);
        return this[index];
    }

    // AIGroup.pas:811
    public void SortOnMovePoints()
    {
        _children.Sort(CompareAIRecruitmentGroupMovePoints);
    }

    // AIGroup.pas:816
    public void OutputDebugInfo()
    {
        string text;
        AIRecruitmentGroup group;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            group = GetRecruitmentGroups(I);
            text = group.Group.GetType().Name + "(ID:" + IntToStr(group.Group.GroupId) + ")";
            text = text + "/Loc:(" + IntToStr(group.Location.X) + "," + IntToStr(group.Location.Y) + "," + IntToStr(group.Location.MapLevel) + ")";
            text = text + "/MP:" + IntToStr(group.MovePoints);
            text = text + "/MPBFS:" + IntToStr(group.PathStrength);
            GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, text);
        }
    }

    // AIGroup.pas:834
    public void Add(AIRecruitmentGroup group)
    {
        AddChild(group);
    }

    // AIGroup.pas:839
    public void Delete(int index)
    {
        RemoveChild(GetRecruitmentGroups(index));
    }

    // AIGroup.pas:844
    public bool Validate(AIGroupControl control)
    {
        AIRecruitmentGroup group;
        bool result = true;
        for (int I = Count - 1; I >= 0; I--)
        {
            group = this[I];
            if (!group.Validate(control))
            {
                RemoveChild(group);
                result = false;
            }
        }

        return result;
    }

    // AIGroup.pas:861
    public bool UnitExists(Troop troop)
    {
        int I;
        bool result = false;
        I = Count;
        while (!result && I != 0)
        {
            I -= 1;
            result = this[I].Group.IndexOf(troop) != -1;
        }

        return result;
    }

    // AIGroup.pas:874
    public AIRecruitmentGroup FindUnit(Troop troop)
    {
        int I;
        AIRecruitmentGroup result = null;
        I = Count;
        while (result == null && I != 0)
        {
            I -= 1;
            if (this[I].Group.IndexOf(troop) != -1)
            {
                result = this[I];
            }
        }

        return result;
    }

    private static int CompareAIRecruitmentGroupMovePoints(object item1, object item2)
    {
        return ((AIRecruitmentGroup)item2).MovePoints - ((AIRecruitmentGroup)item1).MovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
