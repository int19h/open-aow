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
            Aow.Units.AoWE.TMoveStatus Result = default;
            Aow.Units.AoWE.TAoWMapField MapField = default;
            int Direction = default;
            int X = default;
            int Y = default;
            void CheckFreeStrike()
            {
                int HN = default;
                int NX = default;
                int NY = default;
                Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
                var call__28 = FUnit.GetEnabled();
                if (call__28 && (FFreeStrikeUnit == null))
                {
                    MapField = (Aow.Units.AoWE.TAoWMapField)FUnit.FUnitHS.Owner;
                    HN = FNeighbour + 1;
                    while (HN <= 6)
                    {
                        var call__29 = FUnit.FUnitHS.Yhx;
                        var call__30 = FUnit.FUnitHS.Xhx;
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__30, call__29, HN, ref NX, ref NY);
                        var call__31 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__32 = call__31.GetField(NX, NY);
                        var call__33 = call__32.FindOwnedChild(2228484);
                        HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__33;
                        bool bool__34 = (HS != null) && (FPreviousUnit != HS.FUnit);
                        if (bool__34)
                        {
                            var call__35 = HS.FUnit.GetEnabled();
                            bool__34 = call__35;
                        }

                        bool bool__36 = bool__34;
                        if (bool__36)
                        {
                            var call__37 = HS.FUnit.GetLocked();
                            bool__36 = !call__37;
                        }

                        bool bool__38 = bool__36;
                        if (bool__38)
                        {
                            var call__39 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(HS.FUnit);
                            bool__38 = (call__39 != TSet<Aow.Units.AoWE.TDamageType>.Of());
                        }

                        bool bool__40 = bool__38;
                        if (bool__40)
                        {
                            var call__41 = HS.FUnit.GetSide();
                            var call__42 = FUnit.GetSide();
                            bool__40 = (call__41 != call__42);
                        }

                        bool bool__43 = bool__40 && (HS.FAction == Aow.Units.AoWTC.TTCAction.caMove);
                        if (bool__43)
                        {
                            var call__44 = HS.FUnit.GetAbilityEnabled(1);
                            var call__45 = FUnit.GetAbilityEnabled(1);
                            bool__43 = (call__44 == call__45);
                        }

                        if (bool__43)
                        {
                            FNeighbour = unchecked((sbyte)HN);
                            FFreeStrikeUnit = HS.FUnit;
                            {
                                NX = 0;
                                int end__46 = FObjects.Count - 1;
                                for (; NX <= end__46; NX += 1)
                                {
                                    var call__47 = FObjects[NX];
                                    if (call__47 == FFreeStrikeUnit)
                                    {
                                        FFreeStrikeUnit = null;
                                    }
                                }
                            }

                            if (FFreeStrikeUnit != null)
                            {
                                HN = 6;
                                FStep = Aow.Units.CombatTE.TTokenStep.tokStart;
                            }
                        }

                        HN = HN + 1;
                    }

                    if (FFreeStrikeUnit == null)
                    {
                        FNeighbour = 0;
                    }
                    else
                    {
                        FObjects.Add(FFreeStrikeUnit);
                    }
                }
            }

            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
            try
            {
                if (FFreeStrikeUnit != null)
                {
                    Result = Aow.Units.AoWE.TMoveStatus.msMoving;
                    switch (FStep)
                    {
                        case Aow.Units.CombatTE.TTokenStep.tokStart:
                            try
                            {
                                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caBusy);
                                FFreeStrikeUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepUp);
                                var call__48 = FFreeStrikeUnit.FUnitHS.Yhx;
                                var call__49 = FUnit.FUnitHS.Yhx;
                                var call__50 = FFreeStrikeUnit.FUnitHS.Xhx;
                                var call__51 = FUnit.FUnitHS.Xhx;
                                var call__52 = FFreeStrikeUnit.FUnitHS.Xhx;
                                var call__53 = Aow.Units.AoWTC.AoWTC.GetMeleeDirIndex(call__52, call__50 - call__51, call__48 - call__49);
                                FFreeStrikeUnit.FUnitHS.SetDirection(unchecked((byte)call__53));
                                FStep = Aow.Units.CombatTE.TTokenStep.tokInitStrike;
                            }
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCMoveToken", (int)0);
                            }

                            break;
                        case Aow.Units.CombatTE.TTokenStep.tokInitStrike:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    var call__54 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                    Aow.Rtl.Rtl.RandSeed = call__54;
                                    var call__55 = Aow.Units.AoWE.AoWE.CreateStrikeCA(FFreeStrikeUnit, FUnit, true);
                                    FStrike = call__55;
                                    if (FStrike.FDamage == 0)
                                    {
                                        FFreeStrikeUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeMiss);
                                    }
                                    else
                                    {
                                        FFreeStrikeUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeHit);
                                    }

                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caBusy);
                                    FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                                }
                            }
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token InitStrike", (AnsiString)"Error - TCMoveToken", (int)0);
                            }

                            break;
                        case Aow.Units.CombatTE.TTokenStep.tokExeStrike:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                        Aow.Units.CombatTE.CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                    }

                                    var call__56 = FUnit.FUnitHS.Yhx;
                                    var call__57 = FUnit.FUnitHS.Xhx;
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__57, call__56, ref X, ref Y);
                                    if ((FStrike.FDamageTypes.Value - FStrike.FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                    {
                                        Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                    {
                                        Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                    }

                                    FStep = Aow.Units.CombatTE.TTokenStep.tokEndStrike;
                                }
                            }
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token ExeStrike", (AnsiString)"Error - TCMoveToken", (int)0);
                            }

                            break;
                        case Aow.Units.CombatTE.TTokenStep.tokEndStrike:
                            try
                            {
                                if (FUnit.FUnitHS.FActionDone || (FStrike.FDamage == 0))
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                    }

                                    FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                                    FFreeStrikeUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
                                }
                            }
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token EndStrike", (AnsiString)"Error - TCMoveToken", (int)0);
                            }

                            break;
                        case Aow.Units.CombatTE.TTokenStep.tokEnd:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    var call__58 = FUnit.GetEnabled();
                                    if (!call__58)
                                    {
                                        Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        var call__59 = FUnit.GetLocked();
                                        if (call__59)
                                        {
                                            FUnit.FUnitHS.CancelMove();
                                            Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                        }
                                        else
                                        {
                                            FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMove);
                                        }
                                    }

                                    if (FStrike != null)
                                    {
                                        FStrike.Release();
                                        FStrike = null;
                                    }

                                    FFreeStrikeUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMove);
                                    FStep = Aow.Units.CombatTE.TTokenStep.tokStart;
                                    FFreeStrikeUnit = null;
                                    CheckFreeStrike();
                                }
                            }
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token End", (AnsiString)"Error - TCMoveToken", (int)0);
                            }

                            break;
                    }
                }
                else
                {
                    var call__60 = FUnit.GetEnabled();
                    if (call__60)
                    {
                        var call__61 = FUnit.FUnitHS.Moving();
                        if (call__61)
                        {
                            var call__62 = FUnit.FUnitHS.MoveNext();
                            bool bool__63 = (call__62 == 100);
                            if (!bool__63)
                            {
                                var call__64 = FUnit.FUnitHS.GetVisible();
                                bool__63 = !call__64;
                            }

                            if (bool__63)
                            {
                                Result = ExecuteDefaultMove();
                                if (Result == Aow.Units.AoWE.TMoveStatus.msMoving)
                                {
                                    CheckFreeStrike();
                                }
                            }
                            else
                            {
                                Result = Aow.Units.AoWE.TMoveStatus.msMoving;
                            }
                        }
                        else
                        {
                            if (FMoveIndex != 0)
                            {
                                bool bool__65 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                                if (!bool__65)
                                {
                                    var call__66 = FUnit.FUnitHS.Yhx;
                                    var call__67 = FUnit.FUnitHS.Xhx;
                                    var call__68 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__67, call__66, 0);
                                    var call__69 = ((Aow.Units.AoWE.TAoWMapField)call__68).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                    bool__65 = call__69;
                                }

                                if (bool__65)
                                {
                                    var call__70 = FUnit.FUnitHS.Yhx;
                                    var call__71 = FUnit.FUnitHS.Xhx;
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__71, call__70, 0, 3);
                                }

                                var call__72 = GetDirection(FMoveIndex);
                                Direction = call__72;
                                var call__73 = FPath[FMoveIndex - 1];
                                var call__74 = GetMapF(call__73);
                                MapField = call__74;
                                if (MapField.GetPlayer() == -1)
                                {
                                    if (Direction != 0)
                                    {
                                        var call__75 = FUnit.FUnitHS.MoveFirst(Direction, false);
                                        if (call__75 == -1)
                                        {
                                            Result = Aow.Units.AoWE.TMoveStatus.msFailed;
                                        }
                                        else
                                        {
                                            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
                                        }

                                        if (Result == Aow.Units.AoWE.TMoveStatus.msMoving)
                                        {
                                            CheckFreeStrike();
                                        }
                                    }
                                    else
                                    {
                                        Result = ExecuteDefaultMove();
                                    }
                                }
                                else
                                {
                                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                }
                            }
                            else
                            {
                                Result = Aow.Units.AoWE.TMoveStatus.msDone;
                            }
                        }
                    }
                }

                if (Result == Aow.Units.AoWE.TMoveStatus.msDone)
                {
                    var call__76 = FUnit.GetEnabled();
                    if (call__76 && (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer))
                    {
                        var call__77 = FUnit.FUnitHS.Xhx;
                        var call__78 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                        call__78.FLastX = call__77;
                        var call__79 = FUnit.FUnitHS.Yhx;
                        var call__80 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                        call__80.FLastY = call__79;
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token", (AnsiString)"Error - TCMoveToken", (int)0);
            }

            return Result;
        }

        // CombatTE.pas:220
        public virtual bool Setup(Aow.Units.AoWTC.TTacticalCombatUnit UnitObject, Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            int StartIndex = default;
            int I = default;
            FPath.Clear();
            var call__81 = Path.MovePointsToPos(UnitObject.FMovePoints);
            StartIndex = call__81;
            if (StartIndex != (Path.Count - 1))
            {
                {
                    I = StartIndex;
                    int end__82 = Path.Count - 1;
                    for (; I <= end__82; I += 1)
                    {
                        var call__83 = Path[I];
                        FPath.Add(call__83);
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
            var call__84 = FPath[FMoveIndex];
            var call__85 = GetMapF(call__84);
            NewField = call__85;
            FUnit.FUnitHS.MoveTo(NewField, 0);
            var call__86 = NewField.FindChildIndex(FUnit.FUnitHS);
            if (call__86 != -1)
            {
                Result = ProcessMove();
                var call__87 = FPath[FMoveIndex];
                FUnit.MovedTo(OldField, NewField, call__87.Flag);
                var call__88 = Aow.Rtl.Rtl.Rect(OldField.Xhx, OldField.Yhx, OldField.Xhx + 1, OldField.Yhx + 1);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(call__88, 0);
                bool bool__89 = (FMoveIndex == 0);
                if (bool__89)
                {
                    var call__90 = NewField.FindChild(2229544);
                    bool__89 = (call__90 != null);
                }

                if (bool__89 && (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FDefenderPlayer))
                {
                    FUnit.SetStatus(TSet<Aow.Units.AoWE.TCombatObjectStatusElement>.Of(Aow.Units.AoWE.TCombatObjectStatusElement.csFled));
                    var call__91 = FUnit.FUnitHS.Yhx;
                    var call__92 = FUnit.FUnitHS.Xhx;
                    var call__93 = FUnit.FUnitHS.Yhx;
                    var call__94 = FUnit.FUnitHS.Xhx;
                    var call__95 = Aow.Rtl.Rtl.Rect(call__94, call__93, call__92 + 1, call__91 + 1);
                    R = call__95;
                    FUnit.FUnitHS.Free();
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                    FUnit.FUnitHS = null;
                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                }
                else
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                    {
                        var call__96 = FUnit.GetPlayer();
                        var call__97 = FUnit.GetVisibilityRange();
                        var call__98 = FUnit.FUnitHS.Yhx;
                        var call__99 = FUnit.FUnitHS.Xhx;
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.ExploreArea(call__99, call__98, 0, unchecked((sbyte)call__96), call__97);
                        var call__100 = FUnit.GetVisibilityRange();
                        var call__101 = FUnit.FUnitHS.Yhx;
                        var call__102 = FUnit.FUnitHS.Xhx;
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapChangedRad(call__102, call__101, 0, call__100 + 1);
                    }
                    else
                    {
                        var call__103 = FUnit.FUnitHS.Yhx;
                        var call__104 = FUnit.FUnitHS.Xhx;
                        var call__105 = FUnit.FUnitHS.Yhx;
                        var call__106 = FUnit.FUnitHS.Xhx;
                        var call__107 = Aow.Rtl.Rtl.Rect(call__106, call__105, call__104 + 1, call__103 + 1);
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(call__107, 0);
                    }

                    var call__108 = FUnit.GetPlayer();
                    var call__109 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__108);
                    bool bool__110 = (call__109.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
                    if (!bool__110)
                    {
                        var call__111 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)FUnit.FCombatPlayer));
                        bool__110 = call__111.FAuto;
                    }

                    if (bool__110)
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
            Aow.Units.AoWE.TMoveStatus Result = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            int X = default;
            int Y = default;
            Aow.Units.AoWE.TMeleeStrike Strike = default;
            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
            try
            {
                switch (FStep)
                {
                    case Aow.Units.CombatTE.TTokenStep.tokStart:
                        try
                        {
                            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
                            FUnit.FUnitHS.CancelMove();
                            bool bool__112 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                            if (!bool__112)
                            {
                                var call__113 = FUnit.FUnitHS.Yhx;
                                var call__114 = FUnit.FUnitHS.Xhx;
                                var call__115 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__114, call__113, 0);
                                var call__116 = ((Aow.Units.AoWE.TAoWMapField)call__115).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                bool__112 = call__116;
                            }

                            if (bool__112)
                            {
                                var call__117 = FUnit.FUnitHS.Yhx;
                                var call__118 = FUnit.FUnitHS.Xhx;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__118, call__117, 0, 5);
                            }

                            var call__119 = FPath[0];
                            var call__120 = FPath[0];
                            var call__121 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__122 = call__121.GetField(call__120.X, call__119.Y);
                            var call__123 = call__122.FindOwnedChild(2228484);
                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__123;
                            if (HS == null)
                            {
                                FPreviousUnit = null;
                            }
                            else
                            {
                                FPreviousUnit = HS.FUnit;
                            }

                            if (FPreviousUnit == null)
                            {
                                Result = Aow.Units.AoWE.TMoveStatus.msDone;
                            }
                            else
                            {
                                var call__124 = Aow.Units.AoWE.TMeleeRound.Create();
                                FMeleeRound = call__124;
                                FMeleeRound.Calculate(FUnit, FPreviousUnit);
                                if (FMeleeRound.FCount == 0)
                                {
                                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                }
                                else
                                {
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepUp);
                                    var call__125 = FUnit.FUnitHS.Yhx;
                                    var call__126 = FPreviousUnit.FUnitHS.Yhx;
                                    var call__127 = FUnit.FUnitHS.Xhx;
                                    var call__128 = FPreviousUnit.FUnitHS.Xhx;
                                    var call__129 = FUnit.FUnitHS.Xhx;
                                    var call__130 = Aow.Units.AoWTC.AoWTC.GetMeleeDirIndex(call__129, call__127 - call__128, call__125 - call__126);
                                    FUnit.FUnitHS.SetDirection(unchecked((byte)call__130));
                                    FPreviousUnit.FUnitHS.SetDirection(unchecked((byte)(FUnit.FUnitHS.FDirection + 3)));
                                    if (FPreviousUnit.FUnitHS.FDirection > 6)
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection(unchecked((byte)(FPreviousUnit.FUnitHS.FDirection - 6)));
                                    }
                                }
                            }

                            FStep = Aow.Units.CombatTE.TTokenStep.tokFirstStrike;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCMelMoveToken", (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokFirstStrike:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            ExecuteStrike();
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokDelayStrike:
                        if (FDelay > 0)
                        {
                            FDelay = FDelay - 1;
                        }
                        else
                        {
                            ExecuteStrike();
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokInitStrike:
                        try
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if ((Strike.Side == Aow.Units.AoWE.TCombatSideIndex.csDefender) && FPreviousUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                    Aow.Units.CombatTE.CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                    var call__131 = FUnit.FUnitHS.Yhx;
                                    var call__132 = FUnit.FUnitHS.Xhx;
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__132, call__131, ref X, ref Y);
                                    if ((FStrike.FDamageTypes.Value - FStrike.FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                    {
                                        Aow.Units.CombatTE.CombatTE.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                    {
                                        Aow.Units.CombatTE.CombatTE.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                    }
                                }

                                FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                            }
                            else
                            {
                                FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                                if ((Strike.Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker) && FUnit.FUnitHS.FActionDone)
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        FPreviousUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                        Aow.Units.CombatTE.CombatTE.MakeHitBlood(FPreviousUnit, FStrike.FDamage);
                                        var call__133 = FPreviousUnit.FUnitHS.Yhx;
                                        var call__134 = FPreviousUnit.FUnitHS.Xhx;
                                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__134, call__133, ref X, ref Y);
                                        if ((FStrike.FDamageTypes.Value - FStrike.FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                        {
                                            Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                        }

                                        if (FStrike.FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                        {
                                            Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                        }
                                    }

                                    FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token InitStrike", (AnsiString)"Error - TCMelMoveToken", (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokExeStrike:
                        try
                        {
                            if (FStrike.FDamage == 0)
                            {
                                NextStrike();
                            }
                            else
                            {
                                FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                                if ((Strike.Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker) && FPreviousUnit.FUnitHS.FActionDone)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                    var call__135 = FPreviousUnit.GetEnabled();
                                    if (!call__135)
                                    {
                                        FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                                        FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
                                    }
                                    else
                                    {
                                        NextStrike();
                                    }
                                }
                                else
                                {
                                    if (FUnit.FUnitHS.FActionDone)
                                    {
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                        var call__136 = FUnit.GetEnabled();
                                        if (call__136)
                                        {
                                            NextStrike();
                                        }
                                        else
                                        {
                                            Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token ExeStrike", (AnsiString)"Error - TCMelMoveToken", (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokEnd:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            Result = Aow.Units.AoWE.TMoveStatus.msDone;
                        }
                        else
                        {
                            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
                        }

                        break;
                }

                if (Result == Aow.Units.AoWE.TMoveStatus.msDone)
                {
                    if (FMeleeRound != null)
                    {
                        FMeleeRound.Free();
                    }

                    if (FStrike != null)
                    {
                        FStrike.Release();
                    }

                    var call__137 = FUnit.GetEnabled();
                    if (call__137)
                    {
                        FUnit.FMovePoints = 0;
                        FUnit.FUnitHS.Unselect();
                        if (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__138 = FUnit.FUnitHS.Xhx;
                            var call__139 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__139.FLastX = call__138;
                            var call__140 = FUnit.FUnitHS.Yhx;
                            var call__141 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__141.FLastY = call__140;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token", (AnsiString)"Error - TCMelMoveToken", (int)0);
            }

            return Result;
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
                    var call__142 = FUnit.GetLocked();
                    Locked = call__142;
                }
                else
                {
                    var call__143 = FPreviousUnit.GetLocked();
                    Locked = call__143;
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

            var call__144 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
            Aow.Rtl.Rtl.RandSeed = call__144;
            var call__145 = FMeleeRound.CreateStrikeCA(FStrikeIndex);
            FStrike = call__145;
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
            Aow.Units.AoWE.TMoveStatus Result = default;
            Aow.Units.aowFX.TXYZ Point = default;
            int I = default;
            int X = default;
            int Y = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Result = Aow.Units.AoWE.TMoveStatus.msMoving;
            try
            {
                switch (FStep)
                {
                    case Aow.Units.CombatTE.TTokenStep.tokStart:
                        try
                        {
                            FEffectType = Aow.Units.AoWTC.AoWTC.AbilTypes[FAbilityID];
                            FUnit.FUnitHS.CancelMove();
                            FUnit.FUnitHS.Unselect();
                            FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                            bool bool__146 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                            if (!bool__146)
                            {
                                var call__147 = FUnit.FUnitHS.Yhx;
                                var call__148 = FUnit.FUnitHS.Xhx;
                                var call__149 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__148, call__147, 0);
                                var call__150 = ((Aow.Units.AoWE.TAoWMapField)call__149).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                bool__146 = call__150;
                            }

                            if (bool__146)
                            {
                                var call__151 = FUnit.FUnitHS.Yhx;
                                var call__152 = FUnit.FUnitHS.Xhx;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__152, call__151, 0, 5);
                            }

                            var call__153 = FUnit.FUnitHS.Yhx;
                            var call__154 = FPath[0];
                            var call__155 = FUnit.FUnitHS.Xhx;
                            var call__156 = FPath[0];
                            var call__157 = FUnit.FUnitHS.Xhx;
                            var call__158 = Aow.Units.AoWTC.AoWTC.GetMeleeDirIndex(call__157, call__155 - call__156.X, call__153 - call__154.Y);
                            FUnit.FUnitHS.SetDirection(unchecked((byte)call__158));
                            if (FAbilityID == 65)
                            {
                                FStep = Aow.Units.CombatTE.TTokenStep.tokInitStrike;
                            }
                            else
                            {
                                if (FAbilityID == 114)
                                {
                                    FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caRoundAttack);
                                }
                                else
                                {
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepUp);
                                    var call__159 = FPath[0];
                                    var call__160 = FPath[0];
                                    var call__161 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__162 = call__161.GetField(call__160.X, call__159.Y);
                                    var call__163 = call__162.FindOwnedChild(2228484);
                                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__163;
                                    if (HS == null)
                                    {
                                        FPreviousUnit = null;
                                    }
                                    else
                                    {
                                        FPreviousUnit = HS.FUnit;
                                    }

                                    if ((FPreviousUnit != null) && (FAbilityID != 117))
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection(unchecked((byte)(FUnit.FUnitHS.FDirection + 3)));
                                        if (FPreviousUnit.FUnitHS.FDirection > 6)
                                        {
                                            FPreviousUnit.FUnitHS.SetDirection(unchecked((byte)(FPreviousUnit.FUnitHS.FDirection - 6)));
                                        }
                                    }
                                    else
                                    {
                                        if (FAbilityID == 117)
                                        {
                                            var call__164 = FPath[0];
                                            var call__165 = FPath[0];
                                            var call__166 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__167 = call__166.GetField(call__165.X, call__164.Y);
                                            var call__168 = call__167.FindOwnedChild(2229508);
                                            FWall = (Aow.Units.CityWall.TCityWall)call__168;
                                            if (FWall == null)
                                            {
                                                var call__169 = FPath[0];
                                                var call__170 = FPath[0];
                                                var call__171 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__172 = call__171.GetField(call__170.X, call__169.Y);
                                                var call__173 = call__172.FindOwnedChild(2229510);
                                                FWall = (Aow.Units.CityWall.TCityWall)call__173;
                                            }

                                            if ((FWall == null) && (FAbilityID == 117))
                                            {
                                                Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                            }
                                        }
                                        else
                                        {
                                            Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                        }
                                    }

                                    FStep = Aow.Units.CombatTE.TTokenStep.tokStepUp;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token Start", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokStepUp:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone)
                            {
                                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caAbility);
                                FStep = Aow.Units.CombatTE.TTokenStep.tokInitStrike;
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token StepUp", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokInitStrike:
                        try
                        {
                            if (FPreviousUnit != null)
                            {
                                if (FUnit.FUnitHS.FActionDone)
                                {
                                    var call__174 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(FEffectType);
                                    FEffect = call__174;
                                    FEffect.FAbilityIndex = FAbilityID;
                                    Point = Aow.Units.aowFX.TXYZ.Create();
                                    var call__175 = FPreviousUnit.FUnitHS.Yhx;
                                    var call__176 = FPreviousUnit.FUnitHS.Xhx;
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__176, call__175, ref Point.X, ref Point.Y);
                                    Point.X += 16;
                                    FEffect.FPath.Add(Point);
                                    FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                                }
                            }
                            else
                            {
                                if ((FWall != null) || (FAbilityID == 65))
                                {
                                    switch (FAbilityID)
                                    {
                                        case 117:
                                            FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                                            FDamage = 0;
                                            var call__177 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                            Aow.Rtl.Rtl.RandSeed = call__177;
                                            var call__178 = FWall.GetDefense();
                                            var call__179 = Aow.Units.AoWE.AoWE.ExecuteDamageRole(Aow.Units.AoWTC.AoWTC.TCDamage[0], Aow.Units.AoWTC.AoWTC.TCAttack[0] - call__178);
                                            FWall.SetHitPoints(FWall.FHitPoints - call__179);
                                            FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
                                            break;
                                        case 65:
                                            var call__180 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(FEffectType);
                                            FEffect = call__180;
                                            FEffect.FAbilityIndex = FAbilityID;
                                            Point = Aow.Units.aowFX.TXYZ.Create();
                                            var call__181 = FPath[1];
                                            var call__182 = FPath[1];
                                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__182.X, call__181.Y, ref Point.X, ref Point.Y);
                                            Point.X += 16;
                                            FEffect.FPath.Add(Point);
                                            FStep = Aow.Units.CombatTE.TTokenStep.tokExeStrike;
                                            break;
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token InitStrike", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokExeStrike:
                        try
                        {
                            if (FEffect == null)
                            {
                                var call__183 = FUnit.GetEnabled();
                                if (!call__183)
                                {
                                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                }
                                else
                                {
                                    switch (FAbilityID)
                                    {
                                        case 114:
                                            if (FUnit.FUnitHS.FActionDone || (FUnit.FUnitHS.FActionFrame >= 17))
                                            {
                                                Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                            }
                                            else
                                            {
                                                if (Aow.Units.AoWTC.AoWTC.raDir[FUnit.FUnitHS.FActionFrame] > 0)
                                                {
                                                    I = ((Aow.Units.AoWTC.AoWTC.raDir[FUnit.FUnitHS.FActionFrame] + FUnit.FUnitHS.FDirection) % 6) + 1;
                                                    var call__184 = FPath[1];
                                                    var call__185 = FPath[1];
                                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__185.X, call__184.Y, I, ref X, ref Y);
                                                    var call__186 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                    var call__187 = call__186.GetField(X, Y);
                                                    var call__188 = call__187.FindOwnedChild(2228484);
                                                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__188;
                                                    if ((HS == null) || (HS == FUnit.FUnitHS))
                                                    {
                                                        FPreviousUnit = null;
                                                    }
                                                    else
                                                    {
                                                        var call__189 = HS.FUnit.GetAbilityEnabled(1);
                                                        if (call__189)
                                                        {
                                                            FPreviousUnit = null;
                                                        }
                                                        else
                                                        {
                                                            var call__190 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                            Aow.Rtl.Rtl.RandSeed = call__190;
                                                            var call__191 = Aow.Units.AoWE.AoWE.CreateStrikeCA(FUnit, HS.FUnit, true);
                                                            FStrike = call__191;
                                                            if (FStrike.FDamage > 0)
                                                            {
                                                                HS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caRoundAttackHit);
                                                                Aow.Units.CombatTE.CombatTE.MakeHitBlood(HS.FUnit, FStrike.FDamage);
                                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                                            }
                                                            else
                                                            {
                                                                FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caRoundAttack);
                                                            }

                                                            FStrike.Release();
                                                            FStrike = null;
                                                        }
                                                    }
                                                }
                                            }

                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (FEffect.FDone)
                                {
                                    var call__192 = FUnit.GetEnabled();
                                    if (!call__192)
                                    {
                                        Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        switch (FAbilityID)
                                        {
                                            case 40:
                                                var call__193 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__193;
                                                var call__194 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__195 = ((Aow.Units.AoWE.TEntangleAbility)call__194).CreateEntangleCA(FUnit, FPreviousUnit);
                                                FEntangleAction = call__195;
                                                FSuccessful = FEntangleAction.FSuccessful;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FEntangleAction);
                                                FEntangleAction.Release();
                                                break;
                                            case 27:
                                                var call__196 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__196;
                                                var call__197 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__198 = ((Aow.Units.AoWE.TWebAbility)call__197).CreateWebCA(FUnit, FPreviousUnit);
                                                FWebAction = call__198;
                                                FSuccessful = FWebAction.FSuccessful;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FWebAction);
                                                FWebAction.Release();
                                                break;
                                            case 28:
                                            case 29:
                                            case 148:
                                                var call__199 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__199;
                                                var call__200 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__201 = ((Aow.Units.AoWE.TCommandAbility)call__200).CreateCommandCA(FUnit, FPreviousUnit);
                                                FCommandAction = call__201;
                                                FSuccessful = FCommandAction.FSuccessful;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FCommandAction);
                                                FCommandAction.Release();
                                                break;
                                            case 38:
                                                var call__202 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__202;
                                                var call__203 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__204 = ((Aow.Units.AoWE.TTurnUndeadAbility)call__203).CreateTurnUndeadCA(FUnit, FPreviousUnit);
                                                FTurnUndeadAction = call__204;
                                                var call__205 = FTurnUndeadAction.GetSuccessfull();
                                                FSuccessful = call__205;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FTurnUndeadAction);
                                                FTurnUndeadAction.Release();
                                                break;
                                            case 35:
                                                var call__206 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__206;
                                                var call__207 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__208 = ((Aow.Units.AoWE.TInvokeDeathAbility)call__207).CreateCA(FUnit, FPreviousUnit);
                                                FDeathAction = call__208;
                                                FSuccessful = FDeathAction.FSuccessful;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FDeathAction);
                                                FDeathAction.Release();
                                                break;
                                            case 47:
                                                var call__209 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__209;
                                                var call__210 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__211 = ((Aow.Units.AoWE.THealingAbility)call__210).CreateHealingCA(FUnit, FPreviousUnit);
                                                FHealingAction = call__211;
                                                FSuccessful = true;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FHealingAction);
                                                FHealingAction.Release();
                                                break;
                                            case 60:
                                                var call__212 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__212;
                                                var call__213 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__214 = ((Aow.Units.AoWE.TDispelMagicAbility)call__213).CreateCA(FUnit, FPreviousUnit);
                                                FDispelAction = call__214;
                                                FSuccessful = FDispelAction.FEnchantmentIDs.Count > 0;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FDispelAction);
                                                FDispelAction.Release();
                                                break;
                                            case 106:
                                            case 107:
                                                var call__215 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__215;
                                                var call__216 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__217 = ((Aow.Units.AoWE.TPossessAbility)call__216).CreatePossessCA(FUnit, FPreviousUnit);
                                                FPossessAction = call__217;
                                                FSuccessful = FPossessAction.FSuccessful;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FPossessAction);
                                                FPossessAction.Release();
                                                var call__218 = FUnit.FUnit.GetUnitGFXResourceIndex();
                                                FUnit.FUnitHS.SetUnitGFXResourceIndex(call__218);
                                                var call__219 = FPreviousUnit.GetEnabled();
                                                if (!call__219)
                                                {
                                                    FUnit.FUnitHS.MoveTo((Aow.Units.HSEngine.TMapField)FPreviousUnit.FUnitHS.Owner, 0);
                                                    FUnit.FUnitHS.SetDirection(unchecked((byte)FPreviousUnit.FUnitHS.FDirection));
                                                    FUnit.FUnitHS.FOffsetX = 0;
                                                    FUnit.FUnitHS.FOffsetY = 0;
                                                    FPreviousUnit.FUnitHS.Free();
                                                    FPreviousUnit.FUnitHS = null;
                                                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                                }

                                                break;
                                        }
                                    }

                                    var call__220 = FUnit.GetEnabled();
                                    if (!call__220 || (((FAbilityID == 106) || (FAbilityID == 107)) && FSuccessful))
                                    {
                                        Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        bool bool__221 = !FSuccessful && (FPreviousUnit != null);
                                        if (bool__221)
                                        {
                                            var call__222 = FPreviousUnit.GetEnabled();
                                            bool__221 = call__222;
                                        }

                                        bool bool__223 = bool__221;
                                        if (bool__223)
                                        {
                                            var call__224 = FPreviousUnit.GetLocked();
                                            bool__223 = !call__224;
                                        }

                                        bool bool__225 = bool__223;
                                        if (bool__225)
                                        {
                                            var call__226 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(FPreviousUnit);
                                            bool__225 = (call__226 != TSet<Aow.Units.AoWE.TDamageType>.Of());
                                        }

                                        bool bool__227 = bool__225;
                                        if (bool__227)
                                        {
                                            var call__228 = FPreviousUnit.GetSide();
                                            var call__229 = FUnit.GetSide();
                                            bool__227 = (call__228 != call__229);
                                        }

                                        if (bool__227)
                                        {
                                            var call__230 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                            Aow.Rtl.Rtl.RandSeed = call__230;
                                            var call__231 = Aow.Units.AoWE.AoWE.CreateStrikeCA(FPreviousUnit, FUnit, true);
                                            FStrike = call__231;
                                            if (FStrike.FDamage == 0)
                                            {
                                                FPreviousUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeMiss);
                                            }
                                            else
                                            {
                                                FPreviousUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMeleeHit);
                                            }

                                            FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caBusy);
                                            FStep = Aow.Units.CombatTE.TTokenStep.tokEndStrike;
                                        }
                                        else
                                        {
                                            FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                                            FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
                                        }
                                    }
                                }
                                else
                                {
                                    var call__232 = FUnit.GetEnabled();
                                    if (call__232)
                                    {
                                        switch (FAbilityID)
                                        {
                                            case 65:
                                                if (FEffect.FFrame == 5)
                                                {
                                                    Msg.MsgID = 200704;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        byte end__233 = 6;
                                                        for (; I <= end__233; I += 1)
                                                        {
                                                            var call__234 = FPath[1];
                                                            var call__235 = FPath[1];
                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__235.X, call__234.Y, I, ref X, ref Y);
                                                            var call__236 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__237 = call__236.GetField(X, Y);
                                                            call__237.SendMapFieldMsg(ref Msg);
                                                            Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, X, Y);
                                                        }
                                                    }

                                                    FUnit.FDeathMethod = Aow.Units.AoWTC.TTCDeathMethod.dmExplode;
                                                }

                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token ExeStrike", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokEndStrike:
                        try
                        {
                            if (FPreviousUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                    Aow.Units.CombatTE.CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                }

                                var call__238 = FUnit.FUnitHS.Yhx;
                                var call__239 = FUnit.FUnitHS.Xhx;
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__239, call__238, ref I, ref X);
                                if ((FStrike.FDamageTypes.Value - FStrike.FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                {
                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                {
                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                }

                                FStep = Aow.Units.CombatTE.TTokenStep.tokExeEndStrike;
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token StrikeBack", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokExeEndStrike:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone || (FStrike.FDamage == 0))
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                }

                                var call__240 = FUnit.GetEnabled();
                                if (call__240)
                                {
                                    FStep = Aow.Units.CombatTE.TTokenStep.tokEnd;
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caStepBack);
                                }
                                else
                                {
                                    Result = Aow.Units.AoWE.TMoveStatus.msDone;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token ExeStrikeBack", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
                        }

                        break;
                    case Aow.Units.CombatTE.TTokenStep.tokEnd:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            Result = Aow.Units.AoWE.TMoveStatus.msDone;
                        }

                        break;
                }

                if (Result == Aow.Units.AoWE.TMoveStatus.msDone)
                {
                    var call__241 = FUnit.GetEnabled();
                    if (call__241)
                    {
                        FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMove);
                        FUnit.FUnitHS.FSelectedAbility = -1;
                        FUnit.FMovePoints = 0;
                        FUnit.FUnitHS.Unselect();
                        if (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__242 = FUnit.FUnitHS.Xhx;
                            var call__243 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__243.FLastX = call__242;
                            var call__244 = FUnit.FUnitHS.Yhx;
                            var call__245 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__245.FLastY = call__244;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token", ((AnsiString)"Error - TCTouchToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID), (int)0);
            }

            return Result;
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
            Aow.Units.NetworkE.TExecuteResult Result = default;
            int I = default;
            int X = default;
            int Y = default;
            int EffectIndex = default;
            Aow.Units.AoWTC.TfxHS Effect = default;
            Aow.Units.AoWTC.TfxHS ExtraEffect = default;
            int EndX = default;
            int EndY = default;
            Aow.Units.AoWE.TSpellCombatInfo Info = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            Aow.Units.AoWTC.TCOozeHS Ooze = default;
            Aow.Units.AoWE.TCombatAction Action = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            base.Execute(TE);
            Result = Aow.Units.NetworkE.TExecuteResult.erRetry;
            try
            {
                switch (FState)
                {
                    case 0:
                        try
                        {
                            var call__246 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__247 = call__246.GetField(FSourceX, FSourceY);
                            var call__248 = call__247.FindOwnedChild(2228484);
                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__248;
                            if (HS == null)
                            {
                                Result = Aow.Units.NetworkE.TExecuteResult.erDone;
                            }
                            else
                            {
                                FUnit = HS.FUnit;
                                FUnit.FUnitHS.Unselect();
                                FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                                FUnit.FUnitHS.FSelectedSpell = FSpellID;
                                if (FAbilityID == 52)
                                {
                                    FEffectType = Aow.Units.AoWTC.AoWTC.SpellTypes[FSpellID];
                                }
                                else
                                {
                                    FEffectType = Aow.Units.AoWTC.AoWTC.AbilTypes[FAbilityID];
                                }

                                if (FAbilityID == 52)
                                {
                                    var call__249 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                    call__249.CombatSpellCast(FUnit);
                                }

                                bool bool__250 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                                if (!bool__250)
                                {
                                    var call__251 = FUnit.FUnitHS.Yhx;
                                    var call__252 = FUnit.FUnitHS.Xhx;
                                    var call__253 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__252, call__251, 0);
                                    var call__254 = ((Aow.Units.AoWE.TAoWMapField)call__253).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                    bool__250 = call__254;
                                }

                                if (bool__250)
                                {
                                    var call__255 = FUnit.FUnitHS.Yhx;
                                    var call__256 = FUnit.FUnitHS.Xhx;
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__256, call__255, 0, 2);
                                }

                                switch (FEffectType)
                                {
                                    case Aow.Units.AoWTC.TfxType.fxBreath:
                                        FStrikeCount = 20;
                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                                        FStrikeCount = 10;
                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxScreenNS:
                                        switch (FSpellID)
                                        {
                                            case 116:
                                                FStrikeCount = 21;
                                                break;
                                            case 124:
                                                FStrikeCount = 19;
                                                break;
                                            default:
                                                FStrikeCount = 35;
                                                break;
                                        }

                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxShockWave:
                                        FTargetX = unchecked((ushort)FSourceX);
                                        FTargetY = unchecked((ushort)FSourceY);
                                        FStrikeCount = 1;
                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxTouch:
                                        FStrikeCount = 1;
                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxSwarm:
                                    case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                                        FStrikeCount = 30;
                                        break;
                                    default:
                                        if (FAbilityID == 52)
                                        {
                                            var call__257 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                            call__257.GetCombatInfo(FUnit.FUnit, ref Info);
                                            FStrikeCount = Info.RepeatCount;
                                        }
                                        else
                                        {
                                            var call__258 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                            var call__259 = ((Aow.Units.AoWE.TRangedAttackAbility)call__258).GetAttackRepeatRA(FUnit.FUnit);
                                            FStrikeCount = call__259;
                                        }

                                        break;
                                }

                                switch (FEffectType)
                                {
                                    case Aow.Units.AoWTC.TfxType.fxTremors:
                                        var call__260 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                        var call__261 = call__260.FCastSounds.PlayEx(0, 64, true, 0, Aow.Units.Sound.TSamplePanning.spCenter, null);
                                        FSound = call__261;
                                        NextStrike();
                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                                        var call__262 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                        var call__263 = call__262.FCastSounds.PlayEx(2, 64, true, 0, Aow.Units.Sound.TSamplePanning.spCenter, null);
                                        FSound = call__263;
                                    {
                                        I = 0;
                                        int end__264 = FStrikeCount - 1;
                                        for (; I <= end__264; I += 1)
                                        {
                                            NextStrike();
                                        }
                                    }

                                        break;
                                    default:
                                        NextStrike();
                                        break;
                                }

                                FUnit.FMovePoints = 0;
                                FState += 1;
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token Start", ((((AnsiString)"Error - TCRangedToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(FSpellID), (int)0);
                        }

                        break;
                    case 1:
                        try
                        {
                            EffectIndex = 0;
                            while (EffectIndex < Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects.Count)
                            {
                                EndX = -1;
                                EndY = -1;
                                var call__265 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects[EffectIndex];
                                Effect = (Aow.Units.AoWTC.TfxHS)call__265;
                                if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 117))
                                {
                                    if (Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxRock)
                                    {
                                        if (Effect.FDone)
                                        {
                                            Effect.FEffectType = Aow.Units.AoWTC.TfxType.fxFBExplosion;
                                            Effect.FDone = false;
                                            while (true)
                                            {
                                                var call__266 = Effect.FPath.Count;
                                                if (!(call__266 > 1))
                                                    break;
                                                {
                                                    Effect.FPath.Delete(0);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        switch (Effect.FFrame)
                                        {
                                            case 5:
                                                Msg.MsgID = 200707;
                                                Msg.Param[0] = FUnit;
                                                Msg.Result = -1;
                                                var call__267 = Effect.FPath[0];
                                                var call__268 = Effect.FPath[0];
                                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__268.X, call__267.Y, ref EndX, ref EndY);
                                            {
                                                I = 0;
                                                byte end__269 = 6;
                                                for (; I <= end__269; I += 1)
                                                {
                                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(EndX, EndY, I, ref X, ref Y);
                                                    var call__270 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                    var call__271 = call__270.GetField(X, Y);
                                                    call__271.SendMapFieldMsg(ref Msg);
                                                    Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, X, Y);
                                                }
                                            }

                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 107))
                                    {
                                        bool bool__272 = (Effect.FFrame != -1);
                                        if (bool__272)
                                        {
                                            var call__273 = Effect.FPath[Effect.FFrame];
                                            bool__272 = (call__273.Z == 15);
                                        }

                                        if (bool__272)
                                        {
                                            var call__274 = Effect.FPath[Effect.FFrame];
                                            var call__275 = Effect.FPath[Effect.FFrame];
                                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__275.X, call__274.Y, ref X, ref Y);
                                            var call__276 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__277 = call__276.GetField(X, Y);
                                            var call__278 = call__277.FindOwnedChild(2228484);
                                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__278;
                                            if (HS != null)
                                            {
                                                var call__279 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__279;
                                                var call__280 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Effect.FSpellIndex);
                                                var call__281 = call__280.CreateCA(FUnit, HS.FUnit, false);
                                                Effect.FCombatAction = call__281;
                                                var call__282 = HS.Yhx;
                                                var call__283 = HS.Xhx;
                                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__283, call__282, ref I, ref X);
                                                if ((((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                                {
                                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(I, X, ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                }

                                                if (((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                                {
                                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(I, X, ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                }

                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                Effect.FCombatAction.Release();
                                                Effect.FCombatAction = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 112))
                                        {
                                            switch (Effect.FFrame)
                                            {
                                                case 89:
                                                    Msg.MsgID = 200708;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Param[1] = 0;
                                                    Msg.Result = -1;
                                                {
                                                    I = 6;
                                                    byte end__284 = 59;
                                                    for (; I <= end__284; I += 1)
                                                    {
                                                        {
                                                            X = 6;
                                                            byte end__285 = 52;
                                                            for (; X <= end__285; X += 1)
                                                            {
                                                                var call__286 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                var call__287 = call__286.GetField(I, X);
                                                                call__287.SendMapFieldMsg(ref Msg);
                                                            }
                                                        }
                                                    }
                                                }

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (((Effect.FAbilityIndex == 52) && (Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxResurrect)) && ((Effect.FSpellIndex == 130) || (Effect.FSpellIndex == 129)))
                                            {
                                                switch (Effect.FFrame)
                                                {
                                                    case 0:
                                                        Msg.MsgID = 200712;
                                                        Msg.Param[0] = FUnit;
                                                        Msg.Param[1] = FSpellID;
                                                        Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        byte end__288 = 6;
                                                        for (; I <= end__288; I += 1)
                                                        {
                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            var call__289 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__290 = call__289.GetField(X, Y);
                                                            call__290.SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                        break;
                                                    case 99:
                                                        Msg.MsgID = 200713;
                                                        Msg.Param[0] = FUnit;
                                                        Msg.Param[1] = FSpellID;
                                                        Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        byte end__291 = 6;
                                                        for (; I <= end__291; I += 1)
                                                        {
                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            var call__292 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__293 = call__292.GetField(X, Y);
                                                            call__293.SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 104))
                                                {
                                                    switch (Effect.FFrame)
                                                    {
                                                        case 199:
                                                            Msg.MsgID = 200709;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                        {
                                                            I = 0;
                                                            byte end__294 = 6;
                                                            for (; I <= end__294; I += 1)
                                                            {
                                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                var call__295 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                var call__296 = call__295.GetField(X, Y);
                                                                call__296.SendMapFieldMsg(ref Msg);
                                                            }
                                                        }

                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 127))
                                                    {
                                                        switch (Effect.FFrame)
                                                        {
                                                            case 175:
                                                                Msg.MsgID = 200717;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                            {
                                                                I = 1;
                                                                byte end__297 = 6;
                                                                for (; I <= end__297; I += 1)
                                                                {
                                                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__298 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__299 = call__298.GetField(X, Y);
                                                                    call__299.SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                                break;
                                                            case 200:
                                                                Msg.MsgID = 200717;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                            {
                                                                I = 7;
                                                                byte end__300 = 18;
                                                                for (; I <= end__300; I += 1)
                                                                {
                                                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__301 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__302 = call__301.GetField(X, Y);
                                                                    call__302.SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                                break;
                                                            case 225:
                                                                Msg.MsgID = 200717;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                            {
                                                                I = 19;
                                                                byte end__303 = 36;
                                                                for (; I <= end__303; I += 1)
                                                                {
                                                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__304 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__305 = call__304.GetField(X, Y);
                                                                    call__305.SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 128))
                                                        {
                                                            switch (Effect.FFrame)
                                                            {
                                                                case 100:
                                                                    var call__306 = FUnit.FUnitHS.Yhx;
                                                                    var call__307 = FUnit.FUnitHS.Xhx;
                                                                    Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, call__307, call__306);
                                                                    Msg.MsgID = 200718;
                                                                    Msg.Param[0] = FUnit;
                                                                    Msg.Result = -1;
                                                                    var call__308 = FUnit.FUnitHS.Yhx;
                                                                    var call__309 = FUnit.FUnitHS.Xhx;
                                                                    var call__310 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__311 = call__310.GetField(call__309, call__308);
                                                                    call__311.SendMapFieldMsg(ref Msg);
                                                                    break;
                                                                case 175:
                                                                    Msg.MsgID = 200718;
                                                                    Msg.Param[0] = FUnit;
                                                                    Msg.Result = -1;
                                                                {
                                                                    I = 0;
                                                                    byte end__312 = 18;
                                                                    for (; I <= end__312; I += 1)
                                                                    {
                                                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                        var call__313 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                        var call__314 = call__313.GetField(X, Y);
                                                                        call__314.SendMapFieldMsg(ref Msg);
                                                                        Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, X, Y);
                                                                    }
                                                                }

                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 105)) && (Effect.FTarget != null))
                                                            {
                                                                {
                                                                    I = 0;
                                                                    int end__315 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                                    for (; I <= end__315; I += 1)
                                                                    {
                                                                        var call__316 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                        bool bool__317 = (call__316 != null);
                                                                        if (bool__317)
                                                                        {
                                                                            var call__318 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            bool__317 = (call__318 is Aow.Units.AoWE.TCombatUnit);
                                                                        }

                                                                        bool bool__319 = bool__317;
                                                                        if (bool__319)
                                                                        {
                                                                            var call__320 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__321 = call__320.GetSide();
                                                                            var call__322 = FUnit.GetSide();
                                                                            bool__319 = (call__321 != call__322);
                                                                        }

                                                                        bool bool__323 = bool__319;
                                                                        if (bool__323)
                                                                        {
                                                                            var call__324 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__325 = call__324.GetEnabled();
                                                                            bool__323 = call__325;
                                                                        }

                                                                        bool bool__326 = bool__323;
                                                                        if (bool__326)
                                                                        {
                                                                            var call__327 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            bool__326 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__327).FUnitHS != null);
                                                                        }

                                                                        if (bool__326)
                                                                        {
                                                                            var call__328 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                            Aow.Rtl.Rtl.RandSeed = call__328;
                                                                            var call__329 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__330 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                                                            var call__331 = call__330.CreateCA(FUnit, call__329, false);
                                                                            Action = call__331;
                                                                            if (((Aow.Units.CombatSpells.TExclusiveCombatSpellCA)Action).FSucceeded)
                                                                            {
                                                                                var call__332 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)call__332).FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                            }

                                                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(Action);
                                                                            Action.Release();
                                                                        }
                                                                    }
                                                                }

                                                                Effect.FTarget = null;
                                                            }
                                                            else
                                                            {
                                                                if ((Effect.FAbilityIndex == 52) && (Effect.FSpellIndex == 120))
                                                                {
                                                                    switch (Effect.FFrame)
                                                                    {
                                                                        case 2999:
                                                                            Msg.MsgID = 200716;
                                                                            Msg.Param[0] = FUnit;
                                                                            Msg.Result = -1;
                                                                        {
                                                                            I = 0;
                                                                            byte end__333 = 6;
                                                                            for (; I <= end__333; I += 1)
                                                                            {
                                                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                                var call__334 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                var call__335 = call__334.GetField(X, Y);
                                                                                call__335.SendMapFieldMsg(ref Msg);
                                                                            }
                                                                        }

                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (((Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxSacredWrath) && (Effect.FAbilityIndex == 52)) && (Effect.FSpellIndex == 125))
                                                                    {
                                                                        switch (Effect.FFrame)
                                                                        {
                                                                            case 5:
                                                                            {
                                                                                I = 0;
                                                                                int end__336 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                                                for (; I <= end__336; I += 1)
                                                                                {
                                                                                    var call__337 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                    bool bool__338 = (call__337 is Aow.Units.AoWE.TCombatUnit);
                                                                                    if (bool__338)
                                                                                    {
                                                                                        var call__339 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                        var call__340 = call__339.GetEnabled();
                                                                                        bool__338 = call__340;
                                                                                    }

                                                                                    if (bool__338)
                                                                                    {
                                                                                        var call__341 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                        HS = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__341).FUnitHS;
                                                                                        if (HS != null)
                                                                                        {
                                                                                            var call__342 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxSWComet);
                                                                                            ExtraEffect = call__342;
                                                                                            ExtraEffect.FAbilityIndex = FAbilityID;
                                                                                            ExtraEffect.FSpellIndex = FSpellID;
                                                                                            ExtraEffect.FTarget = HS.FUnit;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                                break;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        bool bool__343 = Effect.FDone && (Effect.FTarget != null);
                                                                        if (bool__343)
                                                                        {
                                                                            var call__344 = Effect.FTarget.GetEnabled();
                                                                            bool__343 = call__344;
                                                                        }

                                                                        if (bool__343)
                                                                        {
                                                                            I = 0;
                                                                            var call__345 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                            Aow.Rtl.Rtl.RandSeed = call__345;
                                                                            if (FAbilityID == 52)
                                                                            {
                                                                                var call__346 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                                                                var call__347 = call__346.CreateCA(FUnit, Effect.FTarget, false);
                                                                                Effect.FCombatAction = call__347;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FAbilityID == 47)
                                                                                {
                                                                                    var call__348 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                    var call__349 = ((Aow.Units.AoWE.THealingAbility)call__348).CreateHealingCA(FUnit, Effect.FTarget);
                                                                                    Effect.FCombatAction = call__349;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (FAbilityID == 60)
                                                                                    {
                                                                                        var call__350 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                        var call__351 = ((Aow.Units.AoWE.TDispelMagicAbility)call__350).CreateCA(FUnit, Effect.FTarget);
                                                                                        Effect.FCombatAction = call__351;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        var call__352 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                        var call__353 = ((Aow.Units.AoWE.TRangedAttackAbility)call__352).CreateRangedAttackCA(FUnit, Effect.FTarget, 0);
                                                                                        Effect.FCombatAction = call__353;
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (Effect.FCombatAction is Aow.Units.AoWE.TDamageCA)
                                                                            {
                                                                                I = ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FDamage;
                                                                                var call__354 = ((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget).FUnitHS.Yhx;
                                                                                var call__355 = ((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget).FUnitHS.Xhx;
                                                                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__355, call__354, ref X, ref Y);
                                                                                if ((((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value) != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                                                                {
                                                                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                                                }

                                                                                if (((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<Aow.Units.AoWE.TDamageType>.Of())
                                                                                {
                                                                                    Aow.Units.CombatTE.CombatTE.MakeDEHSList(X, Y, ((Aow.Units.AoWE.TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Effect.FCombatAction is Aow.Units.CombatSpells.TEntangleSpellCA)
                                                                                {
                                                                                    if (((Aow.Units.CombatSpells.TEntangleSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                    {
                                                                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if ((Effect.FCombatAction is Aow.Units.CombatSpells.TMindDecayCA) && ((Aow.Units.CombatSpells.TMindDecayCA)Effect.FCombatAction).FSucceeded)
                                                                                    {
                                                                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (I > 0)
                                                                            {
                                                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                                Aow.Units.CombatTE.CombatTE.MakeHitBlood((Aow.Units.AoWTC.TTacticalCombatUnit)Effect.FTarget, I);
                                                                                Effect.FCancelled = true;
                                                                            }
                                                                        }

                                                                        if (Effect.FDone)
                                                                        {
                                                                            var call__356 = Effect.FPath.Count;
                                                                            var call__357 = Effect.FPath[call__356 - 1];
                                                                            var call__358 = Effect.FPath.Count;
                                                                            var call__359 = Effect.FPath[call__358 - 1];
                                                                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__359.X, call__357.Y, ref EndX, ref EndY);
                                                                            if ((Effect.FCombatAction != null) && (Effect.FTarget != null))
                                                                            {
                                                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                                                Effect.FCombatAction.Release();
                                                                                Effect.FCombatAction = null;
                                                                                var call__360 = Effect.FTarget.GetEnabled();
                                                                                if (!call__360)
                                                                                {
                                                                                    {
                                                                                        I = 0;
                                                                                        int end__361 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects.Count - 1;
                                                                                        for (; I <= end__361; I += 1)
                                                                                        {
                                                                                            if (I != EffectIndex)
                                                                                            {
                                                                                                var call__362 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects[I];
                                                                                                if (((Aow.Units.AoWTC.TfxHS)call__362).FTarget == Effect.FTarget)
                                                                                                {
                                                                                                    var call__363 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects[I];
                                                                                                    ((Aow.Units.AoWTC.TfxHS)call__363).FTarget = null;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (EndX > -1)
                                                                                {
                                                                                    switch (Effect.FAbilityIndex)
                                                                                    {
                                                                                        case 30:
                                                                                        case >= 86 and <= 90:
                                                                                            if (Effect.FTargetX > 0)
                                                                                            {
                                                                                                Msg.MsgID = 200722;
                                                                                                Msg.Param[0] = FUnit;
                                                                                                Msg.Param[1] = Effect.FAbilityIndex;
                                                                                                Msg.Result = -1;
                                                                                                var call__364 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                var call__365 = call__364.GetField(Effect.FTargetX, Effect.FTargetY);
                                                                                                call__365.SendMapFieldMsg(ref Msg);
                                                                                            }

                                                                                            break;
                                                                                        case 24:
                                                                                            Msg.MsgID = 200705;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__366 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__367 = call__366.GetField(EndX, EndY);
                                                                                            call__367.SendMapFieldMsg(ref Msg);
                                                                                            break;
                                                                                        case 44:
                                                                                            Msg.MsgID = 200706;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__368 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__369 = call__368.GetField(EndX, EndY);
                                                                                            call__369.SendMapFieldMsg(ref Msg);
                                                                                            break;
                                                                                        case 31:
                                                                                            Msg.MsgID = 200714;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__370 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__371 = call__370.GetField(EndX, EndY);
                                                                                            call__371.SendMapFieldMsg(ref Msg);
                                                                                            Aow.Units.CombatFire.CombatFire.CreateFire(false, 100, EndX, EndY);
                                                                                            break;
                                                                                        case 52:
                                                                                            switch (Effect.FEffectType)
                                                                                            {
                                                                                                case Aow.Units.AoWTC.TfxType.fxBreath:
                                                                                                    if (Effect.FTargetX > 0)
                                                                                                    {
                                                                                                        Msg.MsgID = 200723;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Param[1] = Effect.FSpellIndex;
                                                                                                        Msg.Result = -1;
                                                                                                        var call__372 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                        var call__373 = call__372.GetField(Effect.FTargetX, Effect.FTargetY);
                                                                                                        call__373.SendMapFieldMsg(ref Msg);
                                                                                                    }

                                                                                                    break;
                                                                                                case Aow.Units.AoWTC.TfxType.fxScreenNS:
                                                                                                    switch (Effect.FSpellIndex)
                                                                                                    {
                                                                                                        case 116:
                                                                                                            Msg.MsgID = 200710;
                                                                                                            Msg.Param[0] = FUnit;
                                                                                                            Msg.Result = -1;
                                                                                                            var call__374 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__375 = call__374.GetField(EndX, EndY);
                                                                                                            call__375.SendMapFieldMsg(ref Msg);
                                                                                                            break;
                                                                                                        case 124:
                                                                                                            var call__376 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__377 = call__376.GetField(EndX, EndY);
                                                                                                            bool bool__378 = (call__377.TerrainType != 0);
                                                                                                            if (bool__378)
                                                                                                            {
                                                                                                                var call__379 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__380 = call__379.GetField(EndX, EndY);
                                                                                                                bool__378 = (call__380.TerrainType != 10);
                                                                                                            }

                                                                                                            bool bool__381 = bool__378;
                                                                                                            if (bool__381)
                                                                                                            {
                                                                                                                var call__382 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__383 = call__382.GetField(EndX, EndY);
                                                                                                                bool__381 = (call__383.OverlayType != 2);
                                                                                                            }

                                                                                                            bool bool__384 = bool__381;
                                                                                                            if (bool__384)
                                                                                                            {
                                                                                                                var call__385 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__386 = call__385.GetField(EndX, EndY);
                                                                                                                bool__384 = (call__386.OverlayType != 11);
                                                                                                            }

                                                                                                            bool bool__387 = bool__384;
                                                                                                            if (bool__387)
                                                                                                            {
                                                                                                                var call__388 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__389 = call__388.GetField(EndX, EndY);
                                                                                                                bool__387 = (call__389.OverlayType != 8);
                                                                                                            }

                                                                                                            bool bool__390 = bool__387;
                                                                                                            if (bool__390)
                                                                                                            {
                                                                                                                var call__391 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__392 = call__391.GetField(EndX, EndY);
                                                                                                                bool__390 = (call__392.TerrainType != 9);
                                                                                                            }

                                                                                                            bool bool__393 = bool__390;
                                                                                                            if (bool__393)
                                                                                                            {
                                                                                                                var call__394 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__395 = call__394.GetField(EndX, EndY);
                                                                                                                bool__393 = (call__395.TerrainType != 15);
                                                                                                            }

                                                                                                            bool bool__396 = bool__393;
                                                                                                            if (bool__396)
                                                                                                            {
                                                                                                                var call__397 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__398 = call__397.GetField(EndX, EndY);
                                                                                                                bool__396 = (call__398.TerrainType != 7);
                                                                                                            }

                                                                                                            if (bool__396)
                                                                                                            {
                                                                                                                Ooze = Aow.Units.AoWTC.TCOozeHS.Create(null);
                                                                                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Ooze, EndX, EndY, 0);
                                                                                                            }

                                                                                                            break;
                                                                                                    }

                                                                                                    break;
                                                                                                case Aow.Units.AoWTC.TfxType.fxEffect:
                                                                                                    switch (Effect.FSpellIndex)
                                                                                                    {
                                                                                                        case 113:
                                                                                                            Msg.MsgID = 200715;
                                                                                                            Msg.Param[0] = FUnit;
                                                                                                            Msg.Result = -1;
                                                                                                            var call__399 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__400 = call__399.GetField(EndX, EndY);
                                                                                                            call__400.SendMapFieldMsg(ref Msg);
                                                                                                            Aow.Units.CombatFire.CombatFire.CreateFire(false, 100, EndX, EndY);
                                                                                                            break;
                                                                                                    }

                                                                                                    break;
                                                                                            }

                                                                                            break;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                EffectIndex += 1;
                            }

                            bool bool__401 = (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FEffects.Count == 0);
                            if (bool__401)
                            {
                                bool bool__402 = (FStrikeIndex >= FStrikeCount);
                                if (!bool__402)
                                {
                                    var call__403 = FUnit.GetEnabled();
                                    bool__402 = !call__403;
                                }

                                bool__401 = bool__402;
                            }

                            if (bool__401)
                            {
                                var call__404 = FUnit.GetEnabled();
                                if (call__404)
                                {
                                    FUnit.FUnitHS.SetAction(Aow.Units.AoWTC.TTCAction.caMove);
                                }

                                Result = Aow.Units.NetworkE.TExecuteResult.erDone;
                            }

                            var call__405 = FUnit.GetEnabled();
                            if (call__405)
                            {
                                if (FEffectType == Aow.Units.AoWTC.TfxType.fxBreath)
                                {
                                    NextStrike();
                                }
                                else
                                {
                                    if (FUnit.FUnitHS.FActionDone || ((FAbilityID == 52) && (FDelay <= 0)))
                                    {
                                        var call__406 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                                        FDelay = call__406 + 4;
                                    }

                                    FDelay -= 1;
                                    if (FDelay == 0)
                                    {
                                        NextStrike();
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token Execute", ((((AnsiString)"Error - TCRangedToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(FSpellID), (int)0);
                        }

                        break;
                }

                if (Result == Aow.Units.NetworkE.TExecuteResult.erDone)
                {
                    if (FSound != null)
                    {
                        FSound.Free();
                    }

                    bool bool__407 = (FUnit != null);
                    if (bool__407)
                    {
                        var call__408 = FUnit.GetEnabled();
                        bool__407 = call__408;
                    }

                    if (bool__407)
                    {
                        FUnit.FUnitHS.FSelectedAbility = -1;
                        FUnit.FUnitHS.FSelectedSpell = -1;
                        FUnit.FUnitHS.Unselect();
                        if (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__409 = FUnit.FUnitHS.Xhx;
                            var call__410 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__410.FLastX = call__409;
                            var call__411 = FUnit.FUnitHS.Yhx;
                            var call__412 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                            call__412.FLastY = call__411;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Token", ((((AnsiString)"Error - TCRangedToken ") + Aow.Rtl.Rtl.IntToStr(FAbilityID)) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(FSpellID), (int)0);
            }

            return Result;
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
                    int end__413 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__413; I += 1)
                    {
                        var call__414 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__415 = (call__414 is Aow.Units.AoWE.TCombatUnit);
                        if (bool__415)
                        {
                            var call__416 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__417 = call__416.GetEnabled();
                            bool__415 = call__417;
                        }

                        bool bool__418 = bool__415;
                        if (bool__418)
                        {
                            var call__419 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__420 = call__419.GetSide();
                            var call__421 = FUnit.GetSide();
                            bool__418 = (call__420 != call__421);
                        }

                        if (bool__418)
                        {
                            Available = true;
                            {
                                J = 0;
                                int end__422 = Used.Count - 1;
                                for (; J <= end__422; J += 1)
                                {
                                    var call__423 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    var call__424 = Used[J];
                                    if (call__423 == call__424)
                                    {
                                        Available = false;
                                    }
                                }
                            }

                            if (Available)
                            {
                                var call__425 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__426 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__425).FUnitHS.Yhx;
                                var call__427 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__428 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__427).FUnitHS.Xhx;
                                var call__429 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__428, call__426);
                                Distance = call__429;
                                if (Distance < BestDistance)
                                {
                                    var call__430 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__430;
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
                    int end__431 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__431; I += 1)
                    {
                        var call__432 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__433 = (call__432 is Aow.Units.AoWE.TCombatUnit);
                        if (bool__433)
                        {
                            var call__434 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__435 = call__434.GetEnabled();
                            bool__433 = call__435;
                        }

                        bool bool__436 = bool__433;
                        if (bool__436)
                        {
                            var call__437 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            bool__436 = (call__437 != Target);
                        }

                        if (bool__436)
                        {
                            var call__438 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__439 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__438).FUnitHS.Yhx;
                            var call__440 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__441 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__440).FUnitHS.Xhx;
                            var call__442 = Aow.Units.HSEngine.HSEngine.dHXtoRad(FTargetX, FTargetY, call__441, call__439);
                            Distance = call__442;
                            if (Distance < 13)
                            {
                                Total = Total + (13 - Distance);
                                var call__443 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                Used.Add(call__443);
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

                    var call__447 = Used[Index];
                    Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__447;
                }

                Used.Free();
                Weights.Free();
            }

            if (FStrikeIndex >= FStrikeCount)
            {
                return;
            }

            var call__448 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxNone);
            Effect = call__448;
            Obstacles = new Aow.Rtl.TList();
            switch (FEffectType)
            {
                case Aow.Units.AoWTC.TfxType.fxBreath:
                    var call__449 = FUnit.FUnitHS.Yhx;
                    var call__450 = FUnit.FUnitHS.Xhx;
                    var call__451 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__450, call__449, FTargetX, FTargetY);
                    var call__452 = FUnit.FUnitHS.Yhx;
                    var call__453 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__453, call__452, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[call__451 - 1] * 12) - 3], ref J, ref K);
                    var call__454 = FUnit.FUnitHS.Yhx;
                    var call__455 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__455, call__454, ref X1, ref Y1);
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
                        var call__456 = System.Math.PI;
                        var call__457 = System.Math.PI;
                        Angle = (float)(((double)(((double)(((double)call__456) / 2)) - A)) + (((double)(((double)(((double)(((double)Sweep) * 6)) - 30)) * call__457)) / 180));
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

                    var call__458 = FUnit.FUnitHS.Xhx;
                    var call__459 = FUnit.FUnitHS.Yhx;
                    var call__460 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__460, 50, 20, 50, unchecked((short)call__458), unchecked((short)call__459), unchecked((short)J), unchecked((short)K), unchecked((short)DX), unchecked((short)DY), false, ref Effect.FPath, ref Obstacles);
                    if (FStrikeIndex < 12)
                    {
                        var call__461 = FUnit.FUnitHS.Yhx;
                        var call__462 = FUnit.FUnitHS.Xhx;
                        var call__463 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__462, call__461, FTargetX, FTargetY);
                        var call__464 = FUnit.FUnitHS.Yhx;
                        var call__465 = FUnit.FUnitHS.Xhx;
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__465, call__464, Aow.Units.AoWTC.AoWTC.BreathHN[((Aow.Units.AoWTC.AoWTC.BreathDirection[call__463 - 1] * 12) + Aow.Units.AoWTC.AoWTC.BreathHit[FStrikeIndex]) - 12], ref Effect.FTargetX, ref Effect.FTargetY);
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
                            var call__466 = FUnit.FUnitHS.Yhx;
                            var call__467 = FUnit.FUnitHS.Xhx;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__467, call__466, 0);
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacFlame:
                            var call__468 = FUnit.FUnitHS.Yhx;
                            var call__469 = FUnit.FUnitHS.Xhx;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__469, call__468 - 3, 0);
                            break;
                        default:
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                            break;
                    }

                    Point = Aow.Units.aowFX.TXYZ.Create();
                    Point.X = X2 + 16;
                    Point.Y = Y2 + 16;
                    Effect.FPath.Add(Point);
                    var call__470 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__471 = call__470.GetField(FTargetX, FTargetY);
                    var call__472 = call__471.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__472;
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
                    var call__473 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__474 = call__473.GetField(FTargetX, FTargetY);
                    var call__475 = call__474.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__475;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    var call__476 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(37);
                    I = call__476 + 1;
                    Found = false;
                    while (!Found)
                    {
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                        bool bool__477 = (((J > 5) && (J < 60)) && (K > 5)) && (K < 53);
                        if (bool__477)
                        {
                            var call__478 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__479 = call__478.GetField(J, K);
                            var call__480 = call__479.FindOwnedChild(2228484);
                            bool__477 = (call__480 == null);
                        }

                        bool bool__481 = bool__477;
                        if (bool__481)
                        {
                            var call__482 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__483 = call__482.GetField(J, K);
                            bool bool__484 = (call__483.OverlayType != 8);
                            if (!bool__484)
                            {
                                var call__485 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__486 = call__485.GetField(J, K);
                                var call__487 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__488 = call__487.GetField(FTargetX, FTargetY);
                                bool__484 = (call__486.OverlayType == call__488.OverlayType);
                            }

                            bool__481 = bool__484;
                        }

                        bool bool__489 = bool__481;
                        if (bool__489)
                        {
                            var call__490 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__491 = call__490.GetField(J, K);
                            bool__489 = (call__491.TerrainType != 15);
                        }

                        bool bool__492 = bool__489;
                        if (bool__492)
                        {
                            var call__493 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__494 = call__493.GetField(J, K);
                            bool__492 = (call__494.TerrainType != 7);
                        }

                        bool bool__495 = bool__492;
                        if (bool__495)
                        {
                            var call__496 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__497 = call__496.GetField(J, K);
                            bool bool__498 = (call__497.TerrainType != 0);
                            if (bool__498)
                            {
                                var call__499 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__500 = call__499.GetField(J, K);
                                bool__498 = (call__500.TerrainType != 10);
                            }

                            bool bool__501 = bool__498;
                            if (bool__501)
                            {
                                var call__502 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__503 = call__502.GetField(J, K);
                                bool__501 = (call__503.TerrainType != 9);
                            }

                            bool bool__504 = bool__501;
                            if (!bool__504)
                            {
                                var call__505 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__506 = call__505.GetField(J, K);
                                var call__507 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__508 = call__507.GetField(FTargetX, FTargetY);
                                bool__504 = (call__506.TerrainType == call__508.TerrainType);
                            }

                            bool__495 = bool__504;
                        }

                        if (bool__495)
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
                    byte end__509 = 29;
                    for (; I <= end__509; I += 1)
                    {
                        var call__510 = Aow.Units.aowFX.TXYZ.Create();
                        Effect.FPath.Add(call__510);
                        var call__511 = Effect.FPath[0];
                        var call__512 = Aow.Rtl.Rtl.Random(30);
                        var call__513 = Effect.FPath.Count;
                        var call__514 = Effect.FPath[call__513 - 1];
                        call__514.X = (call__511.X + call__512) - 15;
                        var call__515 = Effect.FPath[0];
                        var call__516 = Aow.Rtl.Rtl.Random(30);
                        var call__517 = Effect.FPath.Count;
                        var call__518 = Effect.FPath[call__517 - 1];
                        call__518.Y = (call__515.Y + call__516) - 15;
                    }
                }

                    J = Aow.Rtl.Rtl.Random(300);
                    K = Aow.Rtl.Rtl.Random(300);
                    var call__519 = Effect.FPath.Count;
                    var call__520 = Effect.FPath[call__519 - 1];
                    var call__521 = Effect.FPath.Count;
                    var call__522 = Effect.FPath[call__521 - 1];
                    var call__523 = Effect.FPath.Count;
                    var call__524 = Effect.FPath[call__523 - 1];
                    var call__525 = Effect.FPath.Count;
                    var call__526 = Effect.FPath[call__525 - 1];
                    var call__527 = Effect.FPath.Count;
                    var call__528 = Effect.FPath[call__527 - 1];
                    var call__529 = Effect.FPath.Count;
                    var call__530 = Effect.FPath[call__529 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__530.X), unchecked((short)(call__524.Y - call__526.Z)), unchecked((short)((call__528.X + J) - 150)), unchecked((short)(((call__520.Y - call__522.Z) + K) - 150)), unchecked((short)(((System.Math.Abs(J - 150) + System.Math.Abs(K - 150)) / 50) + 2)), 100, ref Effect.FPath);
                    var call__531 = Effect.FPath.Count;
                    Effect.FTargetX = call__531;
                    J = Aow.Rtl.Rtl.Random(200);
                    K = Aow.Rtl.Rtl.Random(200);
                    var call__532 = Effect.FPath.Count;
                    var call__533 = Effect.FPath[call__532 - 1];
                    var call__534 = Effect.FPath.Count;
                    var call__535 = Effect.FPath[call__534 - 1];
                    var call__536 = Effect.FPath.Count;
                    var call__537 = Effect.FPath[call__536 - 1];
                    var call__538 = Effect.FPath.Count;
                    var call__539 = Effect.FPath[call__538 - 1];
                    var call__540 = Effect.FPath.Count;
                    var call__541 = Effect.FPath[call__540 - 1];
                    var call__542 = Effect.FPath.Count;
                    var call__543 = Effect.FPath[call__542 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__543.X), unchecked((short)(call__537.Y - call__539.Z)), unchecked((short)((call__541.X + J) - 100)), unchecked((short)(((call__533.Y - call__535.Z) + K) - 100)), unchecked((short)(((System.Math.Abs(J - 100) + System.Math.Abs(K - 100)) / 50) + 3)), 100, ref Effect.FPath);
                    var call__544 = Effect.FPath.Count;
                    Effect.FTargetY = call__544;
                    var call__545 = Effect.FPath.Count;
                    var call__546 = Effect.FPath[call__545 - 1];
                    var call__547 = Effect.FPath.Count;
                    var call__548 = Effect.FPath[call__547 - 1];
                    var call__549 = Effect.FPath.Count;
                    var call__550 = Effect.FPath[call__549 - 1];
                    var call__551 = Effect.FPath.Count;
                    var call__552 = Effect.FPath[call__551 - 1];
                    var call__553 = Effect.FPath.Count;
                    var call__554 = Effect.FPath[call__553 - 1];
                    var call__555 = Effect.FPath.Count;
                    var call__556 = Effect.FPath[call__555 - 1];
                    Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__556.X), unchecked((short)(call__552.Y - call__554.Z)), unchecked((short)(DX + 16)), unchecked((short)(DY + 16)), unchecked((short)(((System.Math.Abs((call__546.Y - call__548.Z) - DY) + System.Math.Abs(call__550.X - DX)) / 50) + 4)), 100, ref Effect.FPath);
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
                            var call__557 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__558 = call__557.GetField(DX, DY);
                            var call__559 = call__558.FindOwnedChild(2228484);
                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__559;
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                            break;
                    }

                    break;
                case Aow.Units.AoWTC.TfxType.fxChain:
                    var call__560 = FUnit.FUnitHS.Yhx;
                    var call__561 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__561, call__560, ref X1, ref Y1);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Aow.Units.aowFX.aowFX.MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                    var call__562 = Effect.FPath.Count;
                    var call__563 = Effect.FPath[call__562 - 1];
                    call__563.Z = 16;
                    I = 3;
                    Used = new Aow.Rtl.TList();
                    Used.Add(FUnit);
                    var call__564 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__565 = call__564.GetField(FTargetX, FTargetY);
                    var call__566 = call__565.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__566;
                    if (HS != null)
                    {
                        Used.Add(HS.FUnit);
                    }

                    var call__567 = FindTarget(FTargetX, FTargetY);
                    Target = call__567;
                    while ((Target != null) && (I > 0))
                    {
                        Used.Add(Target);
                        X1 = X2 + 16;
                        Y1 = Y2 + 16;
                        var call__568 = Target.FUnitHS.Yhx;
                        var call__569 = Target.FUnitHS.Xhx;
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__569, call__568, ref X2, ref Y2);
                        Aow.Units.aowFX.aowFX.MakeLightningPath(X1, Y1, X2 + 16, Y2, ref Effect.FPath);
                        I -= 1;
                        var call__570 = Target.FUnitHS.Yhx;
                        var call__571 = Target.FUnitHS.Xhx;
                        var call__572 = FindTarget(call__571, call__570);
                        Target = call__572;
                    }

                    Used.Free();
                    break;
                case Aow.Units.AoWTC.TfxType.fxSwarm:
                    var call__573 = FUnit.FUnitHS.Yhx;
                    var call__574 = FUnit.FUnitHS.Xhx;
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__574, call__573, ref X1, ref Y1);
                    var call__575 = Aow.Rtl.Rtl.Random(360);
                    var call__576 = Aow.Rtl.Rtl.Random(20);
                    var call__577 = Aow.Rtl.Rtl.Random(20);
                    var call__578 = Aow.Rtl.Rtl.Random(2);
                    var call__579 = Aow.Rtl.Rtl.Random(90);
                    var call__580 = Aow.Rtl.Rtl.Random(5);
                    var call__581 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__581 + 10, call__580 + 20, call__579, call__575, (call__576 + X1) + 6, (call__577 + Y1) - 10, (Aow.Units.aowFX.TSwarmDirection)call__578, ref Effect.FPath);
                    var call__582 = Effect.FPath.Count;
                    K = call__582 - 1;
                {
                    I = 0;
                    byte end__583 = 2;
                    for (; I <= end__583; I += 1)
                    {
                        {
                            J = 0;
                            int end__584 = K;
                            for (; J <= end__584; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__585 = Effect.FPath[J];
                                Point.X = call__585.X;
                                var call__586 = Effect.FPath[J];
                                Point.Y = call__586.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    var call__587 = Effect.FPath.Count;
                    var call__588 = Effect.FPath[call__587 - 1];
                    var call__589 = Effect.FPath.Count;
                    var call__590 = Effect.FPath[call__589 - 1];
                    Aow.Units.aowFX.aowFX.MakeLinePath(call__590.X, call__588.Y, X2 + 16, Y2, 15, ref Effect.FPath);
                    Path = Aow.Units.aowFX.TXYZList.Create();
                    var call__591 = Aow.Rtl.Rtl.Random(360);
                    var call__592 = Effect.FPath.Count;
                    var call__593 = Effect.FPath[call__592 - 1];
                    var call__594 = Effect.FPath.Count;
                    var call__595 = Effect.FPath[call__594 - 1];
                    var call__596 = Aow.Rtl.Rtl.Random(2);
                    var call__597 = Aow.Rtl.Rtl.Random(90);
                    var call__598 = Aow.Rtl.Rtl.Random(5);
                    var call__599 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__599 + 10, call__598 + 20, call__597, call__591, call__593.X, call__595.Y, (Aow.Units.aowFX.TSwarmDirection)call__596, ref Path);
                    var call__600 = Path[0];
                    var call__601 = Effect.FPath.Count;
                    var call__602 = Effect.FPath[call__601 - 1];
                    var call__603 = Effect.FPath.Count;
                    var call__604 = Effect.FPath[call__603 - 1];
                    var call__605 = Path[0];
                    Aow.Units.aowFX.aowFX.MakeLinePath(call__604.X, call__602.Y, call__605.X, call__600.Y, 5, ref Effect.FPath);
                {
                    I = 0;
                    int end__606 = Path.Count - 1;
                    for (; I <= end__606; I += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        var call__607 = Path[I];
                        Effect.FPath.Add(call__607);
                    }
                }

                    var call__608 = Path.Count;
                    K = call__608 - 1;
                {
                    I = 0;
                    byte end__609 = 2;
                    for (; I <= end__609; I += 1)
                    {
                        {
                            J = 0;
                            int end__610 = K;
                            for (; J <= end__610; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__611 = Path[J];
                                Point.X = call__611.X;
                                var call__612 = Path[J];
                                Point.Y = call__612.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                    Path.Free();
                    break;
                case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                    var call__613 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__614 = call__613.GetField(FTargetX, FTargetY);
                    var call__615 = call__614.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__615;
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
                        var call__616 = Target.FUnitHS.Xhx;
                        DX = call__616;
                        var call__617 = Target.FUnitHS.Yhx;
                        DY = call__617;
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
                    var call__618 = Aow.Rtl.Rtl.Random(360);
                    var call__619 = Aow.Rtl.Rtl.Random(20);
                    var call__620 = Aow.Rtl.Rtl.Random(20);
                    var call__621 = Aow.Rtl.Rtl.Random(2);
                    var call__622 = Aow.Rtl.Rtl.Random(90);
                    var call__623 = Aow.Rtl.Rtl.Random(10);
                    var call__624 = Aow.Rtl.Rtl.Random(10);
                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__624 + 10, call__623 + 25, call__622, call__618, call__619, call__620 - 10, (Aow.Units.aowFX.TSwarmDirection)call__621, ref Effect.FPath);
                    var call__625 = Effect.FPath.Count;
                    K = call__625 - 1;
                {
                    I = 0;
                    byte end__626 = 4;
                    for (; I <= end__626; I += 1)
                    {
                        {
                            J = 1;
                            int end__627 = K;
                            for (; J <= end__627; J += 1)
                            {
                                Point = Aow.Units.aowFX.TXYZ.Create();
                                var call__628 = Effect.FPath[J];
                                Point.X = call__628.X;
                                var call__629 = Effect.FPath[J];
                                Point.Y = call__629.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                {
                    I = 0;
                    byte end__630 = 49;
                    for (; I <= end__630; I += 1)
                    {
                        var call__631 = Effect.FPath[I + 1];
                        call__631.Z = 100 - (I * 2);
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
                    var call__632 = FUnit.FUnitHS.Xhx;
                    var call__633 = FUnit.FUnitHS.Yhx;
                    var call__634 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__634, 10, 0, 50, unchecked((short)call__632), unchecked((short)call__633), unchecked((short)FTargetX), unchecked((short)FTargetY), 0, 0, true, ref Effect.FPath, ref Obstacles);
                    break;
                default:
                    var call__635 = FUnit.FUnitHS.Xhx;
                    var call__636 = FUnit.FUnitHS.Yhx;
                    var call__637 = FUnit.GetSide();
                    Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__637, unchecked((short)FAbilityID), unchecked((short)FSpellID), unchecked((short)call__635), unchecked((short)call__636), unchecked((short)FTargetX), unchecked((short)FTargetY), ref Effect.FPath, ref Obstacles);
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
                    var call__638 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__639 = call__638.GetField(FTargetX, FTargetY);
                    var call__640 = call__639.FindOwnedChild(2228484);
                    HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__640;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    I = 0;
                    while (I < Obstacles.Count)
                    {
                        var call__641 = Obstacles[I];
                        Obstacle = (Aow.Units.AoWTC.TROb)call__641;
                        var call__642 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                        if (call__642 < Obstacle.Chance)
                        {
                            var call__643 = Effect.FPath[Obstacle.PathIndex];
                            var call__644 = Effect.FPath[Obstacle.PathIndex];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__644.X, call__643.Y, ref J, ref K);
                            if (Effect.FEffectType == Aow.Units.AoWTC.TfxType.fxBreath)
                            {
                                HitObject = null;
                            }
                            else
                            {
                                var call__645 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__646 = call__645.GetField(J, K);
                                var call__647 = call__646.FindOwnedChild(2228484);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__647;
                            }

                            if (HitObject == null)
                            {
                                var call__648 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__649 = call__648.GetField(J, K);
                                var call__650 = call__649.FindOwnedChild(2228582);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__650;
                            }

                            if (HitObject == null)
                            {
                                var call__651 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__652 = call__651.GetField(J, K);
                                var call__653 = call__652.FindOwnedChild(2229508);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__653;
                            }

                            if (HitObject == null)
                            {
                                var call__654 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__655 = call__654.GetField(J, K);
                                var call__656 = call__655.FindOwnedChild(2229510);
                                HitObject = (Aow.Units.HSEngine.THexagonSprite)call__656;
                            }

                            bool bool__657 = (HitObject != null);
                            if (!bool__657)
                            {
                                var call__658 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__659 = call__658.GetField(J, K);
                                bool__657 = (call__659.TerrainType == 15);
                            }

                            bool bool__660 = bool__657;
                            if (!bool__660)
                            {
                                var call__661 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__662 = call__661.GetField(J, K);
                                bool__660 = (call__662.TerrainType == 7);
                            }

                            bool bool__663 = bool__660;
                            if (!bool__663)
                            {
                                var call__664 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__665 = call__664.GetField(J, K);
                                bool__663 = (call__665.OverlayType == 8);
                            }

                            if (bool__663)
                            {
                                {
                                    J = Obstacle.PathIndex + 1;
                                    int end__666 = Effect.FPath.Count - 1;
                                    for (; J <= end__666; J += 1)
                                    {
                                        var call__667 = Effect.FPath.Count;
                                        Effect.FPath.Delete(call__667 - 1);
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
                                var call__668 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                                if (call__668 < 75)
                                {
                                    Effect.FTarget = null;
                                }

                                break;
                            case 100:
                                var call__669 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxSFOverlay);
                                ExtraEffect = call__669;
                                var call__670 = FUnit.FUnitHS.Yhx;
                                var call__671 = FUnit.FUnitHS.Xhx;
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__671, call__670, ref X1, ref Y1);
                                while (true)
                                {
                                    var call__672 = ExtraEffect.FPath.Count;
                                    var call__673 = Effect.FPath.Count;
                                    if (!(call__672 < call__673))
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

            var call__674 = FUnit.FUnitHS.Yhx;
            var call__675 = FUnit.FUnitHS.Xhx;
            var call__676 = Aow.Units.AoWTC.AoWTC.GetRangedDirIndex(FTargetX - call__675, FTargetY - call__674);
            Effect.FVariant = call__676;
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
                int end__677 = Obstacles.Count - 1;
                for (; I <= end__677; I += 1)
                {
                    var call__678 = Obstacles[I];
                    ((Aow.Rtl.TObject)call__678).Free();
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
                    int end__679 = Count - 1;
                    for (; I <= end__679; I += 1)
                    {
                        var call__680 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(EffectType);
                        Effect = call__680;
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
            bool bool__681 = (Player != -1);
            if (bool__681)
            {
                var call__682 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Player);
                bool__681 = call__682;
            }

            if (bool__681)
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
            var call__683 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            var call__684 = call__683.GetBusy();
            if (!call__684)
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
            var call__685 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxBlood);
            Effect = call__685;
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

            var call__686 = UnitObject.FUnit.GetBloodType();
            switch (call__686)
            {
                case Aow.Units.AoWE.TBloodType.btNone:
                    Point.Z = Point.Z + 5;
                    break;
            }

            var call__687 = UnitObject.FUnitHS.Yhx;
            var call__688 = UnitObject.FUnitHS.Xhx;
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__688, call__687, ref Point.X, ref Point.Y);
            var call__689 = Aow.Rtl.Rtl.Random(10);
            Point.X = (call__689 + Point.X) + 11;
            var call__690 = UnitObject.FUnit.GetUnitSize();
            var call__691 = Aow.Rtl.Rtl.Random(10);
            Point.Y = ((call__691 + Point.Y) + 9) - (((int)call__690) * 10);
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
                var call__692 = UnitObject.FUnitHS.Xhx;
                Result.FSourceX = unchecked((ushort)call__692);
                var call__693 = UnitObject.FUnitHS.Yhx;
                Result.FSourceY = unchecked((ushort)call__693);
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
            var call__694 = UnitObject.GetPlayer();
            var call__695 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__694);
            if (call__695 && (Path.Count > 1))
            {
                MovePoints = UnitObject.FMovePoints;
                Cost = 0;
                Index = Path.Count;
                while ((Index != 0) && (Cost <= MovePoints))
                {
                    Index = Index - 1;
                    var call__696 = Path[Index];
                    Cost = Cost + call__696.Flag;
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
                        int end__697 = Path.Count - 1;
                        for (; I <= end__697; I += 1)
                        {
                            var call__698 = Path[I];
                            NewPath.Add(call__698);
                        }
                    }

                    Event = Aow.Units.CombatTE.CombatTE.CreateCombatMoveTE(UnitObject, NewPath);
                    if (Event != null)
                    {
                        var call__699 = UnitObject.GetPlayer();
                        var call__700 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__699);
                        call__700.AddEvent(Event);
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
            var call__701 = Moves.Count;
            bool bool__702 = (call__701 != 0);
            if (bool__702)
            {
                var call__703 = Moves.GetMoveSettings(0);
                var call__704 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__703.FMoveObject).FUnit.GetPlayer();
                var call__705 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__704);
                bool__702 = call__705;
            }

            if (bool__702)
            {
                Path = Aow.Units.AoWE.TMovepath.Create();
                Multi = Aow.Units.AoWE.TMultiMoveTE.Create(null);
                Multi.FSimultaneous = false;
                Moves.CalculateMovePositions(Aow.Units.CombatTE.CombatTE.ValidateTCMovePosition);
                {
                    I = 1;
                    int end__706 = Moves.Count - 1;
                    for (; I <= end__706; I += 1)
                    {
                        {
                            J = 0;
                            int end__707 = I - 1;
                            for (; J <= end__707; J += 1)
                            {
                                var call__708 = Moves.GetMoveSettings(J);
                                var call__709 = Moves.GetMoveSettings(I);
                                if (call__708.FPath.Count > call__709.FPath.Count)
                                {
                                    Moves.FChildren.Exchange(I, J);
                                }
                            }
                        }
                    }
                }

                {
                    K = 0;
                    int end__710 = Moves.Count - 1;
                    for (; K <= end__710; K += 1)
                    {
                        {
                            var w__711 = Moves.GetMoveSettings(K);
                            if (w__711.FMovePos < (w__711.FPath.Count - 1))
                            {
                                Path.Clear();
                                {
                                    L = w__711.FMovePos;
                                    int end__712 = w__711.FPath.Count - 1;
                                    for (; L <= end__712; L += 1)
                                    {
                                        var call__713 = w__711.FPath[L];
                                        Path.Add(call__713);
                                    }
                                }

                                MoveEvent = Aow.Units.CombatTE.TCombatMoveTE.Create(null);
                                var call__714 = MoveEvent.Setup(((Aow.Units.AoWTC.TTacticalCombatUnitHS)w__711.FMoveObject).FUnit, Path);
                                if (call__714)
                                {
                                    Multi.FEvents.Add(MoveEvent);
                                }

                                MoveEvent.Release();
                            }
                        }
                    }
                }

                var call__715 = Multi.FEvents.Count;
                if (call__715 != 0)
                {
                    var call__716 = Moves.GetMoveSettings(0);
                    var call__717 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__716.FMoveObject).FUnit.GetPlayer();
                    var call__718 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__717);
                    call__718.AddEvent(Multi);
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
                    int end__719 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.Count - 1;
                    for (; K <= end__719; K += 1)
                    {
                        {
                            var w__720 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(K);
                            w__720.FPath.Clear();
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
                    byte end__721 = 0;
                    for (; I >= end__721; I -= 1)
                    {
                        if (I != Index)
                        {
                            Settings = Moves.GetMoveSettings(I);
                            bool bool__722 = Settings.FValid;
                            if (bool__722)
                            {
                                var call__723 = Moves.GetMoveSettings(Index);
                                var call__724 = call__723.GetMovePrio();
                                var call__725 = Settings.GetMovePrio();
                                bool__722 = (call__724 >= call__725);
                            }

                            bool bool__726 = bool__722;
                            if (bool__726)
                            {
                                var call__727 = Moves.GetMoveSettings(Index);
                                var call__728 = call__727.GetMoveLoc();
                                var call__729 = Settings.GetMoveLoc();
                                bool__726 = (call__729.XYL == call__728.XYL);
                            }

                            if (bool__726)
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
