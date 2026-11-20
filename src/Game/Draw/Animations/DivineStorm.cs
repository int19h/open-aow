// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Draw.Animations;

public class DivineStorm(CustomNode owner) : BlastStorm(owner)
{

    // Storms.pas:608
    public new const int ClassId = 135_740;

    // Storms.pas:618
    public override void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (terrainType is not 6 and not 0)
        {
            terrainType = 1;
        }
    }

    // Storms.pas:624
    public override string StormFileName() => "EFFECTS\\STORMLGT.ILB";

    // Storms.pas:629
    public override StormType StormType() => Magic.Spells.StormType.Holy;

    // Storms.pas:613
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.DivineStormText);

}
