// Translated from CombatFire.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

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
            if (!Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
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
        FPlayer = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer;
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
            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer == FPlayer)
            {
                Msg.MsgID = 200711;
                Msg.Param[0] = this;
                Msg.Result = -1;
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).SendMapFieldMsg(ref Msg);
                for (int I = 1; I <= 6; I++)
                {
                    var yhx2 = GetYhx();
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(GetXhx(), yhx2, I, ref Xhx, ref Yhx);
                    if (Xhx > 0 && Yhx > 0)
                    {
                        Aow.Game.Draw.EffectPaths.CreateFire(true, 50, Xhx, Yhx);
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
        Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        Sequence = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FImageSequences.GetImageSequence(60);
        if (Sequence != null)
        {
            do
            {
                ImageIndex = Sequence.GetFrameIndex(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
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
