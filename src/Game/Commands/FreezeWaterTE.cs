// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class FreezeWaterTE : GlobalTargetSpellTE
{
    public FreezeWaterTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135939;

    // GlobalTargetSpells.pas:679
    public override bool Process(AbstractUnit source, int value)
    {
        TwinkleAnimation Twinkle = default;
        base.Process(source, value);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2);
        if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
        {
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(10).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand), 100, 64, 50, 30);
            AoWHSMap.Current.PlaceHX(Twinkle, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
        }

        // Use the map's seated player.
        if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).X, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, (sbyte)(byte)Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 2))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(10).FCastSounds.Play(0);
        }

        AoWHSMap.Current.ChangeTerrainEx(null, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 1, ChangeTerrain, TerrainChanged);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:641
    public void ChangeTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        FrozenWaterHS Ice;
        if (((AoWMapField)MapF).GetArmyHS() == null)
        {
            if (TerrainType == (int)AoWTerrainType.Water)
            {
                TerrainType = (int)AoWTerrainType.Ice;
            }
            else
            {
                if (TerrainType == (int)AoWTerrainType.UndergroundWater)
                {
                    TerrainType = (int)AoWTerrainType.UndergroundIce;
                }
                else
                {
                    if (Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)TerrainType))
                    {
                        Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
                        if (Ice != null)
                        {
                            Ice.FDaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
                            Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
                        }
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:661
    public void TerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
    {
        FrozenWaterHS Ice;
        if (Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)MapF.TerrainType) && !Set<byte>.WithBounds(0, 255).With((int)AoWTerrainType.Ice).With((int)AoWTerrainType.UndergroundIce).Contains((byte)OriginalTerrainType))
        {
            Ice = (FrozenWaterHS)MapF.FindOwnedHS(typeof(FrozenWaterHS));
            if (Ice == null)
            {
                Ice = new FrozenWaterHS(null);
                AoWHSMap.Current.PlaceHX(Ice, MapF.Xhx, MapF.Yhx, MapF.Lhx);
            }

            Ice.FDaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
            Ice.FCasterPlayer = (sbyte)FEventPlayerIndex;
        }
    }

    // NetworkE.pas:230

}
