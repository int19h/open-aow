// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FMiscImages. FTargetImages. FFrame. FFrameCount. FSelection. FRange
public partial class RangedPathHS : FixedVisibleHS
{
    public RangedPathHS(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:4846
    public new const int ClassID = 2228485;

    public CustomImageLibrary FMiscImages;
    public CustomImageLibrary FTargetImages;
    public ushort FFrame;
    public ushort FFrameCount;
    public CombatUnitSelectionControl FSelection;
    public ushort FRange;

    // AoWTC.pas:4849
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:4859
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Count;
        int First;
        int Last;
        int NX = default;
        int NY = default;
        int PX = default;
        int PY = default;
        TfxType EffectType;
        if (FSelection != null)
        {
            if (FRange < 100)
            {
                Count = 6;
                First = 1;
                for (int I = 0, last = FRange - 2; I <= last; I++)
                {
                    First += Count;
                    Count += 6;
                }

                Last = First + Count;
                for (int I = First; I <= Last - 1; I++)
                {
                    var yhx = FSelection.GetTCUnitHS().GetYhx();
                    HexGeometry.CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx, I, ref NX, ref NY);
                    HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                    FTargetImages[2].Show(PX - XhpSurf - 8, PY - YhpSurf + 2, SurfaceDesc);
                }
            }

            if (FSelection.FRangedPath.Count > 0)
            {
                FFrameCount = (ushort)(FFrameCount + 1);
                if (FFrameCount >= 5)
                {
                    FFrameCount = 0;
                    FFrame = (ushort)(FFrame + 1);
                }

                if (FFrame > 1)
                {
                    FFrame = 0;
                }

                if (FSelection.GetTCUnitHS().FSelectedAbility == 52)
                {
                    var tcUnitHS = FSelection.GetTCUnitHS();
                    EffectType = TacticalTables.SpellTypes[tcUnitHS.FSelectedSpell];
                }
                else
                {
                    var tcUnitHS = FSelection.GetTCUnitHS();
                    EffectType = TacticalTables.AbilTypes[tcUnitHS.FSelectedAbility];
                }

                switch (EffectType)
                {
                    case TfxType.Breath:
                        var count = FSelection.FRangedPath.Count;
                        var rangedPath = FSelection.FRangedPath[count - 1];
                        var fRangedPathCount = FSelection.FRangedPath.Count;
                        HexGeometry.HPtoHX(FSelection.FRangedPath[fRangedPathCount - 1].X, rangedPath.Y, ref X, ref Y);
                        {
                            for (int I = 0; I <= 11; I++)
                            {
                                var yhx = FSelection.GetTCUnitHS().GetYhx();
                                var dHXtoHN2 = HexGeometry.dHXtoHN(FSelection.GetTCUnitHS().GetXhx(), yhx, X, Y);
                                var yhx2 = FSelection.GetTCUnitHS().GetYhx();
                                HexGeometry.CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx2, TacticalTables.BreathHN[dHXtoHN2 * 12 + I - 12], ref NX, ref NY);
                                HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                                FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                            }
                        }

                        break;
                    default:
                        switch (FSelection.GetTCUnitHS().FSelectedAbility)
                        {
                            case 52:
                                switch (FSelection.GetTCUnitHS().FSelectedSpell)
                                {
                                    case 104:
                                    case 106:
                                    case 116:
                                    case 117:
                                    case 120:
                                    case 129:
                                    case 130:
                                        {
                                            for (int I = 0; I <= 18; I++)
                                            {
                                                var count2 = FSelection.FRangedPath.Count;
                                                var rangedPathY = FSelection.FRangedPath[count2 - 1];
                                                var count3 = FSelection.FRangedPath.Count;
                                                HexGeometry.HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                                                FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                        break;
                                    case 124:
                                    case 128:
                                        {
                                            for (int I = 0; I <= 6; I++)
                                            {
                                                var count2 = FSelection.FRangedPath.Count;
                                                var rangedPathY = FSelection.FRangedPath[count2 - 1];
                                                var count3 = FSelection.FRangedPath.Count;
                                                HexGeometry.HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                                                FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                        break;
                                    default:
                                        var count4 = FSelection.FRangedPath.Count;
                                        var rangedPath2 = FSelection.FRangedPath[count4 - 1];
                                        var count5 = FSelection.FRangedPath.Count;
                                        var rangedPathX = FSelection.FRangedPath[count5 - 1];
                                        FMiscImages[FFrame + 3].ShowClipped(rangedPathX.X - 21 - XhpSurf, rangedPath2.Y - 20 - YhpSurf, SurfaceDesc);
                                        break;
                                }

                                break;
                            default:
                                var count6 = FSelection.FRangedPath.Count;
                                var rangedPath3 = FSelection.FRangedPath[count6 - 1];
                                var count7 = FSelection.FRangedPath.Count;
                                var rangedPath4 = FSelection.FRangedPath[count7 - 1];
                                FMiscImages[FFrame + 3].ShowClipped(rangedPath4.X - 21 - XhpSurf, rangedPath3.Y - 20 - YhpSurf, SurfaceDesc);
                                break;
                        }

                        break;
                }

                for (int I = 0, last = FSelection.FRangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = FSelection.FRangedPath[I];
                    var rangedPathX = FSelection.FRangedPath[I];
                    FMiscImages[2].Show(rangedPathX.X - XhpSurf, rangedPathY.Y - YhpSurf, SurfaceDesc);
                }

                for (int I = 0, last = FSelection.FRangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = FSelection.FRangedPath[I];
                    var rangedPathZ = FSelection.FRangedPath[I];
                    var rangedPathX = FSelection.FRangedPath[I];
                    FMiscImages[0].Show(rangedPathX.X - XhpSurf, rangedPathY.Y - rangedPathZ.Z - 8 - YhpSurf, SurfaceDesc);
                }

                for (int I = 0, last = FSelection.FObstacles.Count - 1; I <= last; I++)
                {
                    var obstacle = FSelection.FObstacles[I];
                    var rangedPath = FSelection.FRangedPath[((ROb)obstacle).PathIndex];
                    var obstacle2 = FSelection.FObstacles[I];
                    HexGeometry.HPtoHX(FSelection.FRangedPath[((ROb)obstacle2).PathIndex].X, rangedPath.Y, ref NX, ref NY);
                    HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                    FMiscImages[1].Show(PX + 5 - XhpSurf, PY + 5 - YhpSurf, SurfaceDesc);
                    GameTables.AoWEngine.FCombatFont.Color = 0;
                    var obstacle3 = FSelection.FObstacles[I];
                    GameTables.AoWEngine.FCombatFont.SetTextCentered(PX + 16 - XhpSurf, PY + 12 - YhpSurf, IntToStr(((ROb)obstacle3).Chance), SurfaceDesc);
                }
            }
        }
    }

    // AoWTC.pas:4852
    public override void Activate()
    {
        base.Activate();
        FMiscImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        FTargetImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
    }

    // AoWTC.pas:4839
    public override void Deactivate()
    {
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
        AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
        base.Deactivate();
    }

    // Engine.pas:524

}
