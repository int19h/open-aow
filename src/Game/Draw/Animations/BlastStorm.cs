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
    public BlastStorm(CustomNode owner) : base(owner)
    { }

    private CustomImageLibrary _stormImages;

    // Storms.pas:338
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int x = default;
        int y = default;
        int frame;
        frame = FrameCounter % _stormImages.Count;
        var hexY = GetHexY();
        HexGeometry.HexToPixel(GetHexX(), hexY, out x, out y);
        var stormImageWidth = _stormImages[frame];
        x = x - (int)(((uint)stormImageWidth.Width) >> 1) + 10;
        var stormImageHeight = _stormImages[frame];
        y = y - (int)(((uint)stormImageHeight.Height) >> 1) - 42;
        _stormImages[frame].Show(x - pixelXSurface, y - pixelYSurface, lockedSurface);
    }

    // Storms.pas:358
    public override void GetTargetDamageInfo(sbyte player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo info)
    {
        AbilityRules.RetrieveStormDamageInfo(player, StormType(), X, Y, L, 0, 4, out info);
    }

    // Storms.pas:349
    public override void UpdateTargetSelection(TargetSelectionControl selection)
    {
        base.UpdateTargetSelection(selection);
        selection.HexNumbers.Clear();
        for (int i = 0, last = HexGeometry.RadiusToHexNumber(5) - 1; i <= last; i++)
        {
            selection.HexNumbers.Add(0);
        }
    }

    // Storms.pas:276
    public override void InitializeStorm(sbyte player, sbyte X, sbyte Y, sbyte L)
    {
        Rect r = default;
        base.InitializeStorm(player, X, Y, L);
        var stormFileName = StormFileName();
        _stormImages = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
        if (GetVisible())
        {
            _stormImages.LoadDiscardableImageData();
        }

        r.Left = -(int)(((uint)_stormImages[0].Width) >> 1);
        r.Right = (int)(((uint)_stormImages[0].Width) >> 1);
        r.Top = -(int)(((uint)_stormImages[0].Height) >> 1);
        r.Bottom = (int)(((uint)_stormImages[0].Height) >> 1);
        SetDimensions(r);
        StrategicMap.Current.ValidateTerrainObservation(StrategicMap.Current.CurrentPlayer, X, Y, L, 4);
        if (StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.TerrainVisible(X, Y, L, 4))
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
        }

        PlaceHex(StrategicMap.Current.MapContainer, X, Y, L);
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
            StrategicMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 1, ChangeStormTerrain, StormTerrainChanged);
        }
        else
        {
            if (FrameCounter == 12)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                StrategicMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 2, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FrameCounter == 18)
                {
                    var hexLevel = GetHexLevel();
                    var hexY = GetHexY();
                    var hexX = GetHexX();
                    StrategicMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 3, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FrameCounter == 22)
                    {
                        var hexLevel = GetHexLevel();
                        var hexY = GetHexY();
                        var hexX = GetHexX();
                        StrategicMap.Current.ChangeTerrain(null, hexX, hexY, hexLevel, 4, ChangeStormTerrain, StormTerrainChanged);
                    }
                }
            }
        }

        if (_stormImages.Count == FrameCounter)
        {
            AbilityRules.TriggerStormDamage(mapLevel: GetHexLevel(), y: GetHexY(), x: GetHexX(), storm: StormType(), innerRadius: 0, outerRadius: 4, player: StrategicMap.Current.CurrentPlayer);
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
        _stormImages.ReleaseDiscardableImageData();
        var stormFileName = StormFileName();
        GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
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
