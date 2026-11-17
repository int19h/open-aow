#nullable disable
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
public partial class PestilenceStorm : Storm
{
    public PestilenceStorm(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135745;

    private PoisonCloud.Resource _cloudResource;

    // Storms.pas:644
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame;
        int blend;
        ImageSequence sequence;
        sbyte hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, out int x, out int y);
        x = x - pixelXSurface + 8;
        y = y - pixelYSurface;
        sequence = _cloudResource.ImageSequenceList.ImageSequences[0];
        while (sequence != null)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            if (frame != -1 && sequence.ImageLibrary.ImageExist(frame))
            {
                blend = sequence.GetTotalSequenceFrames();
                blend = blend != 0 ? FrameCounter * 100 / blend : 100;

                sequence.ImageLibrary[frame].ShowBlended(x + sequence.OffsetX, y + sequence.OffsetY, lockedSurface, GraphicsState.ShadowBlendTable.LookupTables[blend]);
            }

            sequence = sequence.Next;
        }
    }

    // Storms.pas:725
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info) => AbilityRules.RetrieveStormDamageInfo(player, StormType.Pestilence, X, Y, L, 0, 2, out info);

    // Storms.pas:716
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection without clearing its existing hexes. }
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; i <= last; i++)
        {
            StrategicMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:668
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        _cloudResource = GamePack.Current.MapObjectResourceList.FindChild<PoisonCloud.Resource>(135744);
        base.InitializeStorm(player, X, Y, L);
        r.Left = -80;
        r.Right = 96;
        r.Top = -80;
        r.Bottom = 96;
        SetDimensions(r);
        StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, 2);
        if (StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.TerrainVisible(X, Y, L, 2))
        {
            GamePack.Current.SpellControl.GetSpell(9).CastSounds.Play(0);
        }

        PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:690
    public override bool UpdateStorm()
    {
        bool result;
        ImageSequence sequence;
        int frame;
        PoisonCloud cloud;
        NewFrame();
        sequence = _cloudResource.ImageSequenceList.ImageSequences[0];
        frame = -1;
        while (sequence != null && frame == -1)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            sequence = sequence.Next;
        }

        if (frame == -1)
        {
            cloud = _cloudResource.CreateResourceUser<PoisonCloud>();
            cloud.TurnsRemaining = 3;
            cloud.Player = StrategicMap.Current.CurrentPlayer;
            sbyte hexLevel = GetHexLevel();
            sbyte hexY = GetHexY();
            sbyte hexX = GetHexX();
            if (StrategicMap.Current.PlaceHex(cloud, hexX, hexY, hexLevel))
            {
                cloud.TriggerDamage();
            }
            else
            {
                cloud?.Dispose();
            }

            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Storms.pas:685
    public override void FinalizeStorm() => base.FinalizeStorm();

    // Animation.pas:43

    // Storms.pas:639
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.PestilenceStormText);

}
