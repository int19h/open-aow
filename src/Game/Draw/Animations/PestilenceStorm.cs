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

    public new const int ClassID = 135745;

    public PoisonCloud.Resource FCloudResource;

    // Storms.pas:644
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Frame;
        int Blend;
        ImageSequence Sequence;
        var yhx = GetYhx();
        HexGeometry.HXtoHP(GetXhx(), yhx, ref X, ref Y);
        X = X - XhpSurf + 8;
        Y = Y - YhpSurf;
        Sequence = FCloudResource.ImageSequenceList.ImageSequences[0];
        while (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
            if (Frame != -1 && Sequence.ImageLibrary.ImageExist(Frame))
            {
                Blend = Sequence.GetTotalSequenceFrames();
                if (Blend != 0)
                {
                    Blend = FFrameCounter * 100 / Blend;
                }
                else
                {
                    Blend = 100;
                }

                Sequence.ImageLibrary[Frame].ShowBlended(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc, GfxState.ShadowBlendTable.LookupTables[Blend]);
            }

            Sequence = Sequence.Next;
        }
    }

    // Storms.pas:725
    public override void GetTargetDamageInfo(sbyte Player, sbyte X, sbyte Y, sbyte L, ref StormDamageInfo Info)
    {
        AbilityRules.RetrieveStormDamageInfo(Player, StormType.Pestilence, X, Y, L, 0, 2, ref Info);
    }

    // Storms.pas:716
    public override void UpdateTargetSelection(TargetSelectionControl Selection)
    {
        base.UpdateTargetSelection(Selection);
        // { Original adds to the map selection without clearing its existing hexes. }
        for (int I = 0, last = HexGeometry.RadToHN(3) - 1; I <= last; I++)
        {
            AoWHSMap.Current.FTargetSelection.FHNs.Add(0);
        }
    }

    // Storms.pas:668
    public override void InitializeStorm(sbyte Player, sbyte X, sbyte Y, sbyte L)
    {
        Rect R = default;
        FCloudResource = (PoisonCloud.Resource)AoWHSSet.Current.MapObjectResourceList.FindChild(135744);
        base.InitializeStorm(Player, X, Y, L);
        R.Left = -80;
        R.Right = 96;
        R.Top = -80;
        R.Bottom = 96;
        SetDimensions(R);
        AoWHSMap.Current.ValidateTerrainObservation(AoWHSMap.Current.FCurrentPlayer, X, Y, L, 2);
        if (AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.TerrainVisible(X, Y, L, 2))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(9).FCastSounds.Play(0);
        }

        PlaceHX(AoWHSMap.Current.MapContainer, X, Y, L);
    }

    // Storms.pas:690
    public override bool UpdateStorm()
    {
        bool Result;
        ImageSequence Sequence;
        int Frame;
        PoisonCloud Cloud;
        NewFrame();
        Sequence = FCloudResource.ImageSequenceList.ImageSequences[0];
        Frame = -1;
        while (Sequence != null && Frame == -1)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrameCounter);
            Sequence = Sequence.Next;
        }

        if (Frame == -1)
        {
            Cloud = (PoisonCloud)FCloudResource.CreateResourceUser();
            Cloud.FTurnsRemaining = 3;
            Cloud.FPlayer = AoWHSMap.Current.FCurrentPlayer;
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            if (AoWHSMap.Current.PlaceHX(Cloud, xhx, yhx, lhx))
            {
                Cloud.TriggerDamage();
            }
            else
            {
                Cloud?.Dispose();
            }

            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Storms.pas:685
    public override void FinalizeStorm()
    {
        base.FinalizeStorm();
    }

    // Animation.pas:43

    // Storms.pas:639
    public override string Name() => AbilityRules.TranslateRStr(GameTables.PestilenceStormRStr);

}
