// Translated from CombatTE.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.Engine;
using Aow.Units.Sound;
using Aow.Units.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.CityWall;
using static Aow.Units.CombatFire.CombatFire;
using Aow.Units.CombatSpells;

namespace Aow.Units.CombatTE
{
    // { ot=1 }
    public partial class TCEndTurnTE : TTokenEvent
    {
        public TCEndTurnTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:175
        public new const int ClassID = 2236420;

        // CombatTE.pas:178
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            int TurnIndex = default;
            int I = default;
            bool Found = default;
            base.Execute(TE);
            var call__1 = AoWHSMap.Random(16777215);
            RandSeed = call__1;
            var call__2 = AoWCombatMap.FCombat.FCombatData.FTurnOrder.IndexOf((byte)AoWCombatMap.FCurrentCombatPlayer);
            TurnIndex = call__2;
            Found = false;
            while (!Found)
            {
                TurnIndex = TurnIndex + 1;
                if (AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count <= TurnIndex)
                {
                    TurnIndex = 0;
                    AoWCombatMap.FCombat.NewRound();
                }

                {
                    I = 0;
                    int end__3 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__3; I += 1)
                    {
                        var call__4 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__5 = call__4 != null;
                        if (bool__5)
                        {
                            var call__6 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            bool__5 = call__6 is TCombatUnit;
                        }

                        bool bool__7 = bool__5;
                        if (bool__7)
                        {
                            var call__8 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__9 = AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
                            bool__7 = call__8.FCombatPlayer == call__9;
                        }

                        bool bool__10 = bool__7;
                        if (bool__10)
                        {
                            var call__11 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
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

            var call__13 = AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
            AoWCombatMap.NewTurn((sbyte)call__13);
            Result = TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230


    }

    public partial class TCEndCombatTE : TTokenEvent
    {
        public TCEndCombatTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:158
        public new const int ClassID = 2236421;

        // CombatTE.pas:161
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            int I = default;
            base.Execute(TE);
            {
                I = 0;
                int end__14 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                for (; I <= end__14; I += 1)
                {
                    var call__15 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                    bool bool__16 = call__15 != null;
                    if (bool__16)
                    {
                        var call__17 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool__16 = call__17 is TCombatUnit;
                    }

                    bool bool__18 = bool__16;
                    if (bool__18)
                    {
                        var call__19 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        var call__20 = call__19.GetEnabled();
                        bool__18 = call__20;
                    }

                    bool bool__21 = bool__18;
                    if (bool__21)
                    {
                        var call__22 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool__21 = call__22.FCombatPlayer == AoWCombatMap.FCurrentCombatPlayer;
                    }

                    if (bool__21)
                    {
                        var call__23 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        call__23.Surrender();
                    }
                }
            }

            Result = TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230


    }

    public partial class TCombatMoveTE : TMoveTE
    {
        public TCombatMoveTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:206
        public new const int ClassID = 2236416;

        public TTacticalCombatUnit FUnit;
        public int FDelay;
        public TList FObjects;
        public TTacticalCombatUnit FPreviousUnit;
        public TTacticalCombatUnit FFreeStrikeUnit;
        public sbyte FNeighbour;
        public TTokenStep FStep;
        public TStrikeCA FStrike;
        // CombatTE.pas:214
        public override void Destroy()
        {
            if (FObjects != null)
            {
                FObjects.Free();
            }

            base.Destroy();
        }

        // CombatTE.pas:209
        public override TAoWMapField GetMapF(TXYL Location)
        {
            TAoWMapField Result = default;
            var call__24 = AoWCombatMap.MapContainer.GetField(Location.X, Location.Y, 0);
            Result = (TAoWMapField)call__24;
            return Result;
        }

        // CombatTE.pas:234
        public override bool Validate()
        {
            bool Result = default;
            TAoWMapField MapField = default;
            int Index = default;
            TTacticalCombatUnitHS HS = default;
            base.Validate();
            Index = FPath.Count - 1;
            var call__25 = FPath[Index];
            var call__26 = GetMapF(call__25);
            MapField = call__26;
            HS = (TTacticalCombatUnitHS)MapField.FindOwnedChild(2228484);
            if (HS != null)
            {
                var call__27 = new TList();
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
        public override TMoveStatus DefaultMove()
        {
            TMoveStatus Result = default;
            TAoWMapField MapField = default;
            int Direction = default;
            int X = default;
            int Y = default;
            void CheckFreeStrike()
            {
                int HN = default;
                int NX = default;
                int NY = default;
                TTacticalCombatUnitHS HS = default;
                var call__28 = FUnit.GetEnabled();
                if (call__28 && FFreeStrikeUnit == null)
                {
                    MapField = (TAoWMapField)FUnit.FUnitHS.Owner;
                    HN = FNeighbour + 1;
                    while (HN <= 6)
                    {
                        var call__29 = FUnit.FUnitHS.Yhx;
                        var call__30 = FUnit.FUnitHS.Xhx;
                        CenterHNtoHX(call__30, call__29, HN, ref NX, ref NY);
                        var call__31 = AoWCombatMap.MapContainer.MapLevels[0];
                        var call__32 = call__31.GetField(NX, NY);
                        var call__33 = call__32.FindOwnedChild(2228484);
                        HS = (TTacticalCombatUnitHS)call__33;
                        bool bool__34 = HS != null && FPreviousUnit != HS.FUnit;
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
                            var call__39 = MeleeDamageTypes(HS.FUnit);
                            bool__38 = call__39 != TSet<TDamageType>.Of();
                        }

                        bool bool__40 = bool__38;
                        if (bool__40)
                        {
                            var call__41 = HS.FUnit.GetSide();
                            var call__42 = FUnit.GetSide();
                            bool__40 = call__41 != call__42;
                        }

                        bool bool__43 = bool__40 && HS.FAction == TTCAction.caMove;
                        if (bool__43)
                        {
                            var call__44 = HS.FUnit.GetAbilityEnabled(1);
                            var call__45 = FUnit.GetAbilityEnabled(1);
                            bool__43 = call__44 == call__45;
                        }

                        if (bool__43)
                        {
                            FNeighbour = (sbyte)HN;
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
                                FStep = TTokenStep.tokStart;
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

            Result = TMoveStatus.msMoving;
            try
            {
                if (FFreeStrikeUnit != null)
                {
                    Result = TMoveStatus.msMoving;
                    switch (FStep)
                    {
                        case TTokenStep.tokStart:
                            try
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                                FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                                var call__48 = FFreeStrikeUnit.FUnitHS.Yhx;
                                var call__49 = FUnit.FUnitHS.Yhx;
                                var call__50 = FFreeStrikeUnit.FUnitHS.Xhx;
                                var call__51 = FUnit.FUnitHS.Xhx;
                                var call__52 = FFreeStrikeUnit.FUnitHS.Xhx;
                                var call__53 = GetMeleeDirIndex(call__52, call__50 - call__51, call__48 - call__49);
                                FFreeStrikeUnit.FUnitHS.SetDirection((byte)call__53);
                                FStep = TTokenStep.tokInitStrike;
                            }
                            catch (System.Exception)
                            {
                                MessageBox(0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCMoveToken", 0);
                            }

                            break;
                        case TTokenStep.tokInitStrike:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    var call__54 = AoWHSMap.Random(16777215);
                                    RandSeed = call__54;
                                    var call__55 = CreateStrikeCA(FFreeStrikeUnit, FUnit, true);
                                    FStrike = call__55;
                                    if (FStrike.FDamage == 0)
                                    {
                                        FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                                    }
                                    else
                                    {
                                        FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                                    }

                                    FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                                    FStep = TTokenStep.tokExeStrike;
                                }
                            }
                            catch (System.Exception)
                            {
                                MessageBox(0, (AnsiString)"Error during Token InitStrike", (AnsiString)"Error - TCMoveToken", 0);
                            }

                            break;
                        case TTokenStep.tokExeStrike:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                        CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                    }

                                    var call__56 = FUnit.FUnitHS.Yhx;
                                    var call__57 = FUnit.FUnitHS.Xhx;
                                    HXtoHP(call__57, call__56, ref X, ref Y);
                                    if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        CombatTE.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        CombatTE.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                    }

                                    FStep = TTokenStep.tokEndStrike;
                                }
                            }
                            catch (System.Exception)
                            {
                                MessageBox(0, (AnsiString)"Error during Token ExeStrike", (AnsiString)"Error - TCMoveToken", 0);
                            }

                            break;
                        case TTokenStep.tokEndStrike:
                            try
                            {
                                if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                    }

                                    FStep = TTokenStep.tokEnd;
                                    FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                }
                            }
                            catch (System.Exception)
                            {
                                MessageBox(0, (AnsiString)"Error during Token EndStrike", (AnsiString)"Error - TCMoveToken", 0);
                            }

                            break;
                        case TTokenStep.tokEnd:
                            try
                            {
                                if (FFreeStrikeUnit.FUnitHS.FActionDone)
                                {
                                    var call__58 = FUnit.GetEnabled();
                                    if (!call__58)
                                    {
                                        Result = TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        var call__59 = FUnit.GetLocked();
                                        if (call__59)
                                        {
                                            FUnit.FUnitHS.CancelMove();
                                            Result = TMoveStatus.msDone;
                                        }
                                        else
                                        {
                                            FUnit.FUnitHS.SetAction(TTCAction.caMove);
                                        }
                                    }

                                    if (FStrike != null)
                                    {
                                        FStrike.Release();
                                        FStrike = null;
                                    }

                                    FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMove);
                                    FStep = TTokenStep.tokStart;
                                    FFreeStrikeUnit = null;
                                    CheckFreeStrike();
                                }
                            }
                            catch (System.Exception)
                            {
                                MessageBox(0, (AnsiString)"Error during Token End", (AnsiString)"Error - TCMoveToken", 0);
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
                            bool bool__63 = call__62 == 100;
                            if (!bool__63)
                            {
                                var call__64 = FUnit.FUnitHS.GetVisible();
                                bool__63 = !call__64;
                            }

                            if (bool__63)
                            {
                                Result = ExecuteDefaultMove();
                                if (Result == TMoveStatus.msMoving)
                                {
                                    CheckFreeStrike();
                                }
                            }
                            else
                            {
                                Result = TMoveStatus.msMoving;
                            }
                        }
                        else
                        {
                            if (FMoveIndex != 0)
                            {
                                bool bool__65 = !AoWCombatMap.FExplorationEnabled;
                                if (!bool__65)
                                {
                                    var call__66 = FUnit.FUnitHS.Yhx;
                                    var call__67 = FUnit.FUnitHS.Xhx;
                                    var call__68 = AoWCombatMap.MapContainer.GetField(call__67, call__66, 0);
                                    var call__69 = ((TAoWMapField)call__68).GetExplored(AoWCombatMap.FSeatedPlayer);
                                    bool__65 = call__69;
                                }

                                if (bool__65)
                                {
                                    var call__70 = FUnit.FUnitHS.Yhx;
                                    var call__71 = FUnit.FUnitHS.Xhx;
                                    AoWCombatMap.MakeVisible(call__71, call__70, 0, 3);
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
                                            Result = TMoveStatus.msFailed;
                                        }
                                        else
                                        {
                                            Result = TMoveStatus.msMoving;
                                        }

                                        if (Result == TMoveStatus.msMoving)
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
                                    Result = TMoveStatus.msDone;
                                }
                            }
                            else
                            {
                                Result = TMoveStatus.msDone;
                            }
                        }
                    }
                }

                if (Result == TMoveStatus.msDone)
                {
                    var call__76 = FUnit.GetEnabled();
                    if (call__76 && FUnit.FCombatPlayer == AoWCombatMap.FCurrentCombatPlayer)
                    {
                        var call__77 = FUnit.FUnitHS.Xhx;
                        var call__78 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                        call__78.FLastX = call__77;
                        var call__79 = FUnit.FUnitHS.Yhx;
                        var call__80 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                        call__80.FLastY = call__79;
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Error during Token", (AnsiString)"Error - TCMoveToken", 0);
            }

            return Result;
        }

