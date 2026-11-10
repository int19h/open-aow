// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

public partial class IceStorm : BlastStorm
{
    public IceStorm(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:363
    public new const int ClassID = 135738;

    // Storms.pas:390
    public override void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        FrozenWaterHS Ice;
        // { Original sends terrain 13 to the default branch before the ice refresh test. }
        if (TerrainType == 0)
        {
            TerrainType = 6;
        }
        else
        {
            if (TerrainType != 6)
            {
                TerrainType = 3;
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)TerrainType))
                {
                    Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                    if (Ice != null)
                    {
                        Ice.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Current.Random(11) - 5) / 5);
                        Ice.FCasterPlayer = FPlayer;
                    }
                }
            }
        }
    }

    // Storms.pas:373
    public override void StormTerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
    {
        FrozenWaterHS Ice;
        if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)OriginalTerrainType))
        {
            Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
            if (Ice == null)
            {
                Ice = new FrozenWaterHS(null);
                AoWHSMap.Current.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
            }

            Ice.FDaysRemaining = (sbyte)(3 + (AoWHSMap.Current.Random(11) - 5) / 5);
            Ice.FCasterPlayer = FPlayer;
        }
    }

    // Storms.pas:409
    public override string StormFileName() => "EFFECTS\\STORM1.ILB";

    // Storms.pas:414
    public override StormType StormType() => Magic.Spells.StormType.Ice;

    // Animation.pas:43

    // Storms.pas:368
    public override string Name() => AbilityRules.TranslateRStr(GameTables.IceStormRStr);

}
