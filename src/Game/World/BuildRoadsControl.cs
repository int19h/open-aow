// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.World;

public partial class BuildRoadsControl(CustomNode owner) : ConstructionControl(owner)
{

    // AoWAb.PAS:5458
    public override void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        text = AbilityRules.TranslateResourceString(GetEngine(), s_buildRoadText);
        gold = 0;
        days = 0;
    }

    // AoWAb.PAS:5464
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        bool result;
        MapCoordinate coordinate29 = troop.GetLocation();
        _ = coordinate29.X;
        _ = coordinate29.Y;
        sbyte l = coordinate29.MapLevel;
        if (l == 0)
        {
            result = base.CanConstruct(troop, ref reason);
        }
        else
        {
            result = false;
            string roads = AbilityRules.TranslateResourceString(GetEngine(), s_roadsText);
            reason = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_cannotBuildUndergroundFormat), [roads]);
        }

        return result;
    }

    // AoWAb.PAS:5476
    public override bool Construct(Troop troop)
    {
        bool result;
        string reason = "";
        if (CanConstruct(troop, ref reason))
        {
            StrategicMap.Current.SelectedArmy.SelectBuildRoad(troop);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // intf_P4_03.inc:16

    // AoWAb.PAS:5454
    public override int Id() => 2;

    private static readonly string s_buildRoadText = "Build Roads";
    private static readonly string s_cannotBuildUndergroundFormat = "Cannot build %s underground";
    private static readonly string s_roadsText = "Roads";

}
