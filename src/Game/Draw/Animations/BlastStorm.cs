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

    public CustomImageLibrary FStormImages;

    // Storms.pas:338
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Frame;
        Frame = FFrameCounter % FStormImages.Count;
        var yhx = GetYhx();
        HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
        var stormImageWidth = FStormImages[Frame];
        X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 10;
        var stormImageHeight = FStormImages[Frame];
        Y = Y - (int)(((uint)stormImageHeight.Height) >> 1) - 42;
        FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
    }

    // Storms.pas:358
    public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
    {
        AbilityRules.RetrieveStormDamageInfo(Player, StormType(), X, Y, L, 0, 4, ref Info);
    }

    // Storms.pas:349
    public override void UpdateTargetSelection(TargetSelectionControl Selection)
    {
        base.UpdateTargetSelection(Selection);
        Selection.FHNs.Clear();
        for (int I = 0, last = HexGeometry.RadToHN(5) - 1; I <= last; I++)
        {
            Selection.FHNs.Add(0);
        }
    }

    // Storms.pas:276
    public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(Player, X, Y, L);
        var stormFileName = StormFileName();
        FStormImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
        if (GetVisible())
        {
            FStormImages.LoadDiscardableImageData();
        }

        R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
        R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
        R.Top = -(int)(((uint)FStormImages[0].Height) >> 1);
        R.Bottom = (int)(((uint)FStormImages[0].Height) >> 1);
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 4);
        if (AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.TerrainVisible(X, Y, L, 4))
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\STORMBIG.WAV");
        }

        PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:312
    public override bool UpdateStorm()
    {
        bool Result = default;
        NewFrame();
        if (FFrameCounter == 8)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 1, ChangeStormTerrain, StormTerrainChanged);
        }
        else
        {
            if (FFrameCounter == 12)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 2, ChangeStormTerrain, StormTerrainChanged);
            }
            else
            {
                if (FFrameCounter == 18)
                {
                    var lhx = GetLhx();
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 3, ChangeStormTerrain, StormTerrainChanged);
                }
                else
                {
                    if (FFrameCounter == 22)
                    {
                        var lhx = GetLhx();
                        var yhx = GetYhx();
                        var xhx = GetXhx();
                        AoWHSMap.Current.ChangeTerrainEx(null, xhx, yhx, lhx, 4, ChangeStormTerrain, StormTerrainChanged);
                    }
                }
            }
        }

        if (FStormImages.Count == FFrameCounter)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AbilityRules.TriggerStormDamage(StormType(), xhx, yhx, lhx, 0, 4, AoWHSMap.Current.FCurrentPlayer);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Storms.pas:331
    public override void FinalizeStorm()
    {
        FStormImages.ReleaseDiscardableImageData();
        var stormFileName = StormFileName();
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), stormFileName);
        base.FinalizeStorm();
    }

    // Storms.pas:304
    public virtual void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
    }

    // Storms.pas:308
    public virtual void StormTerrainChanged(MapField MapF, sbyte OriginalTerrainType, sbyte OriginalOverlayType)
    {
    }

    // Storms.pas:294
    public virtual string StormFileName() => "";

    // Storms.pas:299
    public virtual StormType StormType() => Magic.Spells.StormType.None;
}
