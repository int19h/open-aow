// Translated from CombatFire.pas.
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.CityWall;
using Aow.Units.CombatILTer;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.ILTer;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.CombatFire;

public partial class TCombatFire : TSingleHS
{
    public TCombatFire(TECustomNode owner) : base(owner)
    {
    }

    // CombatFire.pas:80
    public new const int ClassID = 2229578;

    public int FXShift;
    public int FYShift;
    public int FPlayer;
    public bool FFirstTurn;

    // CombatFire.pas:60

    // CombatFire.pas:119
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537006081)
        {
            if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                NewTurn();
            }
        }
    }

    // CombatFire.pas:83
    public override byte GetLevel() => 27;

    // CombatFire.pas:70
    public override void SetVisible(bool Value)
    {
        base.SetVisible(Value);
    }

    // CombatFire.pas:86
    public override int GetShowPriority() => 1811939328;

    // CombatFire.pas:146
    public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
    {
        int Penalty;
        if (Info.HN == 0)
        {
            if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(7))
            {
                Penalty = 0;
            }
            else
            {
                if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(71))
                {
                    Penalty = 10;
                }
                else
                {
                    Penalty = 20;
                }
            }

            if (MoveResult.Code > 127 - Penalty)
            {
                MoveResult.Code = 127;
            }
            else
            {
                MoveResult.Code += Penalty;
            }
        }
    }

    // CombatFire.pas:89
    public override void Activate()
    {
        base.Activate();
        FXShift = Random(10) + 11;
        FYShift = Random(10) + 11;
        FPlayer = AoWCombatMap.FCurrentCombatPlayer;
        FFirstTurn = true;
    }

    // CombatFire.pas:55
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // CombatFire.pas:75
    public override void MakeVisible(TVisibilityList VisibilityList)
    {
        base.MakeVisible(VisibilityList);
    }

    // CombatFire.pas:98
    public void NewTurn()
    {
        int Xhx = default;
        int Yhx = default;
        TMapFieldMsg Msg = default;
        if (FFirstTurn)
        {
            FFirstTurn = false;
        }
        else
        {
            if (AoWCombatMap.FCurrentCombatPlayer == FPlayer)
            {
                Msg.MsgID = 200711;
                Msg.Param[0] = this;
                Msg.Result = -1;
                var yhx = GetYhx();
                var xhx = GetXhx();
                AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).SendMapFieldMsg(ref Msg);
                for (int I = 1; I <= 6; I++)
                {
                    var yhx2 = GetYhx();
                    CenterHNtoHX(GetXhx(), yhx2, I, ref Xhx, ref Yhx);
                    if (Xhx > 0 && Yhx > 0)
                    {
                        CombatFireUnit.CreateFire(true, 50, Xhx, Yhx);
                    }
                }

                this.Dispose();
            }
        }
    }

    // CombatFire.pas:26
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        TImageSequence Sequence;
        int ImageIndex;
        int Xhp = default;
        int Yhp = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        Sequence = AoWHSSet.FImageSequences.GetImageSequence(60);
        if (Sequence != null)
        {
            do
            {
                ImageIndex = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (ImageIndex != -1)
                {
                    Sequence.ImageLibrary.Get(ImageIndex).Show(XhpSurf + Sequence.dXshift + FXShift, YhpSurf + Sequence.dYshift + FYShift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
            while (!(Sequence == null));
        }
    }
}

public partial class TCombatFireResource : TILTerrainMOR
{
    public TCombatFireResource(TECustomNode owner) : base(owner)
    { }

    // CombatFire.pas:160
    public new const int ClassID = 2229579;

    // CombatFire.pas:166
    public override int ResourceUserClassID() => 2229578;

    // CombatFire.pas:163
    public override string ResourceEditName() => "Combat Fire";

}

public static partial class CombatFireUnit
{
    // CombatFire.pas:169
    public static void CreateFire(bool Spread, int Chance, int Xhx, int Yhx)
    {
        TCombatFire Fire;
        bool CanBurn;
        TCombatILTerrain Terrain;
        TCityWall Wall;
        if (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).TerrainType != 15)
        {
            if (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2229578) == null)
            {
                if (AoWHSMap.Random(100) < Chance)
                {
                    CanBurn = false;
                    if (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 9 || AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 1 || AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 2 || AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).GetDecorationType() == 3)
                    {
                        CanBurn = true;
                    }
                    else
                    {
                        Terrain = (TCombatILTerrain)AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2228534);
                        if (Terrain != null)
                        {
                            if (Terrain.GetBurnValue() > 0)
                            {
                                CanBurn = true;
                            }
                        }

                        Wall = (TCityWall)AoWCombatMap.MapContainer.GetMapLevel(0).GetField(Xhx, Yhx).FindOwnedChild(2229508);
                        if (Wall != null)
                        {
                            if (Wall.FWallType == 1)
                            {
                                if (Wall.FHitPoints > 0)
                                {
                                    CanBurn = true;
                                }
                            }
                        }
                    }

                    if (CanBurn)
                    {
                        Fire = new TCombatFire(null);
                        if (!AoWCombatMap.PlaceHX(Fire, Xhx, Yhx, 0))
                        {
                            Fire?.Dispose();
                        }
                    }
                }
            }
        }
    }

    // CombatFire.pas:204
    public static void RegisterCombatFire(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TCombatFireResource>(), EClass.Of<TCombatFire>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
