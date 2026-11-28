#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.World;

public class BuildRoadsControl(CustomNode owner) : ConstructionControl(owner)
{

    // AoWAb.PAS:5458
    public override void GetInfo(Troop troop, out string text, out int gold, out int days)
    {
        text = AbilityRules.TranslateResourceString(GetEngine(), S: s_buildRoadText);
        gold = 0;
        days = 0;
    }

    // AoWAb.PAS:5464
    public override bool CanConstruct(Troop troop, ref string reason)
    {
        MapCoordinate coordinate29 = troop.GetLocation();
        _ = coordinate29.X;
        _ = coordinate29.Y;
        int l = coordinate29.MapLevel;
        if (l == 0)
        {
            return base.CanConstruct(troop, ref reason);
        }
        else
        {
            string roads = AbilityRules.TranslateResourceString(GetEngine(), S: s_roadsText);
            reason = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_cannotBuildUndergroundFormat), arguments: [roads]);
        }

        return false;
    }

    // AoWAb.PAS:5476
    public override bool Construct(Troop troop)
    {
        string reason = "";
        if (CanConstruct(troop, ref reason))
        {
            GetEngine().StrategicMap.SelectedArmy.SelectBuildRoad(troop);
            return true;
        }
        else
        {
            return false;
        }
    }

    // AoWAb.PAS:5454
    public override int Id() => 2;

    private static readonly string s_buildRoadText = "Build Roads";
    private static readonly string s_cannotBuildUndergroundFormat = "Cannot build %s underground";
    private static readonly string s_roadsText = "Roads";

}
