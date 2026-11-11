// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.World;

public partial class BuildRoadsControl : ConstructionControl
{
    // AoWAb.PAS:5446
    public BuildRoadsControl(ECustomNode owner) : base(owner)
    {
    }

    // AoWAb.PAS:5458
    public override void GetInfo(Troop AUnit, ref string Text, ref int Gold, ref int Days)
    {
        Text = AbilityRules.TranslateRStr(P4BuildRoadRStr);
        Gold = 0;
        Days = 0;
    }

    // AoWAb.PAS:5464
    public override bool CanConstruct(Troop AUnit, ref string Reason)
    {
        bool Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        AUnit.GetLocation(ref X, ref Y, ref L);
        if (L == 0)
        {
            Result = base.CanConstruct(AUnit, ref Reason);
        }
        else
        {
            Result = false;
            var p4Roads = AbilityRules.TranslateRStr(P4RoadsRStr);
            Reason = AbilityRules.Format(AbilityRules.TranslateRStr(P4CannotBuildUndergroundRStr), [p4Roads]);
        }

        return Result;
    }

    // AoWAb.PAS:5476
    public override bool Construct(Troop AUnit)
    {
        bool Result;
        string Reason = "";
        if (CanConstruct(AUnit, ref Reason))
        {
            AoWHSMap.Current.FSelectedArmy.SelectBuildRoad(AUnit);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // intf_P4_03.inc:16

    // AoWAb.PAS:5454
    public override int ID() => 2;

    private static readonly string P4BuildRoadRStr = "Build Roads";
    private static readonly string P4CannotBuildUndergroundRStr = "Cannot build %s underground";
    private static readonly string P4RoadsRStr = "Roads";

}
