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
    public FireStorm(CustomNode owner) : base(owner)
    { }

    // Storms.pas:517
    public new const int ClassId = 135742;

    // Storms.pas:527
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int x = default;
        int y = default;
        int frame;
        ImageSequence sequence;
        var hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, out x, out y);
        x = x - pixelXSurface + 8;
        y = y - pixelYSurface;
        sequence = GamePack.Current.ImageSequences.ImageSequences[0];
        while (sequence != null)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            if (frame != -1 && sequence.ImageLibrary.ImageExist(frame))
            {
                sequence.ImageLibrary[frame].Show(x + sequence.OffsetX, y + sequence.OffsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    // Storms.pas:603
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType.Fire, X, Y, L, 0, 2, out info);
    }

    // Storms.pas:594
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; i <= last; i++)
        {
            StrategicMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:545
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        base.InitializeStorm(player, X, Y, L);
        r.Left = -80;
        r.Right = 96;
        r.Top = -80;
        r.Bottom = 96;
        SetDimensions(r);
        StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, 2);
        PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
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
            AbilityRules.TriggerStormDamage(mapLevel: GetHexLevel(), y: GetHexY(), storm: StormType.Fire, x: GetHexX(), innerRadius: 0, outerRadius: 2, player: StrategicMap.Current.CurrentPlayer);
            var hexLevel2 = GetHexLevel();
            var hexY2 = GetHexY();
            var hexX = GetHexX();
            StrategicMap.Current.ChangeTerrain(this, hexX, hexY2, hexLevel2, 2, ChangeStormTerrain, null);
        }

        sequence = GamePack.Current.ImageSequences.ImageSequences[0];
        frame = -1;
        while (sequence != null && frame == -1)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            sequence = sequence.Next;
        }

        if (frame == 1)
        {
            bool condition = StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer;
            if (!condition)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                condition = StrategicMap.Current.TerrainVisible(hexX, hexY, hexLevel, 2);
            }

            if (condition)
            {
                var random = Random(2);
                AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
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
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.FireStormText);

}
