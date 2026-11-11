// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animations;

public partial class FireStorm : Storm
{
    public FireStorm(ECustomNode owner) : base(owner)
    { }

    // Storms.pas:517
    public new const int ClassID = 135742;

    // Storms.pas:527
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Frame;
        ImageSequence Sequence;
        var yhx = GetYhx();
        HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
        X = X - XhpSurf + 8;
        Y = Y - YhpSurf;
        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[0];
        while (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
            if (Frame != -1 && Sequence.ImageLibrary.ImageExist(Frame))
            {
                Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // Storms.pas:603
    public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
    {
        AbilityRules.RetrieveStormDamageInfo(Player, StormType.Fire, X, Y, L, 0, 2, ref Info);
    }

    // Storms.pas:594
    public override void UpdateTargetSelection(TargetSelectionControl Selection)
    {
        base.UpdateTargetSelection(Selection);
        // { Original adds to the map selection, without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadToHN(3) - 1; I <= last; I++)
        {
            AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
        }
    }

    // Storms.pas:545
    public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        base.InitializeStorm(Player, X, Y, L);
        R.Left = -80;
        R.Right = 96;
        R.Top = -80;
        R.Bottom = 96;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
        PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:569
    public override bool UpdateStorm()
    {
        bool Result = default;
        ImageSequence Sequence = default;
        int Frame = default;
        NewFrame();
        if (FFrameCounter == 20)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            AbilityRules.TriggerStormDamage(StormType.Fire, GetXhx(), yhx, lhx, 0, 2, AoWHSMap.Current.FCurrentPlayer);
            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx = GetXhx();
            AoWHSMap.Current.ChangeTerrainEx(this, xhx, yhx2, lhx2, 2, ChangeStormTerrain, null);
        }

        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[0];
        Frame = -1;
        while (Sequence != null && Frame == -1)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
            Sequence = Sequence.Next;
        }

        if (Frame == 1)
        {
            bool condition = AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = AoWHSMap.Current.TerrainVisible(xhx, yhx, lhx, 2);
            }

            if (condition)
            {
                var random = Random(2);
                AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[random]);
            }
        }

        if (Frame == -1)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Storms.pas:559
    public override void FinalizeStorm()
    {
        base.FinalizeStorm();
    }

    // Storms.pas:564
    public void ChangeStormTerrain(MapField MapF, ref sbyte TerrainType, ref sbyte OverlayType)
    {
        if (TerrainType == 6)
        {
            TerrainType = 0;
        }
    }

    // Animation.pas:43

    // Storms.pas:522
    public override string Name() => AbilityRules.TranslateRStr(GameTables.FireStormRStr);

}
