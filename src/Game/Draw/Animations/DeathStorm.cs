// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Draw.Animations;

public partial class DeathStorm : BlastStorm
{
    public DeathStorm(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:492
    public new const int ClassId = 135741;

    // Storms.pas:502
    public override void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (terrainType != 0 && terrainType != 6)
        {
            terrainType = 5;
        }
    }

    // Storms.pas:507
    public override string StormFileName() => "EFFECTS\\STORMD.ILB";

    // Storms.pas:512
    public override StormType StormType() => Magic.Spells.StormType.Death;

    // Animation.pas:43

    // Storms.pas:497
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.DeathStormText);

}
