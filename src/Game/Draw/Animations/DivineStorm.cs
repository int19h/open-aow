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
    public new const int ClassId = 135740;

    // Storms.pas:618
    public override void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (terrainType != 6 && terrainType != 0)
        {
            terrainType = 1;
        }
    }

    // Storms.pas:624
    public override string StormFileName() => "EFFECTS\\STORMLGT.ILB";

    // Storms.pas:629
    public override StormType StormType() => Magic.Spells.StormType.Holy;

    // Animation.pas:43

    // Storms.pas:613
    public override string Name() => AbilityRules.TranslateResourceString(GameTables.s_divineStormText);

}
