#nullable disable
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
public partial class ArmyCombatMoveTokenEvent(CustomNode owner) : MoveArmyTokenEvent(owner)
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
                return field.GetPlayer() != -1 ? ArmyHexSprite.GetMoveStatus() < 50 : ArmyHexSprite.GetMoveStatus() < 100;
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
            sbyte player = GetMapField(Path[0].Location).GetPlayer();
            if (ArmyHexSprite.Army.Player != player)
            {
                return StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).GameOverStatus == GameOverStatus.None;
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
            sbyte player = field.GetPlayer();
            if (player != -1 && player != ArmyHexSprite.Army.Player && !_ignoreDefenders)
            {
                int direction = GetDirection(MoveIndex + 1);
                if (direction == 0)
                {
                    direction = GetHexDirection(MoveIndex + 1);
                }

                if (direction is > 0 and <= 6)
                {
                    ArmyHexSprite.SetDirection((byte)direction);
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
            DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).DiplomaticRelations.GetDiplomaticRelation(TargetPlayer);
            condition = Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(diplomaticRelation);
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
                    Mode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.UnlockAIExecutor, CombatFlowControlModeElement.ActivateTacticalBattle, CombatFlowControlModeElement.EnableTurnTimer)
                };
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    flow.Mode += Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch);
                }

                flow.Player = request.AttackerPlayer;
                StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(flow);
                _ = flow.Release();
                ArmyHexSprite armyHexSprite = ArmyHexSprite;
                byte selection = Selection;
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

                    ArmyCombatMoveTokenEvent move = ArmyHexSprite.CreateArmyCombatMoveTokenEvent(ArmyHexSprite, selection: Selection, path: Path, mode: Set<ArmyMoveModeElement>.Of());
                    if (move is not null)
                    {
                        // { created combat move }
                        move._combatType = request.CombatType;
                        StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(move);
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
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    flow = new CombatFlowControlTokenEvent(null)
                    {
                        Holder = CreationHolder(),
                        Mode = Set<CombatFlowControlModeElement>.Of(CombatFlowControlModeElement.SetAutoSwitch, CombatFlowControlModeElement.UnlockAIExecutor, CombatFlowControlModeElement.EnableTurnTimer)
                    };
                    StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(flow);
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
        StrategicMap.Current.SetSeatedPlayer(-1);
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = StrategicMap.Current.CurrentPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of()
        };
        if (StrategicMap.Current.CurrentPlayer == 0)
        {
            log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_independentsAreMovingText);
        }
        else
        {
            string name = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).GetName();
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_playerIsMovingFormat), arguments: [name]);
        }

        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: true, done: null);
        _ = log.Release();
    }

    // MoveT.PAS:2584
    public void RestoreHotSeatModeCallback(EventLog log)
    {
        StrategicMap.Current.SynchronizeBegin();
        RestoreHotSeatMode();
        StrategicMap.Current.AIExecutor.Unlock();
        StrategicMap.Current.UnlockGameOverEvent();
        _combatMode = ArmyCombatMoveTokenEventMode.Moving;
        StrategicMap.Current.SynchronizeEnd();
    }

    // MoveT.PAS:2594
    public void ShowHotSeatPlayerCallback(NewTurnEventLog log)
    {
        StrategicMap.Current.SetSeatedPlayer(log.TurnPlayer);
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
            if (StrategicMap.Current.RequestCombatType && StrategicMap.Current.ConnectionType == PlayerConnectionType.Local)
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
                        bool condition = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                        if (!condition)
                        {
                            bool test = field.GetPlayer() > 0;
                            if (test)
                            {
                                sbyte player = field.GetPlayer();
                                test = StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && ((!StrategicMap.Current.RequestCombatType && StrategicMap.Current.Players.LocalPlayers.Count == 1) || (StrategicMap.Current.RequestCombatType && StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human)))
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
                            bool condition = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!condition)
                            {
                                bool test = field.GetPlayer() > 0;
                                if (test)
                                {
                                    sbyte player = field.GetPlayer();
                                    test = StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                    bool condition = StrategicMap.Current.ConnectionType == PlayerConnectionType.Email && StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && field.GetPlayer() >= 0;
                    if (condition)
                    {
                        sbyte player = field.GetPlayer();
                        condition = StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human;
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
                                bool test = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                                if (!test)
                                {
                                    bool passed = field.GetPlayer() > 0;
                                    if (passed)
                                    {
                                        sbyte player = field.GetPlayer();
                                        passed = StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
                        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network)
                        {
                            bool test = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human;
                            if (!test)
                            {
                                bool passed = field.GetPlayer() > 0;
                                if (passed)
                                {
                                    sbyte player = field.GetPlayer();
                                    passed = StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
                                }

                                test = passed;
                            }

                            if (test)
                            {
                                switch (StrategicMap.Current.VictoryMode)
                                {
                                    case 0:
                                        _combatType = CombatType.Fast;
                                        _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
                                        break;
                                    case 2:
                                        _combatMode = ArmyCombatMoveTokenEventMode.RequestingCombatType;
                                        break;
                                    case 1:
                                        bool passed = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human && field.GetPlayer() > 0;
                                        if (passed)
                                        {
                                            sbyte player = field.GetPlayer();
                                            passed = StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human;
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
            sbyte hexY = ArmyHexSprite.HexY;
            sbyte hexLevel = ArmyHexSprite.HexLevel;
            sbyte player = field.GetPlayer();
            byte independentRelation = field.GetIndependentRelation();
            PathStep item = Path[0];
            PathStep fPathItem = Path[0];
            PathStep item2 = Path[0];
            sbyte hexX = ArmyHexSprite.HexX;
            _ = combat.Setup((byte)ArmyHexSprite.Army.Player, attackerRelation: (byte)ArmyHexSprite.Army.IndependentRelation, hexX, hexY, attackerL: hexLevel, defenderPlayer: (byte)player, defenderRelation: independentRelation, item.Location.X, fPathItem.Location.Y, L: item2.Location.MapLevel, logActions: false);
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
                sbyte fArmyHexSpriteHexLevel = ArmyHexSprite.HexLevel;
                sbyte fArmyHexSpriteHexY = ArmyHexSprite.HexY;
                sbyte fArmyHexSpriteHexX = ArmyHexSprite.HexX;
                combat.InitializeDefaultArmies(ArmyHexSprite.Army, fArmyHexSpriteHexX, fArmyHexSpriteHexY, L: fArmyHexSpriteHexLevel);
            }

            if (!combat.DefendersAvailable() || !combat.PlayerTypeInvolved(Set<PlayerType>.Of(PlayerType.Human)))
            {
                _combatType = CombatType.Fast;
                _combatMode = ArmyCombatMoveTokenEventMode.SetupReady;
            }

            combat?.Dispose();
        }

        // { Original uses connection type here too. }
        CombatTypeRequestEventLog request;
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network && _combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            StrategicMap.Current.DisableTurnTimer();
            StrategicMap.Current.AIExecutor.Lock();
            sbyte player2 = StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType == PlayerType.Human
                ? ArmyHexSprite.Army.Player
                : field.GetPlayer();

            if (StrategicMap.Current.TokenManager.GetTokenControl(player2).Local())
            {
                StrategicMap.Current.TokenManager.GetTokenControl(player2).ClearEvents();
            }

            StrategicMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(player2);
            if (StrategicMap.Current.SeatedPlayer == player2)
            {
                request = new CombatTypeRequestEventLog(null)
                {
                    Holder = CreationHolder(),
                    AttackerPlayer = ArmyHexSprite.Army.Player,
                    DefenderPlayer = field.GetPlayer(),
                    Location = new MapCoordinate(field.HexX, field.HexY, field.HexLevel)
                };
                request.SetTokenEvent(this);
                request.CreateNewMove = true;
                request.ArmyId = ArmyHexSprite.Army.CreateId(Selection);
                if (GetEngine().Registry.GetCombatResolveMode() == GameSettings.CombatResolveMode.Choose)
                {
                    if (StrategicMap.Current.SeatedPlayer != request.AttackerPlayer)
                    {
                        sbyte hexLevel = ArmyHexSprite.HexLevel;
                        sbyte hexY = ArmyHexSprite.HexY;
                        sbyte hexX = ArmyHexSprite.HexX;
                        StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
                    }

                    FinishMove();
                    ArmyHexSprite = null;
                    StrategicMap.Current.TriggerTypedExecuteEventLog(request, immediate: false, done: RequestCombatTypeCallback);
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
        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType && StrategicMap.Current.RequestCombatType && StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
        {
            log = new NewTurnEventLog(null)
            {
                Holder = CreationHolder(),
                TurnPlayer = field.GetPlayer(),
                Mode = NewTurnEventLogMode.Dialog,
                DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable),
                Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_youAreBeingAttackedText)
            };
            StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ShowHotSeatPlayerCallback);
            _ = log.Release();
        }
        else
        {
            if (_combatMode == ArmyCombatMoveTokenEventMode.SetupReady && _combatType == CombatType.Tactical && StrategicMap.Current.RequestCombatType && StrategicMap.Current.Players.GetPlayers(ArmyHexSprite.Army.Player).PlayerType != PlayerType.Human)
            {
                _combatMode = ArmyCombatMoveTokenEventMode.SetupHotSeat;
                log = new NewTurnEventLog(null)
                {
                    Holder = CreationHolder(),
                    TurnPlayer = field.GetPlayer(),
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable),
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_youAreBeingAttackedText)
                };
                StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ShowHotSeatPlayerCallback);
                _ = log.Release();
            }
        }

        if (_combatMode == ArmyCombatMoveTokenEventMode.RequestingCombatType)
        {
            ArmyHexSprite.PauseMove();
            request = new CombatTypeRequestEventLog(null)
            {
                Holder = CreationHolder(),
                AttackerPlayer = ArmyHexSprite.Army.Player,
                DefenderPlayer = field.GetPlayer(),
                Location = new MapCoordinate(field.HexX, field.HexY, field.HexLevel)
            };
            if (StrategicMap.Current.SeatedPlayer != request.AttackerPlayer)
            {
                sbyte hexLevel = ArmyHexSprite.HexLevel;
                sbyte hexY = ArmyHexSprite.HexY;
                sbyte hexX = ArmyHexSprite.HexX;
                StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            }

            StrategicMap.Current.TriggerTypedExecuteEventLog(request, immediate: false, done: RequestCombatTypeCallback);
            _ = request.Release();
        }
    }

    // MoveT.PAS:2801
    public void CombatExecuted(Battle combat)
    {
        if (combat is not FastBattle)
        {
            ArmyHexSprite armyHexSprite;
            if (Set<CombatResult>.Of(CombatResult.Victory).Contains(combat.CombatResult))
            {
                armyHexSprite = StrategicMap.Current.MapContainer.GetField(combat.CombatData.AttackerLocation.X, combat.CombatData.AttackerLocation.Y, combat.CombatData.AttackerLocation.MapLevel).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    Army army = armyHexSprite.Army;
                    byte selection = 0;
                    for (int i = army.Units.Count - 1; i >= 0; i--)
                    {
                        selection = (byte)(selection << 1);
                        if (army.Units.GetUnit(i).UnitStatus.Contains(UnitStatusElement.Moving))
                        {
                            selection = (byte)(selection | 1);
                            Troop troop = army.Units.GetUnit(i);
                            army.Units.GetUnit(i).UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                        }
                    }

                    if (selection != 0)
                    {
                        MapField field = StrategicMap.Current.MapContainer.GetField(combat.CombatData.Location.X, combat.CombatData.Location.Y, combat.CombatData.Location.MapLevel);
                        MoveArmyTokenEvent move;
                        if (field.GetPlayer() == armyHexSprite.Army.Player)
                        {
                            move = CreateArmyMoveMapCoordinateTokenEvent(armyHexSprite, selection, combat.CombatData.Location.X, combat.CombatData.Location.Y, L: combat.CombatData.Location.MapLevel, action: MoveAction.Normal);
                        }
                        else
                        {
                            ArmyCombatMoveTokenEvent combatMove = CreateCombatMoveMapCoordinateTokenEvent(armyHexSprite, selection: selection, x: combat.CombatData.Location.X, y: combat.CombatData.Location.Y, l: combat.CombatData.Location.MapLevel);
                            combatMove?._ignoreDefenders = true;

                            move = combatMove;
                        }

                        if (move is not null)
                        {
                            StrategicMap.Current.TokenManager.TokenExecutor.AddSystemEvent(move);
                            _ = move.Release();
                        }
                    }
                }
            }
            else
            {
                armyHexSprite = StrategicMap.Current.MapContainer.GetField(combat.CombatData.AttackerLocation.X, combat.CombatData.AttackerLocation.Y, combat.CombatData.AttackerLocation.MapLevel).GetArmyHexSprite();
                if (armyHexSprite is not null)
                {
                    for (int i = armyHexSprite.Army.Units.Count - 1; i >= 0; i--)
                    {
                        Troop troop = armyHexSprite.Army.Units.GetUnit(i);
                        armyHexSprite.Army.Units.GetUnit(i).UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.Moving);
                    }

                    if (armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
                    {
                        StrategicMap.Current.SelectedArmy.Select(armyHexSprite);
                    }
                }
            }

            if (StrategicMap.Current.RequestCombatType && combat.CombatData.Players.GetPlayers(combat.CombatData.AttackerPlayer).Player != StrategicMap.Current.SeatedPlayer)
            {
                RestoreHotSeatMode();
            }

            StrategicMap.Current.SynchronizeBegin();
            DistributeCombatEvent(combat.Logbook, execute: false);
            StrategicMap.Current.SynchronizeEnd();
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

    private static void DistributeCombatEvent(CombatLogbook logbook, bool execute)
    {
        CombatData fFinalData = logbook.FinalData;
        int i = 1;
        for (int number = StrategicMap.Current.Players.GetCount() - 1; i <= number; i++)
        {
            if (StrategicMap.Current.VisibleForPlayer(fFinalData.Location.X, fFinalData.Location.Y, fFinalData.Location.MapLevel, player: (sbyte)i) || logbook.InitialData.Players.IndexOfPlayer((byte)i) != -1)
            {
                CombatEventLog combatEventLog = new(null) { Holder = logbook };
                combatEventLog.SetCombatLogbook(logbook);
                StrategicMap.Current.Players.GetPlayers(i).EventLogbook.AddEvent(combatEventLog);
                if (execute && i == StrategicMap.Current.SeatedPlayer && StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human && logbook.FinalData.CombatResult != 6)
                {
                    _ = combatEventLog.Execute(mode: false, trigger: ExecuteTrigger.Event, done: null);
                }
                _ = combatEventLog.Release();
            }
        }
    }

    private static MoveArmyTokenEvent CreateArmyMoveMapCoordinateTokenEvent(ArmyHexSprite army, byte selection, sbyte X, sbyte Y, sbyte L, MoveAction action)
    {
        MoveControlSettings moveControlSettings = CreateMoveControlSettings(army, selection: selection, x: X, y: Y, l: L);
        MoveArmyTokenEvent result = (moveControlSettings.Path.Count == 0) ? null : ArmyHexSprite.CreateArmyMoveTokenEvent(army, selection: selection, path: moveControlSettings.Path, action: action, mode: Set<ArmyMoveModeElement>.Of([]));
        _ = moveControlSettings.Release();
        return result;
    }

    private static ArmyCombatMoveTokenEvent CreateCombatMoveMapCoordinateTokenEvent(ArmyHexSprite army, byte selection, sbyte x, sbyte y, sbyte l)
    {
        MoveControlSettings moveControlSettings = CreateMoveControlSettings(army, selection: selection, x: x, y: y, l: l);
        ArmyCombatMoveTokenEvent result = (moveControlSettings.Path.Count == 0) ? null : ArmyHexSprite.CreateArmyCombatMoveTokenEvent(army, selection: selection, path: moveControlSettings.Path, mode: Set<ArmyMoveModeElement>.Of([]));
        _ = moveControlSettings.Release();
        return result;
    }

    private static MoveControlSettings CreateMoveControlSettings(ArmyHexSprite army, byte selection, sbyte x, sbyte y, sbyte l)
    {
        MoveControlSettings moveControlSettings = new(null) { Holder = army };
        sbyte hexLevel = army.HexLevel;
        sbyte hexY = army.HexY;
        moveControlSettings.SetSourcePosition(army.HexX, hexY, hexLevel);
        moveControlSettings.Player = army.Army.Player;
        moveControlSettings.ArmyHexSprite = army;
        moveControlSettings.Selection = selection;
        moveControlSettings.SetDestinationPosition(x, y, l);
        _ = moveControlSettings.MovePointTable;
        MovePointTable movePointTable = army.Army.CreateMovePointTable(selection);
        moveControlSettings.MovePointTable = movePointTable;
        StrategicMap.Current.MoveControl.ProcessMove(moveControlSettings);
        return moveControlSettings;
    }
}

public enum ArmyCombatMoveTokenEventMode : byte
{
    Moving = 0, RequestingCombatType = 1, SetupReady = 2, CombatExecuted = 3, CancelCombat = 4, SetupHotSeat = 5, RestoringHotSeat = 6
}
