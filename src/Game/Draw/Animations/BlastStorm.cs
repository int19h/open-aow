// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// storm image library
public partial class BlastStorm : Storm
{
    public BlastStorm(ECustomNode owner) : base(owner)
    { }

    public CustomImageLibrary StormImages;

    // Storms.pas:338
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int frame;
        frame = FrameCounter % StormImages.Count;
        var hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, ref X, ref Y);
        var stormImageWidth = StormImages[frame];
        X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 10;
        var stormImageHeight = StormImages[frame];
        Y = Y - (int)(((uint)stormImageHeight.Height) >> 1) - 42;
        StormImages[frame].Show(X - pixelXSurface, Y - pixelYSurface, lockedSurface);
    }

    // Storms.pas:358
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType(), X, Y, L, 0, 4, ref info);
    }

    // Storms.pas:349
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        selection.HexNumbers.Clear();
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(5) - 1; I <= last; I++)
        {
            selection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:276
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(player, X, Y, L);
        var stormFileName = StormFileName();
        StormImages = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
        if (GetVisible())
        {
            StormImages.LoadDiscardableImageData();
        }

        R.Left = -(int)(((uint)StormImages[0].Width) >> 1);
        R.Right = (int)(((uint)StormImages[0].Width) >> 1);
        R.Top = -(int)(((uint)StormImages[0].Height) >> 1);
        R.Bottom = (int)(((uint)StormImages[0].Height) >> 1);
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.CurrentPlayer, X, Y, L, 4);
        if (AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.TerrainVisible(X, Y, L, 4))
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
        }

        PlaceHex(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:312
    public override bool UpdateStorm()
    {
        bool result = default;
        NewFrame();
        if (FrameCounter == 8)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            AoWHSMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 1, ChangeStormTerrain, StormTerrainChanged);
        }
        else
        {
            if (FrameCounter == 12)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                AoWHSMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 2, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FrameCounter == 18)
                {
                    var hexLevel = GetHexLevel();
                    var hexY = GetHexY();
                    var hexX = GetHexX();
                    AoWHSMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 3, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FrameCounter == 22)
                    {
                        var hexLevel = GetHexLevel();
                        var hexY = GetHexY();
                        var hexX = GetHexX();
                        AoWHSMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 4, ChangeStormTerrain, StormTerrainChanged);
                    }
                }
            }
        }

        if (StormImages.Count == FrameCounter)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            AbilityRules.TriggerStormDamage(StormType(), hexX, hexY, hexLevel, 0, 4, AoWHSMap.Current.CurrentPlayer);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Storms.pas:331
    public override void FinalizeStorm()
    {
        StormImages.ReleaseDiscardableImageData();
        var stormFileName = StormFileName();
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
        base.FinalizeStorm();
    }

    // Storms.pas:304
    public virtual void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
    }

    // Storms.pas:308
    public virtual void StormTerrainChanged(MapField mapField, sbyte originalTerrainType, sbyte originalOverlayType)
    {
    }

    // Storms.pas:294
    public virtual string StormFileName() => "";

    // Storms.pas:299
    public virtual StormType StormType() => Magic.Spells.StormType.None;
}
