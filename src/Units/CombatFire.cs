// Translated from CombatFire.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.CombatILTer;
using Aow.Units.CityWall;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatFire
{
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
        // CombatFire.pas:65
        public override void Destroy()
        {
            base.Destroy();
        }

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
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 27;
            return Result;
        }

        // CombatFire.pas:70
        public override void SetVisible(bool Value)
        {
            base.SetVisible(Value);
        }

        // CombatFire.pas:86
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 1811939328;
            return Result;
        }

        // CombatFire.pas:146
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            int Penalty = default;
            if (Info.HN == 0)
            {
                var call__1 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(7);
                if (call__1)
                {
                    Penalty = 0;
                }
                else
                {
                    var call__2 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(71);
                    if (call__2)
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
            var call__3 = Random(10);
            FXShift = call__3 + 11;
            var call__4 = Random(10);
            FYShift = call__4 + 11;
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
            int I = default;
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
                    var call__5 = GetYhx();
                    var call__6 = GetXhx();
                    var call__7 = AoWCombatMap.MapContainer.GetMapLevel(0);
                    var call__8 = call__7.GetField(call__6, call__5);
                    call__8.SendMapFieldMsg(ref Msg);
                    {
                        I = 1;
                        byte end__9 = 6;
                        for (; I <= end__9; I += 1)
                        {
                            var call__10 = GetYhx();
                            var call__11 = GetXhx();
                            CenterHNtoHX(call__11, call__10, I, ref Xhx, ref Yhx);
                            if (Xhx > 0 && Yhx > 0)
                            {
                                CombatFire.CreateFire(true, 50, Xhx, Yhx);
                            }
                        }
                    }

                    this.Free();
                }
            }
        }

        // CombatFire.pas:26


    }

    public partial class TCombatFireResource : TILTerrainMOR
    {
        public TCombatFireResource(TECustomNode owner) : base(owner)
        {}

        // CombatFire.pas:160
        public new const int ClassID = 2229579;

        // CombatFire.pas:166
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229578;
            return Result;
        }

        // CombatFire.pas:163
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Fire";
            return Result;
        }

    }

    public static partial class CombatFire
    {
        // CombatFire.pas:169
        public static void CreateFire(bool Spread, int Chance, int Xhx, int Yhx)
        {
            TCombatFire Fire = default;
            bool CanBurn = default;
            TCombatILTerrain Terrain = default;
            TCityWall Wall = default;
            var call__12 = AoWCombatMap.MapContainer.GetMapLevel(0);
            var call__13 = call__12.GetField(Xhx, Yhx);
            if (call__13.TerrainType != 15)
            {
                var call__14 = AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__15 = call__14.GetField(Xhx, Yhx);
                var call__16 = call__15.FindOwnedChild(2229578);
                if (call__16 == null)
                {
                    var call__17 = AoWHSMap.Random(100);
                    if (call__17 < Chance)
                    {
                        CanBurn = false;
                        var call__18 = AoWCombatMap.MapContainer.GetMapLevel(0);
                        var call__19 = call__18.GetField(Xhx, Yhx);
                        var call__20 = call__19.GetDecorationType();
                        bool bool__21 = call__20 == 9;
                        if (!bool__21)
                        {
                            var call__22 = AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__23 = call__22.GetField(Xhx, Yhx);
                            var call__24 = call__23.GetDecorationType();
                            bool__21 = call__24 == 1;
                        }

                        bool bool__25 = bool__21;
                        if (!bool__25)
                        {
                            var call__26 = AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__27 = call__26.GetField(Xhx, Yhx);
                            var call__28 = call__27.GetDecorationType();
                            bool__25 = call__28 == 2;
                        }

                        bool bool__29 = bool__25;
                        if (!bool__29)
                        {
                            var call__30 = AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__31 = call__30.GetField(Xhx, Yhx);
                            var call__32 = call__31.GetDecorationType();
                            bool__29 = call__32 == 3;
                        }

                        if (bool__29)
                        {
                            CanBurn = true;
                        }
                        else
                        {
                            var call__33 = AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__34 = call__33.GetField(Xhx, Yhx);
                            var call__35 = call__34.FindOwnedChild(2228534);
                            Terrain = (TCombatILTerrain)call__35;
                            if (Terrain != null)
                            {
                                if (Terrain.GetBurnValue() > 0)
                                {
                                    CanBurn = true;
                                }
                            }

                            var call__36 = AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__37 = call__36.GetField(Xhx, Yhx);
                            var call__38 = call__37.FindOwnedChild(2229508);
                            Wall = (TCityWall)call__38;
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
                            var call__39 = AoWCombatMap.PlaceHX(Fire, Xhx, Yhx, 0);
                            if (!call__39)
                            {
                                Fire.Free();
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
}
