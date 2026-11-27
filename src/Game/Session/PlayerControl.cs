#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Commands;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;
using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

// exit callback, exit mode, password, disconnected-player list, sync warning shown
public class PlayerControl(CustomNode? owner) : EngineObject(owner)
{

    private ExitGameCallBack? _exitCallback;
    private ExitGameMode _exitMode;
    private string _password = "";
    private readonly DoubleItemList _disconnectedPlayers = [];
    private bool _outOfSyncShown;

    // PlayerC.PAS:7
    public override void Dispose()
    {
        StrategicMap strategicMap = GetEngine()?.StrategicMap;
        if (strategicMap is not null && strategicMap.TokenManager is not null)
        {
            TokenExecutor tokenExecutor = strategicMap.TokenManager.TokenExecutor;
            tokenExecutor.SetGetSyncValueHandler(null);
            tokenExecutor.SetSyncErrorHandler(null);
        }
        base.Dispose();
    }

    // PlayerC.PAS:70
    public virtual void NewFrame()
    {
        ValidateTurnTiming();
        ValidateDisconnectedPlayers();
    }

    // PlayerC.PAS:76
    public virtual void NewDay()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.Day += 1;
        strategicMap.Players.BeginUpdate();
        strategicMap.LockRelationsChanged();
        for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
        {
            TextEventLog log = new NewDayEventLog(null)
            {
                Holder = CreationHolder(),
                Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_newDayFormat), arguments: [GetEngine().StrategicMap.Day])
            };
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.DayLimit != 0 && engineStrategicMap.DayLimit - engineStrategicMap.Day <= 10)
            {
                string formatDaysLeftString = DurationText.FormatDaysLeftString(GetEngine(), days: engineStrategicMap.DayLimit - engineStrategicMap.Day);
                log.Text = $"{log.Text} ({formatDaysLeftString})";
            }

            log.Player = PlayerId.FromOriginal((sbyte)i);
            GetEngine().StrategicMap.Players.GetPlayers(i).EventLogbook.AddEvent(log);
            _ = log.Release();
        }

        StrategicMap generateNewTurnOrderStrategicMap = GetEngine().StrategicMap;
        if (generateNewTurnOrderStrategicMap.TurnOrderMode == StrategicMapTurnOrderMode.Changing)
        {
            generateNewTurnOrderStrategicMap.GenerateNewTurnOrder();
        }

        StrategicMap resetPlayerRelationsStrategicMap = GetEngine().StrategicMap;
        if (resetPlayerRelationsStrategicMap.Day == 1)
        {
            resetPlayerRelationsStrategicMap.Races.ResetPlayerRelations();
            if (resetPlayerRelationsStrategicMap.ConnectionType == PlayerConnectionType.Local)
            {
                Randomize();
                GetEngine().StrategicMap.InitialRandomSeed = RandSeed;
            }

            StrategicMap defendersSeedStrategicMap = GetEngine().StrategicMap;
            defendersSeedStrategicMap.DefendersSeed = defendersSeedStrategicMap.InitialRandomSeed;
        }

        try
        {
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001985, sender: this);
            GetEngine().StrategicMap.SendMessage(ref message, sender: this);
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        try
        {
            GetEngine().StrategicMap.OnDayStarted(this);
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        StrategicMap dayLimitStrategicMap = GetEngine().StrategicMap;
        if (dayLimitStrategicMap.DayLimit == dayLimitStrategicMap.Day)
        {
            MessageEventLog messageLog = new(null, holder: CreationHolder())
            {
                Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.InfoText),
                Buttons = MessageEventLogButtons.Ok,
                Layout = MessageEventLogLayout.Normal
            };
            string turnLimitReached = AbilityRules.TranslateResourceString(GetEngine(), S: s_turnLimitReachedText);
            messageLog.Message.Text = turnLimitReached;
            GetEngine().StrategicMap.TriggerExecuteEventLog(messageLog, immediate: false, done: null);
            _ = messageLog.Release();
            StrategicMap gameOverStrategicMap = GetEngine().StrategicMap;
            gameOverStrategicMap.GameOver(gameOverStrategicMap.SeatedPlayer, status: GameOverStatus.Lost);
            for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
            {
                GetEngine().StrategicMap.GameOver(PlayerId.FromOriginal((sbyte)i), status: GameOverStatus.Lost);
            }
        }

        StrategicMap unlockRelationsChangedStrategicMap = GetEngine().StrategicMap;
        unlockRelationsChangedStrategicMap.UnlockRelationsChanged();
        unlockRelationsChangedStrategicMap.TriggerRelationsChanged();
        GetEngine().StrategicMap.Players.EndUpdate();
    }

    // PlayerC.PAS:177
    public virtual void NewTurn(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Day != 1)
        {
            Player? players = strategicMap.Players.GetPlayers(player);
            Player? fPlayersPlayers = strategicMap.Players.GetPlayers(player);
            int upkeep = strategicMap.Players.GetPlayers(player).GetUpkeep();
            strategicMap.Players.GetPlayers(player).SetGems(players.Gems + fPlayersPlayers.Income - upkeep);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.Players.GetPlayers(player).Gems < 0)
            {
                engineStrategicMap.Players.GetPlayers(player).SetGems(0);
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                Player? players2 = getPlayersStrategicMap.Players.GetPlayers(player);
                players2.MoraleValue = (players2.MoraleValue - 5).TruncateToSByte();
                if (getPlayersStrategicMap.Players.GetPlayers(player).MoraleValue < -50)
                {
                    getPlayersStrategicMap.Players.GetPlayers(player).MoraleValue = -50;
                }
            }
            else
            {
                GetEngine().StrategicMap.Players.GetPlayers(player).MoraleValue = 0;
            }

            GetEngine().StrategicMap.Players.GetPlayers(player).SetEndTurn(false);
        }

        StrategicMap lockUpdateVisibilityStrategicMap = GetEngine().StrategicMap;
        Player? players3 = lockUpdateVisibilityStrategicMap.Players.GetPlayers(player);
        players3.Day += 1;
        lockUpdateVisibilityStrategicMap.LockUpdateVisibility();
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001986, sender: this);
        message.Payload = MsgArgIds.FromPlayer(player);
        lockUpdateVisibilityStrategicMap.SendMessage(ref message, sender: this);
        GetEngine().StrategicMap.OnTurnStarted((sbyte)PlayerIdExtensions.ToOriginal(player));
        EngineMessage.InitializeMessage(out message, messageId: 537001987, sender: this);
        message.Payload = MsgArgIds.FromPlayer(player);
        GetEngine().StrategicMap.SendMessage(ref message, sender: this);
        GetEngine().StrategicMap.UnLockUpdateVisibility();
        StrategicMap saveHsmStrategicMap = GetEngine().StrategicMap;
        if (saveHsmStrategicMap.DebugSave)
        {
            try
            {
                if (saveHsmStrategicMap.PlayMode == PlayMode.Simultaneous)
                {
                    if (player == PlayerId.Independent)
                    {
                        GetEngine().SaveHsm($"{GetEngine().StartupDirectory}DEBUG\\{saveHsmStrategicMap.MapName}{saveHsmStrategicMap.Day.ToString(CultureInfo.InvariantCulture)}_p{saveHsmStrategicMap.SeatedPlayer.ToOriginal().ToString(CultureInfo.InvariantCulture)}{FileBoundary.WriteExtension(DocumentKind.Save)}", progress: null, profile: GameProfile.WorldMap);
                    }
                }
                else
                {
                    StrategicMap toStringStrategicMap = GetEngine().StrategicMap;
                    GetEngine().SaveHsm($"{GetEngine().StartupDirectory}DEBUG\\{toStringStrategicMap.MapName}{toStringStrategicMap.Day.ToString(CultureInfo.InvariantCulture)}-{player.ToOriginal().ToString(CultureInfo.InvariantCulture)}_p{toStringStrategicMap.SeatedPlayer.ToOriginal().ToString(CultureInfo.InvariantCulture)}{FileBoundary.WriteExtension(DocumentKind.Save)}", progress: null, profile: GameProfile.WorldMap);
                }
            }
            catch (Exception error)
            {
                HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
            }
        }
    }

    // PlayerC.PAS:133
    public virtual void AutoSave()
    {
        string fileName = "";
        if (GetEngine().StrategicMap.GameType == GameType.Campaign)
        {
            try
            {
                // The autosave takes the extension the host writes (own-formats.md 4.2).
                fileName = $"autosave{FileBoundary.WriteExtension(DocumentKind.CampaignSave)}";
                GetEngine().SaveCampaign($"{GetEngine().StartupDirectory}Save\\{fileName}");
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [fileName]);
                GetEngine().StrategicMap.ShowWarning(format);
            }
        }
        else
        {
            try
            {
                fileName = $"autosave{FileBoundary.WriteExtension(DocumentKind.Save)}";
                GetEngine().SaveHsm($"{GetEngine().StartupDirectory}Save\\{fileName}", progress: null, profile: GameProfile.WorldMap);
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [fileName]);
                GetEngine().StrategicMap.ShowWarning(format);
            }
        }
    }

    // PlayerC.PAS:515
    public virtual void ValidateTurnTiming()
    {
    }

    // PlayerC.PAS:19
    public virtual void Setup()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.ConnectionType == PlayerConnectionType.Network && !strategicMap.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            if (strategicMap.PlayerCommunicator.PlayerCommunicationControl is null)
            {
                throw new InvalidOperationException("Please setup DirectPlayPCC");
            }

            TokenExecutor tokenExecutor = strategicMap.TokenManager.TokenExecutor;
            tokenExecutor.SetGetSyncValueHandler(GetSyncValue);
            tokenExecutor.SetSyncErrorHandler(OutOfSync);
        }
        else
        {
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if ((engineStrategicMap.Status.Contains(HexSpriteMapStatuses.Editting) || engineStrategicMap.ConnectionType is PlayerConnectionType.Local or PlayerConnectionType.Email) && engineStrategicMap.PlayerCommunicator.PlayerCommunicationControl is null)
            {
                LocalPlayerCommunicationControl playerCommunicationControl = new(this);
                playerCommunicationControl.Setup(engineStrategicMap.Players.Count);
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                for (int i = 0, last = getPlayersStrategicMap.Players.Count - 1; i <= last; i++)
                {
                    if (getPlayersStrategicMap.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                    {
                        playerCommunicationControl.SetConnectionMode(PlayerId.FromInt32(i), mode: ConnectionMode.Local);
                    }
                    else
                    {
                        playerCommunicationControl.SetConnectionMode(PlayerId.FromInt32(i), mode: ConnectionMode.Global);
                    }
                }

                _ = getPlayersStrategicMap.PlayerCommunicator.ActivatePlayerCommunicationControl(playerCommunicationControl);
                playerCommunicationControl.Release();
                GetEngine().StrategicMap.Players.Update();
            }
        }
    }

    // PlayerC.PAS:224
    public virtual void StartNewMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        for (int i = 0, last = strategicMap.Players.Count - 1; i <= last; i++)
        {
            if (i == 0 || strategicMap.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                strategicMap.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }
    }

    // PlayerC.PAS:233
    public virtual void StartLoadedMap()
    {
    }

    // PlayerC.PAS:463
    public virtual void PasswordFailed() => GetEngine().StrategicMap.ExecuteExitGame();

    // PlayerC.PAS:468
    public virtual void PasswordValidated()
    {
    }

    // PlayerC.PAS:236
    public virtual void SetPlayerGameOverStatus(PlayerId? player, GameOverStatus status) => GetEngine().StrategicMap.Players.GetPlayers(player).SetGameOverStatus(status);

    // PlayerC.PAS:241
    public virtual void GameOver(PlayerId? player, GameOverStatus status)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(player).GameOverStatus != status && strategicMap.Players.GetPlayers(player).Status != PlayerStatus.Terminated)
        {
            strategicMap.LockGameOverEvent();
            SetPlayerGameOverStatus(player, status);
            if (status == GameOverStatus.Lost)
            {
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (!engineStrategicMap.AlliedVictory)
                {
                    if (engineStrategicMap.Players.ActivePlayers.Count == 1)
                    {
                        GameOver(PlayerId.FromOriginal((sbyte)engineStrategicMap.Players.ActivePlayers[0]), status: GameOverStatus.Won);
                    }
                }
                else
                {
                    if (GetEngine().StrategicMap.Players.GetActivePlayersAllied())
                    {
                        for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
                        {
                            if (i != PlayerIdExtensions.ToOriginal(player) && GetEngine().StrategicMap.Players.GetPlayers(i).Status == PlayerStatus.Playing && GetEngine().StrategicMap.Players.GetPlayers(i).GameOverStatus != GameOverStatus.Lost)
                            {
                                SetPlayerGameOverStatus(PlayerId.FromOriginal((sbyte)i), status: GameOverStatus.Won);
                            }
                        }
                    }
                }
            }
            else
            {
                if (status == GameOverStatus.Won)
                {
                    for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
                    {
                        if (i != PlayerIdExtensions.ToOriginal(player) && GetEngine().StrategicMap.Players.GetPlayers(i).Status == PlayerStatus.Playing && GetEngine().StrategicMap.Players.GetPlayers(i).GameOverStatus != GameOverStatus.Lost)
                        {
                            StrategicMap getDiplomaticRelationStrategicMap = GetEngine().StrategicMap;
                            if (getDiplomaticRelationStrategicMap.Players.GetDiplomaticRelation(player, otherPlayer: PlayerId.FromOriginal((sbyte)i)) == DiplomaticRelation.Alliance && getDiplomaticRelationStrategicMap.AlliedVictory)
                            {
                                SetPlayerGameOverStatus(PlayerId.FromOriginal((sbyte)i), status: GameOverStatus.Won);
                            }
                            else
                            {
                                SetPlayerGameOverStatus(PlayerId.FromOriginal((sbyte)i), status: GameOverStatus.Lost);
                            }
                        }
                    }
                }
            }

            GetEngine().StrategicMap.UnlockGameOverEvent();
        }
    }

    // PlayerC.PAS:274
    public virtual void GameOverEventHandled(PlayerId? player) => GetEngine().StrategicMap.Players.GetPlayers(player).Status = PlayerStatus.Terminated;

    // PlayerC.PAS:388
    public virtual void ExitGame(ExitGameMode mode, ExitGameCallBack? done)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Combat is null)
        {
            MessageEventLog log;
            if (strategicMap.ConnectionType == PlayerConnectionType.Network && strategicMap.GameStatus != StrategicMapGameStatus.Terminated && strategicMap.GameType != GameType.Tutorial && strategicMap.Players.LocalPlayers.Count > 1)
            {
                if (strategicMap.PlayMode == PlayMode.Simultaneous || strategicMap.CurrentPlayer == strategicMap.SeatedPlayer)
                {
                    _exitMode = mode;
                    _exitCallback = done;
                    log = new MessageEventLog(null, holder: CreationHolder())
                    {
                        Title = AbilityRules.TranslateResourceString(GetEngine(), S: ConfirmText),
                        Buttons = MessageEventLogButtons.YesNoCancel,
                        Layout = MessageEventLogLayout.Normal
                    };
                    string yourAboutToExitAMultiPlayerGame = AbilityRules.TranslateResourceString(GetEngine(), S: s_yourAboutToExitMultiPlayerGameText);
                    _ = log.Message.Add(yourAboutToExitAMultiPlayerGame);
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWantToContinueExitingText);
                    _ = log.Message.Add(translatedText);
                    GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ExitNetworkGameCallback);
                    _ = log.Release();
                }
                else
                {
                    log = new MessageEventLog(null, holder: CreationHolder())
                    {
                        Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.MessageText),
                        Buttons = MessageEventLogButtons.Ok,
                        Layout = MessageEventLogLayout.Normal
                    };
                    string pleaseWaitTillItIsYourTurn = AbilityRules.TranslateResourceString(GetEngine(), S: s_pleaseWaitTillItIsYourTurnText);
                    _ = log.Message.Add(pleaseWaitTillItIsYourTurn);
                    GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: null);
                    _ = log.Release();
                }
            }
            else
            {
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (engineStrategicMap.Status.Contains(HexSpriteMapStatuses.Modified) && engineStrategicMap.GameStatus != StrategicMapGameStatus.Terminated && engineStrategicMap.GameType != GameType.Tutorial)
                {
                    if (_exitCallback is null)
                    {
                        if (mode != ExitGameMode.Reload)
                        {
                            _exitMode = mode;
                            _exitCallback = done;
                            log = new MessageEventLog(null, holder: CreationHolder())
                            {
                                Title = AbilityRules.TranslateResourceString(GetEngine(), S: ConfirmText),
                                Buttons = MessageEventLogButtons.YesNoCancel
                            };
                            if (GetEngine().Campaign is null && !IsEmpty(GetEngine().StrategicMap.FileName) && !DocumentKind.Map.HasExtension(ExtractFileExt(GetEngine().StrategicMap.FileName)))
                            {
                                log.Layout = MessageEventLogLayout.Normal;
                                // The message names the file the save writes (own-formats.md 4.2).
                                string fileName = ExtractFileName(FileBoundary.WriteName(GetEngine().StrategicMap.FileName));
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_saveGameFormat), arguments: [fileName]);
                                _ = log.Message.Add(format);
                            }
                            else
                            {
                                if (GetEngine().Campaign is not null && !IsEmpty(GetEngine().Campaign.FileName) && !DocumentKind.CampaignMap.HasExtension(ExtractFileExt(GetEngine().StrategicMap.FileName)))
                                {
                                    // { Original tests the map extension when saving a campaign. }
                                    log.Layout = MessageEventLogLayout.Normal;
                                    string fileName = ExtractFileName(FileBoundary.WriteName(GetEngine().Campaign.FileName));
                                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_saveGameFormat), arguments: [fileName]);
                                    _ = log.Message.Add(format);
                                }
                                else
                                {
                                    log.Layout = MessageEventLogLayout.Normal;
                                    string doYouWantToSaveThisGame = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWantToSaveThisGameText);
                                    _ = log.Message.Add(doYouWantToSaveThisGame);
                                }
                            }

                            GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: SaveGameOnExitCallback);
                            _ = log.Release();
                        }
                        else
                        {
                            done(mode, Exited: true);
                        }
                    }
                    else
                    {
                        _exitCallback(_exitMode, Exited: false);
                    }
                }
                else
                {
                    GetEngine().StrategicMap.ExecuteExitGame();
                }
            }
        }
        else
        {
            done?.Invoke(mode, Exited: false);
        }
    }

    // PlayerC.PAS:153
    public virtual void EndTurn(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.LockRelationsChanged();
        if (player == strategicMap.SeatedPlayer)
        {
            Player? p = strategicMap.Players.GetPlayers(player);
            p.SetEndTurn(true);
            if (p is not null)
            {
                GetEngine().StrategicMap.GetViewCenter(out int x, out int y, out int l);
                p.View = new MapCoordinate(unchecked((sbyte)(byte)x), unchecked((sbyte)(byte)y), unchecked((sbyte)(byte)l));
            }
        }

        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001991, sender: this);
        message.Payload = MsgArgIds.FromPlayer(player);
        GetEngine().StrategicMap.SendMessage(ref message, sender: this);
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        engineStrategicMap.UnlockRelationsChanged();
        engineStrategicMap.TriggerRelationsChanged();
    }

    // PlayerC.PAS:279
    public virtual void SetSeatedPlayer(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (player != strategicMap.SeatedPlayer)
        {
            strategicMap.SeatedPlayer = player;
            strategicMap.NotifySeatedPlayerChanged();
            GetEngine().StrategicMap.ResetSeatedPlayerView();
        }
    }

    // PlayerC.PAS:518
    public virtual void ValidateDisconnectedPlayers()
    {
        for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
        {
            int player = _disconnectedPlayers[i].First;
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.Players.GetPlayers(player).GameOverStatus == GameOverStatus.None)
            {
                if (strategicMap.TokenManager.PlayerReady(strategicMap.CurrentPlayer) && strategicMap.TokenManager.TokenExecutor.ActiveTacticalBattle.Count == 0)
                {
                    int count = _disconnectedPlayers[i].Second;
                    if (count > 0)
                    {
                        count -= 1;
                        _disconnectedPlayers.List[i] = _disconnectedPlayers.List[i] with { Second = count };
                    }

                    if (count == 0)
                    {
                        PlayerConnectionEventLog log = new(null, holder: CreationHolder());
                        log.Setup(PlayerId.FromOriginal((sbyte)player), reason: 2);
                        GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: null);
                        _ = log.Release();
                        _disconnectedPlayers.Delete(i);
                    }
                }
            }
            else
            {
                _disconnectedPlayers.Delete(i);
            }
        }
    }

    // PlayerC.PAS:556
    public virtual void ConnectionLost()
    {
        PlayerConnectionEventLog log = new(null, holder: CreationHolder());
        log.Setup(null, reason: 2);
        GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: null);
        _ = log.Release();
    }

    // PlayerC.PAS:289
    public virtual bool LockAI()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        return strategicMap.ConnectionType == PlayerConnectionType.Local && !strategicMap.RequestCombatType && strategicMap.AILockCount != 0;
    }

    // PlayerC.PAS:49
    public void GetSyncValue(out int value) => value = GetEngine().StrategicMap.DefendersSeed;

    // PlayerC.PAS:54
    public void OutOfSync(object sender)
    {
        if (GetEngine().StrategicMap.TokenManager.TokenExecutor.ActiveTacticalBattle.Active)
        {
            if (!_outOfSyncShown)
            {
                PlayerConnectionEventLog log = new(null, holder: CreationHolder());
                log.Setup(null, reason: 3);
                StrategicMap strategicMap = GetEngine().StrategicMap;
                strategicMap.TriggerExecuteEventLog(log, immediate: strategicMap.Combat is not null, done: null);
                _ = log.Release();
                _outOfSyncShown = true;
            }
            else
            {
                GetEngine().StrategicMap.TokenManager.TokenExecutor.Lock();
            }
        }
    }

    // PlayerC.PAS:295
    public void ListPlayerInfo(DelphiStringCollection strings)
    {
        PlayerCommunicationControl? playerCommunicationControl = GetEngine().StrategicMap.PlayerCommunicator.PlayerCommunicationControl;
        for (int i = 0, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
        {
            string text = $"Player {i.ToString(CultureInfo.InvariantCulture)}";
            int race = GetEngine().StrategicMap.Players.GetPlayers(i).PlayerRace;
            if (race != -1)
            {
                string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[race]);
                text = $"{text}({raceNamesPlural})";
            }

            text = $"{text} at system {playerCommunicationControl.Players[i].SystemId.ToString(CultureInfo.InvariantCulture)}";
            _ = (strings?.Add(text));

            GetEngine().OutputDebugString(text);
        }
    }

    // PlayerC.PAS:313
    public void ExitSaveGameCallback(SaveGameEventLog log)
    {
        if (!IsEmpty(log.FileName))
        {
            if (GetEngine().Campaign is not null)
            {
                GetEngine().SaveCampaign(log.FileName);
            }
            else
            {
                GetEngine().SaveHsm(log.FileName, progress: null, profile: GameProfile.WorldMap);
            }

            GetEngine().StrategicMap.ExecuteExitGame();
            _exitCallback?.Invoke(_exitMode, Exited: true);
        }
        else
        {
            _exitCallback?.Invoke(_exitMode, Exited: false);
        }

        _exitCallback = null;
    }

    // PlayerC.PAS:326
    public void SaveGameOnExitCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            if ((GetEngine().Campaign is null && (IsEmpty(GetEngine().StrategicMap.FileName) || DocumentKind.Map.HasExtension(ExtractFileExt(GetEngine().StrategicMap.FileName)))) || (GetEngine().Campaign is not null && (IsEmpty(GetEngine().Campaign.FileName) || DocumentKind.CampaignMap.HasExtension(ExtractFileExt(GetEngine().StrategicMap.FileName)))))
            {
                SaveGameEventLog saveLog = new(null, holder: CreationHolder());
                StrategicMap strategicMap = GetEngine().StrategicMap;
                if (!IsEmpty(strategicMap.FileName))
                {
                    saveLog.DefaultName = ExtractFileName(strategicMap.FileName);
                    if (Pos(".", haystack: saveLog.DefaultName) != 0)
                    {
                        saveLog.DefaultName = DelphiStrings.Copy(saveLog.DefaultName, index: 1, count: Pos(".", haystack: saveLog.DefaultName) - 1);
                    }
                }
                else
                {
                    saveLog.DefaultName = strategicMap.MapName;
                }

                saveLog.Directory = $"{GetEngine().StartupDirectory}Save\\";
                strategicMap.TriggerTypedExecuteEventLog(saveLog, immediate: false, done: ExitSaveGameCallback);
                _ = saveLog.Release();
            }
            else
            {
                GameEngine? engine = GetEngine();
                // The save writes the own sibling of an original's name, which stays untouched
                // (own-formats.md 4.2).
                if (engine.Campaign is not null)
                {
                    engine.SaveCampaign(FileBoundary.WriteName(engine.Campaign.FileName));
                }
                else
                {
                    engine.SaveHsm(FileBoundary.WriteName(engine.StrategicMap.FileName), progress: null, profile: GameProfile.WorldMap);
                }

                engine.StrategicMap.ExecuteExitGame();
                _exitCallback?.Invoke(_exitMode, Exited: true);

                _exitCallback = null;
            }
        }
        else
        {
            if (log.PressedButton == PressedEventLogButton.No)
            {
                GetEngine().StrategicMap.ExecuteExitGame();
                _exitCallback?.Invoke(_exitMode, Exited: true);

                _exitCallback = null;
            }
            else
            {
                if (log.PressedButton == PressedEventLogButton.Cancel)
                {
                    _exitCallback?.Invoke(_exitMode, Exited: false);

                    _exitCallback = null;
                }
            }
        }
    }

    // PlayerC.PAS:372
    public void ExitNetworkGameCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            if (strategicMap.PlayMode == PlayMode.Simultaneous || strategicMap.CurrentPlayer == strategicMap.SeatedPlayer)
            {
                TerminatePlayerTokenEvent tokenEvent = new(null)
                {
                    Holder = CreationHolder(),
                    Player = strategicMap.SeatedPlayer,
                    ExitCallback = _exitCallback,
                    ExitMode = _exitMode
                };
                strategicMap.TokenManager.GetTokenControl(strategicMap.SeatedPlayer).AddEvent(tokenEvent);
                _ = tokenEvent.Release();
            }
        }
    }

    // PlayerC.PAS:471
    public void PasswordTryAgainCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            PasswordEventLog passwordLog = new(null, holder: CreationHolder());
            GetEngine().StrategicMap.TriggerTypedExecuteEventLog(passwordLog, immediate: false, done: ValidatePasswordCallback);
            _ = passwordLog.Release();
        }
        else
        {
            PasswordFailed();
        }
    }

    // PlayerC.PAS:484
    public void ValidatePasswordCallback(PasswordEventLog log)
    {
        string password = AbilityRules.Encode(log.Password);
        if (log.PressedButton != PressedEventLogButton.Ok || password != _password)
        {
            MessageEventLog messageLog = new(null, holder: CreationHolder())
            {
                Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarningText),
                Buttons = MessageEventLogButtons.YesNo
            };
            string passwordFailed = AbilityRules.TranslateResourceString(GetEngine(), S: s_passwordFailedText);
            messageLog.Message.Text = passwordFailed;
            _ = messageLog.Message.Add("");
            string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWantToTryAgainText);
            _ = messageLog.Message.Add(translatedText);
            GetEngine().StrategicMap.TriggerTypedExecuteEventLog(messageLog, immediate: false, done: PasswordTryAgainCallback);
            _ = messageLog.Release();
        }
        else
        {
            PasswordValidated();
        }
    }

    // PlayerC.PAS:504
    public void ValidatePassword(string password)
    {
        if (GetEngine().StrategicMap.SeatedPlayer is not null)
        {
            SetSeatedPlayer(null);
        }

        _password = password;
        PasswordEventLog log = new(null, holder: CreationHolder());
        GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ValidatePasswordCallback);
        _ = log.Release();
    }

    // PlayerC.PAS:551
    public void PlayerDisconnected(PlayerId? player)
    {
        if (_disconnectedPlayers.IndexOfItemA(PlayerIdExtensions.ToOriginal(player)) == -1)
        {
            _ = _disconnectedPlayers.AddAbility(PlayerIdExtensions.ToOriginal(player), 150);
        }
    }

    private static readonly string s_newDayFormat = "Day %d dawns";
    private static readonly string s_turnLimitReachedText = "Sorry, turn limit reached!";

    internal static string ConfirmText { get; } = "Confirm";

    private static readonly string s_yourAboutToExitMultiPlayerGameText = "You're about to exit a multi-player game!";
    private static readonly string s_doYouWantToContinueExitingText = "Do you want to continue exiting?";
    private static readonly string s_pleaseWaitTillItIsYourTurnText = "Please, wait till it is your turn.";
    private static readonly string s_saveGameFormat = "Save game to %s?";
    private static readonly string s_doYouWantToSaveThisGameText = "Do you want to save this game?";
    private static readonly string s_passwordFailedText = "Password failed!";
    private static readonly string s_doYouWantToTryAgainText = "Do you want to try again?";
}
