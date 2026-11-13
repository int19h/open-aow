// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public partial class PassiveAIGroup : AIGroup, IAIGroupClass
{
    // MainAG.pas:129
    public new const int ClassId = 136280;

    // MainAG.pas:122
    public PassiveAIGroup(ECustomNode owner) : base(owner)
    {
        ProcessPriority = 0;
        RecruitmentPriority = 1;
    }

    // MainAG.pas:138
    public override MapCoordinate GetLocation()
    {
        MapCoordinate location = default;
        if (TargetMode == (byte)AIGroupDefenseControl.GroupDefendMode.Custom)
        {
            location = TargetMapCoordinate.Location;
        }
        else
        {
            location = base.GetLocation();
        }

        return location;
    }

    // MainAG.pas:147
    public override bool GetRecruitableUnits(AIGroup recruiter, UnitList units, UnitList recruitable)
    {
        MapCoordinate location = default;
        GroupDefenseSelector selector;
        Troop troop;
        location = GetLocation();
        bool result = false;
        selector = new GroupDefenseSelector(this);
        selector.Execute(this, location.X, location.Y, location.MapLevel, units);
        if (selector.Defenders.Count != 0)
        {
            for (int I = units.Count - 1; I >= 0; I--)
            {
                troop = units.GetUnit(I);
                if (selector.Defenders.IndexOf(troop) == -1)
                {
                    recruitable.AddUnit(troop);
                    result = true;
                }
            }
        }
        else
        {
            recruitable.AddUnits(units);
            result = true;
        }

        selector?.Dispose();
        return result;
    }

    // MainAG.pas:135
    public override EngineClass GroupControlClass() => EngineClass.Of<PassiveAIGroupControl>();

    // MainAG.pas:132
    public static new GroupBehavior Behavior => GroupBehavior.Passive;

}
