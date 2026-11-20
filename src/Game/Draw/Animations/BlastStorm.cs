#nullable disable
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
public class BlastStorm(CustomNode owner) : Storm(owner)
{
    private CustomImageLibrary _stormImages;

    // Storms.pas:338
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame = FrameCounter % _stormImages.Count;
        sbyte hexY = HexY;
        HexGeometry.HexToPixel(HexX, hexY, out int x, out int y);
        LibraryImage stormImageWidth = _stormImages[frame];
        x = x - (stormImageWidth.Width >>> 1) + 10;
        LibraryImage stormImageHeight = _stormImages[frame];
        y = y - (stormImageHeight.Height >>> 1) - 42;
        _stormImages[frame].Show(x - pixelXSurface, y - pixelYSurface, lockedSurface);
    }

    // Storms.pas:358
    public override StormDamageInfo GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, storm: StormType(), X, Y, L, innerRadius: 0, outerRadius: 4, out info);
        return info;
    }

    // Storms.pas:349
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        selection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(5) - 1; i <= last; i++)
        {
            _ = selection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:276
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        base.InitializeStorm(player, X, Y, L);
        string stormFileName = StormFileName();
        _stormImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), fileName: stormFileName);
        if (Visible)
        {
            _stormImages.LoadDiscardableImageData();
        }

        r.Left = -(_stormImages[0].Width >>> 1);
        r.Right = _stormImages[0].Width >>> 1;
        r.Top = -(_stormImages[0].Height >>> 1);
        r.Bottom = _stormImages[0].Height >>> 1;
        SetDimensions(r);
        _ = StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, radius: 4);
        if (StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.TerrainVisible(X, Y, L, radius: 4))
        {
            _ = AudioPlayback.PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
        }

        _ = PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:312
    public override bool UpdateStorm()
    {
        NewFrame();
        if (FrameCounter == 8)
        {
            sbyte hexLevel = HexLevel;
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            _ = StrategicMap.Current.ChangeTerrain(null, hexX, hexY, L: hexLevel, radius: 1, changeTerrainProc: ChangeStormTerrain, terrainChangedProc: StormTerrainChanged);
        }
        else
        {
            if (FrameCounter == 12)
            {
                sbyte hexLevel = HexLevel;
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                _ = StrategicMap.Current.ChangeTerrain(null, hexX, hexY, L: hexLevel, radius: 2, changeTerrainProc: ChangeStormTerrain, terrainChangedProc: StormTerrainChanged);
            }
            else
            {
                if (FrameCounter == 18)
                {
                    sbyte hexLevel = HexLevel;
                    sbyte hexY = HexY;
                    sbyte hexX = HexX;
                    _ = StrategicMap.Current.ChangeTerrain(null, hexX, hexY, L: hexLevel, radius: 3, changeTerrainProc: ChangeStormTerrain, terrainChangedProc: StormTerrainChanged);
                }
                else
                {
                    if (FrameCounter == 22)
                    {
                        sbyte hexLevel = HexLevel;
                        sbyte hexY = HexY;
                        sbyte hexX = HexX;
                        _ = StrategicMap.Current.ChangeTerrain(null, hexX, hexY, L: hexLevel, radius: 4, changeTerrainProc: ChangeStormTerrain, terrainChangedProc: StormTerrainChanged);
                    }
                }
            }
        }
        if (_stormImages.Count == FrameCounter)
        {
            AbilityRules.TriggerStormDamage(mapLevel: HexLevel, y: HexY, x: HexX, storm: StormType(), innerRadius: 0, outerRadius: 4, player: StrategicMap.Current.CurrentPlayer);
            return false;
        }
        else
        {
            return true;
        }
    }

    // Storms.pas:331
    public override void FinalizeStorm()
    {
        _stormImages.ReleaseDiscardableImageData();
        string stormFileName = StormFileName();
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), fileName: stormFileName);
        base.FinalizeStorm();
    }

    // Storms.pas:304
    public virtual void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
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
