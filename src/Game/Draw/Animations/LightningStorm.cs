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

    public new const int ClassID = 135739;

    public CustomImageLibrary FStormImages;

    // Storms.pas:429
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Frame;
        Frame = FFrameCounter % FStormImages.Count;
        if (FStormImages[Frame] != null)
        {
            var yhx = GetYhx();
            HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
            var stormImageWidth = FStormImages[Frame];
            X = X - (int)(((uint)stormImageWidth.Width) >> 1) + 20;
            var stormImageHeight = FStormImages[Frame];
            Y = Y - stormImageHeight.Height + 32 + 40;
            FStormImages[Frame].Show(X - XhpSurf, Y - YhpSurf, SurfaceDesc);
        }
    }

    // Storms.pas:487
    public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
    {
        AbilityRules.RetrieveStormDamageInfo(Player, StormType.Lightning, X, Y, L, 0, 1, ref Info);
    }

    // Storms.pas:478
    public override void UpdateTargetSelection(TargetSelectionControl Selection)
    {
        base.UpdateTargetSelection(Selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadToHN(2) - 1; I <= last; I++)
        {
            AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
        }
    }

    // Storms.pas:443
    public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(Player, X, Y, L);
        FStormImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
        if (GetVisible())
        {
            FStormImages.LoadDiscardableImageData();
        }

        R.Left = -(int)(((uint)FStormImages[0].Width) >> 1);
        R.Right = (int)(((uint)FStormImages[0].Width) >> 1);
        R.Top = -FStormImages[0].Height + 32;
        R.Bottom = 32;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
        PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:466
    public override bool UpdateStorm()
    {
        bool Result;
        NewFrame();
        if (FStormImages.GetSafe(FFrameCounter) != null)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\LIGHTNG.WAV");
        }

        if (FStormImages.Count == FFrameCounter)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            AbilityRules.TriggerStormDamage(StormType.Lightning, GetXhx(), yhx, lhx, 0, 1, AoWHSMap.Current.FCurrentPlayer);
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Storms.pas:459
    public override void FinalizeStorm()
    {
        FStormImages.ReleaseDiscardableImageData();
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), "EFFECTS\\LIGHTN.ILB");
        base.FinalizeStorm();
    }

    // Animation.pas:43

    // Storms.pas:424
    public override string Name() => AbilityRules.TranslateRStr(GameTables.LightningStormRStr);

}
