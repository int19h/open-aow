// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// FUnit. FUnitGFX. FFrame. FOffsetX. FOffsetY. FSequenceIndex. FSequenceFrame
public partial class CCorpseHS : SingleHS
{
    public CCorpseHS(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2228488;

    public TacticalCombatUnit FUnit;
    public UnitGFXResource FUnitGFX;
    public int FFrame;
    public int FOffsetX;
    public int FOffsetY;
    public int FSequenceIndex;
    public int FSequenceFrame;

    // AoWTC.pas:4007
    public override byte GetLevel() => 26;

    // AoWTC.pas:4012
    public override int GetShowPriority() => 1744830464 + (FOffsetY << 13) + FOffsetX;

    // AoWTC.pas:3985
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Frame;
        int X = default;
        int Y = default;
        ImageSequence Sequence;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        XhpSurf = X - XhpSurf - 15;
        YhpSurf = Y - YhpSurf - 25;
        if (FSequenceIndex > -1)
        {
            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FSequenceIndex];
            while (Sequence != null)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FSequenceFrame);
                if (Frame != -1)
                {
                    Sequence.ImageLibrary[Frame].Show(XhpSurf + FOffsetX - 5, YhpSurf + FOffsetY + 10, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        Sequence = FUnitGFX.FSequences.ImageSequences[30];
        while (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrame);
            if (Frame != -1)
            {
                Sequence.ImageLibrary[Frame].Show(XhpSurf + FOffsetX, YhpSurf + FOffsetY, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // AoWTC.pas:4025
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        CombatAction AnimateAction;
        CombatAction RaiseAction;
        int N;
        int X = default;
        int Y = default;
        int DX;
        int DY;
        TfxHS Effect;
        float Z = default;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        TacticalCombatUnitHS UnitHS;
        base.MapFieldMsgProc(ref Msg);
        if (FUnit != null)
        {
            if (Msg.MsgID == 200712 && Msg.HN == 0)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                {
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    AoWCombatMap.Current.MakeVisible(xhx2, yhx2, 0, 5);
                    var yhx3 = GetYhx();
                    HexGeometry.HXtoHP(GetXhx(), yhx3, ref X, ref Y);
                    for (int Ring = 1; Ring <= 3; Ring++)
                    {
                        N = Random(5);
                        DX = Random(20) + Ring * 30;
                        DY = Random(20) + Ring * 30;
                        for (int I = 0; I <= 17; I++)
                        {
                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                            Effect.FAbilityIndex = 52;
                            Effect.FSpellIndex = Msg.Param[1].Int;
                            DX = -DX;
                            switch (I)
                            {
                                case 1:
                                case 3:
                                case 5:
                                case 7:
                                case 9:
                                case 11:
                                case 13:
                                case 15:
                                case 17:
                                    DY = -DY;
                                    break;
                            }

                            var pi = Math.PI;
                            var sin = Math.Sin((double)((double)(I * 5 + N) * pi) / 180);
                            var mathPI = Math.PI;
                            EffectPaths.MakeLinePath(Round((double)Math.Cos((double)((double)(I * 5 + N) * mathPI) / 180) * DX) + (X + 16), Round((double)sin * DY) / 2 + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                        }
                    }

                    return;
                }
            }

            if (Msg.MsgID == 200713 && Msg.HN == 0)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                {
                    var yhx2 = GetYhx();
                    HexGeometry.HXtoHP(GetXhx(), yhx2, ref X, ref Y);
                    N = Random(6) + 25;
                    for (int I = 1; I <= N - 1; I++)
                    {
                        Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                        Effect.FAbilityIndex = 52;
                        Effect.FSpellIndex = Msg.Param[1].Int;
                        // { Original uses this smaller value for horizontal speed. }
                        Speed = (float)((double)(Random(10) + 10) * 0.05);
                        Gravity = (float)0.5;
                        VZ = (float)((double)(Random(50) + 50) * 0.05);
                        Z = 0;
                        EffectPaths.MakeArcPath(X + 16, Y + 16, I * 360 / N, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                    }

                    switch (Msg.Param[1].Int)
                    {
                        case 130:
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            AnimateAction = AoWHSSet.Current.FSpellControl.GetSpell(130).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(AnimateAction);
                            AnimateAction.Release();
                            break;
                        case 129:
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            RaiseAction = AoWHSSet.Current.FSpellControl.GetSpell(129).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(RaiseAction);
                            RaiseAction.Release();
                            break;
                    }

                    if (FUnit.GetEnabled())
                    {
                        UnitHS = new TacticalCombatUnitHS(null);
                        UnitHS.Initialize(FUnit);
                        UnitHS.SetDirection((byte)(Random(6) + 1));
                        var yhx3 = GetYhx();
                        var xhx2 = GetXhx();
                        AoWCombatMap.Current.PlaceHX(UnitHS, xhx2, yhx3, 0);
                    }

                    this.Dispose();
                }
            }
        }
    }

    // AoWTC.pas:4017
    public override void Activate()
    {
        base.Activate();
        if (FSequenceIndex > -1)
        {
            AoWCombatMap.Current.NewFrameEvents += NewFrame;
        }

        FUnitGFX.AddRef();
        FSequenceFrame = -1;
    }

    // AoWTC.pas:3966
    public override void Deactivate()
    {
        AoWCombatMap.Current.NewFrameEvents -= NewFrame;
        FUnitGFX.Release();
        base.Deactivate();
    }

    // AoWTC.pas:3978
    public void NewFrame(object Sender)
    {
        ImageSequence Sequence;
        Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FSequenceIndex];
        var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
        if (FSequenceFrame + 1 < totalSequenceFrames)
        {
            FSequenceFrame += 1;
        }
    }

    // Engine.pas:524

}
