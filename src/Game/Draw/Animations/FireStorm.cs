// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animations;

public partial class FireStorm : Storm
{
    public FireStorm(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:517
    public new const int ClassId = 135742;

    // Storms.pas:527
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int frame;
        ImageSequence sequence;
        var hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, ref X, ref Y);
        X = X - pixelXSurface + 8;
        Y = Y - pixelYSurface;
        sequence = AoWHSSet.Current.ImageSequences.ImageSequences[0];
        while (sequence != null)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            if (frame != -1 && sequence.ImageLibrary.ImageExist(frame))
            {
                sequence.ImageLibrary[frame].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    // Storms.pas:603
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType.Fire, X, Y, L, 0, 2, ref info);
    }

    // Storms.pas:594
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; I <= last; I++)
        {
            AoWHSMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:545
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(player, X, Y, L);
        R.Left = -80;
        R.Right = 96;
        R.Top = -80;
        R.Bottom = 96;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.CurrentPlayer, X, Y, L, 2);
        PlaceHex(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:569
    public override bool UpdateStorm()
    {
        bool result = default;
        ImageSequence sequence = default;
        int frame = default;
        NewFrame();
        if (FrameCounter == 20)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            AbilityRules.TriggerStormDamage(StormType.Fire, GetHexX(), hexY, hexLevel, 0, 2, AoWHSMap.Current.CurrentPlayer);
            var hexLevel2 = GetHexLevel();
            var hexY2 = GetHexY();
            var hexX = GetHexX();
            AoWHSMap.Current.ChangeTerrain(this, hexX, hexY2, hexLevel2, 2, ChangeStormTerrain, null);
        }

        sequence = AoWHSSet.Current.ImageSequences.ImageSequences[0];
        frame = -1;
        while (sequence != null && frame == -1)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            sequence = sequence.Next;
        }

        if (frame == 1)
        {
            bool condition = AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer;
            if (!condition)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                condition = AoWHSMap.Current.TerrainVisible(hexX, hexY, hexLevel, 2);
            }

            if (condition)
            {
                var random = Random(2);
                AudioPlayback.PlaySample(AbilityRules.s_explosionSfx[random]);
            }
        }

        if (frame == -1)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Storms.pas:559
    public override void FinalizeStorm()
    {
        base.FinalizeStorm();
    }

    // Storms.pas:564
    public void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (terrainType == 6)
        {
            terrainType = 0;
        }
    }

    // Animation.pas:43

    // Storms.pas:522
    public override string Name() => AbilityRules.TranslateResourceString(GameTables.FireStormText);

}