        // CombatTE.pas:220
        public virtual bool Setup(TTacticalCombatUnit UnitObject, TMovepath Path)
        {
            bool Result = default;
            int StartIndex = default;
            int I = default;
            FPath.Clear();
            var call__81 = Path.MovePointsToPos(UnitObject.FMovePoints);
            StartIndex = call__81;
            if (StartIndex != Path.Count - 1)
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
        public TMoveStatus ExecuteDefaultMove()
        {
            TMoveStatus Result = default;
            TAoWMapField NewField = default;
            TAoWMapField OldField = default;
            TRect R = default;
            Move();
            OldField = (TAoWMapField)FUnit.FUnitHS.Owner;
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
                var call__88 = Rect(OldField.Xhx, OldField.Yhx, OldField.Xhx + 1, OldField.Yhx + 1);
                AoWCombatMap.TriggerMapChangedEvent(call__88, 0);
                bool bool__89 = FMoveIndex == 0;
                if (bool__89)
                {
                    var call__90 = NewField.FindChild(2229544);
                    bool__89 = call__90 != null;
                }

                if (bool__89 && FUnit.FCombatPlayer != AoWCombatMap.FCombat.FCombatData.FDefenderPlayer)
                {
                    FUnit.SetStatus(TSet<TCombatObjectStatusElement>.Of(TCombatObjectStatusElement.csFled));
                    var call__91 = FUnit.FUnitHS.Yhx;
                    var call__92 = FUnit.FUnitHS.Xhx;
                    var call__93 = FUnit.FUnitHS.Yhx;
                    var call__94 = FUnit.FUnitHS.Xhx;
                    var call__95 = Rect(call__94, call__93, call__92 + 1, call__91 + 1);
                    R = call__95;
                    FUnit.FUnitHS.Free();
                    AoWCombatMap.TriggerMapChangedEvent(R, 0);
                    FUnit.FUnitHS = null;
                    Result = TMoveStatus.msDone;
                }
                else
                {
                    if (AoWCombatMap.FExplorationEnabled)
                    {
                        var call__96 = FUnit.GetPlayer();
                        var call__97 = FUnit.GetVisibilityRange();
                        var call__98 = FUnit.FUnitHS.Yhx;
                        var call__99 = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.ExploreArea(call__99, call__98, 0, call__96, call__97);
                        var call__100 = FUnit.GetVisibilityRange();
                        var call__101 = FUnit.FUnitHS.Yhx;
                        var call__102 = FUnit.FUnitHS.Xhx;
                        AoWCombatMap.MapChangedRad(call__102, call__101, 0, call__100 + 1);
                    }
                    else
                    {
                        var call__103 = FUnit.FUnitHS.Yhx;
                        var call__104 = FUnit.FUnitHS.Xhx;
                        var call__105 = FUnit.FUnitHS.Yhx;
                        var call__106 = FUnit.FUnitHS.Xhx;
                        var call__107 = Rect(call__106, call__105, call__104 + 1, call__103 + 1);
                        AoWCombatMap.TriggerMapChangedEvent(call__107, 0);
                    }

                    var call__108 = FUnit.GetPlayer();
                    var call__109 = AoWHSMap.FPlayers.GetPlayers(call__108);
                    bool bool__110 = call__109.FPlayerType != TPlayerType.ptHuman;
                    if (!bool__110)
                    {
                        var call__111 = AoWCombatMap.FPlayers.GetPlayers(FUnit.FCombatPlayer);
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
                Result = TMoveStatus.msDone;
            }

            return Result;
        }

        // intf_P5_07.inc:765


    }

    public partial class TCMeleeMoveTE : TCombatMoveTE
    {
        public TCMeleeMoveTE(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 2236417;

        public TMeleeRound FMeleeRound;
        public int FStrikeIndex;
        // CombatTE.pas:454
        public override TMoveStatus LastMove()
        {
            TMoveStatus Result = default;
            TTacticalCombatUnitHS HS = default;
            int X = default;
            int Y = default;
            TMeleeStrike Strike = default;
            Result = TMoveStatus.msMoving;
            try
            {
                switch (FStep)
                {
                    case TTokenStep.tokStart:
                        try
                        {
                            Result = TMoveStatus.msMoving;
                            FUnit.FUnitHS.CancelMove();
                            bool bool__112 = !AoWCombatMap.FExplorationEnabled;
                            if (!bool__112)
                            {
                                var call__113 = FUnit.FUnitHS.Yhx;
                                var call__114 = FUnit.FUnitHS.Xhx;
                                var call__115 = AoWCombatMap.MapContainer.GetField(call__114, call__113, 0);
                                var call__116 = ((TAoWMapField)call__115).GetExplored(AoWCombatMap.FSeatedPlayer);
                                bool__112 = call__116;
                            }

                            if (bool__112)
                            {
                                var call__117 = FUnit.FUnitHS.Yhx;
                                var call__118 = FUnit.FUnitHS.Xhx;
                                AoWCombatMap.MakeVisible(call__118, call__117, 0, 5);
                            }

                            var call__119 = FPath[0];
                            var call__120 = FPath[0];
                            var call__121 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__122 = call__121.GetField(call__120.X, call__119.Y);
                            var call__123 = call__122.FindOwnedChild(2228484);
                            HS = (TTacticalCombatUnitHS)call__123;
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
                                Result = TMoveStatus.msDone;
                            }
                            else
                            {
                                var call__124 = new TMeleeRound();
                                FMeleeRound = call__124;
                                FMeleeRound.Calculate(FUnit, FPreviousUnit);
                                if (FMeleeRound.FCount == 0)
                                {
                                    Result = TMoveStatus.msDone;
                                }
                                else
                                {
                                    FUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                                    var call__125 = FUnit.FUnitHS.Yhx;
                                    var call__126 = FPreviousUnit.FUnitHS.Yhx;
                                    var call__127 = FUnit.FUnitHS.Xhx;
                                    var call__128 = FPreviousUnit.FUnitHS.Xhx;
                                    var call__129 = FUnit.FUnitHS.Xhx;
                                    var call__130 = GetMeleeDirIndex(call__129, call__127 - call__128, call__125 - call__126);
                                    FUnit.FUnitHS.SetDirection((byte)call__130);
                                    FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                    if (FPreviousUnit.FUnitHS.FDirection > 6)
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                    }
                                }
                            }

                            FStep = TTokenStep.tokFirstStrike;
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCMelMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokFirstStrike:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            ExecuteStrike();
                        }

                        break;
                    case TTokenStep.tokDelayStrike:
                        if (FDelay > 0)
                        {
                            FDelay = FDelay - 1;
                        }
                        else
                        {
                            ExecuteStrike();
                        }

                        break;
                    case TTokenStep.tokInitStrike:
                        try
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if (Strike.Side == TCombatSideIndex.csDefender && FPreviousUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                    CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                    var call__131 = FUnit.FUnitHS.Yhx;
                                    var call__132 = FUnit.FUnitHS.Xhx;
                                    HXtoHP(call__132, call__131, ref X, ref Y);
                                    if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        CombatTE.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        CombatTE.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                    }
                                }

                                FStep = TTokenStep.tokExeStrike;
                            }
                            else
                            {
                                FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                                if (Strike.Side == TCombatSideIndex.csAttacker && FUnit.FUnitHS.FActionDone)
                                {
                                    if (FStrike.FDamage > 0)
                                    {
                                        FPreviousUnit.FUnitHS.SetAction(TTCAction.caHit);
                                        CombatTE.MakeHitBlood(FPreviousUnit, FStrike.FDamage);
                                        var call__133 = FPreviousUnit.FUnitHS.Yhx;
                                        var call__134 = FPreviousUnit.FUnitHS.Xhx;
                                        HXtoHP(call__134, call__133, ref X, ref Y);
                                        if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                        {
                                            CombatTE.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                        }

                                        if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                        {
                                            CombatTE.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                        }
                                    }

                                    FStep = TTokenStep.tokExeStrike;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token InitStrike", (AnsiString)"Error - TCMelMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokExeStrike:
                        try
                        {
                            if (FStrike.FDamage == 0)
                            {
                                NextStrike();
                            }
                            else
                            {
                                FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                                if (Strike.Side == TCombatSideIndex.csAttacker && FPreviousUnit.FUnitHS.FActionDone)
                                {
                                    AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                    var call__135 = FPreviousUnit.GetEnabled();
                                    if (!call__135)
                                    {
                                        FStep = TTokenStep.tokEnd;
                                        FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
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
                                        AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                        var call__136 = FUnit.GetEnabled();
                                        if (call__136)
                                        {
                                            NextStrike();
                                        }
                                        else
                                        {
                                            Result = TMoveStatus.msDone;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token ExeStrike", (AnsiString)"Error - TCMelMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokEnd:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            Result = TMoveStatus.msDone;
                        }
                        else
                        {
                            Result = TMoveStatus.msMoving;
                        }

                        break;
                }

                if (Result == TMoveStatus.msDone)
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
                        if (FUnit.FCombatPlayer == AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__138 = FUnit.FUnitHS.Xhx;
                            var call__139 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__139.FLastX = call__138;
                            var call__140 = FUnit.FUnitHS.Yhx;
                            var call__141 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__141.FLastY = call__140;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Error during Token", (AnsiString)"Error - TCMelMoveToken", 0);
            }

            return Result;
        }

        // CombatTE.pas:577
        public void NextStrike()
        {
            bool Locked = default;
            TMeleeStrike Strike = default;
            Locked = true;
            while (Locked && FStrikeIndex < FMeleeRound.FCount)
            {
                FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
                if (Strike.Side == TCombatSideIndex.csAttacker)
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
                FStep = TTokenStep.tokDelayStrike;
                FDelay = 3;
            }
            else
            {
                FStep = TTokenStep.tokEnd;
                FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
            }
        }

        // CombatTE.pas:602
        public void ExecuteStrike()
        {
            TMeleeStrike Strike = default;
            if (FStrike != null)
            {
                FStrike.Release();
            }

            var call__144 = AoWHSMap.Random(16777215);
            RandSeed = call__144;
            var call__145 = FMeleeRound.CreateStrikeCA(FStrikeIndex);
            FStrike = call__145;
            FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
            if (Strike.Side == TCombatSideIndex.csAttacker)
            {
                if (FStrike.FDamage == 0)
                {
                    FUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                }
                else
                {
                    FUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                }
            }
            else
            {
                if (FStrike.FDamage == 0)
                {
                    FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                }
                else
                {
                    FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                }
            }

            FStrikeIndex = FStrikeIndex + 1;
            FStep = TTokenStep.tokInitStrike;
        }

        // intf_P5_07.inc:765


    }

    public partial class TCAbTouchMoveTE : TCombatMoveTE
    {
        public TCAbTouchMoveTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:624
        public new const int ClassID = 2236418;

        public TfxHS FEffect;
        public int FAbilityID;
        public TPossessCA FPossessAction;
        public TEntangleCA FEntangleAction;
        public TWebCA FWebAction;
        public TTurnUndeadCA FTurnUndeadAction;
        public THealingCA FHealingAction;
        public TCommandCA FCommandAction;
        public TInvokeDeathCA FDeathAction;
        public TDispelMagicAbilityCA FDispelAction;
        public TfxType FEffectType;
        public bool FSuccessful;
        public int FDamage;
        public TCityWall FWall;
        // CombatTE.pas:627
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAbilityID, 30);
        }

        // CombatTE.pas:633
        public override TMoveStatus LastMove()
        {
            TMoveStatus Result = default;
            TXYZ Point = default;
            int I = default;
            int X = default;
            int Y = default;
            TTacticalCombatUnitHS HS = default;
            TMapFieldMsg Msg = default;
            Result = TMoveStatus.msMoving;
            try
            {
                switch (FStep)
                {
                    case TTokenStep.tokStart:
                        try
                        {
                            FEffectType = AbilTypes[FAbilityID];
                            FUnit.FUnitHS.CancelMove();
                            FUnit.FUnitHS.Unselect();
                            FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                            bool bool__146 = !AoWCombatMap.FExplorationEnabled;
                            if (!bool__146)
                            {
                                var call__147 = FUnit.FUnitHS.Yhx;
                                var call__148 = FUnit.FUnitHS.Xhx;
                                var call__149 = AoWCombatMap.MapContainer.GetField(call__148, call__147, 0);
                                var call__150 = ((TAoWMapField)call__149).GetExplored(AoWCombatMap.FSeatedPlayer);
                                bool__146 = call__150;
                            }

                            if (bool__146)
                            {
                                var call__151 = FUnit.FUnitHS.Yhx;
                                var call__152 = FUnit.FUnitHS.Xhx;
                                AoWCombatMap.MakeVisible(call__152, call__151, 0, 5);
                            }

                            var call__153 = FUnit.FUnitHS.Yhx;
                            var call__154 = FPath[0];
                            var call__155 = FUnit.FUnitHS.Xhx;
                            var call__156 = FPath[0];
                            var call__157 = FUnit.FUnitHS.Xhx;
                            var call__158 = GetMeleeDirIndex(call__157, call__155 - call__156.X, call__153 - call__154.Y);
                            FUnit.FUnitHS.SetDirection((byte)call__158);
                            if (FAbilityID == 65)
                            {
                                FStep = TTokenStep.tokInitStrike;
                            }
                            else
                            {
                                if (FAbilityID == 114)
                                {
                                    FStep = TTokenStep.tokExeStrike;
                                    FUnit.FUnitHS.SetAction(TTCAction.caRoundAttack);
                                }
                                else
                                {
                                    FUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                                    var call__159 = FPath[0];
                                    var call__160 = FPath[0];
                                    var call__161 = AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__162 = call__161.GetField(call__160.X, call__159.Y);
                                    var call__163 = call__162.FindOwnedChild(2228484);
                                    HS = (TTacticalCombatUnitHS)call__163;
                                    if (HS == null)
                                    {
                                        FPreviousUnit = null;
                                    }
                                    else
                                    {
                                        FPreviousUnit = HS.FUnit;
                                    }

                                    if (FPreviousUnit != null && FAbilityID != 117)
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                        if (FPreviousUnit.FUnitHS.FDirection > 6)
                                        {
                                            FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                        }
                                    }
                                    else
                                    {
                                        if (FAbilityID == 117)
                                        {
                                            var call__164 = FPath[0];
                                            var call__165 = FPath[0];
                                            var call__166 = AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__167 = call__166.GetField(call__165.X, call__164.Y);
                                            var call__168 = call__167.FindOwnedChild(2229508);
                                            FWall = (TCityWall)call__168;
                                            if (FWall == null)
                                            {
                                                var call__169 = FPath[0];
                                                var call__170 = FPath[0];
                                                var call__171 = AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__172 = call__171.GetField(call__170.X, call__169.Y);
                                                var call__173 = call__172.FindOwnedChild(2229510);
                                                FWall = (TCityWall)call__173;
                                            }

                                            if (FWall == null && FAbilityID == 117)
                                            {
                                                Result = TMoveStatus.msDone;
                                            }
                                        }
                                        else
                                        {
                                            Result = TMoveStatus.msDone;
                                        }
                                    }

                                    FStep = TTokenStep.tokStepUp;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokStepUp:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone)
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caAbility);
                                FStep = TTokenStep.tokInitStrike;
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token StepUp", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokInitStrike:
                        try
                        {
                            if (FPreviousUnit != null)
                            {
                                if (FUnit.FUnitHS.FActionDone)
                                {
                                    var call__174 = AoWCombatMap.AddEffect(FEffectType);
                                    FEffect = call__174;
                                    FEffect.FAbilityIndex = FAbilityID;
                                    Point = new TXYZ();
                                    var call__175 = FPreviousUnit.FUnitHS.Yhx;
                                    var call__176 = FPreviousUnit.FUnitHS.Xhx;
                                    HXtoHP(call__176, call__175, ref Point.X, ref Point.Y);
                                    Point.X += 16;
                                    FEffect.FPath.Add(Point);
                                    FStep = TTokenStep.tokExeStrike;
                                }
                            }
                            else
                            {
                                if (FWall != null || FAbilityID == 65)
                                {
                                    switch (FAbilityID)
                                    {
                                        case 117:
                                            FStep = TTokenStep.tokEnd;
                                            FDamage = 0;
                                            var call__177 = AoWHSMap.Random(16777215);
                                            RandSeed = call__177;
                                            var call__178 = FWall.GetDefense();
                                            var call__179 = ExecuteDamageRole(TCDamage[0], TCAttack[0] - call__178);
                                            FWall.SetHitPoints(FWall.FHitPoints - call__179);
                                            FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                            break;
                                        case 65:
                                            var call__180 = AoWCombatMap.AddEffect(FEffectType);
                                            FEffect = call__180;
                                            FEffect.FAbilityIndex = FAbilityID;
                                            Point = new TXYZ();
                                            var call__181 = FPath[1];
                                            var call__182 = FPath[1];
                                            HXtoHP(call__182.X, call__181.Y, ref Point.X, ref Point.Y);
                                            Point.X += 16;
                                            FEffect.FPath.Add(Point);
                                            FStep = TTokenStep.tokExeStrike;
                                            break;
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token InitStrike", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokExeStrike:
                        try
                        {
                            if (FEffect == null)
                            {
                                var call__183 = FUnit.GetEnabled();
                                if (!call__183)
                                {
                                    Result = TMoveStatus.msDone;
                                }
                                else
                                {
                                    switch (FAbilityID)
                                    {
                                        case 114:
                                            if (FUnit.FUnitHS.FActionDone || FUnit.FUnitHS.FActionFrame >= 17)
                                            {
                                                Result = TMoveStatus.msDone;
                                            }
                                            else
                                            {
                                                if (raDir[FUnit.FUnitHS.FActionFrame] > 0)
                                                {
                                                    I = (raDir[FUnit.FUnitHS.FActionFrame] + FUnit.FUnitHS.FDirection) % 6 + 1;
                                                    var call__184 = FPath[1];
                                                    var call__185 = FPath[1];
                                                    CenterHNtoHX(call__185.X, call__184.Y, I, ref X, ref Y);
                                                    var call__186 = AoWCombatMap.MapContainer.MapLevels[0];
                                                    var call__187 = call__186.GetField(X, Y);
                                                    var call__188 = call__187.FindOwnedChild(2228484);
                                                    HS = (TTacticalCombatUnitHS)call__188;
                                                    if (HS == null || HS == FUnit.FUnitHS)
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
                                                            var call__190 = AoWHSMap.Random(16777215);
                                                            RandSeed = call__190;
                                                            var call__191 = CreateStrikeCA(FUnit, HS.FUnit, true);
                                                            FStrike = call__191;
                                                            if (FStrike.FDamage > 0)
                                                            {
                                                                HS.SetAction(TTCAction.caHit);
                                                                FUnit.FUnitHS.SetAction(TTCAction.caRoundAttackHit);
                                                                CombatTE.MakeHitBlood(HS.FUnit, FStrike.FDamage);
                                                                AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                                            }
                                                            else
                                                            {
                                                                FUnit.FUnitHS.SetAction(TTCAction.caRoundAttack);
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
                                        Result = TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        switch (FAbilityID)
                                        {
                                            case 40:
                                                var call__193 = AoWHSMap.Random(16777215);
                                                RandSeed = call__193;
                                                var call__194 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__195 = ((TEntangleAbility)call__194).CreateEntangleCA(FUnit, FPreviousUnit);
                                                FEntangleAction = call__195;
                                                FSuccessful = FEntangleAction.FSuccessful;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FEntangleAction);
                                                FEntangleAction.Release();
                                                break;
                                            case 27:
                                                var call__196 = AoWHSMap.Random(16777215);
                                                RandSeed = call__196;
                                                var call__197 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__198 = ((TWebAbility)call__197).CreateWebCA(FUnit, FPreviousUnit);
                                                FWebAction = call__198;
                                                FSuccessful = FWebAction.FSuccessful;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FWebAction);
                                                FWebAction.Release();
                                                break;
                                            case 28:
                                            case 29:
                                            case 148:
                                                var call__199 = AoWHSMap.Random(16777215);
                                                RandSeed = call__199;
                                                var call__200 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__201 = ((TCommandAbility)call__200).CreateCommandCA(FUnit, FPreviousUnit);
                                                FCommandAction = call__201;
                                                FSuccessful = FCommandAction.FSuccessful;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FCommandAction);
                                                FCommandAction.Release();
                                                break;
                                            case 38:
                                                var call__202 = AoWHSMap.Random(16777215);
                                                RandSeed = call__202;
                                                var call__203 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__204 = ((TTurnUndeadAbility)call__203).CreateTurnUndeadCA(FUnit, FPreviousUnit);
                                                FTurnUndeadAction = call__204;
                                                var call__205 = FTurnUndeadAction.GetSuccessfull();
                                                FSuccessful = call__205;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FTurnUndeadAction);
                                                FTurnUndeadAction.Release();
                                                break;
                                            case 35:
                                                var call__206 = AoWHSMap.Random(16777215);
                                                RandSeed = call__206;
                                                var call__207 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__208 = ((TInvokeDeathAbility)call__207).CreateCA(FUnit, FPreviousUnit);
                                                FDeathAction = call__208;
                                                FSuccessful = FDeathAction.FSuccessful;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FDeathAction);
                                                FDeathAction.Release();
                                                break;
                                            case 47:
                                                var call__209 = AoWHSMap.Random(16777215);
                                                RandSeed = call__209;
                                                var call__210 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__211 = ((THealingAbility)call__210).CreateHealingCA(FUnit, FPreviousUnit);
                                                FHealingAction = call__211;
                                                FSuccessful = true;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FHealingAction);
                                                FHealingAction.Release();
                                                break;
                                            case 60:
                                                var call__212 = AoWHSMap.Random(16777215);
                                                RandSeed = call__212;
                                                var call__213 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__214 = ((TDispelMagicAbility)call__213).CreateCA(FUnit, FPreviousUnit);
                                                FDispelAction = call__214;
                                                FSuccessful = FDispelAction.FEnchantmentIDs.Count > 0;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FDispelAction);
                                                FDispelAction.Release();
                                                break;
                                            case 106:
                                            case 107:
                                                var call__215 = AoWHSMap.Random(16777215);
                                                RandSeed = call__215;
                                                var call__216 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                var call__217 = ((TPossessAbility)call__216).CreatePossessCA(FUnit, FPreviousUnit);
                                                FPossessAction = call__217;
                                                FSuccessful = FPossessAction.FSuccessful;
                                                AoWCombatMap.FCombat.ExecuteCombatAction(FPossessAction);
                                                FPossessAction.Release();
                                                var call__218 = FUnit.FUnit.GetUnitGFXResourceIndex();
                                                FUnit.FUnitHS.SetUnitGFXResourceIndex(call__218);
                                                var call__219 = FPreviousUnit.GetEnabled();
                                                if (!call__219)
                                                {
                                                    FUnit.FUnitHS.MoveTo((TMapField)FPreviousUnit.FUnitHS.Owner, 0);
                                                    FUnit.FUnitHS.SetDirection(FPreviousUnit.FUnitHS.FDirection);
                                                    FUnit.FUnitHS.FOffsetX = 0;
                                                    FUnit.FUnitHS.FOffsetY = 0;
                                                    FPreviousUnit.FUnitHS.Free();
                                                    FPreviousUnit.FUnitHS = null;
                                                    Result = TMoveStatus.msDone;
                                                }

                                                break;
                                        }
                                    }

                                    var call__220 = FUnit.GetEnabled();
                                    if (!call__220 || ((FAbilityID == 106 || FAbilityID == 107) && FSuccessful))
                                    {
                                        Result = TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        bool bool__221 = !FSuccessful && FPreviousUnit != null;
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
                                            var call__226 = MeleeDamageTypes(FPreviousUnit);
                                            bool__225 = call__226 != TSet<TDamageType>.Of();
                                        }

                                        bool bool__227 = bool__225;
                                        if (bool__227)
                                        {
                                            var call__228 = FPreviousUnit.GetSide();
                                            var call__229 = FUnit.GetSide();
                                            bool__227 = call__228 != call__229;
                                        }

                                        if (bool__227)
                                        {
                                            var call__230 = AoWHSMap.Random(16777215);
                                            RandSeed = call__230;
                                            var call__231 = CreateStrikeCA(FPreviousUnit, FUnit, true);
                                            FStrike = call__231;
                                            if (FStrike.FDamage == 0)
                                            {
                                                FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                                            }
                                            else
                                            {
                                                FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                                            }

                                            FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                                            FStep = TTokenStep.tokEndStrike;
                                        }
                                        else
                                        {
                                            FStep = TTokenStep.tokEnd;
                                            FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
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
                                                            CenterHNtoHX(call__235.X, call__234.Y, I, ref X, ref Y);
                                                            var call__236 = AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__237 = call__236.GetField(X, Y);
                                                            call__237.SendMapFieldMsg(ref Msg);
                                                            CreateFire(false, 50, X, Y);
                                                        }
                                                    }

                                                    FUnit.FDeathMethod = TTCDeathMethod.dmExplode;
                                                }

                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token ExeStrike", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokEndStrike:
                        try
                        {
                            if (FPreviousUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                    CombatTE.MakeHitBlood(FUnit, FStrike.FDamage);
                                }

                                var call__238 = FUnit.FUnitHS.Yhx;
                                var call__239 = FUnit.FUnitHS.Xhx;
                                HXtoHP(call__239, call__238, ref I, ref X);
                                if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    CombatTE.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    CombatTE.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                }

                                FStep = TTokenStep.tokExeEndStrike;
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token StrikeBack", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokExeEndStrike:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                }

                                var call__240 = FUnit.GetEnabled();
                                if (call__240)
                                {
                                    FStep = TTokenStep.tokEnd;
                                    FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                }
                                else
                                {
                                    Result = TMoveStatus.msDone;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            MessageBox(0, (AnsiString)"Error during Token ExeStrikeBack", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                        }

                        break;
                    case TTokenStep.tokEnd:
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            Result = TMoveStatus.msDone;
                        }

                        break;
                }

                if (Result == TMoveStatus.msDone)
                {
                    var call__241 = FUnit.GetEnabled();
                    if (call__241)
                    {
                        FUnit.FUnitHS.SetAction(TTCAction.caMove);
                        FUnit.FUnitHS.FSelectedAbility = -1;
                        FUnit.FMovePoints = 0;
                        FUnit.FUnitHS.Unselect();
                        if (FUnit.FCombatPlayer == AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__242 = FUnit.FUnitHS.Xhx;
                            var call__243 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__243.FLastX = call__242;
                            var call__244 = FUnit.FUnitHS.Yhx;
                            var call__245 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__245.FLastY = call__244;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Error during Token", (AnsiString)"Error - TCTouchToken " + IntToStr(FAbilityID), 0);
            }

            return Result;
        }

        // intf_P5_07.inc:765


    }

    public partial class TCAbRangedTE : TTokenEvent
    {
        public TCAbRangedTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:917
        public new const int ClassID = 2236419;

        public TTacticalCombatUnit FUnit;
        public int FAbilityID;
        public int FSpellID;
        public ushort FSourceX;
        public ushort FSourceY;
        public ushort FTargetX;
        public ushort FTargetY;
        public TfxType FEffectType;
        public int FStrikeCount;
        public TObject FSound;
        public int FDelay;
        public int FStrikeIndex;
        public int FState;
        // CombatTE.pas:920
        public override void ReadWrite(TEStorageStream Stream)
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
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            int I = default;
            int X = default;
            int Y = default;
            int EffectIndex = default;
            TfxHS Effect = default;
            TfxHS ExtraEffect = default;
            int EndX = default;
            int EndY = default;
            TSpellCombatInfo Info = default;
            TTacticalCombatUnitHS HS = default;
            TCOozeHS Ooze = default;
            TCombatAction Action = default;
            TMapFieldMsg Msg = default;
            base.Execute(TE);
            Result = TExecuteResult.erRetry;
            try
            {
                switch (FState)
                {
                    case 0:
                        try
                        {
                            var call__246 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__247 = call__246.GetField(FSourceX, FSourceY);
                            var call__248 = call__247.FindOwnedChild(2228484);
                            HS = (TTacticalCombatUnitHS)call__248;
                            if (HS == null)
                            {
                                Result = TExecuteResult.erDone;
                            }
                            else
                            {
                                FUnit = HS.FUnit;
                                FUnit.FUnitHS.Unselect();
                                FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                                FUnit.FUnitHS.FSelectedSpell = FSpellID;
                                if (FAbilityID == 52)
                                {
                                    FEffectType = SpellTypes[FSpellID];
                                }
                                else
                                {
                                    FEffectType = AbilTypes[FAbilityID];
                                }

                                if (FAbilityID == 52)
                                {
                                    var call__249 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                    call__249.CombatSpellCast(FUnit);
                                }

                                bool bool__250 = !AoWCombatMap.FExplorationEnabled;
                                if (!bool__250)
                                {
                                    var call__251 = FUnit.FUnitHS.Yhx;
                                    var call__252 = FUnit.FUnitHS.Xhx;
                                    var call__253 = AoWCombatMap.MapContainer.GetField(call__252, call__251, 0);
                                    var call__254 = ((TAoWMapField)call__253).GetExplored(AoWCombatMap.FSeatedPlayer);
                                    bool__250 = call__254;
                                }

                                if (bool__250)
                                {
                                    var call__255 = FUnit.FUnitHS.Yhx;
                                    var call__256 = FUnit.FUnitHS.Xhx;
                                    AoWCombatMap.MakeVisible(call__256, call__255, 0, 2);
                                }

                                switch (FEffectType)
                                {
                                    case TfxType.fxBreath:
                                        FStrikeCount = 20;
                                        break;
                                    case TfxType.fxScreenNESW:
                                        FStrikeCount = 10;
                                        break;
                                    case TfxType.fxScreenNS:
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
                                    case TfxType.fxShockWave:
                                        FTargetX = FSourceX;
                                        FTargetY = FSourceY;
                                        FStrikeCount = 1;
                                        break;
                                    case TfxType.fxTouch:
                                        FStrikeCount = 1;
                                        break;
                                    case TfxType.fxSwarm:
                                    case TfxType.fxScreenSwarm:
                                        FStrikeCount = 30;
                                        break;
                                    default:
                                        if (FAbilityID == 52)
                                        {
                                            var call__257 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                            call__257.GetCombatInfo(FUnit.FUnit, ref Info);
                                            FStrikeCount = Info.RepeatCount;
                                        }
                                        else
                                        {
                                            var call__258 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                            var call__259 = ((TRangedAttackAbility)call__258).GetAttackRepeatRA(FUnit.FUnit);
                                            FStrikeCount = call__259;
                                        }

                                        break;
                                }

                                switch (FEffectType)
                                {
                                    case TfxType.fxTremors:
                                        var call__260 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                        var call__261 = call__260.FCastSounds.PlayEx(0, 64, true, 0, TSamplePanning.spCenter, null);
                                        FSound = call__261;
                                        NextStrike();
                                        break;
                                    case TfxType.fxScreenSwarm:
                                        var call__262 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                        var call__263 = call__262.FCastSounds.PlayEx(2, 64, true, 0, TSamplePanning.spCenter, null);
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
                            MessageBox(0, (AnsiString)"Error during Token Start", (AnsiString)"Error - TCRangedToken " + IntToStr(FAbilityID) + AnsiString.FromBytes(47) + IntToStr(FSpellID), 0);
                        }

                        break;
                    case 1:
                        try
                        {
                            EffectIndex = 0;
                            while (EffectIndex < AoWCombatMap.FEffects.Count)
                            {
                                EndX = -1;
                                EndY = -1;
                                var call__265 = AoWCombatMap.FEffects[EffectIndex];
                                Effect = (TfxHS)call__265;
                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 117)
                                {
                                    if (Effect.FEffectType == TfxType.fxRock)
                                    {
                                        if (Effect.FDone)
                                        {
                                            Effect.FEffectType = TfxType.fxFBExplosion;
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
                                                HPtoHX(call__268.X, call__267.Y, ref EndX, ref EndY);
                                            {
                                                I = 0;
                                                byte end__269 = 6;
                                                for (; I <= end__269; I += 1)
                                                {
                                                    CenterHNtoHX(EndX, EndY, I, ref X, ref Y);
                                                    var call__270 = AoWCombatMap.MapContainer.MapLevels[0];
                                                    var call__271 = call__270.GetField(X, Y);
                                                    call__271.SendMapFieldMsg(ref Msg);
                                                    CreateFire(false, 50, X, Y);
                                                }
                                            }

                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 107)
                                    {
                                        bool bool__272 = Effect.FFrame != -1;
                                        if (bool__272)
                                        {
                                            var call__273 = Effect.FPath[Effect.FFrame];
                                            bool__272 = call__273.Z == 15;
                                        }

                                        if (bool__272)
                                        {
                                            var call__274 = Effect.FPath[Effect.FFrame];
                                            var call__275 = Effect.FPath[Effect.FFrame];
                                            HPtoHX(call__275.X, call__274.Y, ref X, ref Y);
                                            var call__276 = AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__277 = call__276.GetField(X, Y);
                                            var call__278 = call__277.FindOwnedChild(2228484);
                                            HS = (TTacticalCombatUnitHS)call__278;
                                            if (HS != null)
                                            {
                                                var call__279 = AoWHSMap.Random(16777215);
                                                RandSeed = call__279;
                                                var call__280 = AoWHSSet.FSpellControl.GetSpell(Effect.FSpellIndex);
                                                var call__281 = call__280.CreateCA(FUnit, HS.FUnit, false);
                                                Effect.FCombatAction = call__281;
                                                var call__282 = HS.Yhx;
                                                var call__283 = HS.Xhx;
                                                HXtoHP(call__283, call__282, ref I, ref X);
                                                if (((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                {
                                                    CombatTE.MakeDEHSList(I, X, ((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                }

                                                if (((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                {
                                                    CombatTE.MakeDEHSList(I, X, ((TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                }

                                                AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                Effect.FCombatAction.Release();
                                                Effect.FCombatAction = null;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 112)
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
                                                                var call__286 = AoWCombatMap.MapContainer.MapLevels[0];
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
                                            if (Effect.FAbilityIndex == 52 && Effect.FEffectType == TfxType.fxResurrect && (Effect.FSpellIndex == 130 || Effect.FSpellIndex == 129))
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
                                                            CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            var call__289 = AoWCombatMap.MapContainer.MapLevels[0];
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
                                                            CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            var call__292 = AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__293 = call__292.GetField(X, Y);
                                                            call__293.SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 104)
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
                                                                CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                var call__295 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                var call__296 = call__295.GetField(X, Y);
                                                                call__296.SendMapFieldMsg(ref Msg);
                                                            }
                                                        }

                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 127)
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
                                                                    CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__298 = AoWCombatMap.MapContainer.MapLevels[0];
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
                                                                    CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__301 = AoWCombatMap.MapContainer.MapLevels[0];
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
                                                                    CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    var call__304 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__305 = call__304.GetField(X, Y);
                                                                    call__305.SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 128)
                                                        {
                                                            switch (Effect.FFrame)
                                                            {
                                                                case 100:
                                                                    var call__306 = FUnit.FUnitHS.Yhx;
                                                                    var call__307 = FUnit.FUnitHS.Xhx;
                                                                    CreateFire(false, 50, call__307, call__306);
                                                                    Msg.MsgID = 200718;
                                                                    Msg.Param[0] = FUnit;
                                                                    Msg.Result = -1;
                                                                    var call__308 = FUnit.FUnitHS.Yhx;
                                                                    var call__309 = FUnit.FUnitHS.Xhx;
                                                                    var call__310 = AoWCombatMap.MapContainer.MapLevels[0];
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
                                                                        CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                        var call__313 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                        var call__314 = call__313.GetField(X, Y);
                                                                        call__314.SendMapFieldMsg(ref Msg);
                                                                        CreateFire(false, 50, X, Y);
                                                                    }
                                                                }

                                                                    break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 105 && Effect.FTarget != null)
                                                            {
                                                                {
                                                                    I = 0;
                                                                    int end__315 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                                    for (; I <= end__315; I += 1)
                                                                    {
                                                                        var call__316 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                        bool bool__317 = call__316 != null;
                                                                        if (bool__317)
                                                                        {
                                                                            var call__318 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            bool__317 = call__318 is TCombatUnit;
                                                                        }

                                                                        bool bool__319 = bool__317;
                                                                        if (bool__319)
                                                                        {
                                                                            var call__320 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__321 = call__320.GetSide();
                                                                            var call__322 = FUnit.GetSide();
                                                                            bool__319 = call__321 != call__322;
                                                                        }

                                                                        bool bool__323 = bool__319;
                                                                        if (bool__323)
                                                                        {
                                                                            var call__324 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__325 = call__324.GetEnabled();
                                                                            bool__323 = call__325;
                                                                        }

                                                                        bool bool__326 = bool__323;
                                                                        if (bool__326)
                                                                        {
                                                                            var call__327 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            bool__326 = ((TTacticalCombatUnit)call__327).FUnitHS != null;
                                                                        }

                                                                        if (bool__326)
                                                                        {
                                                                            var call__328 = AoWHSMap.Random(16777215);
                                                                            RandSeed = call__328;
                                                                            var call__329 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                            var call__330 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                                                            var call__331 = call__330.CreateCA(FUnit, call__329, false);
                                                                            Action = call__331;
                                                                            if (((TExclusiveCombatSpellCA)Action).FSucceeded)
                                                                            {
                                                                                var call__332 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                ((TTacticalCombatUnit)call__332).FUnitHS.SetAction(TTCAction.caHit);
                                                                            }

                                                                            AoWCombatMap.FCombat.ExecuteCombatAction(Action);
                                                                            Action.Release();
                                                                        }
                                                                    }
                                                                }

                                                                Effect.FTarget = null;
                                                            }
                                                            else
                                                            {
                                                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 120)
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
                                                                                CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                                var call__334 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                var call__335 = call__334.GetField(X, Y);
                                                                                call__335.SendMapFieldMsg(ref Msg);
                                                                            }
                                                                        }

                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Effect.FEffectType == TfxType.fxSacredWrath && Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 125)
                                                                    {
                                                                        switch (Effect.FFrame)
                                                                        {
                                                                            case 5:
                                                                            {
                                                                                I = 0;
                                                                                int end__336 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                                                for (; I <= end__336; I += 1)
                                                                                {
                                                                                    var call__337 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                    bool bool__338 = call__337 is TCombatUnit;
                                                                                    if (bool__338)
                                                                                    {
                                                                                        var call__339 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                        var call__340 = call__339.GetEnabled();
                                                                                        bool__338 = call__340;
                                                                                    }

                                                                                    if (bool__338)
                                                                                    {
                                                                                        var call__341 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                                        HS = ((TTacticalCombatUnit)call__341).FUnitHS;
                                                                                        if (HS != null)
                                                                                        {
                                                                                            var call__342 = AoWCombatMap.AddEffect(TfxType.fxSWComet);
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
                                                                        bool bool__343 = Effect.FDone && Effect.FTarget != null;
                                                                        if (bool__343)
                                                                        {
                                                                            var call__344 = Effect.FTarget.GetEnabled();
                                                                            bool__343 = call__344;
                                                                        }

                                                                        if (bool__343)
                                                                        {
                                                                            I = 0;
                                                                            var call__345 = AoWHSMap.Random(16777215);
                                                                            RandSeed = call__345;
                                                                            if (FAbilityID == 52)
                                                                            {
                                                                                var call__346 = AoWHSSet.FSpellControl.GetSpell(FSpellID);
                                                                                var call__347 = call__346.CreateCA(FUnit, Effect.FTarget, false);
                                                                                Effect.FCombatAction = call__347;
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FAbilityID == 47)
                                                                                {
                                                                                    var call__348 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                    var call__349 = ((THealingAbility)call__348).CreateHealingCA(FUnit, Effect.FTarget);
                                                                                    Effect.FCombatAction = call__349;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (FAbilityID == 60)
                                                                                    {
                                                                                        var call__350 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                        var call__351 = ((TDispelMagicAbility)call__350).CreateCA(FUnit, Effect.FTarget);
                                                                                        Effect.FCombatAction = call__351;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        var call__352 = AoWHSSet.FAbilityControl.GetAbility(FAbilityID);
                                                                                        var call__353 = ((TRangedAttackAbility)call__352).CreateRangedAttackCA(FUnit, Effect.FTarget, 0);
                                                                                        Effect.FCombatAction = call__353;
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (Effect.FCombatAction is TDamageCA)
                                                                            {
                                                                                I = ((TDamageCA)Effect.FCombatAction).FDamage;
                                                                                var call__354 = ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.Yhx;
                                                                                var call__355 = ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.Xhx;
                                                                                HXtoHP(call__355, call__354, ref X, ref Y);
                                                                                if (((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                                                {
                                                                                    CombatTE.MakeDEHSList(X, Y, ((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                                                }

                                                                                if (((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                                                {
                                                                                    CombatTE.MakeDEHSList(X, Y, ((TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Effect.FCombatAction is TEntangleSpellCA)
                                                                                {
                                                                                    if (((TEntangleSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                    {
                                                                                        ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Effect.FCombatAction is TMindDecayCA && ((TMindDecayCA)Effect.FCombatAction).FSucceeded)
                                                                                    {
                                                                                        ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (I > 0)
                                                                            {
                                                                                ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                                CombatTE.MakeHitBlood((TTacticalCombatUnit)Effect.FTarget, I);
                                                                                Effect.FCancelled = true;
                                                                            }
                                                                        }

                                                                        if (Effect.FDone)
                                                                        {
                                                                            var call__356 = Effect.FPath.Count;
                                                                            var call__357 = Effect.FPath[call__356 - 1];
                                                                            var call__358 = Effect.FPath.Count;
                                                                            var call__359 = Effect.FPath[call__358 - 1];
                                                                            HPtoHX(call__359.X, call__357.Y, ref EndX, ref EndY);
                                                                            if (Effect.FCombatAction != null && Effect.FTarget != null)
                                                                            {
                                                                                AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                                                Effect.FCombatAction.Release();
                                                                                Effect.FCombatAction = null;
                                                                                var call__360 = Effect.FTarget.GetEnabled();
                                                                                if (!call__360)
                                                                                {
                                                                                    {
                                                                                        I = 0;
                                                                                        int end__361 = AoWCombatMap.FEffects.Count - 1;
                                                                                        for (; I <= end__361; I += 1)
                                                                                        {
                                                                                            if (I != EffectIndex)
                                                                                            {
                                                                                                var call__362 = AoWCombatMap.FEffects[I];
                                                                                                if (((TfxHS)call__362).FTarget == Effect.FTarget)
                                                                                                {
                                                                                                    var call__363 = AoWCombatMap.FEffects[I];
                                                                                                    ((TfxHS)call__363).FTarget = null;
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
                                                                                                var call__364 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                var call__365 = call__364.GetField(Effect.FTargetX, Effect.FTargetY);
                                                                                                call__365.SendMapFieldMsg(ref Msg);
                                                                                            }

                                                                                            break;
                                                                                        case 24:
                                                                                            Msg.MsgID = 200705;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__366 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__367 = call__366.GetField(EndX, EndY);
                                                                                            call__367.SendMapFieldMsg(ref Msg);
                                                                                            break;
                                                                                        case 44:
                                                                                            Msg.MsgID = 200706;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__368 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__369 = call__368.GetField(EndX, EndY);
                                                                                            call__369.SendMapFieldMsg(ref Msg);
                                                                                            break;
                                                                                        case 31:
                                                                                            Msg.MsgID = 200714;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Result = -1;
                                                                                            var call__370 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                            var call__371 = call__370.GetField(EndX, EndY);
                                                                                            call__371.SendMapFieldMsg(ref Msg);
                                                                                            CreateFire(false, 100, EndX, EndY);
                                                                                            break;
                                                                                        case 52:
                                                                                            switch (Effect.FEffectType)
                                                                                            {
                                                                                                case TfxType.fxBreath:
                                                                                                    if (Effect.FTargetX > 0)
                                                                                                    {
                                                                                                        Msg.MsgID = 200723;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Param[1] = Effect.FSpellIndex;
                                                                                                        Msg.Result = -1;
                                                                                                        var call__372 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                        var call__373 = call__372.GetField(Effect.FTargetX, Effect.FTargetY);
                                                                                                        call__373.SendMapFieldMsg(ref Msg);
                                                                                                    }

                                                                                                    break;
                                                                                                case TfxType.fxScreenNS:
                                                                                                    switch (Effect.FSpellIndex)
                                                                                                    {
                                                                                                        case 116:
                                                                                                            Msg.MsgID = 200710;
                                                                                                            Msg.Param[0] = FUnit;
                                                                                                            Msg.Result = -1;
                                                                                                            var call__374 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__375 = call__374.GetField(EndX, EndY);
                                                                                                            call__375.SendMapFieldMsg(ref Msg);
                                                                                                            break;
                                                                                                        case 124:
                                                                                                            var call__376 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__377 = call__376.GetField(EndX, EndY);
                                                                                                            bool bool__378 = call__377.TerrainType != 0;
                                                                                                            if (bool__378)
                                                                                                            {
                                                                                                                var call__379 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__380 = call__379.GetField(EndX, EndY);
                                                                                                                bool__378 = call__380.TerrainType != 10;
                                                                                                            }

                                                                                                            bool bool__381 = bool__378;
                                                                                                            if (bool__381)
                                                                                                            {
                                                                                                                var call__382 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__383 = call__382.GetField(EndX, EndY);
                                                                                                                bool__381 = call__383.OverlayType != 2;
                                                                                                            }

                                                                                                            bool bool__384 = bool__381;
                                                                                                            if (bool__384)
                                                                                                            {
                                                                                                                var call__385 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__386 = call__385.GetField(EndX, EndY);
                                                                                                                bool__384 = call__386.OverlayType != 11;
                                                                                                            }

                                                                                                            bool bool__387 = bool__384;
                                                                                                            if (bool__387)
                                                                                                            {
                                                                                                                var call__388 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__389 = call__388.GetField(EndX, EndY);
                                                                                                                bool__387 = call__389.OverlayType != 8;
                                                                                                            }

                                                                                                            bool bool__390 = bool__387;
                                                                                                            if (bool__390)
                                                                                                            {
                                                                                                                var call__391 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__392 = call__391.GetField(EndX, EndY);
                                                                                                                bool__390 = call__392.TerrainType != 9;
                                                                                                            }

                                                                                                            bool bool__393 = bool__390;
                                                                                                            if (bool__393)
                                                                                                            {
                                                                                                                var call__394 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__395 = call__394.GetField(EndX, EndY);
                                                                                                                bool__393 = call__395.TerrainType != 15;
                                                                                                            }

                                                                                                            bool bool__396 = bool__393;
                                                                                                            if (bool__396)
                                                                                                            {
                                                                                                                var call__397 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                                var call__398 = call__397.GetField(EndX, EndY);
                                                                                                                bool__396 = call__398.TerrainType != 7;
                                                                                                            }

                                                                                                            if (bool__396)
                                                                                                            {
                                                                                                                Ooze = new TCOozeHS(null);
                                                                                                                AoWCombatMap.PlaceHX(Ooze, EndX, EndY, 0);
                                                                                                            }

                                                                                                            break;
                                                                                                    }

                                                                                                    break;
                                                                                                case TfxType.fxEffect:
                                                                                                    switch (Effect.FSpellIndex)
                                                                                                    {
                                                                                                        case 113:
                                                                                                            Msg.MsgID = 200715;
                                                                                                            Msg.Param[0] = FUnit;
                                                                                                            Msg.Result = -1;
                                                                                                            var call__399 = AoWCombatMap.MapContainer.MapLevels[0];
                                                                                                            var call__400 = call__399.GetField(EndX, EndY);
                                                                                                            call__400.SendMapFieldMsg(ref Msg);
                                                                                                            CreateFire(false, 100, EndX, EndY);
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

                            bool bool__401 = AoWCombatMap.FEffects.Count == 0;
                            if (bool__401)
                            {
                                bool bool__402 = FStrikeIndex >= FStrikeCount;
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
                                    FUnit.FUnitHS.SetAction(TTCAction.caMove);
                                }

                                Result = TExecuteResult.erDone;
                            }

                            var call__405 = FUnit.GetEnabled();
                            if (call__405)
                            {
                                if (FEffectType == TfxType.fxBreath)
                                {
                                    NextStrike();
                                }
                                else
                                {
                                    if (FUnit.FUnitHS.FActionDone || (FAbilityID == 52 && FDelay <= 0))
                                    {
                                        var call__406 = AoWHSMap.Random(3);
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
                            MessageBox(0, (AnsiString)"Error during Token Execute", (AnsiString)"Error - TCRangedToken " + IntToStr(FAbilityID) + AnsiString.FromBytes(47) + IntToStr(FSpellID), 0);
                        }

                        break;
                }

                if (Result == TExecuteResult.erDone)
                {
                    if (FSound != null)
                    {
                        FSound.Free();
                    }

                    bool bool__407 = FUnit != null;
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
                        if (FUnit.FCombatPlayer == AoWCombatMap.FCurrentCombatPlayer)
                        {
                            var call__409 = FUnit.FUnitHS.Xhx;
                            var call__410 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__410.FLastX = call__409;
                            var call__411 = FUnit.FUnitHS.Yhx;
                            var call__412 = AoWCombatMap.FPlayers.GetPlayers(AoWCombatMap.FCurrentCombatPlayer);
                            call__412.FLastY = call__411;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Error during Token", (AnsiString)"Error - TCRangedToken " + IntToStr(FAbilityID) + AnsiString.FromBytes(47) + IntToStr(FSpellID), 0);
            }

            return Result;
        }

        // CombatTE.pas:1373
        public void NextStrike()
        {
            TList Used = default;
            TTacticalCombatUnit Target = default;
            TfxHS Effect = default;
            TfxHS ExtraEffect = default;
            TTacticalCombatUnitHS HS = default;
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
            TXYZ Point = default;
            THexagonSprite HitObject = default;
            TXYZList Path = default;
            TList Obstacles = default;
            TROb Obstacle = default;
            bool Found = default;
            TTacticalCombatUnit FindTarget(int X, int Y)
            {
                TTacticalCombatUnit Result = default;
                int I = default;
                int Distance = default;
                int BestDistance = default;
                int J = default;
                bool Available = default;
                Result = null;
                BestDistance = 6;
                {
                    I = 0;
                    int end__413 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__413; I += 1)
                    {
                        var call__414 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__415 = call__414 is TCombatUnit;
                        if (bool__415)
                        {
                            var call__416 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__417 = call__416.GetEnabled();
                            bool__415 = call__417;
                        }

                        bool bool__418 = bool__415;
                        if (bool__418)
                        {
                            var call__419 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__420 = call__419.GetSide();
                            var call__421 = FUnit.GetSide();
                            bool__418 = call__420 != call__421;
                        }

                        if (bool__418)
                        {
                            Available = true;
                            {
                                J = 0;
                                int end__422 = Used.Count - 1;
                                for (; J <= end__422; J += 1)
                                {
                                    var call__423 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    var call__424 = Used[J];
                                    if (call__423 == call__424)
                                    {
                                        Available = false;
                                    }
                                }
                            }

                            if (Available)
                            {
                                var call__425 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__426 = ((TTacticalCombatUnit)call__425).FUnitHS.Yhx;
                                var call__427 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__428 = ((TTacticalCombatUnit)call__427).FUnitHS.Xhx;
                                var call__429 = dHXtoRad(X, Y, call__428, call__426);
                                Distance = call__429;
                                if (Distance < BestDistance)
                                {
                                    var call__430 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    Result = (TTacticalCombatUnit)call__430;
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
                TList Weights = default;
                Used = new TList();
                Weights = new TList();
                Total = 0;
                {
                    I = 0;
                    int end__431 = AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                    for (; I <= end__431; I += 1)
                    {
                        var call__432 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        bool bool__433 = call__432 is TCombatUnit;
                        if (bool__433)
                        {
                            var call__434 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__435 = call__434.GetEnabled();
                            bool__433 = call__435;
                        }

                        bool bool__436 = bool__433;
                        if (bool__436)
                        {
                            var call__437 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            bool__436 = call__437 != Target;
                        }

                        if (bool__436)
                        {
                            var call__438 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__439 = ((TTacticalCombatUnit)call__438).FUnitHS.Yhx;
                            var call__440 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            var call__441 = ((TTacticalCombatUnit)call__440).FUnitHS.Xhx;
                            var call__442 = dHXtoRad(FTargetX, FTargetY, call__441, call__439);
                            Distance = call__442;
                            if (Distance < 13)
                            {
                                Total = Total + (13 - Distance);
                                var call__443 = AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                Used.Add(call__443);
                                Weights.Add(IntToObject(13 - Distance));
                            }
                        }
                    }
                }

                if (Target != null)
                {
                    Used.Add(Target);
                    Weights.Add(IntToObject(Total));
                    Total = Total + Total;
                }

                if (Used.Count > 0)
                {
                    var call__444 = AoWHSMap.Random(Total);
                    var call__445 = Weights[0];
                    I = call__444 - ObjectToInt((TObject)call__445);
                    Index = 0;
                    while (Index < Used.Count - 1 && I > 0)
                    {
                        Index = Index + 1;
                        var call__446 = Weights[Index];
                        I = I - ObjectToInt((TObject)call__446);
                    }

                    var call__447 = Used[Index];
                    Target = (TTacticalCombatUnit)call__447;
                }

                Used.Free();
                Weights.Free();
            }

            if (FStrikeIndex >= FStrikeCount)
            {
                return;
            }

            var call__448 = AoWCombatMap.AddEffect(TfxType.fxNone);
            Effect = call__448;
            Obstacles = new TList();
            switch (FEffectType)
            {
                case TfxType.fxBreath:
                    var call__449 = FUnit.FUnitHS.Yhx;
                    var call__450 = FUnit.FUnitHS.Xhx;
                    var call__451 = dHXtoHN(call__450, call__449, FTargetX, FTargetY);
                    var call__452 = FUnit.FUnitHS.Yhx;
                    var call__453 = FUnit.FUnitHS.Xhx;
                    CenterHNtoHX(call__453, call__452, BreathHN[BreathDirection[call__451 - 1] * 12 - 3], ref J, ref K);
                    var call__454 = FUnit.FUnitHS.Yhx;
                    var call__455 = FUnit.FUnitHS.Xhx;
                    HXtoHP(call__455, call__454, ref X1, ref Y1);
                    HXtoHP(J, K, ref X2, ref Y2);
                    Sweep = FStrikeIndex;
                    if ((double)Sweep > 9)
                    {
                        Sweep = (float)((double)19 - Sweep);
                    }

                    if (Y2 == Y1)
                    {
                        A = 0;
                    }
                    else
                    {
                        if (X2 == X1)
                        {
                            A = (float)((double)Math.PI / 2);
                        }
                        else
                        {
                            A = (float)Math.Atan((double)Math.Abs(X2 - X1) / Math.Abs(Y2 - Y1));
                        }
                    }

                    if (Y2 == Y1 || X2 == X1)
                    {
                        var call__456 = Math.PI;
                        var call__457 = Math.PI;
                        Angle = (float)((double)((double)((double)call__456 / 2) - A) + (double)((double)((double)((double)Sweep * 6) - 30) * call__457) / 180);
                    }
                    else
                    {
                        Angle = (float)((double)A + (double)((double)((double)((double)Sweep * 6) - 30) * Math.PI) / 180);
                    }

                    Radius = (float)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
                    if (X2 >= X1)
                    {
                        DX = Round((double)Math.Sin(Angle) * Radius) - X2 + X1;
                    }
                    else
                    {
                        DX = X1 - X2 - Round((double)Math.Sin(Angle) * Radius);
                    }

                    if (Y2 >= Y1)
                    {
                        DY = Round((double)Math.Cos(Angle) * Radius) - Y2 + Y1;
                    }
                    else
                    {
                        DY = Y1 - Y2 - Round((double)Math.Cos(Angle) * Radius);
                    }

                    var call__458 = FUnit.FUnitHS.Xhx;
                    var call__459 = FUnit.FUnitHS.Yhx;
                    var call__460 = FUnit.GetSide();
                    MakeRangedPath(call__460, 50, 20, 50, call__458, call__459, (short)J, (short)K, (short)DX, (short)DY, false, ref Effect.FPath, ref Obstacles);
                    if (FStrikeIndex < 12)
                    {
                        var call__461 = FUnit.FUnitHS.Yhx;
                        var call__462 = FUnit.FUnitHS.Xhx;
                        var call__463 = dHXtoHN(call__462, call__461, FTargetX, FTargetY);
                        var call__464 = FUnit.FUnitHS.Yhx;
                        var call__465 = FUnit.FUnitHS.Xhx;
                        CenterHNtoHX(call__465, call__464, BreathHN[BreathDirection[call__463 - 1] * 12 + BreathHit[FStrikeIndex] - 12], ref Effect.FTargetX, ref Effect.FTargetY);
                    }

                    break;
                case TfxType.fxTouch:
                case TfxType.fxEffect:
                case TfxType.fxSacredWrath:
                case TfxType.fxGeyser:
                case TfxType.fxCloud:
                case TfxType.fxMindDecay:
                case TfxType.fxShockWave:
                case TfxType.fxSacFlame:
                    if (FEffectType == TfxType.fxSacredWrath)
                    {
                        FTargetX = 32;
                        FTargetY = 29;
                    }

                    HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    switch (FEffectType)
                    {
                        case TfxType.fxGeyser:
                            AoWCombatMap.CenterViewTo(FTargetX, FTargetY - 5, 0);
                            break;
                        case TfxType.fxMindDecay:
                            Effect.FTargetX = FTargetX;
                            Effect.FTargetY = FTargetY;
                            AoWCombatMap.CenterViewTo(FTargetX + 1, FTargetY - 4, 0);
                            break;
                        case TfxType.fxShockWave:
                            var call__466 = FUnit.FUnitHS.Yhx;
                            var call__467 = FUnit.FUnitHS.Xhx;
                            AoWCombatMap.CenterViewTo(call__467, call__466, 0);
                            break;
                        case TfxType.fxSacFlame:
                            var call__468 = FUnit.FUnitHS.Yhx;
                            var call__469 = FUnit.FUnitHS.Xhx;
                            AoWCombatMap.CenterViewTo(call__469, call__468 - 3, 0);
                            break;
                        default:
                            AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                            break;
                    }

                    Point = new TXYZ();
                    Point.X = X2 + 16;
                    Point.Y = Y2 + 16;
                    Effect.FPath.Add(Point);
                    var call__470 = AoWCombatMap.MapContainer.MapLevels[0];
                    var call__471 = call__470.GetField(FTargetX, FTargetY);
                    var call__472 = call__471.FindOwnedChild(2228484);
                    HS = (TTacticalCombatUnitHS)call__472;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    break;
                case TfxType.fxWinds:
                    HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Point = new TXYZ();
                    Point.X = X2 + 16;
                    Point.Y = Y2 + 16;
                    Effect.FPath.Add(Point);
                    var call__473 = AoWCombatMap.MapContainer.MapLevels[0];
                    var call__474 = call__473.GetField(FTargetX, FTargetY);
                    var call__475 = call__474.FindOwnedChild(2228484);
                    HS = (TTacticalCombatUnitHS)call__475;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    var call__476 = AoWHSMap.Random(37);
                    I = call__476 + 1;
                    Found = false;
                    while (!Found)
                    {
                        CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                        bool bool__477 = J > 5 && J < 60 && K > 5 && K < 53;
                        if (bool__477)
                        {
                            var call__478 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__479 = call__478.GetField(J, K);
                            var call__480 = call__479.FindOwnedChild(2228484);
                            bool__477 = call__480 == null;
                        }

                        bool bool__481 = bool__477;
                        if (bool__481)
                        {
                            var call__482 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__483 = call__482.GetField(J, K);
                            bool bool__484 = call__483.OverlayType != 8;
                            if (!bool__484)
                            {
                                var call__485 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__486 = call__485.GetField(J, K);
                                var call__487 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__488 = call__487.GetField(FTargetX, FTargetY);
                                bool__484 = call__486.OverlayType == call__488.OverlayType;
                            }

                            bool__481 = bool__484;
                        }

                        bool bool__489 = bool__481;
                        if (bool__489)
                        {
                            var call__490 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__491 = call__490.GetField(J, K);
                            bool__489 = call__491.TerrainType != 15;
                        }

                        bool bool__492 = bool__489;
                        if (bool__492)
                        {
                            var call__493 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__494 = call__493.GetField(J, K);
                            bool__492 = call__494.TerrainType != 7;
                        }

                        bool bool__495 = bool__492;
                        if (bool__495)
                        {
                            var call__496 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__497 = call__496.GetField(J, K);
                            bool bool__498 = call__497.TerrainType != 0;
                            if (bool__498)
                            {
                                var call__499 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__500 = call__499.GetField(J, K);
                                bool__498 = call__500.TerrainType != 10;
                            }

                            bool bool__501 = bool__498;
                            if (bool__501)
                            {
                                var call__502 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__503 = call__502.GetField(J, K);
                                bool__501 = call__503.TerrainType != 9;
                            }

                            bool bool__504 = bool__501;
                            if (!bool__504)
                            {
                                var call__505 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__506 = call__505.GetField(J, K);
                                var call__507 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__508 = call__507.GetField(FTargetX, FTargetY);
                                bool__504 = call__506.TerrainType == call__508.TerrainType;
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

                    HXtoHP(J, K, ref DX, ref DY);
                    AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                {
                    I = 0;
                    byte end__509 = 29;
                    for (; I <= end__509; I += 1)
                    {
                        var call__510 = new TXYZ();
                        Effect.FPath.Add(call__510);
                        var call__511 = Effect.FPath[0];
                        var call__512 = Random(30);
                        var call__513 = Effect.FPath.Count;
                        var call__514 = Effect.FPath[call__513 - 1];
                        call__514.X = call__511.X + call__512 - 15;
                        var call__515 = Effect.FPath[0];
                        var call__516 = Random(30);
                        var call__517 = Effect.FPath.Count;
                        var call__518 = Effect.FPath[call__517 - 1];
                        call__518.Y = call__515.Y + call__516 - 15;
                    }
                }

                    J = Random(300);
                    K = Random(300);
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
                    MakeArcPathXY((short)call__530.X, (short)(call__524.Y - call__526.Z), (short)(call__528.X + J - 150), (short)(call__520.Y - call__522.Z + K - 150), (short)((Math.Abs(J - 150) + Math.Abs(K - 150)) / 50 + 2), 100, ref Effect.FPath);
                    var call__531 = Effect.FPath.Count;
                    Effect.FTargetX = call__531;
                    J = Random(200);
                    K = Random(200);
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
                    MakeArcPathXY((short)call__543.X, (short)(call__537.Y - call__539.Z), (short)(call__541.X + J - 100), (short)(call__533.Y - call__535.Z + K - 100), (short)((Math.Abs(J - 100) + Math.Abs(K - 100)) / 50 + 3), 100, ref Effect.FPath);
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
                    MakeArcPathXY((short)call__556.X, (short)(call__552.Y - call__554.Z), (short)(DX + 16), (short)(DY + 16), (short)((Math.Abs(call__546.Y - call__548.Z - DY) + Math.Abs(call__550.X - DX)) / 50 + 4), 100, ref Effect.FPath);
                    break;
                case TfxType.fxScreenNESW:
                    Point = new TXYZ();
                    if (Random(100) < 50)
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
                case TfxType.fxScreenNS:
                    switch (FSpellID)
                    {
                        case 124:
                            I = 19;
                            break;
                        default:
                            I = 7;
                            break;
                    }

                    CenterHNtoHX(FTargetX, FTargetY, rndHex[FStrikeIndex % I], ref DX, ref DY);
                    HXtoHP(DX, DY, ref X2, ref Y2);
                    AoWCombatMap.MakeVisible(DX, DY, 0, 5);
                    X1 = Random(20) + 6;
                    Y1 = Random(20) + 6;
                    MakeLinePath(X2 + X1 - 100, Y2 + Y1 - 300, X2 + X1, Y2 + Y1, 30, ref Effect.FPath);
                    switch (FSpellID)
                    {
                        case 124:
                            Effect.FTarget = null;
                            break;
                        default:
                        {
                            var call__557 = AoWCombatMap.MapContainer.MapLevels[0];
                            var call__558 = call__557.GetField(DX, DY);
                            var call__559 = call__558.FindOwnedChild(2228484);
                            HS = (TTacticalCombatUnitHS)call__559;
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                            break;
                    }

                    break;
                case TfxType.fxChain:
                    var call__560 = FUnit.FUnitHS.Yhx;
                    var call__561 = FUnit.FUnitHS.Xhx;
                    HXtoHP(call__561, call__560, ref X1, ref Y1);
                    HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                    var call__562 = Effect.FPath.Count;
                    var call__563 = Effect.FPath[call__562 - 1];
                    call__563.Z = 16;
                    I = 3;
                    Used = new TList();
                    Used.Add(FUnit);
                    var call__564 = AoWCombatMap.MapContainer.MapLevels[0];
                    var call__565 = call__564.GetField(FTargetX, FTargetY);
                    var call__566 = call__565.FindOwnedChild(2228484);
                    HS = (TTacticalCombatUnitHS)call__566;
                    if (HS != null)
                    {
                        Used.Add(HS.FUnit);
                    }

                    var call__567 = FindTarget(FTargetX, FTargetY);
                    Target = call__567;
                    while (Target != null && I > 0)
                    {
                        Used.Add(Target);
                        X1 = X2 + 16;
                        Y1 = Y2 + 16;
                        var call__568 = Target.FUnitHS.Yhx;
                        var call__569 = Target.FUnitHS.Xhx;
                        HXtoHP(call__569, call__568, ref X2, ref Y2);
                        MakeLightningPath(X1, Y1, X2 + 16, Y2, ref Effect.FPath);
                        I -= 1;
                        var call__570 = Target.FUnitHS.Yhx;
                        var call__571 = Target.FUnitHS.Xhx;
                        var call__572 = FindTarget(call__571, call__570);
                        Target = call__572;
                    }

                    Used.Free();
                    break;
                case TfxType.fxSwarm:
                    var call__573 = FUnit.FUnitHS.Yhx;
                    var call__574 = FUnit.FUnitHS.Xhx;
                    HXtoHP(call__574, call__573, ref X1, ref Y1);
                    var call__575 = Random(360);
                    var call__576 = Random(20);
                    var call__577 = Random(20);
                    var call__578 = Random(2);
                    var call__579 = Random(90);
                    var call__580 = Random(5);
                    var call__581 = Random(10);
                    MakeSwarmPath(call__581 + 10, call__580 + 20, call__579, call__575, call__576 + X1 + 6, call__577 + Y1 - 10, (TSwarmDirection)call__578, ref Effect.FPath);
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
                                Point = new TXYZ();
                                var call__585 = Effect.FPath[J];
                                Point.X = call__585.X;
                                var call__586 = Effect.FPath[J];
                                Point.Y = call__586.Y;
                                Effect.FPath.Add(Point);
                            }
                        }
                    }
                }

                    HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    var call__587 = Effect.FPath.Count;
                    var call__588 = Effect.FPath[call__587 - 1];
                    var call__589 = Effect.FPath.Count;
                    var call__590 = Effect.FPath[call__589 - 1];
                    MakeLinePath(call__590.X, call__588.Y, X2 + 16, Y2, 15, ref Effect.FPath);
                    Path = new TXYZList();
                    var call__591 = Random(360);
                    var call__592 = Effect.FPath.Count;
                    var call__593 = Effect.FPath[call__592 - 1];
                    var call__594 = Effect.FPath.Count;
                    var call__595 = Effect.FPath[call__594 - 1];
                    var call__596 = Random(2);
                    var call__597 = Random(90);
                    var call__598 = Random(5);
                    var call__599 = Random(10);
                    MakeSwarmPath(call__599 + 10, call__598 + 20, call__597, call__591, call__593.X, call__595.Y, (TSwarmDirection)call__596, ref Path);
                    var call__600 = Path[0];
                    var call__601 = Effect.FPath.Count;
                    var call__602 = Effect.FPath[call__601 - 1];
                    var call__603 = Effect.FPath.Count;
                    var call__604 = Effect.FPath[call__603 - 1];
                    var call__605 = Path[0];
                    MakeLinePath(call__604.X, call__602.Y, call__605.X, call__600.Y, 5, ref Effect.FPath);
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
                                Point = new TXYZ();
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
                case TfxType.fxScreenSwarm:
                    var call__613 = AoWCombatMap.MapContainer.MapLevels[0];
                    var call__614 = call__613.GetField(FTargetX, FTargetY);
                    var call__615 = call__614.FindOwnedChild(2228484);
                    HS = (TTacticalCombatUnitHS)call__615;
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
                    HXtoHP(DX, DY, ref X2, ref Y2);
                    if (FStrikeIndex == 0)
                    {
                        AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                    }

                    Point = new TXYZ();
                    Point.X = X2;
                    Point.Y = Y2;
                    Effect.FPath.Add(Point);
                    var call__618 = Random(360);
                    var call__619 = Random(20);
                    var call__620 = Random(20);
                    var call__621 = Random(2);
                    var call__622 = Random(90);
                    var call__623 = Random(10);
                    var call__624 = Random(10);
                    MakeSwarmPath(call__624 + 10, call__623 + 25, call__622, call__618, call__619, call__620 - 10, (TSwarmDirection)call__621, ref Effect.FPath);
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
                                Point = new TXYZ();
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
                        call__631.Z = 100 - I * 2;
                    }
                }

                    break;
                case TfxType.fxFrostBeam:
                    HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                    Effect.FTargetX = FTargetX;
                    Effect.FTargetY = FTargetY;
                    AoWCombatMap.CenterViewTo(FTargetX - 3, FTargetY - 3, 0);
                    MakeArcPathXY((short)(X2 - 200), (short)(Y2 - 200), (short)(X2 + 16), (short)(Y2 + 16), 5, 100, ref Effect.FPath);
                    break;
                case TfxType.fxRay:
                    AoWCombatMap.MakeVisible(FTargetX - 3, FTargetY - 3, 0, 3);
                    var call__632 = FUnit.FUnitHS.Xhx;
                    var call__633 = FUnit.FUnitHS.Yhx;
                    var call__634 = FUnit.GetSide();
                    MakeRangedPath(call__634, 10, 0, 50, call__632, call__633, (short)FTargetX, (short)FTargetY, 0, 0, true, ref Effect.FPath, ref Obstacles);
                    break;
                default:
                    var call__635 = FUnit.FUnitHS.Xhx;
                    var call__636 = FUnit.FUnitHS.Yhx;
                    var call__637 = FUnit.GetSide();
                    MakeAbilPath(call__637, (short)FAbilityID, (short)FSpellID, call__635, call__636, (short)FTargetX, (short)FTargetY, ref Effect.FPath, ref Obstacles);
                    break;
            }

            Effect.FAbilityIndex = FAbilityID;
            Effect.FSpellIndex = FSpellID;
            Effect.FEffectType = FEffectType;
            switch (FEffectType)
            {
                case TfxType.fxArrow:
                case TfxType.fxRock:
                case TfxType.fxComet:
                case TfxType.fxBreath:
                case TfxType.fxCometBall:
                case TfxType.fxRay:
                case TfxType.fxSwarm:
                case TfxType.fxAltArrow:
                    var call__638 = AoWCombatMap.MapContainer.MapLevels[0];
                    var call__639 = call__638.GetField(FTargetX, FTargetY);
                    var call__640 = call__639.FindOwnedChild(2228484);
                    HS = (TTacticalCombatUnitHS)call__640;
                    if (HS != null)
                    {
                        Effect.FTarget = HS.FUnit;
                    }

                    I = 0;
                    while (I < Obstacles.Count)
                    {
                        var call__641 = Obstacles[I];
                        Obstacle = (TROb)call__641;
                        var call__642 = AoWHSMap.Random(100);
                        if (call__642 < Obstacle.Chance)
                        {
                            var call__643 = Effect.FPath[Obstacle.PathIndex];
                            var call__644 = Effect.FPath[Obstacle.PathIndex];
                            HPtoHX(call__644.X, call__643.Y, ref J, ref K);
                            if (Effect.FEffectType == TfxType.fxBreath)
                            {
                                HitObject = null;
                            }
                            else
                            {
                                var call__645 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__646 = call__645.GetField(J, K);
                                var call__647 = call__646.FindOwnedChild(2228484);
                                HitObject = (THexagonSprite)call__647;
                            }

                            if (HitObject == null)
                            {
                                var call__648 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__649 = call__648.GetField(J, K);
                                var call__650 = call__649.FindOwnedChild(2228582);
                                HitObject = (THexagonSprite)call__650;
                            }

                            if (HitObject == null)
                            {
                                var call__651 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__652 = call__651.GetField(J, K);
                                var call__653 = call__652.FindOwnedChild(2229508);
                                HitObject = (THexagonSprite)call__653;
                            }

                            if (HitObject == null)
                            {
                                var call__654 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__655 = call__654.GetField(J, K);
                                var call__656 = call__655.FindOwnedChild(2229510);
                                HitObject = (THexagonSprite)call__656;
                            }

                            bool bool__657 = HitObject != null;
                            if (!bool__657)
                            {
                                var call__658 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__659 = call__658.GetField(J, K);
                                bool__657 = call__659.TerrainType == 15;
                            }

                            bool bool__660 = bool__657;
                            if (!bool__660)
                            {
                                var call__661 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__662 = call__661.GetField(J, K);
                                bool__660 = call__662.TerrainType == 7;
                            }

                            bool bool__663 = bool__660;
                            if (!bool__663)
                            {
                                var call__664 = AoWCombatMap.MapContainer.MapLevels[0];
                                var call__665 = call__664.GetField(J, K);
                                bool__663 = call__665.OverlayType == 8;
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

                                if (!(HitObject is TTacticalCombatUnitHS))
                                {
                                    Effect.FTarget = null;
                                    if (Effect.FEffectType == TfxType.fxBreath)
                                    {
                                        Effect.FTargetX = 0;
                                    }
                                }
                                else
                                {
                                    Effect.FTarget = ((TTacticalCombatUnitHS)HitObject).FUnit;
                                }

                                I = Obstacles.Count;
                            }
                        }

                        I += 1;
                    }

                    break;
            }

            if (FEffectType == TfxType.fxBreath)
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
                                var call__668 = AoWHSMap.Random(100);
                                if (call__668 < 75)
                                {
                                    Effect.FTarget = null;
                                }

                                break;
                            case 100:
                                var call__669 = AoWCombatMap.AddEffect(TfxType.fxSFOverlay);
                                ExtraEffect = call__669;
                                var call__670 = FUnit.FUnitHS.Yhx;
                                var call__671 = FUnit.FUnitHS.Xhx;
                                HXtoHP(call__671, call__670, ref X1, ref Y1);
                                while (true)
                                {
                                    var call__672 = ExtraEffect.FPath.Count;
                                    var call__673 = Effect.FPath.Count;
                                    if (!(call__672 < call__673))
                                        break;
                                    {
                                        Point = new TXYZ();
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
            var call__676 = GetRangedDirIndex(FTargetX - call__675, FTargetY - call__674);
            Effect.FVariant = call__676;
            if (FStrikeIndex == 0)
            {
                switch (FEffectType)
                {
                    case TfxType.fxArrow:
                    case TfxType.fxRock:
                    case TfxType.fxComet:
                    case TfxType.fxBreath:
                    case TfxType.fxCometBall:
                    case TfxType.fxEffect:
                    case TfxType.fxRay:
                    case TfxType.fxChain:
                    case TfxType.fxSwarm:
                    case TfxType.fxScreenSwarm:
                    case TfxType.fxSFOverlay:
                    case TfxType.fxDispel:
                    case TfxType.fxAltArrow:
                    case TfxType.fxGeyser:
                    case TfxType.fxFrostBeam:
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

            if (TSet<TfxType>.Of(TfxType.fxRock, TfxType.fxComet, TfxType.fxBreath, TfxType.fxCometBall, TfxType.fxScreenNS).Contains(FEffectType))
            {
                Effect.FVariant = 0;
            }

            if (FEffectType == TfxType.fxBreath && FStrikeIndex == 0)
            {
                FUnit.FUnitHS.SetAction(TTCAction.caBreathAttack);
            }
            else
            {
                if ((FAbilityID != 52 && AbilTypes[FAbilityID] != TfxType.fxBreath) || FStrikeIndex == 0)
                {
                    FUnit.FUnitHS.SetAction(TTCAction.caAbility);
                }
            }

            {
                I = 0;
                int end__677 = Obstacles.Count - 1;
                for (; I <= end__677; I += 1)
                {
                    var call__678 = Obstacles[I];
                    ((TObject)call__678).Free();
                }
            }

            Obstacles.Free();
            FStrikeIndex += 1;
        }

        // NetworkE.pas:230


    }

    public partial class TTerminateCombatTE : TTokenEvent
    {

        public TTerminateCombatTE(TECustomNode owner) : base(owner)
        {}

        // CombatTE.pas:149
        public new const int ClassID = 2236422;

        // CombatTE.pas:152
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = default;
            AoWHSMap.DestroyCombat();
            Result = TExecuteResult.erDone;
            return Result;
        }

        // NetworkE.pas:230


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
        public static void MakeDEHSList(int X, int Y, TSet<TDamageType> DamageTypes, bool Resisted)
        {
            void MakeEffect(TfxType EffectType)
            {
                TfxHS Effect = default;
                int I = default;
                int Count = default;
                float Z = default;
                float Speed = default;
                float VZ = default;
                float Gravity = default;
                Count = Random(5) + 8;
                {
                    I = 1;
                    int end__679 = Count - 1;
                    for (; I <= end__679; I += 1)
                    {
                        var call__680 = AoWCombatMap.AddEffect(EffectType);
                        Effect = call__680;
                        Speed = (float)((double)(Random(10) + 10) * 0.1);
                        Gravity = (float)0.5;
                        if (Resisted || EffectType == TfxType.fxDamMagic)
                        {
                            VZ = (float)((double)(Random(50) + 50) * 0.1);
                        }
                        else
                        {
                            VZ = (float)((double)(Random(25) + 25) * 0.1);
                        }

                        Z = 0;
                        MakeArcPath(X, Y, I * 360 / Count, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                    }
                }
            }

            X = X + 16;
            if (DamageTypes.Contains(TDamageType.dtLightning))
            {
                MakeEffect(TfxType.fxDamLightning);
            }

            if (DamageTypes.Contains(TDamageType.dtFire))
            {
                MakeEffect(TfxType.fxDamFire);
            }

            if (DamageTypes.Contains(TDamageType.dtPoison))
            {
                MakeEffect(TfxType.fxDamPoison);
            }

            if (DamageTypes.Contains(TDamageType.dtMagic))
            {
                MakeEffect(TfxType.fxDamMagic);
            }

            if (DamageTypes.Contains(TDamageType.dtHoly))
            {
                MakeEffect(TfxType.fxDamHoly);
            }

            if (DamageTypes.Contains(TDamageType.dtDeath))
            {
                MakeEffect(TfxType.fxDamDeath);
            }

            if (DamageTypes.Contains(TDamageType.dtCold))
            {
                MakeEffect(TfxType.fxDamCold);
            }
        }

        // CombatTE.pas:1809
        public static TCEndCombatTE CreateTCEndCombatTE()
        {
            TCEndCombatTE Result = default;
            Result = new TCEndCombatTE(null);
            return Result;
        }

        // CombatTE.pas:1812
        public static TCEndTurnTE CreateTCEndTurnTE(sbyte Player)
        {
            TCEndTurnTE Result = default;
            bool bool__681 = Player != -1;
            if (bool__681)
            {
                var call__682 = AoWHSMap.FTokenManager.PlayerReady(Player);
                bool__681 = call__682;
            }

            if (bool__681)
            {
                Result = new TCEndTurnTE(null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // CombatTE.pas:1819
        public static TCEndTurnTE CreateTCCancelEndTurnTE(sbyte Player)
        {
            TCEndTurnTE Result = default;
            var call__683 = AoWHSMap.FPlayers.GetPlayers(Player);
            var call__684 = call__683.GetBusy();
            if (!call__684)
            {
                Result = new TCEndTurnTE(null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // CombatTE.pas:1826
        public static void MakeHitBlood(TTacticalCombatUnit UnitObject, int Damage)
        {
            TfxHS Effect = default;
            TXYZ Point = default;
            var call__685 = AoWCombatMap.AddEffect(TfxType.fxBlood);
            Effect = call__685;
            Point = new TXYZ();
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
                case TBloodType.btNone:
                    Point.Z = Point.Z + 5;
                    break;
            }

            var call__687 = UnitObject.FUnitHS.Yhx;
            var call__688 = UnitObject.FUnitHS.Xhx;
            HXtoHP(call__688, call__687, ref Point.X, ref Point.Y);
            var call__689 = Random(10);
            Point.X = call__689 + Point.X + 11;
            var call__690 = UnitObject.FUnit.GetUnitSize();
            var call__691 = Random(10);
            Point.Y = call__691 + Point.Y + 9 - (int)call__690 * 10;
        }

        // CombatTE.pas:1845
        public static TCMeleeMoveTE CreateTCMeleeMoveTE(TTacticalCombatUnit UnitObject, TMovepath Path)
        {
            TCMeleeMoveTE Result = default;
            Result = new TCMeleeMoveTE(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
            }

            return Result;
        }

        // CombatTE.pas:1851
        public static TCAbTouchMoveTE CreateTCAbTouchMoveTE(TTacticalCombatUnit UnitObject, TMovepath Path, int AbilityID)
        {
            TCAbTouchMoveTE Result = default;
            Result = new TCAbTouchMoveTE(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
                Result.FAbilityID = AbilityID;
            }

            return Result;
        }

        // CombatTE.pas:1861
        public static TCAbRangedTE CreateTCAbRangedTE(TTacticalCombatUnit UnitObject, ushort X, ushort Y, int AbilityID, int SpellID)
        {
            TCAbRangedTE Result = default;
            Result = new TCAbRangedTE(null);
            if (Result != null)
            {
                var call__692 = UnitObject.FUnitHS.Xhx;
                Result.FSourceX = (ushort)call__692;
                var call__693 = UnitObject.FUnitHS.Yhx;
                Result.FSourceY = (ushort)call__693;
                Result.FTargetX = X;
                Result.FTargetY = Y;
                Result.FAbilityID = AbilityID;
                Result.FSpellID = SpellID;
            }

            return Result;
        }

        // CombatTE.pas:1875
        public static TCombatMoveTE CreateCombatMoveTE(TTacticalCombatUnit UnitObject, TMovepath Path)
        {
            TCombatMoveTE Result = default;
            Result = new TCombatMoveTE(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
            }

            return Result;
        }

        // CombatTE.pas:1881
        public static bool MoveCombatUnit(TTacticalCombatUnit UnitObject, TMovepath Path)
        {
            bool Result = default;
            int MovePoints = default;
            int Cost = default;
            int Index = default;
            TCombatMoveTE Event = default;
            TMovepath NewPath = default;
            int I = default;
            Result = false;
            var call__694 = UnitObject.GetPlayer();
            var call__695 = AoWHSMap.FTokenManager.PlayerReady(call__694);
            if (call__695 && Path.Count > 1)
            {
                MovePoints = UnitObject.FMovePoints;
                Cost = 0;
                Index = Path.Count;
                while (Index != 0 && Cost <= MovePoints)
                {
                    Index = Index - 1;
                    var call__696 = Path[Index];
                    Cost = Cost + call__696.Flag;
                }

                if (Cost > MovePoints)
                {
                    Index = Index + 1;
                }

                if (Index < Path.Count - 1)
                {
                    NewPath = new TMovepath();
                    {
                        I = Index;
                        int end__697 = Path.Count - 1;
                        for (; I <= end__697; I += 1)
                        {
                            var call__698 = Path[I];
                            NewPath.Add(call__698);
                        }
                    }

                    Event = CreateCombatMoveTE(UnitObject, NewPath);
                    if (Event != null)
                    {
                        var call__699 = UnitObject.GetPlayer();
                        var call__700 = AoWHSMap.FTokenManager.GetTokenControl(call__699);
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
        public static bool SetupMultiTCUnitMove(TMoveSettingsList Moves)
        {
            bool Result = default;
            TMultiMoveTE Multi = default;
            TCombatMoveTE MoveEvent = default;
            TMovepath Path = default;
            int I = default;
            int J = default;
            int K = default;
            int L = default;
            var call__701 = Moves.Count;
            bool bool__702 = call__701 != 0;
            if (bool__702)
            {
                var call__703 = Moves.GetMoveSettings(0);
                var call__704 = ((TTacticalCombatUnitHS)call__703.FMoveObject).FUnit.GetPlayer();
                var call__705 = AoWHSMap.FTokenManager.PlayerReady(call__704);
                bool__702 = call__705;
            }

            if (bool__702)
            {
                Path = new TMovepath();
                Multi = new TMultiMoveTE(null);
                Multi.FSimultaneous = false;
                Moves.CalculateMovePositions(ValidateTCMovePosition);
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
                        var moveSettings = Moves.GetMoveSettings(K);
                        if (moveSettings.FMovePos < moveSettings.FPath.Count - 1)
                        {
                            Path.Clear();
                            {
                                L = moveSettings.FMovePos;
                                int end__712 = moveSettings.FPath.Count - 1;
                                for (; L <= end__712; L += 1)
                                {
                                    var call__713 = moveSettings.FPath[L];
                                    Path.Add(call__713);
                                }
                            }

                            MoveEvent = new TCombatMoveTE(null);
                            var call__714 = MoveEvent.Setup(((TTacticalCombatUnitHS)moveSettings.FMoveObject).FUnit, Path);
                            if (call__714)
                            {
                                Multi.FEvents.Add(MoveEvent);
                            }

                            MoveEvent.Release();
                        }
                    }
                }

                var call__715 = Multi.FEvents.Count;
                if (call__715 != 0)
                {
                    var call__716 = Moves.GetMoveSettings(0);
                    var call__717 = ((TTacticalCombatUnitHS)call__716.FMoveObject).FUnit.GetPlayer();
                    var call__718 = AoWHSMap.FTokenManager.GetTokenControl(call__717);
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
                    int end__719 = AoWCombatMap.FUnitSelection.FMoveSettingsList.Count - 1;
                    for (; K <= end__719; K += 1)
                    {
                        var moveSettings2 = AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(K);
                        moveSettings2.FPath.Clear();
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
        public static void RegisterCombatTE(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatMoveTE>(), EClass.Of<TCMeleeMoveTE>(), EClass.Of<TCAbTouchMoveTE>(), EClass.Of<TCAbRangedTE>(), EClass.Of<TCEndTurnTE>(), EClass.Of<TCEndCombatTE>(), EClass.Of<TTerminateCombatTE>()]);
        }

        // CombatTE.pas:1916
        public static bool ValidateTCMovePosition(TMoveSettingsList Moves, int Index)
        {
            bool Result = default;
            int I = default;
            TMoveSettings Settings = default;
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
                                bool__722 = call__724 >= call__725;
                            }

                            bool bool__726 = bool__722;
                            if (bool__726)
                            {
                                var call__727 = Moves.GetMoveSettings(Index);
                                var call__728 = call__727.GetMoveLoc();
                                var call__729 = Settings.GetMoveLoc();
                                bool__726 = call__729.XYL == call__728.XYL;
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
