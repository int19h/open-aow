// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class RejuvenateTokenEvent : GlobalTargetSpellTokenEvent
{
    public RejuvenateTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:1036
    public new const int ClassID = 135952;

    // GlobalTargetSpells.pas:1044
    public override bool Process(Troop source, int value)
    {
        TwinkleAnimation Twinkle = default;
        base.Process(source, value);
        AoWHSMap.Current.ValidateTerrainObservation(source.FPlayer, (sbyte)(byte)FLocation.X, (sbyte)(byte)FLocation.Y, (sbyte)(byte)FLocation.L, 2);
        if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)FLocation.X, (sbyte)(byte)FLocation.Y, (sbyte)(byte)FLocation.L, 2))
        {
            Twinkle = new TwinkleAnimation(null);
            Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(41).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
            AoWHSMap.Current.PlaceHX(Twinkle, FLocation.X, FLocation.Y, FLocation.L);
        }

        // Use the map's seated player.
        if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)FLocation.X, (sbyte)(byte)FLocation.Y, (sbyte)(byte)FLocation.L, 2))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(41).FCastSounds.Play(0);
        }

        AoWHSMap.Current.ChangeTerrainEx(null, FLocation.X, FLocation.Y, FLocation.L, 2, RejuvenateTerrain, null);
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:1039
    public void RejuvenateTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (Set<byte>.WithBounds(0, 255).WithRange((int)World.Map.TerrainType.Desert, (int)World.Map.TerrainType.Wasteland).Contains((byte)TerrainType))
        {
            TerrainType = (int)World.Map.TerrainType.Grass;
        }
    }

    // NetworkE.pas:230

}
