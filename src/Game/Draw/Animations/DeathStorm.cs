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
    public new const int ClassID = 135741;

    // Storms.pas:502
    public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (TerrainType != 0 && TerrainType != 6)
        {
            TerrainType = 5;
        }
    }

    // Storms.pas:507
    public override string StormFileName() => "EFFECTS\\STORMD.ILB";

    // Storms.pas:512
    public override StormType StormType() => Magic.Spells.StormType.Death;

    // Animation.pas:43

    // Storms.pas:497
    public override string Name() => AbilityRules.TranslateRStr(GameTables.DeathStormRStr);

}
