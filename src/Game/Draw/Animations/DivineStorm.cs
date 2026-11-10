// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Draw.Animations;

public partial class DivineStorm : BlastStorm
{
    public DivineStorm(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:608
    public new const int ClassID = 135740;

    // Storms.pas:618
    public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (TerrainType != 6 && TerrainType != 0)
        {
            TerrainType = 1;
        }
    }

    // Storms.pas:624
    public override string StormFileName() => "EFFECTS\\STORMLGT.ILB";

    // Storms.pas:629
    public override StormType StormType() => Magic.Spells.StormType.Holy;

    // Animation.pas:43

    // Storms.pas:613
    public override string Name() => AbilityRules.TranslateRStr(GameTables.DivineStormRStr);

}
