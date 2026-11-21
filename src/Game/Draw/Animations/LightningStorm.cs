#nullable disable warnings
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
public class LightningStorm(CustomNode? owner) : Storm(owner)
{
    public new const int ClassId = 135_739;

    private CustomImageLibrary? _stormImages;

    // Storms.pas:429
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame = FrameCounter % _stormImages.Count;
        if (_stormImages[frame] is not null)
        {
            sbyte hexY = HexY;
            HexGeometry.HexToPixel(HexX, hexY, out int x, out int y);
            LibraryImage? stormImageWidth = _stormImages[frame];
            x = x - (stormImageWidth.Width >>> 1) + 20;
            LibraryImage? stormImageHeight = _stormImages[frame];
            y = y - stormImageHeight.Height + 32 + 40;
            _stormImages[frame].Show(x - pixelXSurface, y - pixelYSurface, lockedSurface);
        }
    }

    // Storms.pas:487
    public override StormDamageInfo GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, storm: StormType.Lightning, X, Y, L, innerRadius: 0, outerRadius: 1, out info);
        return info;
    }

    // Storms.pas:478
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(2) - 1; i <= last; i++)
        {
            _ = StrategicMap.Current.TargetSelection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:443
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        base.InitializeStorm(player, X, Y, L);
        _stormImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "EFFECTS\\LIGHTN.ILB");
        if (Visible)
        {
            _stormImages.LoadDiscardableImageData();
        }

        r.Left = -(_stormImages[0].Width >>> 1);
        r.Right = _stormImages[0].Width >>> 1;
        r.Top = -_stormImages[0].Height + 32;
        r.Bottom = 32;
        SetDimensions(r);
        _ = StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, radius: 2);
        _ = PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:466
    public override bool UpdateStorm()
    {
        NewFrame();
        if (_stormImages.GetSafe(FrameCounter) is not null)
        {
            _ = AudioPlayback.PlaySample("SFX\\ATTACK\\LIGHTNG.WAV");
        }
        if (_stormImages.Count == FrameCounter)
        {
            AbilityRules.TriggerStormDamage(mapLevel: HexLevel, y: HexY, storm: StormType.Lightning, x: HexX, innerRadius: 0, outerRadius: 1, player: StrategicMap.Current.CurrentPlayer);
            return false;
        }
        else
        {
            return true;
        }
    }

    // Storms.pas:459
    public override void FinalizeStorm()
    {
        _stormImages.ReleaseDiscardableImageData();
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: "EFFECTS\\LIGHTN.ILB");
        base.FinalizeStorm();
    }

    // Storms.pas:424
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.LightningStormText);

}
