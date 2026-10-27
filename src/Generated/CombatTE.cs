// Generated from CombatTE.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
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
using Aow.Units.CityWall;
using static Aow.Units.CityWall.CityWall;
using Aow.Units.CombatFire;
using static Aow.Units.CombatFire.CombatFire;
using Aow.Units.CombatSpells;
using static Aow.Units.CombatSpells.CombatSpells;

namespace Aow.Units.CombatTE
{
    // { ot=1 }
    // { TCEndTurnTE (instance size 16, VMT 00407548) }
    public partial class TCEndTurnTE : Aow.Units.NetworkE.TTokenEvent
    {
        // CombatTE.pas:178
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            int TurnIndex = default;
            int I = default;
            bool Found = default;
            base.Execute(TE);
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__1;
            var call__2 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder.IndexOf(unchecked((byte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
            TurnIndex = call__2;
            Found = false;
            while (!Found)
            {
                TurnIndex = TurnIndex + 1;
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count <= TurnIndex)
                {
                    TurnIndex = 0;
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.NewRound();
                }

                {
                    I = 0;
                    int end__3 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__3; I += 1)
                    {
                        var call__4 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__5 = (call__4 != null);
                        if (bool__5)
                        {
                            var call__6 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            bool__5 = (call__6 is Aow.Units.AoWE.TCombatUnit);
                        }

                        bool bool__7 = bool__5;
                        if (bool__7)
                        {
                            var call__8 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__9 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
                            bool__7 = (call__8.FCombatPlayer == call__9);
                        }

                        bool bool__10 = bool__7;
                        if (bool__10)
                        {
                            var call__11 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__12 = call__11.GetEnabled();
                            bool__10 = call__12;
                        }

                        if (bool__10)
                        {
                            Found = true;
                        }
                    }
                }
            }

            var call__13 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewTurn(unchecked((sbyte)call__13));
            Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CombatTE.TCEndTurnTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCEndTurnTE o = new Aow.Units.CombatTE.TCEndTurnTE();
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

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // CombatTE.pas:175
            public override int ClassID()
            {
                int Result = default;
                Result = 2236420;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCEndTurnTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CombatTE.TCEndTurnTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCEndTurnTE o = (Aow.Units.CombatTE.TCEndTurnTE)NewInstance();
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

    // { TCEndCombatTE (instance size 16, VMT 0040760C) }
    public partial class TCEndCombatTE : Aow.Units.NetworkE.TTokenEvent
    {
        // CombatTE.pas:161
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            int I = default;
            base.Execute(TE);
            {
                I = 0;
                int end__14 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                for (; I <= end__14; I += 1)
                {
                    var call__15 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                    bool bool__16 = (call__15 != null);
                    if (bool__16)
                    {
                        var call__17 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool__16 = (call__17 is Aow.Units.AoWE.TCombatUnit);
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        var call__20 = call__19.GetEnabled();
                        bool__18 = call__20;
                    }

                    bool bool__21 = bool__18;
                    if (bool__21)
                    {
                        var call__22 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool__21 = (call__22.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer);
                    }

                    if (bool__21)
                    {
                        var call__23 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        call__23.Surrender();
                    }
                }
            }

            Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CombatTE.TCEndCombatTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCEndCombatTE o = new Aow.Units.CombatTE.TCEndCombatTE();
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

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // CombatTE.pas:158
            public override int ClassID()
            {
                int Result = default;
                Result = 2236421;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCEndCombatTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CombatTE.TCEndCombatTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCEndCombatTE o = (Aow.Units.CombatTE.TCEndCombatTE)NewInstance();
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

    // { TCombatMoveTE (instance size 56, VMT 004076D4) }
    public partial class TCombatMoveTE : Aow.Units.AoWE.TMoveTE
    {
        public Aow.Units.AoWTC.TTacticalCombatUnit FUnit;
        // { $1C }
        public int FDelay;
        // { $20 }
        public Aow.Rtl.TList FObjects;
        // { $24 }
        public Aow.Units.AoWTC.TTacticalCombatUnit FPreviousUnit;
        // { $28 }
        public Aow.Units.AoWTC.TTacticalCombatUnit FFreeStrikeUnit;
        // { $2C }
        public sbyte FNeighbour;
        // { $30 }
        public Aow.Units.CombatTE.TTokenStep FStep;
        // { $31 }
        public Aow.Units.FixedArray_byte_2 F_CombatMoveTE_32;
        public Aow.Units.AoWE.TStrikeCA FStrike;
        // CombatTE.pas:214
        // { $34 }
        public override void Destroy()
        {
            if (FObjects != null)
            {
                FObjects.Free();
            }

            base.Destroy();
        }

        // CombatTE.pas:209
        public override Aow.Units.AoWE.TAoWMapField GetMapF(Aow.Units.HSEngine.TXYL Location)
        {
            Aow.Units.AoWE.TAoWMapField Result = default;
            var call__24 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(Location.X, Location.Y, 0);
            Result = (Aow.Units.AoWE.TAoWMapField)call__24;
            return Result;
        }

        // CombatTE.pas:234
        public override bool Validate()
        {
            bool Result = default;
            Aow.Units.AoWE.TAoWMapField MapField = default;
            int Index = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            base.Validate();
            Index = FPath.Count - 1;
            var call__25 = FPath[Index];
            var call__26 = GetMapF(call__25);
            MapField = call__26;
            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)MapField.FindOwnedChild(2228484);
            if (HS != null)
            {
                var call__27 = new Aow.Rtl.TList();
                FObjects = call__27;
                FUnit = HS.FUnit;
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CombatTE.pas:299
        public override Aow.Units.AoWE.TMoveStatus DefaultMove()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // CombatTE.pas:220
        public virtual bool Setup(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            int StartIndex = default;
            int I = default;
            FPath.Clear();
            var call__28 = Path.MovePointsToPos(UnitObject.FMovePoints);
            StartIndex = call__28;
            if (StartIndex != (Path.Count - 1))
            {
                {
                    I = StartIndex;
                    int end__29 = Path.Count - 1;
                    for (; I <= end__29; I += 1)
                    {
                        var call__30 = Path[I];
                        FPath.Add(call__30);
                    }
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CombatTE.pas:253
        // { $70 }
        public Aow.Units.AoWE.TMoveStatus ExecuteDefaultMove()
        {
            Aow.Units.AoWE.TMoveStatus Result = default;
            Aow.Units.AoWE.TAoWMapField NewField = default;
            Aow.Units.AoWE.TAoWMapField OldField = default;
            Aow.Rtl.TRect R = default;
            Move();
            OldField = (Aow.Units.AoWE.TAoWMapField)FUnit.FUnitHS.Owner;
            var call__31 = FPath[FMoveIndex];
            var call__32 = GetMapF(call__31);
            NewField = call__32;
            FUnit.FUnitHS.MoveTo(NewField, 0);
            var call__33 = NewField.FindChildIndex(FUnit.FUnitHS);
            if (call__33 != -1)
            {
                Result = ProcessMove();
                var call__34 = FPath[FMoveIndex];
                FUnit.MovedTo(OldField, NewField, call__34.Flag);
                var call__35 = Aow.Rtl.Rtl.Rect(OldField.Xhx, OldField.Yhx, OldField.Xhx + 1, OldField.Yhx + 1);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(call__35, 0);
                bool bool__36 = (FMoveIndex == 0);
                if (bool__36)
                {
                    var call__37 = NewField.FindChild(2229544);
                    bool__36 = (call__37 != null);
                }

                if (bool__36 && (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FDefenderPlayer))
                {
                    FUnit.SetStatus(TSet<Aow.Units.AoWE.TCombatObjectStatusElement>.Of(Aow.Units.AoWE.TCombatObjectStatusElement.csFled));
                    var call__38 = FUnit.FUnitHS.Yhx;
                    var call__39 = FUnit.FUnitHS.Xhx;
                    var call__40 = FUnit.FUnitHS.Yhx;
                    var call__41 = FUnit.FUnitHS.Xhx;
                    var call__42 = Aow.Rtl.Rtl.Rect(call__41, call__40, call__39 + 1, call__38 + 1);
                    R = call__42;
                    FUnit.FUnitHS.Free();
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                    FUnit.FUnitHS = null;
                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                }
                else
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                    {
                        var call__43 = FUnit.GetPlayer();
                        var call__44 = FUnit.GetVisibilityRange();
                        var call__45 = FUnit.FUnitHS.Yhx;
                        var call__46 = FUnit.FUnitHS.Xhx;
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.ExploreArea(call__46, call__45, 0, unchecked((sbyte)call__43), call__44);
                        var call__47 = FUnit.GetVisibilityRange();
                        var call__48 = FUnit.FUnitHS.Yhx;
                        var call__49 = FUnit.FUnitHS.Xhx;
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapChangedRad(call__49, call__48, 0, call__47 + 1);
                    }
                    else
                    {
                        var call__50 = FUnit.FUnitHS.Yhx;
                        var call__51 = FUnit.FUnitHS.Xhx;
                        var call__52 = FUnit.FUnitHS.Yhx;
                        var call__53 = FUnit.FUnitHS.Xhx;
                        var call__54 = Aow.Rtl.Rtl.Rect(call__53, call__52, call__51 + 1, call__50 + 1);
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(call__54, 0);
                    }

                    var call__55 = FUnit.GetPlayer();
                    var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__55);
                    bool bool__57 = (call__56.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
                    if (!bool__57)
                    {
                        var call__58 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)FUnit.FCombatPlayer));
                        bool__57 = call__58.FAuto;
                    }

                    if (bool__57)
                    {
                        FUnit.FMovePoints = 0;
                    }
                }
            }
            else
            {
                FUnit.FUnitHS = null;
                Result = Aow.Units.AoWE.TMoveStatus.msDone;
            }

            return Result;
        }

        // intf_P5_07.inc:765
        public static new Aow.Units.CombatTE.TCombatMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCombatMoveTE o = new Aow.Units.CombatTE.TCombatMoveTE();
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

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // CombatTE.pas:206
            public override int ClassID()
            {
                int Result = default;
                Result = 2236416;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCombatMoveTE => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P5_07.inc:765
            public new Aow.Units.CombatTE.TCombatMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCombatMoveTE o = (Aow.Units.CombatTE.TCombatMoveTE)NewInstance();
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

    // { TCMeleeMoveTE (instance size 64, VMT 004077C0) }
    public partial class TCMeleeMoveTE : Aow.Units.CombatTE.TCombatMoveTE
    {
        public Aow.Units.AoWE.TMeleeRound FMeleeRound;
        // { $38 }
        public int FStrikeIndex;
        // CombatTE.pas:454
        public override Aow.Units.AoWE.TMoveStatus LastMove()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // CombatTE.pas:577
        public void NextStrike()
        {
            bool Locked = default;
            Aow.Units.AoWE.TMeleeStrike Strike = default;
            Locked = true;
            while (Locked && (FStrikeIndex < FMeleeRound.FCount))
            {
                FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
                if (Strike.Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
                {
                    var call__59 = FUnit.GetLocked();
                    Locked = call__59;
                }
                else
                {
                    var call__60 = FPreviousUnit.GetLocked();
                    Locked = call__60;
                }

                if (Locked)
                {
                    FStrikeIndex = FStrikeIndex + 1;
                }
            }

            if (FStrikeIndex < FMeleeRound.FCount)
            {
                FStep = Aow.Units.CombatTE.TTokenStep.tokDelayStrike;
                FDelay = 3;
            }
            else
            {
                FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
            }
        }

        // CombatTE.pas:602
        public void ExecuteStrike()
        {
            Aow.Units.AoWE.TMeleeStrike Strike = default;
            if (FStrike != null)
            {
                FStrike.Release();
            }

            var call__61 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__61;
            var call__62 = FMeleeRound.CreateStrikeCA(FStrikeIndex);
            FStrike = call__62;
            FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
            if (Strike.Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
            {
                if (FStrike.FDamage == 0)
                {
                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeMiss);
                }
                else
                {
                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeHit);
                }
            }
            else
            {
                if (FStrike.FDamage == 0)
                {
                    FPreviousUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeMiss);
                }
                else
                {
                    FPreviousUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeHit);
                }
            }

            FStrikeIndex = FStrikeIndex + 1;
            FStep = Aow.Units.CombatTE.TTokenStep.tokInitStrike;
        }

        // intf_P5_07.inc:765
        public static new Aow.Units.CombatTE.TCMeleeMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCMeleeMoveTE o = new Aow.Units.CombatTE.TCMeleeMoveTE();
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

        public new partial class Meta : Aow.Units.CombatTE.TCombatMoveTE.Meta
        {
            // CombatTE.pas:451
            // { $3C }
            public override int ClassID()
            {
                int Result = default;
                Result = 2236417;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCMeleeMoveTE => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P5_07.inc:765
            public new Aow.Units.CombatTE.TCMeleeMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCMeleeMoveTE o = (Aow.Units.CombatTE.TCMeleeMoveTE)NewInstance();
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

    // { TCAbTouchMoveTE (instance size 108, VMT 004078AC) }
    public partial class TCAbTouchMoveTE : Aow.Units.CombatTE.TCombatMoveTE
    {
        public Aow.Units.AoWTC.TfxHS FEffect;
        // { $38 }
        public int FAbilityID;
        // { $3C }
        public Aow.Units.AoWE.TPossessCA FPossessAction;
        // { $40 }
        public Aow.Units.AoWE.TEntangleCA FEntangleAction;
        // { $44 }
        public Aow.Units.AoWE.TWebCA FWebAction;
        // { $48 }
        public Aow.Units.AoWE.TTurnUndeadCA FTurnUndeadAction;
        // { $4C }
        public Aow.Units.AoWE.THealingCA FHealingAction;
        // { $50 }
        public Aow.Units.AoWE.TCommandCA FCommandAction;
        // { $54 }
        public Aow.Units.AoWE.TInvokeDeathCA FDeathAction;
        // { $58 }
        public Aow.Units.AoWE.TDispelMagicAbilityCA FDispelAction;
        // { $5C }
        public Aow.Units.AoWTC.TfxType FEffectType;
        // { $60 }
        public bool FSuccessful;
        // { $61 }
        public Aow.Units.FixedArray_byte_2 F_CAbTouchMoveTE_62;
        public int FDamage;
        // { $64 }
        public Aow.Units.CityWall.TCityWall FWall;
        // CombatTE.pas:627
        // { $68 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAbilityID, 30);
        }

        // CombatTE.pas:633
        public override Aow.Units.AoWE.TMoveStatus LastMove()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // intf_P5_07.inc:765
        public static new Aow.Units.CombatTE.TCAbTouchMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCAbTouchMoveTE o = new Aow.Units.CombatTE.TCAbTouchMoveTE();
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

        public new partial class Meta : Aow.Units.CombatTE.TCombatMoveTE.Meta
        {
            // CombatTE.pas:624
            public override int ClassID()
            {
                int Result = default;
                Result = 2236418;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCAbTouchMoveTE => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P5_07.inc:765
            public new Aow.Units.CombatTE.TCAbTouchMoveTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCAbTouchMoveTE o = (Aow.Units.CombatTE.TCAbTouchMoveTE)NewInstance();
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

    // { TCAbRangedTE (instance size 60, VMT 0040799C) }
    public partial class TCAbRangedTE : Aow.Units.NetworkE.TTokenEvent
    {
        public Aow.Units.AoWTC.TTacticalCombatUnit FUnit;
        // { $10 }
        public int FAbilityID;
        // { $14 }
        public int FSpellID;
        // { $18 }
        public ushort FSourceX;
        // { $1C }
        public ushort FSourceY;
        // { $1E }
        public ushort FTargetX;
        // { $20 }
        public ushort FTargetY;
        // { $22 }
        public Aow.Units.AoWTC.TfxType FEffectType;
        // { $24 }
        public Aow.Units.FixedArray_byte_3 F_CAbRangedTE_25;
        public int FStrikeCount;
        // { $28 }
        public Aow.Rtl.TObject FSound;
        // { $2C }
        public int FDelay;
        // { $30 }
        public int FStrikeIndex;
        // { $34 }
        public int FState;
        // CombatTE.pas:920
        // { $38 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAbilityID, 30);
            Stream.rwInteger(ref FSpellID, 31);
            Stream.rwWord(ref FTargetX, 32);
            Stream.rwWord(ref FTargetY, 33);
            Stream.rwWord(ref FSourceX, 34);
            Stream.rwWord(ref FSourceY, 35);
        }

        // CombatTE.pas:931
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // CombatTE.pas:1373
        public void NextStrike()
        {
            Aow.Rtl.TList Used = default;
            Aow.Units.AoWTC.TTacticalCombatUnit Target = default;
            Aow.Units.AoWTC.TfxHS Effect = default;
            Aow.Units.AoWTC.TfxHS ExtraEffect = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            float A = default;
            float Sweep = default;
            float Angle = default;
            float Radius = default;
            int X1 = default;
            int Y1 = default;
            int X2 = default;
            int Y2 = default;
            int DX = default;
            int DY = default;
            int I = default;
            int J = default;
            int K = default;
            Aow.Units.aowFX.TXYZ Point = default;
            Aow.Units.HSEngine.THexagonSprite HitObject = default;
            Aow.Units.aowFX.TXYZList Path = default;
            Aow.Rtl.TList Obstacles = default;
            Aow.Units.AoWTC.TROb Obstacle = default;
            bool Found = default;
            Aow.Units.AoWTC.TTacticalCombatUnit FindTarget(int X, int Y)
            {
                Aow.Units.AoWTC.TTacticalCombatUnit Result = default;
                int I = default;
                int Distance = default;
                int BestDistance = default;
                int J = default;
                bool Available = default;
                Result = null;
                BestDistance = 6;
                {
                    I = 0;
                    int end__63 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__63; I += 1)
                    {
                        var call__64 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__65 = (call__64 is Aow.Units.AoWE.TCombatUnit);
                        if (bool__65)
                        {
                            var call__66 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__67 = call__66.GetEnabled();
                            bool__65 = call__67;
                        }

                        bool bool__68 = bool__65;
                        if (bool__68)
                        {
                            var call__69 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__70 = call__69.GetSide();
                            var call__71 = FUnit.GetSide();
                            bool__68 = (call__70 != call__71);
                        }

                        if (bool__68)
                        {
                            Available = true;
                            {
                                J = 0;
                                int end__72 = Used.Count - 1;
                                for (; J <= end__72; J += 1)
                                {
                                    var call__73 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    var call__74 = Used[J];
                                    if (call__73 == call__74)
                                    {
                                        Available = false;
                                    }
                                }
                            }

                            if (Available)
                            {
                                var call__75 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__76 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__75).FUnitHS.Yhx;
                                var call__77 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__78 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__77).FUnitHS.Xhx;
                                var call__79 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__78, call__76);
                                Distance = call__79;
                                if (Distance < BestDistance)
                                {
                                    var call__80 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__80;
                                    BestDistance = Distance;
                                }
                            }
                        }
                    }
                }

                return Result;
            }

            void ChooseTarget()
            {
                int I = default;
                int Index = default;
                int Total = default;
                int Distance = default;
                Aow.Rtl.TList Weights = default;
                Used = new Aow.Rtl.TList();
                Weights = new Aow.Rtl.TList();
                Total = 0;
                {
                    I = 0;
                    int end__81 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__81; I += 1)
                    {
                        var call__82 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__83 = (call__82 is Aow.Units.AoWE.TCombatUnit);
                        if (bool__83)
                        {
                            var call__84 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__85 = call__84.GetEnabled();
                            bool__83 = call__85;
                        }

                        bool bool__86 = bool__83;
                        if (bool__86)
                        {
                            var call__87 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            bool__86 = (call__87 != Target);
                        }

                        if (bool__86)
                        {
                            var call__88 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__89 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__88).FUnitHS.Yhx;
                            var call__90 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__91 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__90).FUnitHS.Xhx;
                            var call__92 = Aow.Units.HSEngine.HSEngine.dHXtoRad(FTargetX, FTargetY, call__91, call__89);
                            Distance = call__92;
                            if (Distance < 13)
                            {
                                Total = Total + (13 - Distance);
                                var call__93 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                Used.Add(call__93);
                                /* TODO(T-A3) Integer address requires managed pointer provenance */
                                throw new System.NotImplementedException();
                            }
                        }
                    }
                }

                if (Target != null)
                {
                    Used.Add(Target);
                    /* TODO(T-A3) Integer address requires managed pointer provenance */
                    throw new System.NotImplementedException();
                    Total = Total + Total;
                }

                if (Used.Count > 0)
                {
                    /* TODO(T-N7) Integer read from an untyped pointer requires a typed transport override */
                    throw new System.NotImplementedException();
                    Index = 0;
                    while ((Index < (Used.Count - 1)) && (I > 0))
                    {
                        Index = Index + 1;
                        /* TODO(T-N7) Integer read from an untyped pointer requires a typed transport override */
                        throw new System.NotImplementedException();
                    }

                    var call__97 = Used[Index];
                    Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__97;
                }

                Used.Free();
                Weights.Free();
            }

            if (FStrikeIndex >= FStrikeCount)
            {
                return;
            }

            var call__98 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxNone);
            Effect = call__98;
            Obstacles = new Aow.Rtl.TList();
            switch (FEffectType)
            {
                case Aow.Units.AoWTC.TfxType.fxBreath:
                    var call__99 = FUnit.FUnitHS.Yhx;
                    var call__100 = FUnit.FUnitHS.Xhx;
                    var call__101 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__100, call__99, FTargetX, FTargetY);
                    var call__102 = FUnit.FUnitHS.Yhx;
                    var call__103 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__103, call__102, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[call__101 - 1] * 12) - 3], ref J, ref K);
                    var call__104 = FUnit.FUnitHS.Yhx;
                    var call__105 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__105, call__104, ref X1, ref Y1);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(J, K, ref X2, ref Y2);
                    Sweep = FStrikeIndex;
                    if (((double)Sweep) > 9)
                    {
                        Sweep = (float)(((double)19) - Sweep);
                    }

                    if (Y2 == Y1)
                    {
                        A = 0;
                    }
                    else
                    {
                        if (X2 == X1)
                        {
                            A = (float)(((double)System.Math.PI) / 2);
                        }
                        else
                        {
                            A = (float)System.Math.Atan(((double)System.Math.Abs(X2 - X1)) / System.Math.Abs(Y2 - Y1));
                        }
                    }

                    if ((Y2 == Y1) || (X2 == X1))
                    {
                        var call__106 = System.Math.PI;
                        var call__107 = System.Math.PI;
                        Angle = (float)(((double)(((double)(((double)call__106) / 2)) - A)) + (((double)(((double)(((double)(((double)Sweep) * 6)) - 30)) * call__107)) / 180));
                    }
                    else
                    {
                        Angle = (float)(((double)A) + (((double)(((double)(((double)(((double)Sweep) * 6)) - 30)) * System.Math.PI)) / 180));
                    }

                    Radius = (float)System.Math.Sqrt(((X2 - X1) * (X2 - X1)) + ((Y2 - Y1) * (Y2 - Y1)));
                    if (X2 >= X1)
                    {
                        DX = (Aow.Rtl.Rtl.Round(((double)System.Math.Sin(Angle)) * Radius) - X2) + X1;
                    }
                    else
                    {
                        DX = (X1 - X2) - Aow.Rtl.Rtl.Round(((double)System.Math.Sin(Angle)) * Radius);
                    }

                    if (Y2 >= Y1)
                    {
                        DY = (Aow.Rtl.Rtl.Round(((double)System.Math.Cos(Angle)) * Radius) - Y2) + Y1;
                    }
                    else
                    {
                        DY = (Y1 - Y2) - Aow.Rtl.Rtl.Round(((double)System.Math.Cos(Angle)) * Radius);
                    }

                    var call__108 = FUnit.FUnitHS.Xhx;
                    var call__109 = FUnit.FUnitHS.Yhx;
                    var call__110 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__110, 50, 20, 50, unchecked((short)call__108), unchecked((short)call__109), unchecked((short)J), unchecked((short)K), unchecked((short)DX), unchecked((short)DY), false, ref Effect.FPath, ref Obstacles);
                    if (FStrikeIndex < 12)
                    {
                        var call__111 = FUnit.FUnitHS.Yhx;
                        var call__112 = FUnit.FUnitHS.Xhx;
                        var call__113 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__112, call__111, FTargetX, FTargetY);
                        var call__114 = FUnit.FUnitHS.Yhx;
                        var call__115 = FUnit.FUnitHS.Xhx;
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__115, call__114, Aow.Units.AoWTC.AoWTC.BreathHN[((Aow.Units.AoWTC.AoWTC.BreathDirection[call__113 - 1] * 12) + Aow.Units.AoWTC.AoWTC.BreathHit[FStrikeIndex]) - 12], ref Effect.FTargetX, ref Effect.FTargetY);
                    }

                    break;
                case Aow.Units.AoWTC.TfxType.fxTouch:
                case Aow.Units.AoWTC.TfxType.fxEffect:
                case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                case Aow.Units.AoWTC.TfxType.fxGeyser:
                case Aow.Units.AoWTC.TfxType.fxCloud:
                case Aow.Units.AoWTC.TfxType.fxMindDecay:
                case Aow.Units.AoWTC.TfxType.fxShockWave:
                case Aow.Units.AoWTC.TfxType.fxSacFlame:
                    if (FEffectType == Aow.Units.AoWTC.TfxType.fxSacredWrath)
                    {
                        FTargetX = 32;
                        FTargetY = 29;
                    }

                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    switch (FEffectType)
                    {
                        case Aow.Units.AoWTC.TfxType.fxGeyser:
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX, FTargetY - 5, 0);
                            break;
                        case Aow.Units.AoWTC.TfxType.fxMindDecay:
                            Effect.FTargetX = FTargetX;
                            Effect.FTargetY = FTargetY;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX + 1, FTargetY - 4, 0);
                            break;
                        case Aow.Units.AoWTC.TfxType.fxShockWave:
                            var call__116 = FUnit.FUnitHS.Yhx;
                            var call__117 = FUnit.FUnitHS.Xhx;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__117, call__116, 0);
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacFlame:
                            var call__118 = FUnit.FUnitHS.Yhx;
                            var call__119 = FUnit.FUnitHS.Xhx;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__119, call__118 - 3, 0);
                            break;
                        default:
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                            break;
                    }

                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Point.X = X2 + 16;
                    Point.Y = Y2 + 16;
                    Effect.FPath.Add(Point);
                    var call__120 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__121 = call__120.GetField(FTargetX, FTargetY);
                    var call__122 = call__121.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__122;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    break;
                case Aow.Units.AoWTC.TfxType.fxWinds:
                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Point.X = X2 + 16;
                    Point.Y = Y2 + 16;
                    Effect.FPath.Add(Point);
                    var call__123 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__124 = call__123.GetField(FTargetX, FTargetY);
                    var call__125 = call__124.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__125;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    var call__126 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(37);
                    I = call__126 + 1;
                    Found = false;
                    while (!Found)
                    {
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                        bool bool__127 = (((J > 5) && (J < 60)) && (K > 5)) && (K < 53);
                        if (bool__127)
                        {
                            var call__128 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__129 = call__128.GetField(J, K);
                            var call__130 = call__129.FindOwnedChild(2228484);
                            bool__127 = (call__130 == null);
                        }

                        bool bool__131 = bool__127;
                        if (bool__131)
                        {
                            var call__132 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__133 = call__132.GetField(J, K);
                            bool bool__134 = (call__133.OverlayType != 8);
                            if (!bool__134)
                            {
                                var call__135 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__136 = call__135.GetField(J, K);
                                var call__137 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__138 = call__137.GetField(FTargetX, FTargetY);
                                bool__134 = (call__136.OverlayType == call__138.OverlayType);
                            }

                            bool__131 = bool__134;
                        }

                        bool bool__139 = bool__131;
                        if (bool__139)
                        {
                            var call__140 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__141 = call__140.GetField(J, K);
                            bool__139 = (call__141.TerrainType != 15);
                        }

                        bool bool__142 = bool__139;
                        if (bool__142)
                        {
                            var call__143 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__144 = call__143.GetField(J, K);
                            bool__142 = (call__144.TerrainType != 7);
                        }

                        bool bool__145 = bool__142;
                        if (bool__145)
                        {
                            var call__146 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__147 = call__146.GetField(J, K);
                            bool bool__148 = (call__147.TerrainType != 0);
                            if (bool__148)
                            {
                                var call__149 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__150 = call__149.GetField(J, K);
                                bool__148 = (call__150.TerrainType != 10);
                            }

                            bool bool__151 = bool__148;
                            if (bool__151)
                            {
                                var call__152 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__153 = call__152.GetField(J, K);
                                bool__151 = (call__153.TerrainType != 9);
                            }

                            bool bool__154 = bool__151;
                            if (!bool__154)
                            {
                                var call__155 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__156 = call__155.GetField(J, K);
                                var call__157 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__158 = call__157.GetField(FTargetX, FTargetY);
                                bool__154 = (call__156.TerrainType == call__158.TerrainType);
                            }

                            bool__145 = bool__154;
                        }

                        if (bool__145)
                        {
                            Found = true;
                        }
                        else
                        {
                            I += 1;
                        }
                    }

                    Aow.Units.HSEngine.HSEngine.HXtoHP(J, K, ref DX, ref DY);
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                {
                    I = 0;
                    byte end__159 = 29;
                    for (; I <= end__159; I += 1)
                    {
                        var call__160 = Aow.Units.aowFX.TXYZ.Create();
                        Effect.FPath.Add(call__160);
                        var call__161 = Effect.FPath[0];
                        var call__162 = Aow.Rtl.Rtl.Random(30);
                        var call__163 = Effect.FPath.Count;
                        var call__164 = Effect.FPath[call__163 - 1];
                        call__164.X = (call__161.X + call__162) - 15;
                        var call__165 = Effect.FPath[0];
                        var call__166 = Aow.Rtl.Rtl.Random(30);
                        var call__167 = Effect.FPath.Count;
                        var call__168 = Effect.FPath[call__167 - 1];
                        call__168.Y = (call__165.Y + call__166) - 15;
                    }
                }

                    J = Aow.Rtl.Rtl.Random(300);
                    K = Aow.Rtl.Rtl.Random(300);
                    var call__169 = Effect.FPath.Count;
                    var call__170 = Effect.FPath[call__169 - 1];
                    var call__171 = Effect.FPath.Count;
                    var call__172 = Effect.FPath[call__171 - 1];
                    var call__173 = Effect.FPath.Count;
                    var call__174 = Effect.FPath[call__173 - 1];
                    var call__175 = Effect.FPath.Count;
                    var call__176 = Effect.FPath[call__175 - 1];
                    var call__177 = Effect.FPath.Count;
                    var call__178 = Effect.FPath[call__177 - 1];
                    var call__179 = Effect.FPath.Count;
                    var call__180 = Effect.FPath[call__179 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__180.X), unchecked((short)(call__174.Y - call__176.Z)), unchecked((short)((call__178.X + J) - 150)), unchecked((short)(((call__170.Y - call__172.Z) + K) - 150)), unchecked((short)(((System.Math.Abs(J - 150) + System.Math.Abs(K - 150)) / 50) + 2)), 100, ref Effect.FPath);
                    var call__181 = Effect.FPath.Count;
                    Effect.FTargetX = call__181;
                    J = Aow.Rtl.Rtl.Random(200);
                    K = Aow.Rtl.Rtl.Random(200);
                    var call__182 = Effect.FPath.Count;
                    var call__183 = Effect.FPath[call__182 - 1];
                    var call__184 = Effect.FPath.Count;
                    var call__185 = Effect.FPath[call__184 - 1];
                    var call__186 = Effect.FPath.Count;
                    var call__187 = Effect.FPath[call__186 - 1];
                    var call__188 = Effect.FPath.Count;
                    var call__189 = Effect.FPath[call__188 - 1];
                    var call__190 = Effect.FPath.Count;
                    var call__191 = Effect.FPath[call__190 - 1];
                    var call__192 = Effect.FPath.Count;
                    var call__193 = Effect.FPath[call__192 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__193.X), unchecked((short)(call__187.Y - call__189.Z)), unchecked((short)((call__191.X + J) - 100)), unchecked((short)(((call__183.Y - call__185.Z) + K) - 100)), unchecked((short)(((System.Math.Abs(J - 100) + System.Math.Abs(K - 100)) / 50) + 3)), 100, ref Effect.FPath);
                    var call__194 = Effect.FPath.Count;
                    Effect.FTargetY = call__194;
                    var call__195 = Effect.FPath.Count;
                    var call__196 = Effect.FPath[call__195 - 1];
                    var call__197 = Effect.FPath.Count;
                    var call__198 = Effect.FPath[call__197 - 1];
                    var call__199 = Effect.FPath.Count;
                    var call__200 = Effect.FPath[call__199 - 1];
                    var call__201 = Effect.FPath.Count;
                    var call__202 = Effect.FPath[call__201 - 1];
                    var call__203 = Effect.FPath.Count;
                    var call__204 = Effect.FPath[call__203 - 1];
                    var call__205 = Effect.FPath.Count;
                    var call__206 = Effect.FPath[call__205 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__206.X), unchecked((short)(call__202.Y - call__204.Z)), unchecked((short)(DX + 16)), unchecked((short)(DY + 16)), unchecked((short)(((System.Math.Abs((call__196.Y - call__198.Z) - DY) + System.Math.Abs(call__200.X - DX)) / 50) + 4)), 100, ref Effect.FPath);
                    break;
                case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                    Point = Aow.Units.aowFX.TXYZ.Create();
                    if (Aow.Rtl.Rtl.Random(100) < 50)
                    {
                        Point.X = 0;
                    }
                    else
                    {
                        Point.X = 1;
                    }

                    Effect.FPath.Add(Point);
                    if (FStrikeIndex == 9)
                    {
                        Effect.FTarget = FUnit;
                    }

                    break;
                case Aow.Units.AoWTC.TfxType.fxScreenNS:
                    switch (FSpellID)
                    {
                        case 124:
                            I = 19;
                            break;
                        default:
                            I = 7;
                            break;
                    }

                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, Aow.Units.AoWTC.AoWTC.rndHex[FStrikeIndex % I], ref DX, ref DY);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(DX, DY, ref X2, ref Y2);
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(DX, DY, 0, 5);
                    X1 = Aow.Rtl.Rtl.Random(20) + 6;
                    Y1 = Aow.Rtl.Rtl.Random(20) + 6;
                    Aow.Units.aowFX.aowFX.MakeLinePath((X2 + X1) - 100, (Y2 + Y1) - 300, X2 + X1, Y2 + Y1, 30, ref Effect.FPath);
                    switch (FSpellID)
                    {
                        case 124:
                            Effect.FTarget = null;
                            break;
                        default:
                        {
                            var call__207 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__208 = call__207.GetField(DX, DY);
                            var call__209 = call__208.FindOwnedChild(2228484);
                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__209;
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                            break;
                    }

                    break;
                case Aow.Units.AoWTC.TfxType.fxChain:
                    var call__210 = FUnit.FUnitHS.Yhx;
                    var call__211 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__211, call__210, ref X1, ref Y1);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Aow.Units.aowFX.aowFX.MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                    var call__212 = Effect.FPath.Count;
                    var call__213 = Effect.FPath[call__212 - 1];
                    call__213.Z = 16;
                    I = 3;
                    Used = new Aow.Rtl.TList();
                    Used.Add(FUnit);
                    var call__214 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__215 = call__214.GetField(FTargetX, FTargetY);
                    var call__216 = call__215.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__216;
                    if (HS != null)
                    {
                        Used.Add(HS.FUnit);
                    }

                    var call__217 = FindTarget(FTargetX, FTargetY);
                    Target = call__217;
                    while ((Target != null) && (I > 0))
                    {
                        Used.Add(Target);
                        X1 = X2 + 16;
                        Y1 = Y2 + 16;
                        var call__218 = Target.FUnitHS.Yhx;
                        var call__219 = Target.FUnitHS.Xhx;
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__219, call__218, ref X2, ref Y2);
                        Aow.Units.aowFX.aowFX.MakeLightningPath(X1, Y1, X2 + 16, Y2, ref Effect.FPath);
                        I -= 1;
                        var call__220 = Target.FUnitHS.Yhx;
                        var call__221 = Target.FUnitHS.Xhx;
                        var call__222 = FindTarget(call__221, call__220);
                        Target = call__222;
                    }

                    Used.Free();
                    break;
                case Aow.Units.AoWTC.TfxType.fxSwarm:
                    var call__223 = FUnit.FUnitHS.Yhx;
                    var call__224 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__224, call__223, ref X1, ref Y1);
                    var call__225 = Aow.Rtl.Rtl.Random(360);
                    var call__226 = Aow.Rtl.Rtl.Random(20);
                    var call__227 = Aow.Rtl.Rtl.Random(20);
                    var call__228 = Aow.Rtl.Rtl.Random(2);
                    var call__229 = Aow.Rtl.Rtl.Random(90);
                    var call__230 = Aow.Rtl.Rtl.Random(5);
                    var call__231 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__231 + 10, call__230 + 20, call__229, call__225, (call__226 + X1) + 6, (call__227 + Y1) - 10, (Aow.Units.aowFX.TSwarmDirection)call__228, ref Effect.FPath);
                    var call__232 = Effect.FPath.Count;
                    K = call__232 - 1;
                {
                    I = 0;
                    byte end__233 = 2;
                    for (; I <= end__233; I += 1)
                    {
                        {
                            J = 0;
                            int end__234 = K;
                            for (; J <= end__234; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__235 = Effect.FPath[J];
                                Point.X = call__235.X;
                                var call__236 = Effect.FPath[J];
                                Point.Y = call__236.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    var call__237 = Effect.FPath.Count;
                    var call__238 = Effect.FPath[call__237 - 1];
                    var call__239 = Effect.FPath.Count;
                    var call__240 = Effect.FPath[call__239 - 1];
                    Aow.Units.aowFX.aowFX.MakeLinePath(call__240.X, call__238.Y, X2 + 16, Y2, 15, ref Effect.FPath);
                    Path = Aow.Units.aowFX.TXYZList.Create();
                    var call__241 = Aow.Rtl.Rtl.Random(360);
                    var call__242 = Effect.FPath.Count;
                    var call__243 = Effect.FPath[call__242 - 1];
                    var call__244 = Effect.FPath.Count;
                    var call__245 = Effect.FPath[call__244 - 1];
                    var call__246 = Aow.Rtl.Rtl.Random(2);
                    var call__247 = Aow.Rtl.Rtl.Random(90);
                    var call__248 = Aow.Rtl.Rtl.Random(5);
                    var call__249 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__249 + 10, call__248 + 20, call__247, call__241, call__243.X, call__245.Y, (Aow.Units.aowFX.TSwarmDirection)call__246, ref Path);
                    var call__250 = Path[0];
                    var call__251 = Effect.FPath.Count;
                    var call__252 = Effect.FPath[call__251 - 1];
                    var call__253 = Effect.FPath.Count;
                    var call__254 = Effect.FPath[call__253 - 1];
                    var call__255 = Path[0];
                    Aow.Units.aowFX.aowFX.MakeLinePath(call__254.X, call__252.Y, call__255.X, call__250.Y, 5, ref Effect.FPath);
                {
                    I = 0;
                    int end__256 = Path.Count - 1;
                    for (; I <= end__256; I += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        var call__257 = Path[I];
                        Effect.FPath.Add(call__257);
                    }
                }

                    var call__258 = Path.Count;
                    K = call__258 - 1;
                {
                    I = 0;
                    byte end__259 = 2;
                    for (; I <= end__259; I += 1)
                    {
                        {
                            J = 0;
                            int end__260 = K;
                            for (; J <= end__260; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__261 = Path[J];
                                Point.X = call__261.X;
                                var call__262 = Path[J];
                                Point.Y = call__262.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                    Path.Free();
                    break;
                case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                    var call__263 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__264 = call__263.GetField(FTargetX, FTargetY);
                    var call__265 = call__264.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__265;
                    if (HS == null)
                    {
                        Target = null;
                    }
                    else
                    {
                        Target = HS.FUnit;
                    }

                    DX = FTargetX;
                    DY = FTargetY;
                    ChooseTarget();
                    if (Target != null)
                    {
                        var call__266 = Target.FUnitHS.Xhx;
                        DX = call__266;
                        var call__267 = Target.FUnitHS.Yhx;
                        DY = call__267;
                    }

                    Effect.FTarget = Target;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(DX, DY, ref X2, ref Y2);
                    if (FStrikeIndex == 0)
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                    }

                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Point.X = X2;
                    Point.Y = Y2;
                    Effect.FPath.Add(Point);
                    var call__268 = Aow.Rtl.Rtl.Random(360);
                    var call__269 = Aow.Rtl.Rtl.Random(20);
                    var call__270 = Aow.Rtl.Rtl.Random(20);
                    var call__271 = Aow.Rtl.Rtl.Random(2);
                    var call__272 = Aow.Rtl.Rtl.Random(90);
                    var call__273 = Aow.Rtl.Rtl.Random(10);
                    var call__274 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__274 + 10, call__273 + 25, call__272, call__268, call__269, call__270 - 10, (Aow.Units.aowFX.TSwarmDirection)call__271, ref Effect.FPath);
                    var call__275 = Effect.FPath.Count;
                    K = call__275 - 1;
                {
                    I = 0;
                    byte end__276 = 4;
                    for (; I <= end__276; I += 1)
                    {
                        {
                            J = 1;
                            int end__277 = K;
                            for (; J <= end__277; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__278 = Effect.FPath[J];
                                Point.X = call__278.X;
                                var call__279 = Effect.FPath[J];
                                Point.Y = call__279.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                {
                    I = 0;
                    byte end__280 = 49;
                    for (; I <= end__280; I += 1)
                    {
                        var call__281 = Effect.FPath[I + 1];
                        call__281.Z = 100 - (I * 2);
                    }
                }

                    break;
                case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Effect.FTargetX = FTargetX;
                    Effect.FTargetY = FTargetY;
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX - 3, FTargetY - 3, 0);
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)(X2 - 200)), unchecked((short)(Y2 - 200)), unchecked((short)(X2 + 16)), unchecked((short)(Y2 + 16)), 5, 100, ref Effect.FPath);
                    break;
                case Aow.Units.AoWTC.TfxType.fxRay:
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(FTargetX - 3, FTargetY - 3, 0, 3);
                    var call__282 = FUnit.FUnitHS.Xhx;
                    var call__283 = FUnit.FUnitHS.Yhx;
                    var call__284 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__284, 10, 0, 50, unchecked((short)call__282), unchecked((short)call__283), unchecked((short)FTargetX), unchecked((short)FTargetY), 0, 0, true, ref Effect.FPath, ref Obstacles);
                    break;
                default:
                    var call__285 = FUnit.FUnitHS.Xhx;
                    var call__286 = FUnit.FUnitHS.Yhx;
                    var call__287 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__287, unchecked((short)FAbilityID), unchecked((short)FSpellID), unchecked((short)call__285), unchecked((short)call__286), unchecked((short)FTargetX), unchecked((short)FTargetY), ref Effect.FPath, ref Obstacles);
                    break;
            }

            Effect.FAbilityIndex = FAbilityID;
            Effect.FSpellIndex = FSpellID;
            Effect.FEffectType = FEffectType;
            switch (FEffectType)
            {
                case Aow.Units.AoWTC.TfxType.fxArrow:
                case Aow.Units.AoWTC.TfxType.fxRock:
                case Aow.Units.AoWTC.TfxType.fxComet:
                case Aow.Units.AoWTC.TfxType.fxBreath:
                case Aow.Units.AoWTC.TfxType.fxCometBall:
                case Aow.Units.AoWTC.TfxType.fxRay:
                case Aow.Units.AoWTC.TfxType.fxSwarm:
                case Aow.Units.AoWTC.TfxType.fxAltArrow:
                    var call__288 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__289 = call__288.GetField(FTargetX, FTargetY);
                    var call__290 = call__289.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__290;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    I = 0;
                    while (I < Obstacles.Count)
                    {
                        var call__291 = Obstacles[I];
                        Obstacle = (Aow.Units.AoWTC.TROb)call__291;
                        var call__292 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                        if (call__292 < Obstacle.Chance)
                        {
                            var call__293 = Effect.FPath[Obstacle.PathIndex];
                            var call__294 = Effect.FPath[Obstacle.PathIndex];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__294.X, call__293.Y, ref J, ref K);
                            if (Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxBreath)
                            {
                                HitObject = null;
                            }
                            else
                            {
                                var call__295 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__296 = call__295.GetField(J, K);
                                var call__297 = call__296.FindOwnedChild(2228484);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__297;
                            }

                            if (HitObject == null)
                            {
                                var call__298 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__299 = call__298.GetField(J, K);
                                var call__300 = call__299.FindOwnedChild(2228582);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__300;
                            }

                            if (HitObject == null)
                            {
                                var call__301 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__302 = call__301.GetField(J, K);
                                var call__303 = call__302.FindOwnedChild(2229508);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__303;
                            }

                            if (HitObject == null)
                            {
                                var call__304 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__305 = call__304.GetField(J, K);
                                var call__306 = call__305.FindOwnedChild(2229510);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__306;
                            }

                            bool bool__307 = (HitObject != null);
                            if (!bool__307)
                            {
                                var call__308 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__309 = call__308.GetField(J, K);
                                bool__307 = (call__309.TerrainType == 15);
                            }

                            bool bool__310 = bool__307;
                            if (!bool__310)
                            {
                                var call__311 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__312 = call__311.GetField(J, K);
                                bool__310 = (call__312.TerrainType == 7);
                            }

                            bool bool__313 = bool__310;
                            if (!bool__313)
                            {
                                var call__314 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__315 = call__314.GetField(J, K);
                                bool__313 = (call__315.OverlayType == 8);
                            }

                            if (bool__313)
                            {
                                {
                                    J = Obstacle.PathIndex + 1;
                                    int end__316 = Effect.FPath.Count - 1;
                                    for (; J <= end__316; J += 1)
                                    {
                                        var call__317 = Effect.FPath.Count;
                                        Effect.FPath.Delete(call__317 - 1);
                                    }
                                }

                                if (!(HitObject is Aow.Units.AoWTC.TTacticalCombatUnitHS))
                                {
                                    Effect.FTarget = null;
                                    if (Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxBreath)
                                    {
                                        Effect.FTargetX = 0;
                                    }
                                }
                                else
                                {
                                    Effect.FTarget = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)HitObject).FUnit;
                                }

                                I = Obstacles.Count;
                            }
                        }

                        I += 1;
                    }

                    break;
            }

            if (FEffectType == Aow.Units.AoWTC.TfxType.fxBreath)
            {
                Effect.FTarget = null;
            }
            else
            {
                switch (FAbilityID)
                {
                    case 31:
                        Effect.FTarget = null;
                        break;
                    case 52:
                        switch (FSpellID)
                        {
                            case 104:
                            case 107:
                            case 113:
                            case 116:
                            case 117:
                            case 127:
                            case 129:
                            case 130:
                                Effect.FTarget = null;
                                break;
                            case 102:
                            case 106:
                            case 120:
                            case 128:
                                Effect.FTarget = FUnit;
                                break;
                            case 115:
                                var call__318 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                                if (call__318 < 75)
                                {
                                    Effect.FTarget = null;
                                }

                                break;
                            case 100:
                                var call__319 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxSFOverlay);
                                ExtraEffect = call__319;
                                var call__320 = FUnit.FUnitHS.Yhx;
                                var call__321 = FUnit.FUnitHS.Xhx;
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__321, call__320, ref X1, ref Y1);
                                while (true)
                                {
                                    var call__322 = ExtraEffect.FPath.Count;
                                    var call__323 = Effect.FPath.Count;
                                    if (!(call__322 < call__323))
                                        break;
                                    {
                                        Point = Aow.Units.aowFX.TXYZ.Create();
                                        Point.X = X1 + 16;
                                        Point.Y = Y1;
                                        ExtraEffect.FPath.Add(Point);
                                    }
                                }

                                ExtraEffect.FAbilityIndex = FAbilityID;
                                ExtraEffect.FSpellIndex = FSpellID;
                                break;
                        }

                        break;
                }
            }

            var call__324 = FUnit.FUnitHS.Yhx;
            var call__325 = FUnit.FUnitHS.Xhx;
            var call__326 = Aow.Units.AoWTC.AoWTC.GetRangedDirIndex(FTargetX - call__325, FTargetY - call__324);
            Effect.FVariant = call__326;
            if (FStrikeIndex == 0)
            {
                switch (FEffectType)
                {
                    case Aow.Units.AoWTC.TfxType.fxArrow:
                    case Aow.Units.AoWTC.TfxType.fxRock:
                    case Aow.Units.AoWTC.TfxType.fxComet:
                    case Aow.Units.AoWTC.TfxType.fxBreath:
                    case Aow.Units.AoWTC.TfxType.fxCometBall:
                    case Aow.Units.AoWTC.TfxType.fxEffect:
                    case Aow.Units.AoWTC.TfxType.fxRay:
                    case Aow.Units.AoWTC.TfxType.fxChain:
                    case Aow.Units.AoWTC.TfxType.fxSwarm:
                    case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                    case Aow.Units.AoWTC.TfxType.fxSFOverlay:
                    case Aow.Units.AoWTC.TfxType.fxDispel:
                    case Aow.Units.AoWTC.TfxType.fxAltArrow:
                    case Aow.Units.AoWTC.TfxType.fxGeyser:
                    case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                        switch (Effect.FVariant)
                        {
                            case 0:
                            case 1:
                            case 23:
                                FUnit.FUnitHS.SetDirection(1);
                                break;
                            case >= 2 and <= 6:
                                FUnit.FUnitHS.SetDirection(2);
                                break;
                            case >= 7 and <= 10:
                                FUnit.FUnitHS.SetDirection(3);
                                break;
                            case >= 11 and <= 13:
                                FUnit.FUnitHS.SetDirection(4);
                                break;
                            case >= 14 and <= 17:
                                FUnit.FUnitHS.SetDirection(5);
                                break;
                            case >= 18 and <= 22:
                                FUnit.FUnitHS.SetDirection(6);
                                break;
                        }

                        break;
                }
            }

            if (TSet<Aow.Units.AoWTC.TfxType>.Of(Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxCometBall, Aow.Units.AoWTC.TfxType.fxScreenNS).Contains(FEffectType))
            {
                Effect.FVariant = 0;
            }

            if ((FEffectType == Aow.Units.AoWTC.TfxType.fxBreath) && (FStrikeIndex == 0))
            {
                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caBreathAttack);
            }
            else
            {
                if (((FAbilityID != 52) && (Aow.Units.AoWTC.AoWTC.AbilTypes[FAbilityID] != Aow.Units.AoWTC.TfxType.fxBreath)) || (FStrikeIndex == 0))
                {
                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caAbility);
                }
            }

            {
                I = 0;
                int end__327 = Obstacles.Count - 1;
                for (; I <= end__327; I += 1)
                {
                    var call__328 = Obstacles[I];
                    ((Aow.Rtl.TObject)call__328).Free();
                }
            }

            Obstacles.Free();
            FStrikeIndex += 1;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CombatTE.TCAbRangedTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TCAbRangedTE o = new Aow.Units.CombatTE.TCAbRangedTE();
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

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // CombatTE.pas:917
            public override int ClassID()
            {
                int Result = default;
                Result = 2236419;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TCAbRangedTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CombatTE.TCAbRangedTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TCAbRangedTE o = (Aow.Units.CombatTE.TCAbRangedTE)NewInstance();
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

    // { TTerminateCombatTE (instance size 16, VMT 00407A64) }
    public partial class TTerminateCombatTE : Aow.Units.NetworkE.TTokenEvent
    {
        // CombatTE.pas:152
        public override Aow.Units.NetworkE.TExecuteResult Execute(Aow.Units.NetworkE.TTokenExecuter TE)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Aow.Units.AoWE.AoWE.AoWHSMap.DestroyCombat();
            Result = Aow.Units.NetworkE.TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230
        public static new Aow.Units.CombatTE.TTerminateCombatTE Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatTE.TTerminateCombatTE o = new Aow.Units.CombatTE.TTerminateCombatTE();
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

        public new partial class Meta : Aow.Units.NetworkE.TTokenEvent.Meta
        {
            // CombatTE.pas:149
            public override int ClassID()
            {
                int Result = default;
                Result = 2236422;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatTE.TTerminateCombatTE => (Meta)MetaRegistry.Of(typeof(T));
            // NetworkE.pas:230
            public new Aow.Units.CombatTE.TTerminateCombatTE Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatTE.TTerminateCombatTE o = (Aow.Units.CombatTE.TTerminateCombatTE)NewInstance();
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

    public enum TTokenStep : byte
    {
        tokStart = 0,
        tokEnd = 1,
        tokFirstStrike = 2,
        tokInitStrike = 3,
        tokExeStrike = 4,
        tokEndStrike = 5,
        tokExeEndStrike = 6,
        tokStepUp = 7,
        tokDelayStrike = 8
    }

    public static partial class CombatTE
    {
        // CombatTE.pas:1777
        public static void MakeDEHSList(int X, int Y, TSet<Aow.Units.AoWE.TDamageType> DamageTypes, bool Resisted)
        {
            void MakeEffect(Aow.Units.AoWTC.TfxType EffectType)
            {
                Aow.Units.AoWTC.TfxHS Effect = default;
                int I = default;
                int Count = default;
                float Z = default;
                float Speed = default;
                float VZ = default;
                float Gravity = default;
                Count = Aow.Rtl.Rtl.Random(5) + 8;
                {
                    I = 1;
                    int end__329 = Count - 1;
                    for (; I <= end__329; I += 1)
                    {
                        var call__330 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(EffectType);
                        Effect = call__330;
                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(10) + 10)) * 0.1);
                        Gravity = (float)0.5;
                        if (Resisted || (EffectType == Aow.Units.AoWTC.TfxType.fxDamMagic))
                        {
                            VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) * 0.1);
                        }
                        else
                        {
                            VZ = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) * 0.1);
                        }

                        Z = 0;
                        Aow.Units.aowFX.aowFX.MakeArcPath(X, Y, (I * 360) / Count, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                    }
                }
            }

            X = X + 16;
            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtLightning))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamLightning);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtFire))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamFire);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtPoison))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamPoison);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtMagic))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamMagic);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtHoly))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamHoly);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtDeath))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamDeath);
            }

            if (DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtCold))
            {
                MakeEffect(Aow.Units.AoWTC.TfxType.fxDamCold);
            }
        }

        // CombatTE.pas:1809
        public static Aow.Units.CombatTE.TCEndCombatTE CreateTCEndCombatTE()
        {
            Aow.Units.CombatTE.TCEndCombatTE Result = default;
            Result = Aow.Units.CombatTE.TCEndCombatTE.Create(null);
            return Result;
        }

        // CombatTE.pas:1812
        public static Aow.Units.CombatTE.TCEndTurnTE CreateTCEndTurnTE(sbyte Player)
        {
            Aow.Units.CombatTE.TCEndTurnTE Result = default;
            bool bool__331 = (Player != -1);
            if (bool__331)
            {
                var call__332 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Player);
                bool__331 = call__332;
            }

            if (bool__331)
            {
                Result = Aow.Units.CombatTE.TCEndTurnTE.Create(null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // CombatTE.pas:1819
        public static Aow.Units.CombatTE.TCEndTurnTE CreateTCCancelEndTurnTE(sbyte Player)
        {
            Aow.Units.CombatTE.TCEndTurnTE Result = default;
            var call__333 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__334 = call__333.GetBusy();
            if (!call__334)
            {
                Result = Aow.Units.CombatTE.TCEndTurnTE.Create(null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // CombatTE.pas:1826
        public static void MakeHitBlood(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, int Damage)
        {
            Aow.Units.AoWTC.TfxHS Effect = default;
            Aow.Units.aowFX.TXYZ Point = default;
            var call__335 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxBlood);
            Effect = call__335;
            Point = Aow.Units.aowFX.TXYZ.Create();
            Effect.FPath.Add(Point);
            switch (Damage)
            {
                case >= 1 and <= 3:
                    Point.Z = 1;
                    break;
                case >= 4 and <= 7:
                    Point.Z = 2;
                    break;
                case >= 8 and <= 100:
                    Point.Z = 3;
                    break;
            }

            var call__336 = UnitObject.FUnit.GetBloodType();
            switch (call__336)
            {
                case Aow.Units.AoWE.TBloodType.btNone:
                    Point.Z = Point.Z + 5;
                    break;
            }

            var call__337 = UnitObject.FUnitHS.Yhx;
            var call__338 = UnitObject.FUnitHS.Xhx;
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__338, call__337, ref Point.X, ref Point.Y);
            var call__339 = Aow.Rtl.Rtl.Random(10);
            Point.X = (call__339 + Point.X) + 11;
            var call__340 = UnitObject.FUnit.GetUnitSize();
            var call__341 = Aow.Rtl.Rtl.Random(10);
            Point.Y = ((call__341 + Point.Y) + 9) - (((int)call__340) * 10);
        }

        // CombatTE.pas:1845
        public static Aow.Units.CombatTE.TCMeleeMoveTE CreateTCMeleeMoveTE(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path)
        {
            Aow.Units.CombatTE.TCMeleeMoveTE Result = default;
            Result = Aow.Units.CombatTE.TCMeleeMoveTE.Create(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
            }

            return Result;
        }

        // CombatTE.pas:1851
        public static Aow.Units.CombatTE.TCAbTouchMoveTE CreateTCAbTouchMoveTE(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path, int AbilityID)
        {
            Aow.Units.CombatTE.TCAbTouchMoveTE Result = default;
            Result = Aow.Units.CombatTE.TCAbTouchMoveTE.Create(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
                Result.FAbilityID = AbilityID;
            }

            return Result;
        }

        // CombatTE.pas:1861
        public static Aow.Units.CombatTE.TCAbRangedTE CreateTCAbRangedTE(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, ushort X, ushort Y, int AbilityID, int SpellID)
        {
            Aow.Units.CombatTE.TCAbRangedTE Result = default;
            Result = Aow.Units.CombatTE.TCAbRangedTE.Create(null);
            if (Result != null)
            {
                var call__342 = UnitObject.FUnitHS.Xhx;
                Result.FSourceX = unchecked((ushort)call__342);
                var call__343 = UnitObject.FUnitHS.Yhx;
                Result.FSourceY = unchecked((ushort)call__343);
                Result.FTargetX = unchecked((ushort)X);
                Result.FTargetY = unchecked((ushort)Y);
                Result.FAbilityID = AbilityID;
                Result.FSpellID = SpellID;
            }

            return Result;
        }

        // CombatTE.pas:1875
        public static Aow.Units.CombatTE.TCombatMoveTE CreateCombatMoveTE(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path)
        {
            Aow.Units.CombatTE.TCombatMoveTE Result = default;
            Result = Aow.Units.CombatTE.TCombatMoveTE.Create(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
            }

            return Result;
        }

        // CombatTE.pas:1881
        public static bool MoveCombatUnit(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            int MovePoints = default;
            int Cost = default;
            int Index = default;
            Aow.Units.CombatTE.TCombatMoveTE Event = default;
            Aow.Units.AoWE.TMovepath NewPath = default;
            int I = default;
            Result = false;
            var call__344 = UnitObject.GetPlayer();
            var call__345 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__344);
            if (call__345 && (Path.Count > 1))
            {
                MovePoints = UnitObject.FMovePoints;
                Cost = 0;
                Index = Path.Count;
                while ((Index != 0) && (Cost <= MovePoints))
                {
                    Index = Index - 1;
                    var call__346 = Path[Index];
                    Cost = Cost + call__346.Flag;
                }

                if (Cost > MovePoints)
                {
                    Index = Index + 1;
                }

                if (Index < (Path.Count - 1))
                {
                    NewPath = Aow.Units.AoWE.TMovepath.Create();
                    {
                        I = Index;
                        int end__347 = Path.Count - 1;
                        for (; I <= end__347; I += 1)
                        {
                            var call__348 = Path[I];
                            NewPath.Add(call__348);
                        }
                    }

                    Event = Aow.Units.CombatTE.CombatTE.CreateCombatMoveTE(UnitObject, NewPath);
                    if (Event != null)
                    {
                        var call__349 = UnitObject.GetPlayer();
                        var call__350 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__349);
                        call__350.AddEvent(Event);
                        Event.Release();
                        Result = true;
                    }

                    NewPath.Free();
                }
            }

            return Result;
        }

        // CombatTE.pas:1933
        public static bool SetupMultiTCUnitMove(Aow.Units.AoWE.TMoveSettingsList Moves)
        {
            bool Result = default;
            Aow.Units.AoWE.TMultiMoveTE Multi = default;
            Aow.Units.CombatTE.TCombatMoveTE MoveEvent = default;
            Aow.Units.AoWE.TMovepath Path = default;
            int I = default;
            int J = default;
            int K = default;
            int L = default;
            var call__351 = Moves.Count;
            bool bool__352 = (call__351 != 0);
            if (bool__352)
            {
                var call__353 = Moves.GetMoveSettings(0);
                var call__354 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__353.FMoveObject).FUnit.GetPlayer();
                var call__355 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__354);
                bool__352 = call__355;
            }

            if (bool__352)
            {
                Path = Aow.Units.AoWE.TMovepath.Create();
                Multi = Aow.Units.AoWE.TMultiMoveTE.Create(null);
                Multi.FSimultaneous = false;
                Moves.CalculateMovePositions(Aow.Units.CombatTE.CombatTE.ValidateTCMovePosition);
                {
                    I = 1;
                    int end__356 = Moves.Count - 1;
                    for (; I <= end__356; I += 1)
                    {
                        {
                            J = 0;
                            int end__357 = I - 1;
                            for (; J <= end__357; J += 1)
                            {
                                var call__358 = Moves.GetMoveSettings(J);
                                var call__359 = Moves.GetMoveSettings(I);
                                if (call__358.FPath.Count > call__359.FPath.Count)
                                {
                                    Moves.FChildren.Exchange(I, J);
                                }
                            }
                        }
                    }
                }

                {
                    K = 0;
                    int end__360 = Moves.Count - 1;
                    for (; K <= end__360; K += 1)
                    {
                        {
                            var w__361 = Moves.GetMoveSettings(K);
                            if (w__361.FMovePos < (w__361.FPath.Count - 1))
                            {
                                Path.Clear();
                                {
                                    L = w__361.FMovePos;
                                    int end__362 = w__361.FPath.Count - 1;
                                    for (; L <= end__362; L += 1)
                                    {
                                        var call__363 = w__361.FPath[L];
                                        Path.Add(call__363);
                                    }
                                }

                                MoveEvent = Aow.Units.CombatTE.TCombatMoveTE.Create(null);
                                var call__364 = MoveEvent.Setup(((Aow.Units.AoWTC.TTacticalCombatUnitHS)w__361.FMoveObject).FUnit, Path);
                                if (call__364)
                                {
                                    Multi.FEvents.Add(MoveEvent);
                                }

                                MoveEvent.Release();
                            }
                        }
                    }
                }

                var call__365 = Multi.FEvents.Count;
                if (call__365 != 0)
                {
                    var call__366 = Moves.GetMoveSettings(0);
                    var call__367 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__366.FMoveObject).FUnit.GetPlayer();
                    var call__368 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__367);
                    call__368.AddEvent(Multi);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                Multi.Release();
                Path.Free();
                {
                    K = 0;
                    int end__369 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.Count - 1;
                    for (; K <= end__369; K += 1)
                    {
                        {
                            var w__370 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(K);
                            w__370.FPath.Clear();
                        }
                    }
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CombatTE.pas:1978
        public static void RegisterCombatTE(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCombatMoveTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCMeleeMoveTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCAbTouchMoveTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCAbRangedTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCEndTurnTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TCEndCombatTE>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatTE.TTerminateCombatTE>()]);
        }

        // CombatTE.pas:1916
        public static bool ValidateTCMovePosition(Aow.Units.AoWE.TMoveSettingsList Moves, int Index)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TMoveSettings Settings = default;
            Result = true;
            if (Result)
            {
                {
                    I = Moves.Count - 1;
                    byte end__371 = 0;
                    for (; I >= end__371; I -= 1)
                    {
                        if (I != Index)
                        {
                            Settings = Moves.GetMoveSettings(I);
                            bool bool__372 = Settings.FValid;
                            if (bool__372)
                            {
                                var call__373 = Moves.GetMoveSettings(Index);
                                var call__374 = call__373.GetMovePrio();
                                var call__375 = Settings.GetMovePrio();
                                bool__372 = (call__374 >= call__375);
                            }

                            bool bool__376 = bool__372;
                            if (bool__376)
                            {
                                var call__377 = Moves.GetMoveSettings(Index);
                                var call__378 = call__377.GetMoveLoc();
                                var call__379 = Settings.GetMoveLoc();
                                bool__376 = (call__379.XYL == call__378.XYL);
                            }

                            if (bool__376)
                            {
                                Result = false;
                            }
                        }
                    }
                }
            }

            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
