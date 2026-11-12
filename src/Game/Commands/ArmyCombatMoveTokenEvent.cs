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

    public new const int ClassId = 135426;

    private ArmyCombatMoveTokenEventMode _combatMode;
    private CombatType _combatType;
    private bool _ignoreDefenders;

    // MoveT.PAS:2513
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(Bytes.Of(ref _combatType, 1), 1, 50);
    }

    // MoveT.PAS:3085
    public override MoveStatus ProcessMove()
    {
        // { Retain Result assignment and place combat execution first. }
        MoveStatus result = MoveStatus.Moving;
        if (_combatMode == ArmyCombatMoveTokenEventMode.RestoringHotSeat)
        {
            return MoveStatus.Moving;
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.SetupHotSeat)
        {
            result = MoveStatus.Moving;
        }
        else
        {
            if (_combatMode == ArmyCombatMoveTokenEventMode.CancelCombat)
            {
                result = MoveStatus.Done;
            }
            else
            {
                if (_combatMode != ArmyCombatMoveTokenEventMode.RequestingCombatType)
                {
                    if (_combatMode != ArmyCombatMoveTokenEventMode.SetupReady)
                    {
                        result = base.ProcessMove();
                    }

                    if (_combatMode == ArmyCombatMoveTokenEventMode.SetupReady)
                    {
                        if (_combatType != CombatType.None)
                        {
                            ExecuteCombat();
                            if (ArmyHexSprite == null)
                            {
                                result = MoveStatus.Done;
                            }
                            else
                            {
                                result = MoveStatus.Moving;
                            }
                        }
                        else
                        {
                            result = MoveStatus.Done;
                        }
                    }
                }
            }
        }

        return result;
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
        bool result;
        AoWMapField field;
        if (MoveIndex == 1 && _combatMode != ArmyCombatMoveTokenEventMode.CombatExecuted)
        {
            if (ArmyHexSprite.GetMoveStatus() != -1)
            {
                field = GetMapField(Path[MoveIndex - 1]);
                if (field.GetPlayer() != -1)
                {
                    result = ArmyHexSprite.GetMoveStatus() < 50;
                }
                else
                {
                    result = ArmyHexSprite.GetMoveStatus() < 100;
                }
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = base.Animating();
        }

        return result;
    }

    // MoveT.PAS:3046
    public override bool CanMoveNext()
    {
        if (MoveIndex == 1)
        {
            // { Evaluate the field without retaining a second reference. }
            var player = GetMapField(Path[0]).GetPlayer();
            if (ArmyHexSprite.Army.Player != player)
            {
                return AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).GameOverStatus == GameOverStatus.None;
            }
        }

        return base.CanMoveNext();
    }

    // MoveT.PAS:3061
    public override MoveStatus ExecuteMove()
    {
        MoveStatus result;
        AoWMapField field;
        sbyte player;
        int direction;
        if (MoveIndex == 1 && _combatMode == ArmyCombatMoveTokenEventMode.Moving)
        {
            field = GetMapField(Path[0]);
            player = field.GetPlayer();
            if (player != -1 && player != ArmyHexSprite.Army.Player && !_ignoreDefenders)
            {
                direction = GetDirection(MoveIndex + 1);
                if (direction == 0)
                {
                    direction = GetHexDirection(MoveIndex + 1);
                }

                if (direction > 0 && direction <= 6)
                {
                    ArmyHexSprite.SetDirection((byte)direction);
                }
                else
                {
                    ArmyHexSprite.SetDirection(0);
                }

                SetupCombat();
                result = MoveStatus.Moving;
            }
            else
            {
                result = base.ExecuteMove();
            }
        }
        else
        {
            result = base.ExecuteMove();
        }

        return result;
    }

    // MoveT.PAS:2504
    public override bool Setup(ArmyHexSprite armyHexSprite, byte selection, MovePath path, Set<ArmyMoveModeElement> mode)
    {
        bool result = base.Setup(armyHexSprite, selection, path, mode);
        if (result && armyHexSprite.Army.Player == TargetPlayer)
        {
            result = false;
        }

        bool condition = result && armyHexSprite.Army.Player > 0 && TargetPlayer > 0;
        if (condition)
        {
            var diplomaticRelation = AoWHSMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).DiplomaticRelations.GetDiplomaticRelation(TargetPlayer);
            condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(diplomaticRelation);
        }

        if (condition)
        {
            result = false;
        }

        return result;
    }

    // MoveT.PAS:2525
    public void RequestCombatTypeCallback(EventLog log)
    {
        CombatFlowControlTokenEvent flow;
        CombatTypeRequestEventLog request;
        ArmyCombatMoveTokenEvent move;
        request = (CombatTypeRequestEventLog)log;
        _combatType = request.CombatType;
        if (request.CreateNewMove)
        {
            if (_combatType != CombatType.None)
            {
                flow = new CombatFlowControlTokenEvent(null);
                flow.Mode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.UnlockAIExecutor, CombatFlowControlModeElement.ActivateTacticalBattle, CombatFlowControlModeElement.EnableTurnTimer);
                if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    flow.Mode = flow.Mode + Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch);
                }

                flow.Player = request.Player;
                AoWHSMap.Current.TokenManager.GetTokenControl(AoWHSMap.Current.SeatedPlayer).AddEvent(flow);
                flow.Release();
                if (request.ArmyId.RetrieveArmy(ref ArmyHexSprite, ref Selection))
                {
                    while (Path.Count > 2)
                    {
                        Path.Delete(2);
                    }

                    if (Path.Count == 2)
                    {
                        Path.List[1].Tag = 0;
                    }

                    move = (ArmyCombatMoveTokenEvent)ArmyHexSprite.CreateArmyMoveTokenEvent(ArmyHexSprite, Selection, Path, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                    if (move != null)
                    {
                        // { created combat move }
                        move._combatType = ((CombatTypeRequestEventLog)log).CombatType;
                        AoWHSMap.Current.TokenManager.GetTokenControl(AoWHSMap.Current.SeatedPlayer).AddEvent(move);
                        move.Release();
                    }
                    else
                    {
                        GameTables.GameEngine.OutputDebugString("Cannot create CombatMoveTE");
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    flow = new CombatFlowControlTokenEvent(null);
                    flow.Mode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch, CombatFlowControlModeElement.UnlockAIExecutor, CombatFlowControlModeElement.EnableTurnTimer);
                    AoWHSMap.Current.TokenManager.GetTokenControl(AoWHSMap.Current.SeatedPlayer).AddEvent(flow);
                    flow.Release();
                }
            }
        }
        else
        {
            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
        }
    }

    // MoveT.PAS:2568
    public void RestoreHotSeatMode()
    {
        NewTurnEventLog log;
        AoWHSMap.Current.SetSeatedPlayer(-1);
        log = new NewTurnEventLog(null);
        log.TurnPlayer = AoWHSMap.Current.CurrentPlayer;
        log.Mode = NewTurnEventLogMode.Dialog;
        log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
        if (AoWHSMap.Current.CurrentPlayer == 0)
        {
            log.Text = AbilityRules.TranslateResourceString(s_independentsAreMovingText);
        }
        else
        {
            var name = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).GetName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_playerIsMovingFormat), [name]);
        }

        AoWHSMap.Current.TriggerExecuteEventLog(log, true, null);
        log.Release();
    }

    // MoveT.PAS:2584
    public void RestoreHotSeatModeCallback(EventLog log)
    {
        AoWHSMap.Current.SynchronizeBegin();
        RestoreHotSeatMode();
        AoWHSMap.Current.AIExecutor.Unlock();
        AoWHSMap.Current.UnlockGameOverEvent();
        _combatMode = ArmyCombatMoveTokenEventMode.Moving;
        AoWHSMap.Current.SynchronizeEnd();
    }

    // MoveT.PAS:2594
    public void ShowHotSeatPlayerCallback(EventLog log)
    {
        AoWHSMap.Current.SetSeatedPlayer(((NewTurnEventLog)log).TurnPlayer);
        if (_combatMode == ArmyCombatMoveTokenEventMode.SetupHotSeat)
        {
            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
        }
    }

    // MoveT.PAS:2600
    public void SetupCombat()
    {
        AoWMapField field = default;
        sbyte Player = default;
        Combat.Battle combat = default;
        MapFieldMessage message = default;
        CombatTypeRequestEventLog request = default;
        NewTurnEventLog log = default;
        field = GetMapField(Path[0]);
        if (_combatType == CombatType.None)
        {
            if (AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local)
            {
                if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                {
                    _combatType = CombatType.Fast;
                    _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                }
                else
                {
                    if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                    {
                        bool condition = AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                        if (!condition)
                        {
                            bool test = field.GetPlayer() > 0;
                            if (test)
                            {
                                var player = field.GetPlayer();
                                test = AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                            }

                            condition = test;
                        }

                        if (condition)
                        {
                            _combatType = CombatType.Tactical;
                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                        }
                        else
                        {
                            _combatType = CombatType.Fast;
                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                        }
                    }
                    else
                    {
                        // { Set the mode in both branches, as in the original. }
                        _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local && (!AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.Players.LocalPlayers.Count == 1 || AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human))
                {
                    if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                    {
                        _combatType = CombatType.Fast;
                        _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                    }
                    else
                    {
                        if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                        {
                            bool condition = AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!condition)
                            {
                                bool test = field.GetPlayer() > 0;
                                if (test)
                                {
                                    var player = field.GetPlayer();
                                    test = AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                }

                                condition = test;
                            }

                            if (condition)
                            {
                                _combatType = CombatType.Tactical;
                                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                            }
                            else
                            {
                                _combatType = CombatType.Fast;
                                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                            }
                        }
                        else
                        {
                            // { Set the mode in both branches, as in the original. }
                            _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                        }
                    }
                }
                else
                {
                    bool condition = AoWHSMap.Current.ConnectionType == PlayerConnectionType.Email && AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && field.GetPlayer() >= 0;
                    if (condition)
                    {
                        var player = field.GetPlayer();
                        condition = AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human;
                    }

                    if (condition)
                    {
                        if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                        {
                            _combatType = CombatType.Fast;
                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                        }
                        else
                        {
                            if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                            {
                                bool test = AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                                if (!test)
                                {
                                    bool passed = field.GetPlayer() > 0;
                                    if (passed)
                                    {
                                        var player = field.GetPlayer();
                                        passed = AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                    }

                                    test = passed;
                                }

                                if (test)
                                {
                                    _combatType = CombatType.Tactical;
                                    _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                }
                                else
                                {
                                    _combatType = CombatType.Fast;
                                    _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                }
                            }
                            else
                            {
                                // { Set the mode in both branches, as in the original. }
                                _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                            }
                        }
                    }
                    else
                    {
                        // { Original tests connection type here, not play mode. }
                        if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network)
                        {
                            bool test = AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!test)
                            {
                                bool passed = field.GetPlayer() > 0;
                                if (passed)
                                {
                                    var player = field.GetPlayer();
                                    passed = AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                }

                                test = passed;
                            }

                            if (test)
                            {
                                switch (AoWHSMap.Current.VictoryMode)
                                {
                                    case 0:
                                        _combatType = CombatType.Fast;
                                        _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                        break;
                                    case 2:
                                        _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                                        break;
                                    case 1:
                                        bool passed = AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && field.GetPlayer() > 0;
                                        if (passed)
                                        {
                                            var player = field.GetPlayer();
                                            passed = AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                        }

                                        if (passed)
                                        {
                                            _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                                        }
                                        else
                                        {
                                            _combatType = CombatType.Fast;
                                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                        }

                                        break;
                                    default:
                                        {
                                            // { The original tests the always-available case before the human-only case. }
                                            _combatType = CombatType.Fast;
                                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                _combatType = CombatType.Fast;
                                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                            }
                        }
                        else
                        {
                            _combatType = CombatType.Fast;
                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                        }
                    }
                }
            }
        }
        else
        {
            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType || (_combatType == CombatType.Tactical && _combatMode == ArmyCombatMoveTokenEventMode.SetupReady))
        {
            combat = new Combat.Battle(null);
            var hexY = ArmyHexSprite.GetHexY();
            var hexLevel = ArmyHexSprite.GetHexLevel();
            var player = field.GetPlayer();
            var independentRelation = field.GetIndependentRelation();
            var item = Path[0];
            var fPathItem = Path[0];
            var item2 = Path[0];
            var hexX = ArmyHexSprite.GetHexX();
            combat.Setup((byte)ArmyHexSprite.Army.Player, (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, hexLevel, (byte)player, independentRelation, item.X, fPathItem.Y, item2.MapLevel, false);
            message.MessageId = 4359;
            message.Param[0] = combat;
            message.Param[1] = ArmyHexSprite;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            if (message.Result.Int == 0)
            {
                var fArmyHexSpriteHexLevel = ArmyHexSprite.GetHexLevel();
                var fArmyHexSpriteHexY = ArmyHexSprite.GetHexY();
                var fArmyHexSpriteHexX = ArmyHexSprite.GetHexX();
                combat.InitializeDefaultArmies(ArmyHexSprite.Army, fArmyHexSpriteHexX, fArmyHexSpriteHexY, fArmyHexSpriteHexLevel);
            }

            if (!combat.DefendersAvailable() || !combat.PlayerTypeInvolved(Set<PlayerType>.Of(PlayerType.Human)))
            {
                _combatType = CombatType.Fast;
                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
            }

            combat?.Dispose();
        }

        // { Original uses connection type here too. }
        if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network && _combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            AoWHSMap.Current.DisableTurnTimer();
            AoWHSMap.Current.AIExecutor.Lock();
            if (AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human)
            {
                Player = ArmyHexSprite.Army.Player;
            }
            else
            {
                Player = field.GetPlayer();
            }

            if (AoWHSMap.Current.TokenManager.GetTokenControl(Player).Local())
            {
                AoWHSMap.Current.TokenManager.GetTokenControl(Player).ClearEvents();
            }

            AoWHSMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
            AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(Player);
            if (AoWHSMap.Current.SeatedPlayer == Player)
            {
                request = new CombatTypeRequestEventLog(null);
                request.Player = ArmyHexSprite.Army.Player;
                request.DefenderPlayer = field.GetPlayer();
                request.X = field.HexX;
                request.Y = field.HexY;
                request.MapLevel = field.HexLevel;
                request.SetTokenEvent(this);
                request.CreateNewMove = true;
                request.ArmyId = ArmyHexSprite.Army.CreateId(Selection);
                if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.Choose)
                {
                    if (AoWHSMap.Current.SeatedPlayer != request.Player)
                    {
                        var hexLevel = ArmyHexSprite.GetHexLevel();
                        var hexY = ArmyHexSprite.GetHexY();
                        var hexX = ArmyHexSprite.GetHexX();
                        AoWHSMap.Current.CenterViewTo(hexX, hexY, hexLevel);
                    }

                    FinishMove();
                    ArmyHexSprite = null;
                    AoWHSMap.Current.TriggerExecuteEventLog(request, false, RequestCombatTypeCallback);
                }
                else
                {
                    FinishMove();
                    ArmyHexSprite = null;
                    if (GameTables.GameEngine.Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                    {
                        request.Select(CombatType.Tactical);
                    }
                    else
                    {
                        request.Select(CombatType.Fast);
                    }

                    RequestCombatTypeCallback(request);
                }

                request.Release();
            }
            else
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            _combatMode = ArmyCombatMoveTokenEventMode.CancelCombat;
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType && AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
        {
            log = new NewTurnEventLog(null);
            log.TurnPlayer = field.GetPlayer();
            log.Mode = NewTurnEventLogMode.Dialog;
            log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
            log.Text = AbilityRules.TranslateResourceString(s_youAreBeingAttackedText);
            AoWHSMap.Current.TriggerExecuteEventLog(log, false, ShowHotSeatPlayerCallback);
            log.Release();
        }
        else
        {
            if (_combatMode == ArmyCombatMoveTokenEventMode.SetupReady && _combatType == CombatType.Tactical && AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
            {
                _combatMode = ArmyCombatMoveTokenEventMode.SetupHotSeat;
                log = new NewTurnEventLog(null);
                log.TurnPlayer = field.GetPlayer();
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(s_youAreBeingAttackedText);
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, ShowHotSeatPlayerCallback);
                log.Release();
            }
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            ArmyHexSprite.PauseMove();
            request = new CombatTypeRequestEventLog(null);
            request.Player = ArmyHexSprite.Army.Player;
            request.DefenderPlayer = field.GetPlayer();
            request.X = field.HexX;
            request.Y = field.HexY;
            request.MapLevel = field.HexLevel;
            if (AoWHSMap.Current.SeatedPlayer != request.Player)
            {
                var hexLevel = ArmyHexSprite.GetHexLevel();
                var hexY = ArmyHexSprite.GetHexY();
                var hexX = ArmyHexSprite.GetHexX();
                AoWHSMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            }

            AoWHSMap.Current.TriggerExecuteEventLog(request, false, RequestCombatTypeCallback);
            request.Release();
        }
    }

    // MoveT.PAS:2801
    public void CombatExecuted(Combat.Battle combat)
    {
        ArmyHexSprite armyHexSprite;
        Army army;
        byte selection;
        MoveArmyTokenEvent move;
        AoWMapField field;
        if (!(combat is FastBattle))
        {
            if (Set<CombatResult>.Of(CombatResult.Victory).Contains(combat.CombatResult))
            {
                armyHexSprite = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(combat.CombatData.AttackerX, combat.CombatData.AttackerY, combat.CombatData.AttackerL).FindOwnedChild(131607);
                if (armyHexSprite != null)
                {
                    army = armyHexSprite.Army;
                    selection = 0;
                    for (int I = army.Count - 1; I >= 0; I--)
                    {
                        selection = (byte)(selection << 1);
                        if (army.GetUnit(I).UnitStatus.Contains(UnitStatusElement.Moving))
                        {
                            selection = (byte)(selection | 1);
                            var troop = army.GetUnit(I);
                            army.GetUnit(I).UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                        }
                    }

                    if (selection != 0)
                    {
                        field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(combat.CombatData.X, combat.CombatData.Y, combat.CombatData.MapLevel);
                        if (field.GetPlayer() == armyHexSprite.Army.Player)
                        {
                            move = CreateArmyMoveMapCoordinateTokenEvent(armyHexSprite, selection, (sbyte)combat.CombatData.X, (sbyte)combat.CombatData.Y, (sbyte)combat.CombatData.MapLevel, MoveAction.Normal, Set<ArmyMoveModeElement>.Of());
                        }
                        else
                        {
                            move = CreateArmyMoveMapCoordinateTokenEvent(armyHexSprite, selection, (sbyte)combat.CombatData.X, (sbyte)combat.CombatData.Y, (sbyte)combat.CombatData.MapLevel, MoveAction.Attack, Set<ArmyMoveModeElement>.Of());
                            if (move != null)
                            {
                                ((ArmyCombatMoveTokenEvent)move)._ignoreDefenders = true;
                            }
                        }

                        if (move != null)
                        {
                            AoWHSMap.Current.TokenManager.TokenExecutor.AddSystemEvent(move);
                            move.Release();
                        }
                    }
                }
            }
            else
            {
                armyHexSprite = (ArmyHexSprite)AoWHSMap.Current.MapContainer.GetField(combat.CombatData.AttackerX, combat.CombatData.AttackerY, combat.CombatData.AttackerL).FindOwnedChild(131607);
                if (armyHexSprite != null)
                {
                    for (int I = armyHexSprite.Army.Count - 1; I >= 0; I--)
                    {
                        var troop = armyHexSprite.Army.GetUnit(I);
                        armyHexSprite.Army.GetUnit(I).UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                    }

                    if (armyHexSprite.Army.Player == AoWHSMap.Current.SeatedPlayer)
                    {
                        AoWHSMap.Current.SelectedArmy.Select(armyHexSprite);
                    }
                }
            }

            if (AoWHSMap.Current.RequestCombatType && combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player != AoWHSMap.Current.SeatedPlayer)
            {
                RestoreHotSeatMode();
            }

            AoWHSMap.Current.SynchronizeBegin();
            DistributeCombatEvent(combat.Logbook, false);
            AoWHSMap.Current.SynchronizeEnd();
        }
    }

    // MoveT.PAS:3019
    public MoveStatus ExecuteLastMove()
    {
        if (GetMapField(Path[MoveIndex - 1]).FindOwnedChild(131607) == null)
        {
            var mapField = GetMapField(Path[MoveIndex - 1]);
            var item = Path[MoveIndex - 1];
            ArmyHexSprite.MoveTo(mapField, item.Tag);
            ArmyHexSprite = null;
        }
        else
        {
            ArmyHexSprite.CancelMove();
        }

        return MoveStatus.Done;
    }

    private static readonly string s_independentsAreMovingText = "Independents are moving";
    private static readonly string s_playerIsMovingFormat = "%s is moving";
    private static readonly string s_youAreBeingAttackedText = "You're being attacked!";

    private static void DistributeCombatEvent(CombatLogbook logbook, bool execute)
    {
        CombatData fFinalData = logbook.FinalData;
        int i = 1;
        for (int number = AoWHSMap.Current.Players.GetCount() - 1; i <= number; i++)
        {
            if (AoWHSMap.Current.VisibleForPlayer(fFinalData.X, fFinalData.Y, fFinalData.MapLevel, (sbyte)i) || logbook.InitialData.Players.IndexOfPlayer((byte)i) != -1)
            {
                CombatEventLog combatEventLog = new CombatEventLog(null);
                combatEventLog.SetCombatLogbook(logbook);
                AoWHSMap.Current.Players.GetPlayers(i).EventLogbook.AddEvent(combatEventLog);
                if (execute && i == AoWHSMap.Current.SeatedPlayer && AoWHSMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human && logbook.FinalData.CombatResult != 6)
                {
                    combatEventLog.Execute(mode: false, ExecuteTrigger.Event, null);
                }
                combatEventLog.Release();
            }
        }
    }

    private static MoveArmyTokenEvent CreateArmyMoveMapCoordinateTokenEvent(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L, MoveAction action, Set<ArmyMoveModeElement> mode)
    {
        MoveControlSettings moveControlSettings = new MoveControlSettings(null);
        sbyte hexLevel = army.GetHexLevel();
        sbyte hexY = army.GetHexY();
        moveControlSettings.SetSourcePosition(army.GetHexX(), hexY, hexLevel);
        moveControlSettings.Player = army.Army.Player;
        moveControlSettings.ArmyHexSprite = army;
        moveControlSettings.Selection = selection;
        moveControlSettings.SetDestinationPosition(X, Y, L);
        army.Army.CreateMovePointTable(selection, Bytes.Of<MovePointTable>(ref moveControlSettings.MovePointTable));
        AoWHSMap.Current.MoveControl.ProcessMove(moveControlSettings);
        MoveArmyTokenEvent result = ((moveControlSettings.Path.Count == 0) ? null : ArmyHexSprite.CreateArmyMoveTokenEvent(army, selection, moveControlSettings.Path, action, Set<ArmyMoveModeElement>.Of(Array.Empty<ArmyMoveModeElement>())));
        moveControlSettings.Release();
        return result;
    }

    // intf_P5_07.inc:816

    // intf_P5_07.inc:816

}

public enum ArmyCombatMoveTokenEventMode : byte
{
    Moving = 0, RequestingCombatType = 1, SetupReady = 2, CombatExecuted = 3, CancelCombat = 4, SetupHotSeat = 5, RestoringHotSeat = 6
}
