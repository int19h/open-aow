// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Ai.Groups;

public partial class PassiveAG : AIGroup, IAIGroupClass
{
    // MainAG.pas:129
    public new const int ClassID = 136280;

    // MainAG.pas:122
    public PassiveAG(ECustomNode owner) : base(owner)
    {
        FProcessPriority = 0;
        FRecruitmentPriority = 1;
    }

    // MainAG.pas:138
    public override int GetLoc()
    {
        XYL Location = default;
        if (FTargetMode == (byte)AIGroupDefenseControl.GroupDefendMode.Custom)
        {
            Location = FTargetXYL;
        }
        else
        {
            Location.Packed = base.GetLoc();
        }

        return Location.Packed;
    }

    // MainAG.pas:147
    public override bool GetRecruitableUnits(AIGroup Recruiter, UnitList Units, UnitList Recruitable)
    {
        XYL Location = default;
        GroupDefenseSelector Selector;
        AbstractUnit AUnit;
        Location.Packed = GetLoc();
        bool Result = false;
        Selector = new GroupDefenseSelector();
        Selector.Execute(this, Location.X, Location.Y, Location.L, Units);
        if (Selector.FDefenders.Count != 0)
        {
            for (int I = Units.Count - 1; I >= 0; I--)
            {
                AUnit = Units.GetUnit(I);
                if (Selector.FDefenders.IndexOf(AUnit) == -1)
                {
                    Recruitable.AddUnit(AUnit);
                    Result = true;
                }
            }
        }
        else
        {
            Recruitable.AddUnits(Units);
            Result = true;
        }

        Selector?.Dispose();
        return Result;
    }

    // MainAG.pas:135
    public override EClass GroupControlClass() => EClass.Of<PassiveAGC>();

    // MainAG.pas:132
    public static new GroupBehavior Behavior => GroupBehavior.Passive;

}
