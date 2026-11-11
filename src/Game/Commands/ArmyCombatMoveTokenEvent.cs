// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Commands;

// combat mode, combat type, ignore defenders
public partial class ArmyCombatMoveTokenEvent : MoveArmyTokenEvent
{
    public ArmyCombatMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135426;

    public Mode FCombatMode;
    public CombatType FCombatType;
    public bool FIgnoreDefenders;

    // MoveT.PAS:2513
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FCombatType, 1), 1, 50);
    }

    // MoveT.PAS:3085
    public override MoveStatus ProcessMove()
    {
        // { Retain Result assignment and place combat execution first. }
        MoveStatus Result = MoveStatus.Moving;
        if (FCombatMode == Mode.RestoringHotseat)
        {
            return MoveStatus.Moving;
        }

        if (FCombatMode == Mode.SetupHotseat)
        {
            Result = MoveStatus.Moving;
        }
        else
        {
            if (FCombatMode == Mode.CancelCombat)
            {
                Result = MoveStatus.Done;
            }
            else
            {
                if (FCombatMode != Mode.RequestingCombatType)
                {
                    if (FCombatMode != Mode.SetupReady)
                    {
                        Result = base.ProcessMove();
                    }

                    if (FCombatMode == Mode.SetupReady)
                    {
                        if (FCombatType != CombatType.None)
                        {
                            ExecuteCombat();
                            if (FArmyHS == null)
                            {
                                Result = MoveStatus.Done;
                            }
                            else
                            {
                                Result = MoveStatus.Moving;
                            }
                        }
                        else
                        {
                            Result = MoveStatus.Done;
                        }
                    }
                }
            }
        }

        return Result;
    }

    // MoveT.PAS:2519
    public override bool Validate() => base.Validate();

    // MoveT.PAS:2522
    public override void InitializeMove()
    {
        base.InitializeMove();
    }

    // MoveT.PAS:3030
    public override bool Animating()
    {
        bool Result;
        AoWMapField Field;
        if (FMoveIndex == 1 && FCombatMode != Mode.CombatExecuted)
        {
            if (FArmyHS.GetMoveStatus() != -1)
            {
                Field = GetMapF(FPath[FMoveIndex - 1]);
                if (Field.GetPlayer() != -1)
                {
                    Result = FArmyHS.GetMoveStatus() < 50;
                }
                else
                {
                    Result = FArmyHS.GetMoveStatus() < 100;
                }
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = base.Animating();
        }

        return Result;
    }

    // MoveT.PAS:3046
    public override bool CanMoveNext()
    {
        if (FMoveIndex == 1)
        {
            // { Evaluate the field without retaining a second reference. }
            var player = GetMapF(FPath[0]).GetPlayer();
            if (FArmyHS.FArmy.FPlayer != player)
            {
                return AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FGameOverStatus == GameOverStatus.None;
            }
        }

        return base.CanMoveNext();
    }

    // MoveT.PAS:3061
    public override MoveStatus ExecuteMove()
    {
        MoveStatus Result;
        AoWMapField Field;
        sbyte Player;
        int Direction;
        if (FMoveIndex == 1 && FCombatMode == Mode.Moving)
        {
            Field = GetMapF(FPath[0]);
            Player = Field.GetPlayer();
            if (Player != -1 && Player != FArmyHS.FArmy.FPlayer && !FIgnoreDefenders)
            {
                Direction = GetDirection(FMoveIndex + 1);
                if (Direction == 0)
                {
                    Direction = GetHT(FMoveIndex + 1);
                }

                if (Direction > 0 && Direction <= 6)
                {
                    FArmyHS.SetDirection((byte)Direction);
                }
                else
                {
                    FArmyHS.SetDirection(0);
                }

                SetupCombat();
                Result = MoveStatus.Moving;
            }
            else
            {
                Result = base.ExecuteMove();
            }
        }
        else
        {
            Result = base.ExecuteMove();
        }

        return Result;
    }

    // MoveT.PAS:2504
    public override bool Setup(ArmyHexSprite ArmyHS, byte Selection, MovePath Path, Set<ArmyMoveModeElement> Mode)
    {
        bool Result = base.Setup(ArmyHS, Selection, Path, Mode);
        if (Result && ArmyHS.FArmy.FPlayer == FTargetPlayer)
        {
            Result = false;
        }

        bool condition = Result && ArmyHS.FArmy.FPlayer > 0 && FTargetPlayer > 0;
        if (condition)
        {
            var diplomaticRelation = AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FDiplomaticRelations.GetDiplomaticRelation(FTargetPlayer);
            condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(diplomaticRelation);
        }

        if (condition)
        {
            Result = false;
        }

        return Result;
    }

    // MoveT.PAS:2525
    public void RequestCombatTypeCallBack(EventLog Log)
    {
        CombatFlowControlTokenEvent Flow;
        CombatTypeRequestEventLog Request;
        ArmyCombatMoveTokenEvent Move;
        Request = (CombatTypeRequestEventLog)Log;
        FCombatType = Request.FCombatType;
        if (Request.FCreateNewMove)
        {
            if (FCombatType != CombatType.None)
            {
                Flow = new CombatFlowControlTokenEvent(null);
                Flow.FMode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.UnlockAIExecuter, CombatFlowControlModeElement.ActivateTC, CombatFlowControlModeElement.EnableTurnTimer);
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
                {
                    Flow.FMode = Flow.FMode + Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch);
                }

                Flow.FPlayer = Request.FPlayer;
                AoWHSMap.Current.FTokenManager.GetTokenControl(AoWHSMap.Current.FSeatedPlayer).AddEvent(Flow);
                Flow.Release();
                if (Request.FArmyID.RetrieveArmy(ref FArmyHS, ref FSelection))
                {
                    while (FPath.Count > 2)
                    {
                        FPath.Delete(2);
                    }

                    if (FPath.Count == 2)
                    {
                        FPath.List[1].Flag = 0;
                    }

                    Move = (ArmyCombatMoveTokenEvent)ArmyHexSprite.CreateArmyMoveTE(FArmyHS, FSelection, FPath, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                    if (Move != null)
                    {
                        // { created combat move }
                        Move.FCombatType = ((CombatTypeRequestEventLog)Log).FCombatType;
                        AoWHSMap.Current.FTokenManager.GetTokenControl(AoWHSMap.Current.FSeatedPlayer).AddEvent(Move);
                        Move.Release();
                    }
                    else
                    {
                        GameTables.AoWEngine.OutputDebugStr("Cannot create CombatMoveTE");
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
                {
                    Flow = new CombatFlowControlTokenEvent(null);
                    Flow.FMode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch, CombatFlowControlModeElement.UnlockAIExecuter, CombatFlowControlModeElement.EnableTurnTimer);
                    AoWHSMap.Current.FTokenManager.GetTokenControl(AoWHSMap.Current.FSeatedPlayer).AddEvent(Flow);
                    Flow.Release();
                }
            }
        }
        else
        {
            FCombatMode = Mode.SetupReady;
        }
    }

    // MoveT.PAS:2568
    public void RestoreHotseatMode()
    {
        NewTurnEventLog Log;
        AoWHSMap.Current.SetSeatedPlayer(-1);
        Log = new NewTurnEventLog(null);
        Log.FTurnPlayer = AoWHSMap.Current.FCurrentPlayer;
        Log.FMode = NewTurnEventLog.Mode.Dialog;
        Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
        if (AoWHSMap.Current.FCurrentPlayer == 0)
        {
            Log.FText = AbilityRules.TranslateRStr(P5IndependentsAreMovingRStr);
        }
        else
        {
            var name = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).GetName();
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(P5PlayerIsMovingRStr), [name]);
        }

        AoWHSMap.Current.TriggerExecuteEventLog(Log, true, null);
        Log.Release();
    }

    // MoveT.PAS:2584
    public void RestoreHotSeatModeCallBack(EventLog Log)
    {
        AoWHSMap.Current.SynchroniseBegin();
        RestoreHotseatMode();
        AoWHSMap.Current.FAIExecuter.Unlock();
        AoWHSMap.Current.UnlockGameOverEvent();
        FCombatMode = Mode.Moving;
        AoWHSMap.Current.SynchroniseEnd();
    }

    // MoveT.PAS:2594
    public void ShowHotSeatPlayerCallBack(EventLog Log)
    {
        AoWHSMap.Current.SetSeatedPlayer(((NewTurnEventLog)Log).FTurnPlayer);
        if (FCombatMode == Mode.SetupHotseat)
        {
            FCombatMode = Mode.SetupReady;
        }
    }

    // MoveT.PAS:2600
    public void SetupCombat()
    {
        AoWMapField Field = default;
        sbyte Player = default;
        Combat.Battle Combat = default;
        MapFieldMessage Msg = default;
        CombatTypeRequestEventLog Request = default;
        NewTurnEventLog Log = default;
        Field = GetMapF(FPath[0]);
        if (FCombatType == CombatType.None)
        {
            if (AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local)
            {
                if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFC)
                {
                    FCombatType = CombatType.Fast;
                    FCombatMode = Mode.SetupReady;
                }
                else
                {
                    if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTC)
                    {
                        bool condition = AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human;
                        if (!condition)
                        {
                            bool test = Field.GetPlayer() > 0;
                            if (test)
                            {
                                var player = Field.GetPlayer();
                                test = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType == PlayerType.Human;
                            }

                            condition = test;
                        }

                        if (condition)
                        {
                            FCombatType = CombatType.Tactical;
                            FCombatMode = Mode.SetupReady;
                        }
                        else
                        {
                            FCombatType = CombatType.Fast;
                            FCombatMode = Mode.SetupReady;
                        }
                    }
                    else
                    {
                        // { Set the mode in both branches, as in the original. }
                        FCombatMode = Mode.RequestingCombatType;
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local && (!AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FPlayers.FLocalPlayers.Count == 1 || AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human))
                {
                    if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFC)
                    {
                        FCombatType = CombatType.Fast;
                        FCombatMode = Mode.SetupReady;
                    }
                    else
                    {
                        if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTC)
                        {
                            bool condition = AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human;
                            if (!condition)
                            {
                                bool test = Field.GetPlayer() > 0;
                                if (test)
                                {
                                    var player = Field.GetPlayer();
                                    test = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType == PlayerType.Human;
                                }

                                condition = test;
                            }

                            if (condition)
                            {
                                FCombatType = CombatType.Tactical;
                                FCombatMode = Mode.SetupReady;
                            }
                            else
                            {
                                FCombatType = CombatType.Fast;
                                FCombatMode = Mode.SetupReady;
                            }
                        }
                        else
                        {
                            // { Set the mode in both branches, as in the original. }
                            FCombatMode = Mode.RequestingCombatType;
                        }
                    }
                }
                else
                {
                    bool condition = AoWHSMap.Current.FConnectionType == PlayerConnectionType.Email && AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human && Field.GetPlayer() >= 0;
                    if (condition)
                    {
                        var player = Field.GetPlayer();
                        condition = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType != PlayerType.Human;
                    }

                    if (condition)
                    {
                        if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFC)
                        {
                            FCombatType = CombatType.Fast;
                            FCombatMode = Mode.SetupReady;
                        }
                        else
                        {
                            if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTC)
                            {
                                bool test = AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human;
                                if (!test)
                                {
                                    bool passed = Field.GetPlayer() > 0;
                                    if (passed)
                                    {
                                        var player = Field.GetPlayer();
                                        passed = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType == PlayerType.Human;
                                    }

                                    test = passed;
                                }

                                if (test)
                                {
                                    FCombatType = CombatType.Tactical;
                                    FCombatMode = Mode.SetupReady;
                                }
                                else
                                {
                                    FCombatType = CombatType.Fast;
                                    FCombatMode = Mode.SetupReady;
                                }
                            }
                            else
                            {
                                // { Set the mode in both branches, as in the original. }
                                FCombatMode = Mode.RequestingCombatType;
                            }
                        }
                    }
                    else
                    {
                        // { Original tests connection type here, not play mode. }
                        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network)
                        {
                            bool test = AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human;
                            if (!test)
                            {
                                bool passed = Field.GetPlayer() > 0;
                                if (passed)
                                {
                                    var player = Field.GetPlayer();
                                    passed = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType == PlayerType.Human;
                                }

                                test = passed;
                            }

                            if (test)
                            {
                                switch (AoWHSMap.Current.FVictoryMode)
                                {
                                    case 0:
                                        FCombatType = CombatType.Fast;
                                        FCombatMode = Mode.SetupReady;
                                        break;
                                    case 2:
                                        FCombatMode = Mode.RequestingCombatType;
                                        break;
                                    case 1:
                                        bool passed = AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human && Field.GetPlayer() > 0;
                                        if (passed)
                                        {
                                            var player = Field.GetPlayer();
                                            passed = AoWHSMap.Current.FPlayers.GetPlayers(player).FPlayerType == PlayerType.Human;
                                        }

                                        if (passed)
                                        {
                                            FCombatMode = Mode.RequestingCombatType;
                                        }
                                        else
                                        {
                                            FCombatType = CombatType.Fast;
                                            FCombatMode = Mode.SetupReady;
                                        }

                                        break;
                                    default:
                                        {
                                            // { The original tests the always-available case before the human-only case. }
                                            FCombatType = CombatType.Fast;
                                            FCombatMode = Mode.SetupReady;
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                FCombatType = CombatType.Fast;
                                FCombatMode = Mode.SetupReady;
                            }
                        }
                        else
                        {
                            FCombatType = CombatType.Fast;
                            FCombatMode = Mode.SetupReady;
                        }
                    }
                }
            }
        }
        else
        {
            FCombatMode = Mode.SetupReady;
        }

        if (FCombatMode == Mode.RequestingCombatType || (FCombatType == CombatType.Tactical && FCombatMode == Mode.SetupReady))
        {
            Combat = new Combat.Battle(null);
            var yhx = FArmyHS.GetYhx();
            var lhx = FArmyHS.GetLhx();
            var player = Field.GetPlayer();
            var independentRelation = Field.GetIndependentRelation();
            var item = FPath[0];
            var fPathItem = FPath[0];
            var item2 = FPath[0];
            var xhx = FArmyHS.GetXhx();
            Combat.Setup((byte)FArmyHS.FArmy.FPlayer, (byte)FArmyHS.FArmy.FIndependentRelation, xhx, yhx, lhx, (byte)player, independentRelation, item.X, fPathItem.Y, item2.L, false);
            Msg.MsgID = 4359;
            Msg.Param[0] = Combat;
            Msg.Param[1] = FArmyHS;
            Msg.Result = 0;
            Field.SendMapFieldMsg(ref Msg);
            if (Msg.Result.Int == 0)
            {
                var fArmyHSLhx = FArmyHS.GetLhx();
                var fArmyHSYhx = FArmyHS.GetYhx();
                var fArmyHSXhx = FArmyHS.GetXhx();
                Combat.InitDefaultArmies(FArmyHS.FArmy, fArmyHSXhx, fArmyHSYhx, fArmyHSLhx);
            }

            if (!Combat.DefendersAvailable() || !Combat.PlayerTypeInvolved(Set<PlayerType>.Of(PlayerType.Human)))
            {
                FCombatType = CombatType.Fast;
                FCombatMode = Mode.SetupReady;
            }

            Combat?.Dispose();
        }

        // { Original uses connection type here too. }
        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network && FCombatMode == Mode.RequestingCombatType)
        {
            AoWHSMap.Current.DisableTurnTimer();
            AoWHSMap.Current.FAIExecuter.Lock();
            if (AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType == PlayerType.Human)
            {
                Player = FArmyHS.FArmy.FPlayer;
            }
            else
            {
                Player = Field.GetPlayer();
            }

            if (AoWHSMap.Current.FTokenManager.GetTokenControl(Player).Local())
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(Player).ClearEvents();
            }

            AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = true;
            AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(Player);
            if (AoWHSMap.Current.FSeatedPlayer == Player)
            {
                Request = new CombatTypeRequestEventLog(null);
                Request.FPlayer = FArmyHS.FArmy.FPlayer;
                Request.FDefenderPlayer = Field.GetPlayer();
                Request.FX = Field.Xhx;
                Request.FY = Field.Yhx;
                Request.FL = Field.Lhx;
                Request.SetTokenEvent(this);
                Request.FCreateNewMove = true;
                Request.FArmyID = FArmyHS.FArmy.CreateID(FSelection);
                if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.Choose)
                {
                    if (AoWHSMap.Current.FSeatedPlayer != Request.FPlayer)
                    {
                        var lhx = FArmyHS.GetLhx();
                        var yhx = FArmyHS.GetYhx();
                        var xhx = FArmyHS.GetXhx();
                        AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
                    }

                    FinishMove();
                    FArmyHS = null;
                    AoWHSMap.Current.TriggerExecuteEventLog(Request, false, RequestCombatTypeCallBack);
                }
                else
                {
                    FinishMove();
                    FArmyHS = null;
                    if (GameTables.AoWEngine.FRegistry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTC)
                    {
                        Request.Select(CombatType.Tactical);
                    }
                    else
                    {
                        Request.Select(CombatType.Fast);
                    }

                    RequestCombatTypeCallBack(Request);
                }

                Request.Release();
            }
            else
            {
                FinishMove();
                FArmyHS = null;
            }

            FCombatMode = Mode.CancelCombat;
        }

        if (FCombatMode == Mode.RequestingCombatType && AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType != PlayerType.Human)
        {
            Log = new NewTurnEventLog(null);
            Log.FTurnPlayer = Field.GetPlayer();
            Log.FMode = NewTurnEventLog.Mode.Dialog;
            Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
            Log.FText = AbilityRules.TranslateRStr(P5YouAreBeingAttackedRStr);
            AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ShowHotSeatPlayerCallBack);
            Log.Release();
        }
        else
        {
            if (FCombatMode == Mode.SetupReady && FCombatType == CombatType.Tactical && AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FPlayers.GetPlayers(FArmyHS.FArmy.FPlayer).FPlayerType != PlayerType.Human)
            {
                FCombatMode = Mode.SetupHotseat;
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = Field.GetPlayer();
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                Log.FText = AbilityRules.TranslateRStr(P5YouAreBeingAttackedRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ShowHotSeatPlayerCallBack);
                Log.Release();
            }
        }

        if (FCombatMode == Mode.RequestingCombatType)
        {
            FArmyHS.PauseMove();
            Request = new CombatTypeRequestEventLog(null);
            Request.FPlayer = FArmyHS.FArmy.FPlayer;
            Request.FDefenderPlayer = Field.GetPlayer();
            Request.FX = Field.Xhx;
            Request.FY = Field.Yhx;
            Request.FL = Field.Lhx;
            if (AoWHSMap.Current.FSeatedPlayer != Request.FPlayer)
            {
                var lhx = FArmyHS.GetLhx();
                var yhx = FArmyHS.GetYhx();
                var xhx = FArmyHS.GetXhx();
                AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            }

            AoWHSMap.Current.TriggerExecuteEventLog(Request, false, RequestCombatTypeCallBack);
            Request.Release();
        }
    }

    // MoveT.PAS:2801
    public void CombatExecuted(Combat.Battle Combat)
    {
        ArmyHexSprite ArmyHS;
        Army Army;
        byte Selection;
        MoveArmyTokenEvent Move;
        AoWMapField Field;
        if (!(Combat is FastBattle))
        {
            if (Set<CombatResult>.Of(CombatResult.Victory).Contains(Combat.FCombatResult))
            {
                ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(Combat.FCombatData.FAttackerX, Combat.FCombatData.FAttackerY, Combat.FCombatData.FAttackerL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    Army = ArmyHS.FArmy;
                    Selection = 0;
                    for (int I = Army.Count - 1; I >= 0; I--)
                    {
                        Selection = (byte)(Selection << 1);
                        if (Army.GetUnit(I).FUnitStatus.Contains(UnitStatusElement.Moving))
                        {
                            Selection = (byte)(Selection | 1);
                            var unit = Army.GetUnit(I);
                            Army.GetUnit(I).FUnitStatus = unit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                        }
                    }

                    if (Selection != 0)
                    {
                        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(Combat.FCombatData.FX, Combat.FCombatData.FY, Combat.FCombatData.FL);
                        if (Field.GetPlayer() == ArmyHS.FArmy.FPlayer)
                        {
                            Move = CreateArmyMoveXYLTE(ArmyHS, Selection, (sbyte)Combat.FCombatData.FX, (sbyte)Combat.FCombatData.FY, (sbyte)Combat.FCombatData.FL, MoveAction.Normal, Set<ArmyMoveModeElement>.Of());
                        }
                        else
                        {
                            Move = CreateArmyMoveXYLTE(ArmyHS, Selection, (sbyte)Combat.FCombatData.FX, (sbyte)Combat.FCombatData.FY, (sbyte)Combat.FCombatData.FL, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                            if (Move != null)
                            {
                                ((ArmyCombatMoveTokenEvent)Move).FIgnoreDefenders = true;
                            }
                        }

                        if (Move != null)
                        {
                            AoWHSMap.Current.FTokenManager.TokenExecuter.AddSystemEvent(Move);
                            Move.Release();
                        }
                    }
                }
            }
            else
            {
                ArmyHS = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(Combat.FCombatData.FAttackerX, Combat.FCombatData.FAttackerY, Combat.FCombatData.FAttackerL).FindOwnedChild(131607);
                if (ArmyHS != null)
                {
                    for (int I = ArmyHS.FArmy.Count - 1; I >= 0; I--)
                    {
                        var unit = ArmyHS.FArmy.GetUnit(I);
                        ArmyHS.FArmy.GetUnit(I).FUnitStatus = unit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                    }

                    if (ArmyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                    {
                        AoWHSMap.Current.FSelectedArmy.Select(ArmyHS);
                    }
                }
            }

            if (AoWHSMap.Current.FRequestCombatType && Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FAttackerPlayer).FPlayer != AoWHSMap.Current.FSeatedPlayer)
            {
                RestoreHotseatMode();
            }

            AoWHSMap.Current.SynchroniseBegin();
            DistributeCombatEvent(Combat.FLogbook, false);
            AoWHSMap.Current.SynchroniseEnd();
        }
    }

    // MoveT.PAS:3019
    public MoveStatus ExecuteLastMove()
    {
        if (GetMapF(FPath[FMoveIndex - 1]).FindOwnedChild(131607) == null)
        {
            var mapF = GetMapF(FPath[FMoveIndex - 1]);
            var item = FPath[FMoveIndex - 1];
            FArmyHS.MoveTo(mapF, item.Flag);
            FArmyHS = null;
        }
        else
        {
            FArmyHS.CancelMove();
        }

        return MoveStatus.Done;
    }

    private static readonly string P5IndependentsAreMovingRStr = "Independents are moving";
    private static readonly string P5PlayerIsMovingRStr = "%s is moving";
    private static readonly string P5YouAreBeingAttackedRStr = "You're being attacked!";

    private static void DistributeCombatEvent(CombatLogbook Logbook, bool Execute)
    {
        CombatData fFinalData = Logbook.FFinalData;
        int i = 1;
        for (int num = AoWHSMap.Current.FPlayers.GetCount() - 1; i <= num; i++)
        {
            if (AoWHSMap.Current.VisibleForPlayer(fFinalData.FX, fFinalData.FY, fFinalData.FL, (sbyte)i) || Logbook.FInitialData.FPlayers.IndexOfPlayer((byte)i) != -1)
            {
                CombatEventLog tCombatEventLog = new CombatEventLog(null);
                tCombatEventLog.SetCombatLogbook(Logbook);
                AoWHSMap.Current.FPlayers.GetPlayers(i).FEventLogbook.AddEvent(tCombatEventLog);
                if (Execute && i == AoWHSMap.Current.FSeatedPlayer && AoWHSMap.Current.FPlayers.GetPlayers(i).FPlayerType == PlayerType.Human && Logbook.FFinalData.F_CombatData_30 != 6)
                {
                    tCombatEventLog.Execute(Mode: false, ExecuteTrigger.Event, null);
                }
                tCombatEventLog.Release();
            }
        }
    }

    private static MoveArmyTokenEvent CreateArmyMoveXYLTE(ArmyHexSprite Army, byte Selection, sbyte X, sbyte Y, sbyte L, MoveAction Action, Set<ArmyMoveModeElement> Mode)
    {
        MoveControlSettings tMoveControlSettings = new MoveControlSettings(null);
        sbyte lhx = Army.GetLhx();
        sbyte yhx = Army.GetYhx();
        tMoveControlSettings.SetSourcePos(Army.GetXhx(), yhx, lhx);
        tMoveControlSettings.FPlayer = Army.FArmy.FPlayer;
        tMoveControlSettings.FArmyHS = Army;
        tMoveControlSettings.FSelection = Selection;
        tMoveControlSettings.SetDestinationPos(X, Y, L);
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of<SByteArray16By16>(ref tMoveControlSettings.FMovePointTable));
        AoWHSMap.Current.FMoveControl.ProcessMove(tMoveControlSettings);
        MoveArmyTokenEvent result = ((tMoveControlSettings.FPath.Count == 0) ? null : ArmyHexSprite.CreateArmyMoveTE(Army, Selection, tMoveControlSettings.FPath, Action, Set<ArmyMoveModeElement>.Of(Array.Empty<ArmyMoveModeElement>())));
        tMoveControlSettings.Release();
        return result;
    }

    public enum Mode : byte
    {
        Moving = 0, RequestingCombatType = 1, SetupReady = 2, CombatExecuted = 3, CancelCombat = 4, SetupHotseat = 5, RestoringHotseat = 6
    }

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}
