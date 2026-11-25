#nullable disable warnings
// Translated from AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Formats.Tree;
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
public partial class ArmyCombatMoveTokenEvent(CustomNode? owner) : MoveArmyTokenEvent(owner)
{
    public new const int ClassId = 135_426;

    private ArmyCombatMoveTokenEventMode _combatMode;
    private CombatType _combatType;
    private bool _ignoreDefenders;

    // MoveT.PAS:2513
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _combatType = persister.Enum("CombatType", value: _combatType);
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
            return MoveStatus.Moving;
        }
        else
        {
            if (_combatMode == ArmyCombatMoveTokenEventMode.CancelCombat)
            {
                return MoveStatus.Done;
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
                            return ArmyHexSprite is null ? MoveStatus.Done : MoveStatus.Moving;
                        }
                        else
                        {
                            return MoveStatus.Done;
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
    public override void InitializeMove() => base.InitializeMove();

    // MoveT.PAS:3030
    public override bool Animating()
    {
        if (MoveIndex == 1 && _combatMode != ArmyCombatMoveTokenEventMode.CombatExecuted)
        {
            if (ArmyHexSprite.GetMoveStatus() != -1)
            {
                MapField field = GetMapField(Path[MoveIndex - 1].Location);
                return field.GetPlayer() is not null ? ArmyHexSprite.GetMoveStatus() < 50 : ArmyHexSprite.GetMoveStatus() < 100;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return base.Animating();
        }
    }

    // MoveT.PAS:3046
    public override bool CanMoveNext()
    {
        if (MoveIndex == 1)
        {
            // { Evaluate the field without retaining a second reference. }
            sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(GetMapField(Path[0].Location).GetPlayer());
            if (PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player) != player)
            {
                return GetEngine().StrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).GameOverStatus == GameOverStatus.None;
            }
        }

        return base.CanMoveNext();
    }

    // MoveT.PAS:3061
    public override MoveStatus ExecuteMove()
    {
        if (MoveIndex == 1 && _combatMode == ArmyCombatMoveTokenEventMode.Moving)
        {
            MapField field = GetMapField(Path[0].Location);
            sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
            if (player != -1 && player != PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player) && !_ignoreDefenders)
            {
                int direction = GetDirection(MoveIndex + 1);
                if (direction == 0)
                {
                    direction = GetHexDirection(MoveIndex + 1);
                }

                if (direction is > 0 and <= 6)
                {
                    ArmyHexSprite.SetDirection(direction.TruncateToByte());
                }
                else
                {
                    ArmyHexSprite.SetDirection(0);
                }

                SetupCombat();
                return MoveStatus.Moving;
            }
            else
            {
                return base.ExecuteMove();
            }
        }
        else
        {
            return base.ExecuteMove();
        }
    }

    // MoveT.PAS:2504
    public override bool Setup(ArmyHexSprite? armyHexSprite, int selection, MovePath path, ArmyMoveModes mode)
    {
        bool result = base.Setup(armyHexSprite, selection, path, mode);
        if (result && armyHexSprite.Army.Player == TargetPlayer)
        {
            result = false;
        }

        bool condition = result && PlayerIdExtensions.ToOriginal(armyHexSprite.Army.Player) > 0 && PlayerIdExtensions.ToOriginal(TargetPlayer) > 0;
        if (condition)
        {
            DiplomaticRelation diplomaticRelation = GetEngine().StrategicMap.Players.GetPlayers(armyHexSprite.Army.Player).DiplomaticRelations.GetDiplomaticRelation(TargetPlayer);
            condition = diplomaticRelation is DiplomaticRelation.Peace or DiplomaticRelation.Alliance;
        }

        return !condition && result;
    }

    // MoveT.PAS:2525
    public void RequestCombatTypeCallback(CombatTypeRequestEventLog request)
    {
        _combatType = request.CombatType;
        if (request.CreateNewMove)
        {
            CombatFlowControlTokenEvent flow;
            if (_combatType != CombatType.None)
            {
                flow = new CombatFlowControlTokenEvent(null)
                {
                    Holder = CreationHolder(),
                    Mode = CombatFlowControlModes.UnlockAIExecutor | CombatFlowControlModes.ActivateTacticalBattle | CombatFlowControlModes.EnableTurnTimer
                };
                StrategicMap strategicMap = GetEngine().StrategicMap;
                if (strategicMap.PlayMode == PlayMode.Simultaneous)
                {
                    flow.Mode |= CombatFlowControlModes.SetAutoSwitch;
                }

                flow.Player = request.AttackerPlayer;
                strategicMap.TokenManager.GetTokenControl(strategicMap.SeatedPlayer).AddEvent(flow);
                _ = flow.Release();
                ArmyHexSprite? armyHexSprite = ArmyHexSprite;
                int selection = Selection;
                bool stored = request.ArmyId.RetrieveArmy(ref armyHexSprite, ref selection);
                ArmyHexSprite = armyHexSprite;
                Selection = selection;
                if (stored)
                {
                    while (Path.Count > 2)
                    {
                        Path.Delete(2);
                    }

                    if (Path.Count == 2)
                    {
                        Path.List[1] = Path.List[1] with { MoveCost = 0 };
                    }

                    ArmyCombatMoveTokenEvent move = ArmyHexSprite.CreateArmyCombatMoveTokenEvent(ArmyHexSprite, selection: Selection, path: Path, mode: ArmyMoveModes.None);
                    if (move is not null)
                    {
                        // { created combat move }
                        move._combatType = request.CombatType;
                        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                        engineStrategicMap.TokenManager.GetTokenControl(engineStrategicMap.SeatedPlayer).AddEvent(move);
                        _ = move.Release();
                    }
                    else
                    {
                        GetEngine().OutputDebugString("Cannot create CombatMoveTE");
                    }
                }
            }
            else
            {
                StrategicMap addEventStrategicMap = GetEngine().StrategicMap;
                if (addEventStrategicMap.PlayMode == PlayMode.Simultaneous)
                {
                    flow = new CombatFlowControlTokenEvent(null)
                    {
                        Holder = CreationHolder(),
                        Mode = CombatFlowControlModes.SetAutoSwitch | CombatFlowControlModes.UnlockAIExecutor | CombatFlowControlModes.EnableTurnTimer
                    };
                    addEventStrategicMap.TokenManager.GetTokenControl(addEventStrategicMap.SeatedPlayer).AddEvent(flow);
                    _ = flow.Release();
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
        GetEngine().StrategicMap.SetSeatedPlayer(null);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = strategicMap.CurrentPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = NewTurnEventLogDialogStyles.None
        };
        if (strategicMap.CurrentPlayer == PlayerId.Independent)
        {
            log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_independentsAreMovingText);
        }
        else
        {
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            string name = engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer).GetName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_playerIsMovingFormat), arguments: [name]);
        }

        GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: true, done: null);
        _ = log.Release();
    }

    // MoveT.PAS:2584
    public void RestoreHotSeatModeCallback(EventLog log)
    {
        GetEngine().StrategicMap.SynchronizeBegin();
        RestoreHotSeatMode();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.AIExecutor.Unlock();
        strategicMap.UnlockGameOverEvent();
        _combatMode = ArmyCombatMoveTokenEventMode.Moving;
        GetEngine().StrategicMap.SynchronizeEnd();
    }

    // MoveT.PAS:2594
    public void ShowHotSeatPlayerCallback(NewTurnEventLog log)
    {
        GetEngine().StrategicMap.SetSeatedPlayer(log.TurnPlayer);
        if (_combatMode == ArmyCombatMoveTokenEventMode.SetupHotSeat)
        {
            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
        }
    }

    // MoveT.PAS:2600
    public void SetupCombat()
    {
        MapFieldMessage message = default;
        MapField field = GetMapField(Path[0].Location);
        if (_combatType == CombatType.None)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.RequestCombatType && strategicMap.ConnectionType == PlayerConnectionType.Local)
            {
                if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                {
                    _combatType = CombatType.Fast;
                    _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                }
                else
                {
                    if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                    {
                        bool condition = strategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                        if (!condition)
                        {
                            bool test = PlayerIdExtensions.ToOriginal(field.GetPlayer()) > 0;
                            if (test)
                            {
                                sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                                test = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (engineStrategicMap.ConnectionType == PlayerConnectionType.Local && ((!engineStrategicMap.RequestCombatType && engineStrategicMap.Players.LocalPlayers.Count == 1) || (engineStrategicMap.RequestCombatType && engineStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human)))
                {
                    if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                    {
                        _combatType = CombatType.Fast;
                        _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                    }
                    else
                    {
                        if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                        {
                            bool condition = engineStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!condition)
                            {
                                bool test = PlayerIdExtensions.ToOriginal(field.GetPlayer()) > 0;
                                if (test)
                                {
                                    sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                                    test = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                    StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                    bool condition = getPlayersStrategicMap.ConnectionType == PlayerConnectionType.Email && getPlayersStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && PlayerIdExtensions.ToOriginal(field.GetPlayer()) >= 0;
                    if (condition)
                    {
                        sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                        condition = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType != PlayerType.Human;
                    }

                    if (condition)
                    {
                        if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysFastBattle)
                        {
                            _combatType = CombatType.Fast;
                            _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                        }
                        else
                        {
                            if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                            {
                                bool test = GetEngine().StrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                                if (!test)
                                {
                                    bool passed = PlayerIdExtensions.ToOriginal(field.GetPlayer()) > 0;
                                    if (passed)
                                    {
                                        sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                                        passed = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                        StrategicMap connectionTypeStrategicMap = GetEngine().StrategicMap;
                        if (connectionTypeStrategicMap.ConnectionType == PlayerConnectionType.Network)
                        {
                            bool test = connectionTypeStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!test)
                            {
                                bool passed = PlayerIdExtensions.ToOriginal(field.GetPlayer()) > 0;
                                if (passed)
                                {
                                    sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                                    passed = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                }

                                test = passed;
                            }

                            if (test)
                            {
                                StrategicMap victoryModeStrategicMap = GetEngine().StrategicMap;
                                switch (victoryModeStrategicMap.VictoryMode)
                                {
                                    case 0:
                                        _combatType = CombatType.Fast;
                                        _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                        break;
                                    case 2:
                                        _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                                        break;
                                    case 1:
                                        bool passed = victoryModeStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && PlayerIdExtensions.ToOriginal(field.GetPlayer()) > 0;
                                        if (passed)
                                        {
                                            sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
                                            passed = GetEngine().StrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
            Battle combat = new(null) { Holder = CreationHolder() };
            int hexY = ArmyHexSprite.HexY;
            int hexLevel = ArmyHexSprite.HexLevel;
            sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());
            int independentRelation = field.GetIndependentRelation();
            PathStep item = Path[0];
            PathStep fPathItem = Path[0];
            PathStep item2 = Path[0];
            int hexX = ArmyHexSprite.HexX;
            _ = combat.Setup((byte)PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player), attackerRelation: ((sbyte)ArmyHexSprite.Army.IndependentRelation).TruncateToByte(), hexX, hexY, attackerL: hexLevel, defenderPlayer: (byte)player, defenderRelation: independentRelation, item.Location.X, fPathItem.Location.Y, L: item2.Location.MapLevel, logActions: false);
            message.MessageId = 4359;
            MapFieldMessageParameters param = message.Param;
            param[0] = combat;
            message.Param = param;
            MapFieldMessageParameters param2 = message.Param;
            param2[1] = ArmyHexSprite;
            message.Param = param2;
            message.Result = 0;
            field.SendMapFieldMessage(ref message);
            if (message.Result.Int == 0)
            {
                int fArmyHexSpriteHexLevel = ArmyHexSprite.HexLevel;
                int fArmyHexSpriteHexY = ArmyHexSprite.HexY;
                int fArmyHexSpriteHexX = ArmyHexSprite.HexX;
                combat.InitializeDefaultArmies(ArmyHexSprite.Army, fArmyHexSpriteHexX, fArmyHexSpriteHexY, L: fArmyHexSpriteHexLevel);
            }

            if (!combat.DefendersAvailable() || !combat.PlayerTypeInvolved(PlayerTypes.Human))
            {
                _combatType = CombatType.Fast;
                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
            }

            combat?.Dispose();
        }

        // { Original uses connection type here too. }
        CombatTypeRequestEventLog request;
        StrategicMap disableTurnTimerStrategicMap = GetEngine().StrategicMap;
        if (disableTurnTimerStrategicMap.ConnectionType == PlayerConnectionType.Network && _combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            disableTurnTimerStrategicMap.DisableTurnTimer();
            disableTurnTimerStrategicMap.AIExecutor.Lock();
            sbyte player2 = disableTurnTimerStrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human
                ? (sbyte)PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player) : (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer());

            StrategicMap localStrategicMap = GetEngine().StrategicMap;
            if (localStrategicMap.TokenManager.GetTokenControl(PlayerId.FromOriginal(player2)).Local())
            {
                localStrategicMap.TokenManager.GetTokenControl(PlayerId.FromOriginal(player2)).ClearEvents();
            }

            StrategicMap activateTacticalBattleStrategicMap = GetEngine().StrategicMap;
            activateTacticalBattleStrategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = true;
            activateTacticalBattleStrategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(player2);
            if (PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer) == player2)
            {
                request = new CombatTypeRequestEventLog(null)
                {
                    Holder = CreationHolder(),
                    AttackerPlayer = (sbyte)PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player),
                    DefenderPlayer = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer()),
                    Location = new MapCoordinate((sbyte)field.HexX, (sbyte)field.HexY, (sbyte)field.HexLevel)
                };
                request.SetTokenEvent(this);
                request.CreateNewMove = true;
                request.ArmyId = ArmyHexSprite.Army.CreateId(Selection);
                if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.Choose)
                {
                    StrategicMap toOriginalStrategicMap = GetEngine().StrategicMap;
                    if (PlayerIdExtensions.ToOriginal(toOriginalStrategicMap.SeatedPlayer) != request.AttackerPlayer)
                    {
                        int hexLevel = ArmyHexSprite.HexLevel;
                        int hexY = ArmyHexSprite.HexY;
                        int hexX = ArmyHexSprite.HexX;
                        toOriginalStrategicMap.CenterViewTo(hexX, hexY, hexLevel);
                    }

                    FinishMove();
                    ArmyHexSprite = null;
                    GetEngine().StrategicMap.TriggerTypedExecuteEventLog(request, immediate: false, done: RequestCombatTypeCallback);
                }
                else
                {
                    FinishMove();
                    ArmyHexSprite = null;
                    if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.AlwaysTacticalBattle)
                    {
                        request.Select(CombatType.Tactical);
                    }
                    else
                    {
                        request.Select(CombatType.Fast);
                    }

                    RequestCombatTypeCallback(request);
                }

                _ = request.Release();
            }
            else
            {
                FinishMove();
                ArmyHexSprite = null;
            }

            _combatMode = ArmyCombatMoveTokenEventMode.CancelCombat;
        }

        NewTurnEventLog log;
        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType && GetEngine().StrategicMap.RequestCombatType && GetEngine().StrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
        {
            log = new NewTurnEventLog(null)
            {
                Holder = CreationHolder(),
                TurnPlayer = field.GetPlayer(),
                Mode = NewTurnEventLogMode.Dialog,
                DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_youAreBeingAttackedText)
            };
            GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ShowHotSeatPlayerCallback);
            _ = log.Release();
        }
        else
        {
            if (_combatMode == ArmyCombatMoveTokenEventMode.SetupReady && _combatType == CombatType.Tactical && GetEngine().StrategicMap.RequestCombatType && GetEngine().StrategicMap.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
            {
                _combatMode = ArmyCombatMoveTokenEventMode.SetupHotSeat;
                log = new NewTurnEventLog(null)
                {
                    Holder = CreationHolder(),
                    TurnPlayer = field.GetPlayer(),
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_youAreBeingAttackedText)
                };
                GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ShowHotSeatPlayerCallback);
                _ = log.Release();
            }
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            ArmyHexSprite.PauseMove();
            request = new CombatTypeRequestEventLog(null)
            {
                Holder = CreationHolder(),
                AttackerPlayer = (sbyte)PlayerIdExtensions.ToOriginal(ArmyHexSprite.Army.Player),
                DefenderPlayer = (sbyte)PlayerIdExtensions.ToOriginal(field.GetPlayer()),
                Location = new MapCoordinate((sbyte)field.HexX, (sbyte)field.HexY, (sbyte)field.HexLevel)
            };
            StrategicMap centerViewToStrategicMap = GetEngine().StrategicMap;
            if (PlayerIdExtensions.ToOriginal(centerViewToStrategicMap.SeatedPlayer) != request.AttackerPlayer)
            {
                int hexLevel = ArmyHexSprite.HexLevel;
                int hexY = ArmyHexSprite.HexY;
                int hexX = ArmyHexSprite.HexX;
                centerViewToStrategicMap.CenterViewTo(hexX, hexY, hexLevel);
            }

            GetEngine().StrategicMap.TriggerTypedExecuteEventLog(request, immediate: false, done: RequestCombatTypeCallback);
            _ = request.Release();
        }
    }

    // MoveT.PAS:2801
    public void CombatExecuted(Battle combat)
    {
        if (combat is not FastBattle)
        {
            ArmyHexSprite armyHexSprite;
            if (combat.CombatResult == CombatResult.Victory)
            {
                StrategicMap strategicMap = GetEngine().StrategicMap;
                armyHexSprite = strategicMap.MapContainer.GetField(combat.CombatData.AttackerLocation.X, combat.CombatData.AttackerLocation.Y, combat.CombatData.AttackerLocation.MapLevel).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    Army? army = armyHexSprite.Army;
                    int selection = 0;
                    for (int i = army.Units.Count - 1; i >= 0; i--)
                    {
                        selection = (selection << 1).TruncateToByte();
                        if (army.Units.GetUnit(i).UnitStatus.Contains(UnitStatuses.Moving))
                        {
                            selection |= 1;
                            Troop? troop = army.Units.GetUnit(i);
                            army.Units.GetUnit(i).UnitStatus = troop.UnitStatus & ~UnitStatuses.Moving;
                        }
                    }

                    if (selection != 0)
                    {
                        MapField field = strategicMap.MapContainer.GetField(combat.CombatData.Location.X, combat.CombatData.Location.Y, combat.CombatData.Location.MapLevel);
                        MoveArmyTokenEvent? move;
                        if (field.GetPlayer() == armyHexSprite.Army.Player)
                        {
                            move = CreateArmyMoveMapCoordinateTokenEvent(armyHexSprite, selection, combat.CombatData.Location.X, combat.CombatData.Location.Y, L: combat.CombatData.Location.MapLevel, action: MoveAction.Normal);
                        }
                        else
                        {
                            ArmyCombatMoveTokenEvent? combatMove = CreateCombatMoveMapCoordinateTokenEvent(armyHexSprite, selection: selection, x: combat.CombatData.Location.X, y: combat.CombatData.Location.Y, l: combat.CombatData.Location.MapLevel);
                            combatMove?._ignoreDefenders = true;

                            move = combatMove;
                        }

                        if (move is not null)
                        {
                            GetEngine().StrategicMap.TokenManager.TokenExecutor.AddSystemEvent(move);
                            _ = move.Release();
                        }
                    }
                }
            }
            else
            {
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                armyHexSprite = engineStrategicMap.MapContainer.GetField(combat.CombatData.AttackerLocation.X, combat.CombatData.AttackerLocation.Y, combat.CombatData.AttackerLocation.MapLevel).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    foreach (Troop? troop in armyHexSprite.Army.Units.Reverse())
                    {
                        troop.UnitStatus &= ~UnitStatuses.Moving;
                    }

                    if (armyHexSprite.Army.Player == engineStrategicMap.SeatedPlayer)
                    {
                        engineStrategicMap.SelectedArmy.Select(armyHexSprite);
                    }
                }
            }

            StrategicMap toOriginalStrategicMap = GetEngine().StrategicMap;
            if (toOriginalStrategicMap.RequestCombatType && combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player != PlayerIdExtensions.ToOriginal(toOriginalStrategicMap.SeatedPlayer))
            {
                RestoreHotSeatMode();
            }

            GetEngine().StrategicMap.SynchronizeBegin();
            DistributeCombatEvent(combat.Logbook, execute: false);
            GetEngine().StrategicMap.SynchronizeEnd();
        }
    }

    // MoveT.PAS:3019
    public MoveStatus ExecuteLastMove()
    {
        if (GetMapField(Path[MoveIndex - 1].Location).FindOwnedChild(ArmyHexSprite.ClassId) is null)
        {
            MapField mapField = GetMapField(Path[MoveIndex - 1].Location);
            PathStep item = Path[MoveIndex - 1];
            _ = ArmyHexSprite.MoveTo(mapField, item.MoveCost);
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

    private static void DistributeCombatEvent(CombatLogbook? logbook, bool execute)
    {
        CombatData fFinalData = logbook.FinalData;
        int i = 1;
        for (int number = logbook.GetEngine().StrategicMap.Players.GetCount() - 1; i <= number; i++)
        {
            if (logbook.GetEngine().StrategicMap.VisibleForPlayer(fFinalData.Location.X, fFinalData.Location.Y, fFinalData.Location.MapLevel, player: PlayerId.FromOriginal((sbyte)i)) || logbook.InitialData.Players.IndexOfPlayer((byte)i) != -1)
            {
                CombatEventLog combatEventLog = new(null) { Holder = logbook };
                combatEventLog.SetCombatLogbook(logbook);
                logbook.GetEngine().StrategicMap.Players.GetPlayers(i).EventLogbook.AddEvent(combatEventLog);
                if (execute && i == PlayerIdExtensions.ToOriginal(logbook.GetEngine().StrategicMap.SeatedPlayer) && logbook.GetEngine().StrategicMap.Players.GetPlayers(i).PlayerType == PlayerType.Human && logbook.FinalData.CombatResult != 6)
                {
                    _ = combatEventLog.Execute(mode: false, trigger: ExecuteTrigger.Event, done: null);
                }
                _ = combatEventLog.Release();
            }
        }
    }

    private static MoveArmyTokenEvent? CreateArmyMoveMapCoordinateTokenEvent(ArmyHexSprite army, int selection, int X, int Y, int L, MoveAction action)
    {
        MoveControlSettings moveControlSettings = CreateMoveControlSettings(army, selection: selection, x: X, y: Y, l: L);
        MoveArmyTokenEvent? result = (moveControlSettings.Path.Count == 0) ? null : ArmyHexSprite.CreateArmyMoveTokenEvent(army, selection: selection, path: moveControlSettings.Path, action: action, mode: ArmyMoveModes.None);
        _ = moveControlSettings.Release();
        return result;
    }

    private static ArmyCombatMoveTokenEvent? CreateCombatMoveMapCoordinateTokenEvent(ArmyHexSprite army, int selection, int x, int y, int l)
    {
        MoveControlSettings moveControlSettings = CreateMoveControlSettings(army, selection: selection, x: x, y: y, l: l);
        ArmyCombatMoveTokenEvent? result = (moveControlSettings.Path.Count == 0) ? null : ArmyHexSprite.CreateArmyCombatMoveTokenEvent(army, selection: selection, path: moveControlSettings.Path, mode: ArmyMoveModes.None);
        _ = moveControlSettings.Release();
        return result;
    }

    private static MoveControlSettings CreateMoveControlSettings(ArmyHexSprite army, int selection, int x, int y, int l)
    {
        MoveControlSettings moveControlSettings = new(null) { Holder = army };
        int hexLevel = army.HexLevel;
        int hexY = army.HexY;
        moveControlSettings.SetSourcePosition(army.HexX, hexY, hexLevel);
        moveControlSettings.Player = army.Army.Player;
        moveControlSettings.ArmyHexSprite = army;
        moveControlSettings.Selection = selection;
        moveControlSettings.SetDestinationPosition(x, y, l);
        _ = moveControlSettings.MovePointTable;
        MovePointTable movePointTable = army.Army.CreateMovePointTable(selection);
        moveControlSettings.MovePointTable = movePointTable;
        army.GetEngine().StrategicMap.MoveControl.ProcessMove(moveControlSettings);
        return moveControlSettings;
    }
}

public enum ArmyCombatMoveTokenEventMode : byte
{
    Moving = 0, RequestingCombatType = 1, SetupReady = 2, CombatExecuted = 3, CancelCombat = 4, SetupHotSeat = 5, RestoringHotSeat = 6
}
