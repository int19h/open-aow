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
    public PestilenceStorm(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135745;

    private PoisonCloud.Resource _cloudResource;

    // Storms.pas:644
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int frame;
        int blend;
        ImageSequence sequence;
        var hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, ref X, ref Y);
        X = X - pixelXSurface + 8;
        Y = Y - pixelYSurface;
        sequence = _cloudResource.ImageSequenceList.ImageSequences[0];
        while (sequence != null)
        {
            frame = sequence.GetUnloopedFrameIndex(FrameCounter);
            if (frame != -1 && sequence.ImageLibrary.ImageExist(frame))
            {
                blend = sequence.GetTotalSequenceFrames();
                if (blend != 0)
                {
                    blend = FrameCounter * 100 / blend;
                }
                else
                {
                    blend = 100;
                }

                sequence.ImageLibrary[frame].ShowBlended(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface, GraphicsState.ShadowBlendTable.LookupTables[blend]);
            }

            sequence = sequence.Next;
        }
    }

    // Storms.pas:725
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType.Pestilence, X, Y, L, 0, 2, ref info);
    }

    // Storms.pas:716
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(3) - 1; I <= last; I++)
        {
            AoWHSMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:668
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        _cloudResource = (PoisonCloud.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(135744);
        base.InitializeStorm(player, X, Y, L);
        R.Left = -80;
        R.Right = 96;
        R.Top = -80;
        R.Bottom = 96;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.CurrentPlayer, X, Y, L, 2);
        if (AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.TerrainVisible(X, Y, L, 2))
        {
            AoWHSSet.Current.SpellControl.GetSpell(9).CastSounds.Play(0);
        }

        PlaceHex(AoWHSMap.Current.MapContainer, X, Y, L);
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
            cloud = (PoisonCloud)_cloudResource.CreateResourceUser();
            cloud.TurnsRemaining = 3;
            cloud.Player = AoWHSMap.Current.CurrentPlayer;
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            if (AoWHSMap.Current.PlaceHex(cloud, hexX, hexY, hexLevel))
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
    public override void FinalizeStorm()
    {
        base.FinalizeStorm();
    }

    // Animation.pas:43

    // Storms.pas:639
    public override string Name() => AbilityRules.TranslateResourceString(GameTables.PestilenceStormText);

}
