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
    public PassiveAIGroup(CustomNode owner) : base(owner)
    {
        ProcessPriority = 0;
        RecruitmentPriority = 1;
    }

    // MainAG.pas:138
    public override MapCoordinate GetLocation()
    {
        MapCoordinate location = TargetMode == (byte)AIGroupDefenseControl.GroupDefendMode.Custom ? TargetLocation ?? MapCoordinate.None : base.GetLocation();
        return location;
    }

    // MainAG.pas:147
    public override bool GetRecruitableUnits(AIGroup recruiter, UnitUserList units, UnitUserList recruitable)
    {
        GroupDefenseSelector selector;
        Troop troop;
        MapCoordinate location = GetLocation();
        bool result = false;
        selector = new GroupDefenseSelector(this);
        _ = selector.Execute(this, location.X, location.Y, location.MapLevel, units);
        if (selector.Defenders.Count != 0)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                troop = units.GetUnit(i);
                if (selector.Defenders.IndexOf(troop) == -1)
                {
                    _ = recruitable.AddUnit(troop);
                    result = true;
                }
            }
        }
        else
        {
            _ = recruitable.AddUnits(units.Units);
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
