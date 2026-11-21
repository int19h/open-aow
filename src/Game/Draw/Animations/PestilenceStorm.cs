#nullable disable warnings
// Translated from Storms.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// poison cloud resource
public class PestilenceStorm(CustomNode? owner) : Storm(owner)
{
    public new const int ClassId = 135_745;

    private PoisonCloud.Resource _cloudResource;

    // Storms.pas:644
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        sbyte hexY = HexY;
        HexGeometry.HexToPixel(HexX, hexY, out int x, out int y);
        x = x - pixelXSurface + 8;
        y -= pixelYSurface;
        ImageSequence? sequence = _cloudResource.ImageSequenceList.ImageSequences[0];
        while (sequence is not null)
        {
            int frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            if (frame != -1 && sequence.ImageLibrary.ImageExist(frame))
            {
                int blend = sequence.TotalSequenceFrames;
                blend = blend != 0 ? FrameCounter * 100 / blend : 100;

                sequence.ImageLibrary[frame].ShowBlended(x + sequence.OffsetX, y + sequence.OffsetY, lockedSurface, table: GraphicsState.ShadowBlendTable.LookupTables[blend]);
            }

            sequence = sequence.Next;
        }
    }

    // Storms.pas:725
    public override StormDamageInfo GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, storm: StormType.Pestilence, X, Y, L, innerRadius: 0, outerRadius: 2, out info);
        return info;
    }

    // Storms.pas:716
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection without clearing its existing hexes. }
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; i <= last; i++)
        {
            _ = StrategicMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:668
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        _cloudResource = GamePack.Current.MapObjectResourceList.FindChild<PoisonCloud.Resource>(PoisonCloud.Resource.ClassId);
        base.InitializeStorm(player, X, Y, L);
        r.Left = -80;
        r.Right = 96;
        r.Top = -80;
        r.Bottom = 96;
        SetDimensions(r);
        _ = StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, radius: 2);
        if (StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.TerrainVisible(X, Y, L, radius: 2))
        {
            _ = GamePack.Current.SpellControl[9].CastSounds.Play(0);
        }

        _ = PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:690
    public override bool UpdateStorm()
    {
        NewFrame();
        ImageSequence? sequence = _cloudResource.ImageSequenceList.ImageSequences[0];
        int frame = -1;
        while (sequence is not null && frame == -1)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            sequence = sequence.Next;
        }
        if (frame == -1)
        {
            PoisonCloud cloud = _cloudResource.CreateResourceUser<PoisonCloud>();
            cloud.TurnsRemaining = 3;
            cloud.Player = StrategicMap.Current.CurrentPlayer;
            sbyte hexLevel = HexLevel;
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            if (StrategicMap.Current.PlaceHex(cloud, hexX, hexY, L: hexLevel))
            {
                cloud.TriggerDamage();
            }
            else
            {
                cloud?.Dispose();
            }

            return false;
        }
        else
        {
            return true;
        }
    }

    // Storms.pas:685
    public override void FinalizeStorm() => base.FinalizeStorm();

    // Storms.pas:639
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.PestilenceStormText);

}
