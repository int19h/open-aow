#nullable disable
// Translated from AoWE.pas.
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

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

// exit callback, exit mode, password, disconnected-player list, sync warning shown
public partial class PlayerControl(CustomNode owner) : EngineObject(owner)
{

    private ExitGameCallBack _exitCallback;
    private ExitGameMode _exitMode;
    private string _password = "";
    private readonly DoubleItemList _disconnectedPlayers = new();
    private bool _outOfSyncShown;

    // PlayerC.PAS:7
    public override void Dispose()
    {
        if (StrategicMap.Current != null && StrategicMap.Current.TokenManager != null)
        {
            TokenExecutor tokenExecutor = StrategicMap.Current.TokenManager.TokenExecutor;
            tokenExecutor.ClearGetSyncValueHandler();
            tokenExecutor.ClearSyncErrorHandler();
        }

        _disconnectedPlayers?.Dispose();
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
        EngineMessage message = default;
        TextEventLog log = default;
        MessageEventLog messageLog;
        StrategicMap.Current.Day += 1;
        StrategicMap.Current.Players.BeginUpdate();
        StrategicMap.Current.LockRelationsChanged();
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            log = new NewDayEventLog(null)
            {
                Holder = CreationHolder(),
                Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_newDayFormat), [StrategicMap.Current.Day])
            };
            if (StrategicMap.Current.DayLimit != 0 && StrategicMap.Current.DayLimit - StrategicMap.Current.Day <= 10)
            {
                string formatDaysLeftString = DurationText.FormatDaysLeftString(GetEngine(), days: StrategicMap.Current.DayLimit - StrategicMap.Current.Day);
                log.Text = log.Text + " (" + formatDaysLeftString + ")";
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
            EngineMessage.InitializeMessage(out message, 537001985, this);
            StrategicMap.Current.SendMessage(ref message, this);
        }
        catch
        {
        }

        try
        {
            StrategicMap.Current.NewDayEventsStore.Raise(this);
        }
        catch
        {
        }

        if (StrategicMap.Current.DayLimit == StrategicMap.Current.Day)
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder())
            {
                Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.InfoText),
                Buttons = MessageEventLogButtons.Ok,
                Layout = MessageEventLogLayout.Normal
            };
            string turnLimitReached = AbilityRules.TranslateResourceString(GetEngine(), s_turnLimitReachedText);
            messageLog.Message.Text = turnLimitReached;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, null);
            _ = messageLog.Release();
            StrategicMap.Current.GameOver(StrategicMap.Current.SeatedPlayer, GameOverStatus.Lost);
            for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
            {
                StrategicMap.Current.GameOver((sbyte)i, GameOverStatus.Lost);
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
            Player players = StrategicMap.Current.Players.GetPlayers(player);
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            int upkeep = StrategicMap.Current.Players.GetPlayers(player).GetUpkeep();
            StrategicMap.Current.Players.GetPlayers(player).SetGems(players.Gems + fPlayersPlayers.Income - upkeep);
            if (StrategicMap.Current.Players.GetPlayers(player).Gems < 0)
            {
                StrategicMap.Current.Players.GetPlayers(player).SetGems(0);
                Player players2 = StrategicMap.Current.Players.GetPlayers(player);
                players2.MoraleValue = (sbyte)(players2.MoraleValue - 5);
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

        Player players3 = StrategicMap.Current.Players.GetPlayers(player);
        players3.Day += 1;
        StrategicMap.Current.LockUpdateVisibility();
        EngineMessage.InitializeMessage(out EngineMessage message, 537001986, this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, this);
        StrategicMap.Current.NewTurnEventsStore.Raise(player);
        EngineMessage.InitializeMessage(out message, 537001987, this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, this);
        StrategicMap.Current.UnLockUpdateVisibility();
        if (StrategicMap.Current.DebugSave)
        {
            try
            {
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    if (player == 0)
                    {
                        GetEngine().SaveHsm(GetEngine().StartupDirectory + "DEBUG\\" + StrategicMap.Current.MapName + IntToStr(StrategicMap.Current.Day) + "_p" + IntToStr(StrategicMap.Current.SeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
                    }
                }
                else
                {
                    GetEngine().SaveHsm(GetEngine().StartupDirectory + "DEBUG\\" + StrategicMap.Current.MapName + IntToStr(StrategicMap.Current.Day) + "-" + IntToStr(player) + "_p" + IntToStr(StrategicMap.Current.SeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
                }
            }
            catch
            {
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
                fileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.CampaignSave);
                GetEngine().SaveCampaign(GetEngine().StartupDirectory + "Save\\" + fileName);
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat), [fileName]);
                StrategicMap.Current.ShowWarning(format);
            }
        }
        else
        {
            try
            {
                fileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.Save);
                GetEngine().SaveHsm(GetEngine().StartupDirectory + "Save\\" + fileName, null, GameProfile.WorldMap);
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat), [fileName]);
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
        LocalPlayerCommunicationControl playerCommunicationControl = default;
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl == null)
            {
                throw new InvalidOperationException("Please setup DirectPlayPCC");
            }

            TokenExecutor tokenExecutor = StrategicMap.Current.TokenManager.TokenExecutor;
            tokenExecutor.SetGetSyncValueHandler(GetSyncValue);
            tokenExecutor.SetSyncErrorHandler(OutOfSync);
        }
        else
        {
            if ((StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || Set<PlayerConnectionType>.Of(PlayerConnectionType.Local, PlayerConnectionType.Email).Contains(StrategicMap.Current.ConnectionType)) && StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl == null)
            {
                playerCommunicationControl = new LocalPlayerCommunicationControl(this);
                playerCommunicationControl.Setup(StrategicMap.Current.Players.Count);
                for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                {
                    if (StrategicMap.Current.Players.GetPlayers(i).PlayerType == PlayerType.Human)
                    {
                        playerCommunicationControl.SetConnectionMode(i, ConnectionMode.Local);
                    }
                    else
                    {
                        playerCommunicationControl.SetConnectionMode(i, ConnectionMode.Global);
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
                        GameOver((sbyte)StrategicMap.Current.Players.ActivePlayers[0], GameOverStatus.Won);
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
                                SetPlayerGameOverStatus((sbyte)i, GameOverStatus.Won);
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
                            if (StrategicMap.Current.Players.GetDiplomaticRelation(player, (sbyte)i) == DiplomaticRelation.Alliance && StrategicMap.Current.AlliedVictory)
                            {
                                SetPlayerGameOverStatus((sbyte)i, GameOverStatus.Won);
                            }
                            else
                            {
                                SetPlayerGameOverStatus((sbyte)i, GameOverStatus.Lost);
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
    public virtual void ExitGame(ExitGameMode mode, ExitGameCallBack done)
    {
        MessageEventLog log = default;
        if (StrategicMap.Current.Combat == null)
        {
            if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network && StrategicMap.Current.GameStatus != StrategicMapGameStatus.Terminated && StrategicMap.Current.GameType != GameType.Tutorial && StrategicMap.Current.Players.LocalPlayers.Count > 1)
            {
                if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous || StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
                {
                    _exitMode = mode;
                    _exitCallback = done;
                    log = new MessageEventLog(null, holder: CreationHolder())
                    {
                        Title = AbilityRules.TranslateResourceString(GetEngine(), ConfirmText),
                        Buttons = MessageEventLogButtons.YesNoCancel,
                        Layout = MessageEventLogLayout.Normal
                    };
                    string yourAboutToExitAMultiPlayerGame = AbilityRules.TranslateResourceString(GetEngine(), s_yourAboutToExitMultiPlayerGameText);
                    _ = log.Message.Add(yourAboutToExitAMultiPlayerGame);
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWantToContinueExitingText);
                    _ = log.Message.Add(translatedText);
                    StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ExitNetworkGameCallback);
                    _ = log.Release();
                }
                else
                {
                    log = new MessageEventLog(null, holder: CreationHolder())
                    {
                        Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.MessageText),
                        Buttons = MessageEventLogButtons.Ok,
                        Layout = MessageEventLogLayout.Normal
                    };
                    string pleaseWaitTillItIsYourTurn = AbilityRules.TranslateResourceString(GetEngine(), s_pleaseWaitTillItIsYourTurnText);
                    _ = log.Message.Add(pleaseWaitTillItIsYourTurn);
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
                    _ = log.Release();
                }
            }
            else
            {
                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Modified) && StrategicMap.Current.GameStatus != StrategicMapGameStatus.Terminated && StrategicMap.Current.GameType != GameType.Tutorial)
                {
                    if (!(_exitCallback != null))
                    {
                        if (mode != ExitGameMode.Reload)
                        {
                            _exitMode = mode;
                            _exitCallback = done;
                            log = new MessageEventLog(null, holder: CreationHolder())
                            {
                                Title = AbilityRules.TranslateResourceString(GetEngine(), ConfirmText),
                                Buttons = MessageEventLogButtons.YesNoCancel
                            };
                            if (GetEngine().Campaign == null && !IsEmpty(StrategicMap.Current.FileName) && !DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
                            {
                                log.Layout = MessageEventLogLayout.Normal;
                                // The message names the file the save writes (own-formats.md 4.2).
                                string fileName = ExtractFileName(FileBoundary.WriteName(StrategicMap.Current.FileName));
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_saveGameFormat), [fileName]);
                                _ = log.Message.Add(format);
                            }
                            else
                            {
                                if (GetEngine().Campaign != null && !IsEmpty(GetEngine().Campaign.FileName) && !DocumentKind.CampaignMap.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
                                {
                                    // { Original tests the map extension when saving a campaign. }
                                    log.Layout = MessageEventLogLayout.Normal;
                                    string fileName = ExtractFileName(FileBoundary.WriteName(GetEngine().Campaign.FileName));
                                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_saveGameFormat), [fileName]);
                                    _ = log.Message.Add(format);
                                }
                                else
                                {
                                    log.Layout = MessageEventLogLayout.Normal;
                                    string doYouWantToSaveThisGame = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWantToSaveThisGameText);
                                    _ = log.Message.Add(doYouWantToSaveThisGame);
                                }
                            }

                            StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, SaveGameOnExitCallback);
                            _ = log.Release();
                        }
                        else
                        {
                            done(mode, true);
                        }
                    }
                    else
                    {
                        _exitCallback(_exitMode, false);
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
            done?.Invoke(mode, false);
        }
    }

    // PlayerC.PAS:153
    public virtual void EndTurn(sbyte player)
    {
        Player p;
        StrategicMap.Current.LockRelationsChanged();
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            p = StrategicMap.Current.Players.GetPlayers(player);
            p.SetEndTurn(true);
            if (p != null)
            {
                StrategicMap.Current.GetViewCenter(out int x, out int y, out int l);
                p.View = new MapCoordinate(unchecked((sbyte)(byte)x), unchecked((sbyte)(byte)y), unchecked((sbyte)(byte)l));
            }
        }

        EngineMessage.InitializeMessage(out EngineMessage message, 537001991, this);
        message.Payload = player;
        StrategicMap.Current.SendMessage(ref message, this);
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
        int player;
        int count;
        PlayerConnectionEventLog log;
        for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
        {
            player = _disconnectedPlayers[i].First;
            if (StrategicMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.None)
            {
                if (StrategicMap.Current.TokenManager.PlayerReady(StrategicMap.Current.CurrentPlayer) && StrategicMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Count == 0)
                {
                    count = _disconnectedPlayers[i].Second;
                    if (count > 0)
                    {
                        count -= 1;
                        _disconnectedPlayers.List[i] = _disconnectedPlayers.List[i] with { Second = count };
                    }

                    if (count == 0)
                    {
                        log = new PlayerConnectionEventLog(null, holder: CreationHolder());
                        log.Setup((sbyte)player, 2);
                        StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
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
        PlayerConnectionEventLog log;
        log = new PlayerConnectionEventLog(null, holder: CreationHolder());
        log.Setup(-1, 2);
        StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
        _ = log.Release();
    }

    // PlayerC.PAS:289
    public virtual bool LockAI() => StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && !StrategicMap.Current.RequestCombatType && StrategicMap.Current.AILockCount != 0;

    // PlayerC.PAS:49
    public void GetSyncValue(out int value) => value = StrategicMap.Current.DefendersSeed;

    // PlayerC.PAS:54
    public void OutOfSync(object sender)
    {
        PlayerConnectionEventLog log;
        if (StrategicMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Active)
        {
            if (!_outOfSyncShown)
            {
                log = new PlayerConnectionEventLog(null, holder: CreationHolder());
                log.Setup(-1, 3);
                StrategicMap.Current.TriggerExecuteEventLog(log, StrategicMap.Current.Combat != null, null);
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
        int race;
        string text;
        PlayerCommunicationControl playerCommunicationControl;
        playerCommunicationControl = StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            text = "Player " + IntToStr(i);
            race = StrategicMap.Current.Players.GetPlayers(i).PlayerRace;
            if (race != -1)
            {
                string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[race]);
                text = text + "(" + raceNamesPlural + ")";
            }

            text = text + " at system " + IntToStr(playerCommunicationControl.Players[i].SystemId);
            _ = (strings?.Add(text));

            GetEngine().OutputDebugString(text);
        }
    }

    // PlayerC.PAS:313
    public void ExitSaveGameCallback(SaveGameEventLog log)
    {
        if (!IsEmpty(log.FileName))
        {
            if (GetEngine().Campaign != null)
            {
                GetEngine().SaveCampaign(log.FileName);
            }
            else
            {
                GetEngine().SaveHsm(log.FileName, null, GameProfile.WorldMap);
            }

            StrategicMap.Current.ExecuteExitGame();
            _exitCallback?.Invoke(_exitMode, true);
        }
        else
        {
            _exitCallback?.Invoke(_exitMode, false);
        }

        _exitCallback = null;
    }

    // PlayerC.PAS:326
    public void SaveGameOnExitCallback(MessageEventLog log)
    {
        SaveGameEventLog saveLog = default;
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            if ((GetEngine().Campaign == null && (IsEmpty(StrategicMap.Current.FileName) || DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))) || (GetEngine().Campaign != null && (IsEmpty(GetEngine().Campaign.FileName) || DocumentKind.CampaignMap.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))))
            {
                saveLog = new SaveGameEventLog(null, holder: CreationHolder());
                if (!IsEmpty(StrategicMap.Current.FileName))
                {
                    saveLog.DefaultName = ExtractFileName(StrategicMap.Current.FileName);
                    if (Pos(".", saveLog.DefaultName) != 0)
                    {
                        saveLog.DefaultName = DelphiStrings.Copy(saveLog.DefaultName, 1, Pos(".", saveLog.DefaultName) - 1);
                    }
                }
                else
                {
                    saveLog.DefaultName = StrategicMap.Current.MapName;
                }

                saveLog.Directory = GetEngine().StartupDirectory + "Save\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(saveLog, false, ExitSaveGameCallback);
                _ = saveLog.Release();
            }
            else
            {
                GameEngine engine = GetEngine();
                // The save writes the own sibling of an original's name, which stays untouched
                // (own-formats.md 4.2).
                if (engine.Campaign != null)
                {
                    engine.SaveCampaign(FileBoundary.WriteName(engine.Campaign.FileName));
                }
                else
                {
                    engine.SaveHsm(FileBoundary.WriteName(StrategicMap.Current.FileName), null, GameProfile.WorldMap);
                }

                StrategicMap.Current.ExecuteExitGame();
                _exitCallback?.Invoke(_exitMode, true);

                _exitCallback = null;
            }
        }
        else
        {
            if (log.PressedButton == PressedEventLogButton.No)
            {
                StrategicMap.Current.ExecuteExitGame();
                _exitCallback?.Invoke(_exitMode, true);

                _exitCallback = null;
            }
            else
            {
                if (log.PressedButton == PressedEventLogButton.Cancel)
                {
                    _exitCallback?.Invoke(_exitMode, false);

                    _exitCallback = null;
                }
            }
        }
    }

    // PlayerC.PAS:372
    public void ExitNetworkGameCallback(MessageEventLog log)
    {
        TerminatePlayerTokenEvent tokenEvent;
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous || StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
            {
                tokenEvent = new TerminatePlayerTokenEvent(null)
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
        PasswordEventLog passwordLog = default;
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            passwordLog = new PasswordEventLog(null, holder: CreationHolder());
            StrategicMap.Current.TriggerTypedExecuteEventLog(passwordLog, false, ValidatePasswordCallback);
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
        string password = "";
        MessageEventLog messageLog = default;
        password = AbilityRules.Encode(log.Password);
        if (log.PressedButton != PressedEventLogButton.Ok || password != _password)
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder())
            {
                Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText),
                Buttons = MessageEventLogButtons.YesNo
            };
            string passwordFailed = AbilityRules.TranslateResourceString(GetEngine(), s_passwordFailedText);
            messageLog.Message.Text = passwordFailed;
            _ = messageLog.Message.Add("");
            string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWantToTryAgainText);
            _ = messageLog.Message.Add(translatedText);
            StrategicMap.Current.TriggerTypedExecuteEventLog(messageLog, false, PasswordTryAgainCallback);
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
        PasswordEventLog log = default;
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            SetSeatedPlayer(-1);
        }

        _password = password;
        log = new PasswordEventLog(null, holder: CreationHolder());
        StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ValidatePasswordCallback);
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

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
