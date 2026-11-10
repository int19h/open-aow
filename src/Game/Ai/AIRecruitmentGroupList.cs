// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Ai.Groups;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai;

// children
public partial class AIRecruitmentGroupList : EReferenceNode
{
    public AIRecruitmentGroupList(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:800
    public AIRecruitmentGroup GetRecruitmentGroups(int Index)
    {
        Assert(Index >= 0 && Index < FChildren.Count, "", "AIGroup.pas", 802);
        return (AIRecruitmentGroup)FChildren.List[Index];
    }

    // AIGroup.pas:811
    public void SortOnMovePoints()
    {
        FChildren.Sort(CompareAIRecruitmentGroupMovePoints);
    }

    // AIGroup.pas:816
    public void OutputDebugInfo()
    {
        string Text;
        AIRecruitmentGroup Group;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Group = GetRecruitmentGroups(I);
            Text = Group.FGroup.GetType().Name + "(ID:" + IntToStr(Group.FGroup.FGroupID) + ")";
            Text = Text + "/Loc:(" + IntToStr(Group.FLocation.X) + "," + IntToStr(Group.FLocation.Y) + "," + IntToStr(Group.FLocation.L) + ")";
            Text = Text + "/MP:" + IntToStr(Group.FMovePoints);
            Text = Text + "/MPBFS:" + IntToStr(Group.FPathStrength);
            GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, Text);
        }
    }

    // AIGroup.pas:834
    public void Add(AIRecruitmentGroup Group)
    {
        AddChild(Group);
    }

    // AIGroup.pas:839
    public void Delete(int Index)
    {
        RemoveChild(GetRecruitmentGroups(Index));
    }

    // AIGroup.pas:844
    public bool Validate(AIGroupControl Control)
    {
        AIRecruitmentGroup Group;
        bool Result = true;
        for (int I = Count - 1; I >= 0; I--)
        {
            Group = (AIRecruitmentGroup)FChildren.List[I];
            if (!Group.Validate(Control))
            {
                RemoveChild(Group);
                Result = false;
            }
        }

        return Result;
    }

    // AIGroup.pas:861
    public bool UnitExists(AbstractUnit AUnit)
    {
        int I;
        bool Result = false;
        I = Count;
        while (!Result && I != 0)
        {
            I -= 1;
            Result = ((AIRecruitmentGroup)FChildren.List[I]).FGroup.IndexOf(AUnit) != -1;
        }

        return Result;
    }

    // AIGroup.pas:874
    public AIRecruitmentGroup FindUnit(AbstractUnit AUnit)
    {
        int I;
        AIRecruitmentGroup Result = null;
        I = Count;
        while (Result == null && I != 0)
        {
            I -= 1;
            if (((AIRecruitmentGroup)FChildren.List[I]).FGroup.IndexOf(AUnit) != -1)
            {
                Result = (AIRecruitmentGroup)FChildren.List[I];
            }
        }

        return Result;
    }

    private static int CompareAIRecruitmentGroupMovePoints(object Item1, object Item2)
    {
        return ((AIRecruitmentGroup)Item2).FMovePoints - ((AIRecruitmentGroup)Item1).FMovePoints;
    }

    // Engine.pas:631

    // Engine.pas:631

}
