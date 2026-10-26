// Generated from CombatFire.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.CombatILTer;
using static Aow.Units.CombatILTer.CombatILTer;
using Aow.Units.CityWall;
using static Aow.Units.CityWall.CityWall;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatFire
{
    // { TCombatFire (instance size 28, VMT 00406B24) }
    public partial class TCombatFire : Aow.Units.HSEngine.TSingleHS
    {
        public int FXShift;
        // { $0C }
        public int FYShift;
        // { $10 }
        public int FPlayer;
        // { $14 }
        public bool FFirstTurn;
        // { $18 }
        public Aow.Units.FixedArray_byte_3 F_CombatFire_19;
        // CombatFire.pas:65
        public override void Destroy()
        {
            base.Destroy();
        }

        // CombatFire.pas:60
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // CombatFire.pas:119
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537006081)
            {
                if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
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

        // CombatFire.pas:126
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.GetImageSequence(60);
            Sequence = call__1;
            if (Sequence != null)
            {
                do
                {
                    var call__2 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                    ImageIndex = call__2;
                    if (ImageIndex != -1)
                    {
                        var call__3 = Sequence.ImageLibrary.Get(ImageIndex);
                        call__3.Show((XhpSurf + Sequence.dXshift) + FXShift, (YhpSurf + Sequence.dYshift) + FYShift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }

        // CombatFire.pas:146
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            int Penalty = default;
            if (Info.HN == 0)
            {
                var call__4 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(7);
                if (call__4)
                {
                    Penalty = 0;
                }
                else
                {
                    var call__5 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(71);
                    if (call__5)
                    {
                        Penalty = 10;
                    }
                    else
                    {
                        Penalty = 20;
                    }
                }

                if (MoveResult.Code > (127 - Penalty))
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
            var call__6 = Aow.Rtl.Rtl.Random(10);
            FXShift = call__6 + 11;
            var call__7 = Aow.Rtl.Rtl.Random(10);
            FYShift = call__7 + 11;
            FPlayer = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer;
            FFirstTurn = true;
        }

        // CombatFire.pas:55
        public override void Deactivate()
        {
            base.Deactivate();
        }

        // CombatFire.pas:75
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            base.MakeVisible(VisibilityList);
        }

        // CombatFire.pas:98
        public void NewTurn()
        {
            int I = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            if (FFirstTurn)
            {
                FFirstTurn = false;
            }
            else
            {
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer == FPlayer)
                {
                    Msg.MsgID = 200711;
                    Msg.Param[0] = this;
                    Msg.Result = -1;
                    var call__8 = GetYhx();
                    var call__9 = GetXhx();
                    var call__10 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                    var call__11 = call__10.GetField(call__9, call__8);
                    call__11.SendMapFieldMsg(ref Msg);
                    {
                        I = 1;
                        byte end__12 = 6;
                        for (; I <= end__12; I += 1)
                        {
                            var call__13 = GetYhx();
                            var call__14 = GetXhx();
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__14, call__13, I, ref Xhx, ref Yhx);
                            if ((Xhx > 0) && (Yhx > 0))
                            {
                                Aow.Units.CombatFire.CombatFire.CreateFire(true, 50, Xhx, Yhx);
                            }
                        }
                    }

                    this.Free();
                }
            }
        }

        // CombatFire.pas:26
        public static new Aow.Units.CombatFire.TCombatFire Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatFire.TCombatFire o = new Aow.Units.CombatFire.TCombatFire();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // CombatFire.pas:80
            public override int ClassID()
            {
                int Result = default;
                Result = 2229578;
                return Result;
            }

            // CombatFire.pas:26
            public new Aow.Units.CombatFire.TCombatFire Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatFire.TCombatFire o = (Aow.Units.CombatFire.TCombatFire)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatFire.TCombatFire => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCombatFireResource (instance size 68, VMT 00406CA8) }
    public partial class TCombatFireResource : Aow.Units.ILTer.TILTerrainMOR
    {
        // CombatFire.pas:166
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229578;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // CombatFire.pas:160
            public override int ClassID()
            {
                int Result = default;
                Result = 2229579;
                return Result;
            }

            // CombatFire.pas:163
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat Fire";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatFire.TCombatFireResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.CombatFire.TCombatFireResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatFire.TCombatFireResource o = (Aow.Units.CombatFire.TCombatFireResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    public static partial class CombatFire
    {
        // CombatFire.pas:169
        public static void CreateFire(bool Spread, int Chance, int Xhx, int Yhx)
        {
            Aow.Units.CombatFire.TCombatFire Fire = default;
            bool CanBurn = default;
            Aow.Units.CombatILTer.TCombatILTerrain Terrain = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            var call__15 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
            var call__16 = call__15.GetField(Xhx, Yhx);
            if (call__16.TerrainType != 15)
            {
                var call__17 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__18 = call__17.GetField(Xhx, Yhx);
                var call__19 = call__18.FindOwnedChild(2229578);
                if (call__19 == null)
                {
                    var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                    if (call__20 < Chance)
                    {
                        CanBurn = false;
                        var call__21 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                        var call__22 = call__21.GetField(Xhx, Yhx);
                        var call__23 = call__22.GetDecorationType();
                        bool bool__24 = (call__23 == 9);
                        if (!bool__24)
                        {
                            var call__25 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__26 = call__25.GetField(Xhx, Yhx);
                            var call__27 = call__26.GetDecorationType();
                            bool__24 = (call__27 == 1);
                        }

                        bool bool__28 = bool__24;
                        if (!bool__28)
                        {
                            var call__29 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__30 = call__29.GetField(Xhx, Yhx);
                            var call__31 = call__30.GetDecorationType();
                            bool__28 = (call__31 == 2);
                        }

                        bool bool__32 = bool__28;
                        if (!bool__32)
                        {
                            var call__33 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__34 = call__33.GetField(Xhx, Yhx);
                            var call__35 = call__34.GetDecorationType();
                            bool__32 = (call__35 == 3);
                        }

                        if (bool__32)
                        {
                            CanBurn = true;
                        }
                        else
                        {
                            var call__36 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__37 = call__36.GetField(Xhx, Yhx);
                            var call__38 = call__37.FindOwnedChild(2228534);
                            Terrain = (Aow.Units.CombatILTer.TCombatILTerrain)call__38;
                            if (Terrain != null)
                            {
                                if (Terrain.GetBurnValue() > 0)
                                {
                                    CanBurn = true;
                                }
                            }

                            var call__39 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
                            var call__40 = call__39.GetField(Xhx, Yhx);
                            var call__41 = call__40.FindOwnedChild(2229508);
                            Wall = (Aow.Units.CityWall.TCityWall)call__41;
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
                            Fire = Aow.Units.CombatFire.TCombatFire.Create(null);
                            var call__42 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Fire, Xhx, Yhx, 0);
                            if (!call__42)
                            {
                                Fire.Free();
                            }
                        }
                    }
                }
            }
        }

        // CombatFire.pas:204
        public static void RegisterCombatFire(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatFire.TCombatFireResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatFire.TCombatFire>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
