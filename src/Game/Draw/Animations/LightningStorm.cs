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
public partial class LightningStorm : Storm
{
    public LightningStorm(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135739;

    private CustomImageLibrary _stormImages;

    // Storms.pas:429
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        int frame;
        frame = FrameCounter % _stormImages.Count;
        if (_stormImages[frame] != null)
        {
            var hexY = GetHexY();
            HexGeometry.HexToPixel(GetHexX(), hexY, ref X, ref Y);
            var stormImageWidth = _stormImages[frame];
            X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 20;
            var stormImageHeight = _stormImages[frame];
            Y = Y - stormImageHeight.Height + 32 + 40;
            _stormImages[frame].Show(X - pixelXSurface, Y - pixelYSurface, lockedSurface);
        }
    }

    // Storms.pas:487
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType.Lightning, X, Y, L, 0, 1, ref info);
    }

    // Storms.pas:478
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; I <= last; I++)
        {
            AoWHSMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:443
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(player, X, Y, L);
        _stormImages = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
        if (GetVisible())
        {
            _stormImages.LoadDiscardableImageData();
        }

        R.Left = -(int)(((uint)_stormImages[0].Width) >> 1);
        R.Right = (int)(((uint)_stormImages[0].Width) >> 1);
        R.Top = -_stormImages[0].Height + 32;
        R.Bottom = 32;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.CurrentPlayer, X, Y, L, 2);
        PlaceHex(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:466
    public override bool UpdateStorm()
    {
        bool result;
        NewFrame();
        if (_stormImages.GetSafe(FrameCounter) != null)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\LIGHTNG.WAV");
        }

        if (_stormImages.Count == FrameCounter)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            AbilityRules.TriggerStormDamage(StormType.Lightning, GetHexX(), hexY, hexLevel, 0, 1, AoWHSMap.Current.CurrentPlayer);
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Storms.pas:459
    public override void FinalizeStorm()
    {
        _stormImages.ReleaseDiscardableImageData();
        AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
        base.FinalizeStorm();
    }

    // Animation.pas:43

    // Storms.pas:424
    public override string Name() => AbilityRules.TranslateResourceString(GameTables.LightningStormText);

}
