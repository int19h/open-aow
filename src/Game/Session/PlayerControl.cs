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
        if (StrategicMap.Current is not null && StrategicMap.Current.TokenManager is not null)
        {
            TokenExecutor tokenExecutor = StrategicMap.Current.TokenManager.TokenExecutor;
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
        StrategicMap.Current.Day += 1;
        StrategicMap.Current.Players.BeginUpdate();
        StrategicMap.Current.LockRelationsChanged();
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            TextEventLog log = new NewDayEventLog(null)
            {
                Holder = CreationHolder(),
                Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_newDayFormat), arguments: [StrategicMap.Current.Day])
            };
            if (StrategicMap.Current.DayLimit != 0 && StrategicMap.Current.DayLimit - StrategicMap.Current.Day <= 10)
            {
                string formatDaysLeftString = DurationText.FormatDaysLeftString(GetEngine(), days: StrategicMap.Current.DayLimit - StrategicMap.Current.Day);
                log.Text = $"{log.Text} ({formatDaysLeftString})";
            }

            log.Player = (sbyte)i;
            StrategicMap.Current.Players.GetPlayers(i).EventLogbook.AddEvent(log);
            _ = log.Release();
        }

        if (StrategicMap.Current.TurnOrderMode == StrategicMapTurnOrderMode.Changing)
        {
            StrategicMap.Current.GenerateNewTurnOrder();
        }

        if (StrategicMap.Current.Day == 1)
        {
            StrategicMap.Current.Races.ResetPlayerRelations();
            if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Local)
            {
                Randomize();
                StrategicMap.Current.InitialRandomSeed = RandSeed;
            }

            StrategicMap.Current.DefendersSeed = StrategicMap.Current.InitialRandomSeed;
        }

        try
        {
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001985, sender: this);
            StrategicMap.Current.SendMessage(ref message, sender: this);
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        try
        {
            StrategicMap.Current.NewDayEventsStore.Raise(this);
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }

        if (StrategicMap.Current.DayLimit == StrategicMap.Current.Day)
        {
            MessageEventLog messageLog = new(null, holder: CreationHolder())
            {
                Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.InfoText),
                Buttons = MessageEventLogButtons.Ok,
                Layout = MessageEventLogLayout.Normal
            };
            string turnLimitReached = AbilityRules.TranslateResourceString(GetEngine(), S: s_turnLimitReachedText);
            messageLog.Message.Text = turnLimitReached;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, immediate: false, done: null);
            _ = messageLog.Release();
            StrategicMap.Current.GameOver(StrategicMap.Current.SeatedPlayer, status: GameOverStatus.Lost);
            for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
            {
                StrategicMap.Current.GameOver((sbyte)i, status: GameOverStatus.Lost);
            }
        }

        StrategicMap.Current.UnlockRelationsChanged();
        StrategicMap.Current.TriggerRelationsChanged();
        StrategicMap.Current.Players.EndUpdate();
    }

    // PlayerC.PAS:177
    public virtual void NewTurn(sbyte player)
    {
        if (StrategicMap.Current.Day != 1)
        {
            Player? players = StrategicMap.Current.Players.GetPlayers(player);
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            int upkeep = StrategicMap.Current.Players.GetPlayers(player).GetUpkeep();
            StrategicMap.Current.Players.GetPlayers(player).SetGems(players.Gems + fPlayersPlayers.Income - upkeep);
            if (StrategicMap.Current.Players.GetPlayers(player).Gems < 0)
            {
                StrategicMap.Current.Players.GetPlayers(player).SetGems(0);
                Player? players2 = StrategicMap.Current.Players.GetPlayers(player);
                players2.MoraleValue = (players2.MoraleValue - 5).TruncateToSByte();
                if (StrategicMap.Current.Players.GetPlayers(player).MoraleValue < -50)
                {
                    StrategicMap.Current.Players.GetPlayers(player).MoraleValue = -50;
                }
            }
            else
            {
                StrategicMap.Current.Players.GetPlayers(player).MoraleValue = 0;
            }

            StrategicMap.Current.Players.GetPlayers(player).SetEndTurn(false);
        }

        Player? players3 = StrategicMap.Current.Players.GetPlayers(player);
        players3.Day += 1;
        StrategicMap.Current.LockUpdateVisibility();
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001986, sender: this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, sender: this);
        StrategicMap.Current.NewTurnEventsStore.Raise(player);
        EngineMessage.InitializeMessage(out message, messageId: 537001987, sender: this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, sender: this);
        StrategicMap.Current.UnLockUpdateVisibility();
        if (StrategicMap.Current.DebugSave)
        {
            try
            {
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    if (player == 0)
                    {
                        GetEngine().SaveHsm($"{GetEngine().StartupDirectory}DEBUG\\{StrategicMap.Current.MapName}{StrategicMap.Current.Day.ToString(CultureInfo.InvariantCulture)}_p{((int)StrategicMap.Current.SeatedPlayer).ToString(CultureInfo.InvariantCulture)}{FileBoundary.WriteExtension(DocumentKind.Save)}", progress: null, profile: GameProfile.WorldMap);
                    }
                }
                else
                {
                    GetEngine().SaveHsm($"{GetEngine().StartupDirectory}DEBUG\\{StrategicMap.Current.MapName}{StrategicMap.Current.Day.ToString(CultureInfo.InvariantCulture)}-{((int)player).ToString(CultureInfo.InvariantCulture)}_p{((int)StrategicMap.Current.SeatedPlayer).ToString(CultureInfo.InvariantCulture)}{FileBoundary.WriteExtension(DocumentKind.Save)}", progress: null, profile: GameProfile.WorldMap);
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
        if (StrategicMap.Current.GameType == GameType.Campaign)
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
                StrategicMap.Current.ShowWarning(format);
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
                StrategicMap.Current.ShowWarning(format);
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
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl is null)
            {
                throw new InvalidOperationException("Please setup DirectPlayPCC");
            }

            TokenExecutor tokenExecutor = StrategicMap.Current.TokenManager.TokenExecutor;
            tokenExecutor.SetGetSyncValueHandler(GetSyncValue);
            tokenExecutor.SetSyncErrorHandler(OutOfSync);
        }
        else
        {
            if ((StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) || StrategicMap.Current.ConnectionType is PlayerConnectionType.Local or PlayerConnectionType.Email) && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl is null)
            {
                LocalPlayerCommunicationControl playerCommunicationControl = new(this);
                playerCommunicationControl.Setup(StrategicMap.Current.Players.Count);
                for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                {
                    if (StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                    {
                        playerCommunicationControl.SetConnectionMode(i, mode: ConnectionMode.Local);
                    }
                    else
                    {
                        playerCommunicationControl.SetConnectionMode(i, mode: ConnectionMode.Global);
                    }
                }

                _ = StrategicMap.Current.PlayerCommunicator.ActivatePlayerCommunicationControl(playerCommunicationControl);
                playerCommunicationControl.Release();
                StrategicMap.Current.Players.Update();
            }
        }
    }

    // PlayerC.PAS:224
    public virtual void StartNewMap()
    {
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i == 0 || StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                StrategicMap.Current.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }
    }

    // PlayerC.PAS:233
    public virtual void StartLoadedMap()
    {
    }

    // PlayerC.PAS:463
    public virtual void PasswordFailed() => StrategicMap.Current.ExecuteExitGame();

    // PlayerC.PAS:468
    public virtual void PasswordValidated()
    {
    }

    // PlayerC.PAS:236
    public virtual void SetPlayerGameOverStatus(sbyte player, GameOverStatus status) => StrategicMap.Current.Players.GetPlayers(player).SetGameOverStatus(status);

    // PlayerC.PAS:241
    public virtual void GameOver(sbyte player, GameOverStatus status)
    {
        if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus != status && StrategicMap.Current.Players.GetPlayers(player).Status != PlayerStatus.Terminated)
        {
            StrategicMap.Current.LockGameOverEvent();
            SetPlayerGameOverStatus(player, status);
            if (status == GameOverStatus.Lost)
            {
                if (!StrategicMap.Current.AlliedVictory)
                {
                    if (StrategicMap.Current.Players.ActivePlayers.Count == 1)
                    {
                        GameOver((sbyte)StrategicMap.Current.Players.ActivePlayers[0], status: GameOverStatus.Won);
                    }
                }
                else
                {
                    if (StrategicMap.Current.Players.GetActivePlayersAllied())
                    {
                        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                        {
                            if (i != player && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing && StrategicMap.Current.Players.GetPlayers(i).GameOverStatus != GameOverStatus.Lost)
                            {
                                SetPlayerGameOverStatus((sbyte)i, status: GameOverStatus.Won);
                            }
                        }
                    }
                }
            }
            else
            {
                if (status == GameOverStatus.Won)
                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        if (i != player && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing && StrategicMap.Current.Players.GetPlayers(i).GameOverStatus != GameOverStatus.Lost)
                        {
                            if (StrategicMap.Current.Players.GetDiplomaticRelation(player, otherPlayer: (sbyte)i) == DiplomaticRelation.Alliance && StrategicMap.Current.AlliedVictory)
                            {
                                SetPlayerGameOverStatus((sbyte)i, status: GameOverStatus.Won);
                            }
                            else
                            {
                                SetPlayerGameOverStatus((sbyte)i, status: GameOverStatus.Lost);
                            }
                        }
                    }
                }
            }

            StrategicMap.Current.UnlockGameOverEvent();
        }
    }

    // PlayerC.PAS:274
    public virtual void GameOverEventHandled(sbyte player) => StrategicMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;

    // PlayerC.PAS:388
    public virtual void ExitGame(ExitGameMode mode, ExitGameCallBack? done)
    {
        if (StrategicMap.Current.Combat is null)
        {
            MessageEventLog log;
            if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network && StrategicMap.Current.GameStatus != StrategicMapGameStatus.Terminated && StrategicMap.Current.GameType != GameType.Tutorial && StrategicMap.Current.Players.LocalPlayers.Count > 1)
            {
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous || StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
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
                    StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ExitNetworkGameCallback);
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
                    StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: null);
                    _ = log.Release();
                }
            }
            else
            {
                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Modified) && StrategicMap.Current.GameStatus != StrategicMapGameStatus.Terminated && StrategicMap.Current.GameType != GameType.Tutorial)
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
                            if (GetEngine().Campaign is null && !IsEmpty(StrategicMap.Current.FileName) && !DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
                            {
                                log.Layout = MessageEventLogLayout.Normal;
                                // The message names the file the save writes (own-formats.md 4.2).
                                string fileName = ExtractFileName(FileBoundary.WriteName(StrategicMap.Current.FileName));
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_saveGameFormat), arguments: [fileName]);
                                _ = log.Message.Add(format);
                            }
                            else
                            {
                                if (GetEngine().Campaign is not null && !IsEmpty(GetEngine().Campaign.FileName) && !DocumentKind.CampaignMap.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
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

                            StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: SaveGameOnExitCallback);
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
                    StrategicMap.Current.ExecuteExitGame();
                }
            }
        }
        else
        {
            done?.Invoke(mode, Exited: false);
        }
    }

    // PlayerC.PAS:153
    public virtual void EndTurn(sbyte player)
    {
        StrategicMap.Current.LockRelationsChanged();
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            Player? p = StrategicMap.Current.Players.GetPlayers(player);
            p.SetEndTurn(true);
            if (p is not null)
            {
                StrategicMap.Current.GetViewCenter(out int x, out int y, out int l);
                p.View = new MapCoordinate(unchecked((sbyte)(byte)x), unchecked((sbyte)(byte)y), unchecked((sbyte)(byte)l));
            }
        }

        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537001991, sender: this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, sender: this);
        StrategicMap.Current.UnlockRelationsChanged();
        StrategicMap.Current.TriggerRelationsChanged();
    }

    // PlayerC.PAS:279
    public virtual void SetSeatedPlayer(sbyte player)
    {
        if (player != StrategicMap.Current.SeatedPlayer)
        {
            StrategicMap.Current.SeatedPlayer = player;
            StrategicMap.Current.NotifySeatedPlayerChanged();
            StrategicMap.Current.ResetSeatedPlayerView();
        }
    }

    // PlayerC.PAS:518
    public virtual void ValidateDisconnectedPlayers()
    {
        for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
        {
            int player = _disconnectedPlayers[i].First;
            if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.None)
            {
                if (StrategicMap.Current.TokenManager.PlayerReady(StrategicMap.Current.CurrentPlayer) && StrategicMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Count == 0)
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
                        log.Setup((sbyte)player, reason: 2);
                        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: null);
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
        log.Setup(-1, reason: 2);
        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: null);
        _ = log.Release();
    }

    // PlayerC.PAS:289
    public virtual bool LockAI() => StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && !StrategicMap.Current.RequestCombatType && StrategicMap.Current.AILockCount != 0;

    // PlayerC.PAS:49
    public void GetSyncValue(out int value) => value = StrategicMap.Current.DefendersSeed;

    // PlayerC.PAS:54
    public void OutOfSync(object sender)
    {
        if (StrategicMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Active)
        {
            if (!_outOfSyncShown)
            {
                PlayerConnectionEventLog log = new(null, holder: CreationHolder());
                log.Setup(-1, reason: 3);
                StrategicMap.Current.TriggerExecuteEventLog(log, immediate: StrategicMap.Current.Combat is not null, done: null);
                _ = log.Release();
                _outOfSyncShown = true;
            }
            else
            {
                StrategicMap.Current.TokenManager.TokenExecutor.Lock();
            }
        }
    }

    // PlayerC.PAS:295
    public void ListPlayerInfo(DelphiStringCollection strings)
    {
        PlayerCommunicationControl? playerCommunicationControl = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            string text = $"Player {i.ToString(CultureInfo.InvariantCulture)}";
            int race = StrategicMap.Current.Players.GetPlayers(i).PlayerRace;
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

            StrategicMap.Current.ExecuteExitGame();
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
            if ((GetEngine().Campaign is null && (IsEmpty(StrategicMap.Current.FileName) || DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))) || (GetEngine().Campaign is not null && (IsEmpty(GetEngine().Campaign.FileName) || DocumentKind.CampaignMap.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))))
            {
                SaveGameEventLog saveLog = new(null, holder: CreationHolder());
                if (!IsEmpty(StrategicMap.Current.FileName))
                {
                    saveLog.DefaultName = ExtractFileName(StrategicMap.Current.FileName);
                    if (Pos(".", haystack: saveLog.DefaultName) != 0)
                    {
                        saveLog.DefaultName = DelphiStrings.Copy(saveLog.DefaultName, index: 1, count: Pos(".", haystack: saveLog.DefaultName) - 1);
                    }
                }
                else
                {
                    saveLog.DefaultName = StrategicMap.Current.MapName;
                }

                saveLog.Directory = $"{GetEngine().StartupDirectory}Save\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(saveLog, immediate: false, done: ExitSaveGameCallback);
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
                    engine.SaveHsm(FileBoundary.WriteName(StrategicMap.Current.FileName), progress: null, profile: GameProfile.WorldMap);
                }

                StrategicMap.Current.ExecuteExitGame();
                _exitCallback?.Invoke(_exitMode, Exited: true);

                _exitCallback = null;
            }
        }
        else
        {
            if (log.PressedButton == PressedEventLogButton.No)
            {
                StrategicMap.Current.ExecuteExitGame();
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
            if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous || StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
            {
                TerminatePlayerTokenEvent tokenEvent = new(null)
                {
                    Holder = CreationHolder(),
                    Player = StrategicMap.Current.SeatedPlayer,
                    ExitCallback = _exitCallback,
                    ExitMode = _exitMode
                };
                StrategicMap.Current.TokenManager.GetTokenControl(StrategicMap.Current.SeatedPlayer).AddEvent(tokenEvent);
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
            StrategicMap.Current.TriggerTypedExecuteEventLog(passwordLog, immediate: false, done: ValidatePasswordCallback);
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
            StrategicMap.Current.TriggerTypedExecuteEventLog(messageLog, immediate: false, done: PasswordTryAgainCallback);
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
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            SetSeatedPlayer(-1);
        }

        _password = password;
        PasswordEventLog log = new(null, holder: CreationHolder());
        StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ValidatePasswordCallback);
        _ = log.Release();
    }

    // PlayerC.PAS:551
    public void PlayerDisconnected(sbyte player)
    {
        if (_disconnectedPlayers.IndexOfItemA(player) == -1)
        {
            _ = _disconnectedPlayers.AddAbility(player, 150);
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
