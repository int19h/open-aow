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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// exit callback, exit mode, password, disconnected-player list, sync warning shown
public partial class PlayerControl : EngineObject
{
    // PlayerC.PAS:1
    public PlayerControl(ECustomNode owner) : base(owner)
    {
        _disconnectedPlayers = new DoubleItemList();
    }

    private ExitGameCallBack _exitCallback;
    private ExitGameMode _exitMode;
    private string _password = "";
    private DoubleItemList _disconnectedPlayers;
    private bool _outOfSyncShown;

    // PlayerC.PAS:7
    public override void Dispose()
    {
        if (AoWHSMap.Current != null && AoWHSMap.Current.TokenManager != null)
        {
            var tokenExecutor = AoWHSMap.Current.TokenManager.TokenExecutor;
            tokenExecutor.OnGetSyncValue = null;
            tokenExecutor.OnSyncError = null;
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
        AoWHSMap.Current.Day += 1;
        AoWHSMap.Current.Players.BeginUpdate();
        AoWHSMap.Current.LockRelationsChanged();
        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            log = new NewDayEventLog(null);
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_newDayFormat), [AoWHSMap.Current.Day]);
            if (AoWHSMap.Current.DayLimit != 0 && AoWHSMap.Current.DayLimit - AoWHSMap.Current.Day <= 10)
            {
                var formatDaysLeftString = DurationText.FormatDaysLeftString(AoWHSMap.Current.DayLimit - AoWHSMap.Current.Day);
                log.Text = log.Text + " (" + formatDaysLeftString + ")";
            }

            log._player = (sbyte)I;
            AoWHSMap.Current.Players.GetPlayers(I).EventLogbook.AddEvent(log);
            log.Release();
        }

        if (AoWHSMap.Current.TurnOrderMode == AoWHSMapTurnOrderMode.Changing)
        {
            AoWHSMap.Current.GenerateNewTurnOrder();
        }

        if (AoWHSMap.Current.Day == 1)
        {
            AoWHSMap.Current.Races.ResetPlayerRelations();
            if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local)
            {
                Randomize();
                AoWHSMap.Current.InitialRandomSeed = RandSeed;
            }

            AoWHSMap.Current.DefendersSeed = AoWHSMap.Current.InitialRandomSeed;
        }

        try
        {
            EngineMessage.InitializeMessage(ref message, 537001985, this);
            AoWHSMap.Current.SendMessage(ref message, this);
        }
        catch
        {
        }

        try
        {
            AoWHSMap.Current.NewDayEventsStore.Raise(this);
        }
        catch
        {
        }

        if (AoWHSMap.Current.DayLimit == AoWHSMap.Current.Day)
        {
            messageLog = new MessageEventLog(null);
            messageLog.Title = AbilityRules.TranslateResourceString(GameText.InfoText);
            messageLog.Buttons = MessageEventLogButtons.Ok;
            messageLog.Layout = MessageEventLogLayout.Normal;
            var turnLimitReached = AbilityRules.TranslateResourceString(s_turnLimitReachedText);
            messageLog.Message.Text = turnLimitReached;
            AoWHSMap.Current.TriggerExecuteEventLog(messageLog, false, null);
            messageLog.Release();
            AoWHSMap.Current.GameOver(AoWHSMap.Current.SeatedPlayer, GameOverStatus.Lost);
            for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
            {
                AoWHSMap.Current.GameOver((sbyte)I, GameOverStatus.Lost);
            }
        }

        AoWHSMap.Current.UnlockRelationsChanged();
        AoWHSMap.Current.TriggerRelationsChanged();
        AoWHSMap.Current.Players.EndUpdate();
    }

    // PlayerC.PAS:177
    public virtual void NewTurn(sbyte player)
    {
        EngineMessage message = default;
        if (AoWHSMap.Current.Day != 1)
        {
            var players = AoWHSMap.Current.Players.GetPlayers(player);
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(player);
            var upkeep = AoWHSMap.Current.Players.GetPlayers(player).GetUpkeep();
            AoWHSMap.Current.Players.GetPlayers(player).SetGems(players.Gems + fPlayersPlayers.Income - upkeep);
            if (AoWHSMap.Current.Players.GetPlayers(player).Gems < 0)
            {
                AoWHSMap.Current.Players.GetPlayers(player).SetGems(0);
                var players2 = AoWHSMap.Current.Players.GetPlayers(player);
                players2.MoraleValue = (sbyte)(players2.MoraleValue - 5);
                if (AoWHSMap.Current.Players.GetPlayers(player).MoraleValue < -50)
                {
                    AoWHSMap.Current.Players.GetPlayers(player).MoraleValue = -50;
                }
            }
            else
            {
                AoWHSMap.Current.Players.GetPlayers(player).MoraleValue = 0;
            }

            AoWHSMap.Current.Players.GetPlayers(player).SetEndTurn(false);
        }

        var players3 = AoWHSMap.Current.Players.GetPlayers(player);
        players3.Day += 1;
        AoWHSMap.Current.LockUpdateVisibility();
        EngineMessage.InitializeMessage(ref message, 537001986, this);
        message.Payload = player;
        AoWHSMap.Current.SendMessage(ref message, this);
        AoWHSMap.Current.NewTurnEventsStore.Raise(player);
        EngineMessage.InitializeMessage(ref message, 537001987, this);
        message.Payload = player;
        AoWHSMap.Current.SendMessage(ref message, this);
        AoWHSMap.Current.UnLockUpdateVisibility();
        if (AoWHSMap.Current.DebugSave)
        {
            try
            {
                if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous)
                {
                    if (player == 0)
                    {
                        GameTables.GameEngine.SaveHsm(GameTables.GameEngine.StartupDirectory + "DEBUG\\" + AoWHSMap.Current.MapName + IntToStr(AoWHSMap.Current.Day) + "_p" + IntToStr(AoWHSMap.Current.SeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
                    }
                }
                else
                {
                    GameTables.GameEngine.SaveHsm(GameTables.GameEngine.StartupDirectory + "DEBUG\\" + AoWHSMap.Current.MapName + IntToStr(AoWHSMap.Current.Day) + "-" + IntToStr(player) + "_p" + IntToStr(AoWHSMap.Current.SeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
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
        if (AoWHSMap.Current.GameType == GameType.Campaign)
        {
            try
            {
                // The autosave takes the extension the host writes (own-formats.md 4.2).
                fileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.CampaignSave);
                GameTables.GameEngine.SaveCampaign(GameTables.GameEngine.StartupDirectory + "Save\\" + fileName);
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [fileName]);
                AoWHSMap.Current.ShowWarning(format);
            }
        }
        else
        {
            try
            {
                fileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.Save);
                GameTables.GameEngine.SaveHsm(GameTables.GameEngine.StartupDirectory + "Save\\" + fileName, null, GameProfile.WorldMap);
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [fileName]);
                AoWHSMap.Current.ShowWarning(format);
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
        if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl == null)
            {
                throw new InvalidOperationException("Please setup DirectPlayPCC");
            }

            var tokenExecutor = AoWHSMap.Current.TokenManager.TokenExecutor;
            tokenExecutor.OnGetSyncValue = this.GetSyncValue;
            tokenExecutor.OnSyncError = OutOfSync;
        }
        else
        {
            if ((AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || Set<PlayerConnectionType>.Of(PlayerConnectionType.Local, PlayerConnectionType.Email).Contains(AoWHSMap.Current.ConnectionType)) && AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl == null)
            {
                playerCommunicationControl = new LocalPlayerCommunicationControl();
                playerCommunicationControl.Setup(AoWHSMap.Current.Players.Count);
                for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                {
                    if (AoWHSMap.Current.Players.GetPlayers(I).PlayerType == PlayerType.Human)
                    {
                        playerCommunicationControl.SetConnectionMode(I, ConnectionMode.Local);
                    }
                    else
                    {
                        playerCommunicationControl.SetConnectionMode(I, ConnectionMode.Global);
                    }
                }

                AoWHSMap.Current.PlayerCommunicator.ActivatePlayerCommunicationControl(playerCommunicationControl);
                playerCommunicationControl.Release();
                AoWHSMap.Current.Players.Update();
            }
        }
    }

    // PlayerC.PAS:224
    public virtual void StartNewMap()
    {
        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I == 0 || AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.Players.GetPlayers(I).Status = PlayerStatus.Playing;
            }
        }
    }

    // PlayerC.PAS:233
    public virtual void StartLoadedMap()
    {
    }

    // PlayerC.PAS:463
    public virtual void PasswordFailed()
    {
        AoWHSMap.Current.ExecuteExitGame();
    }

    // PlayerC.PAS:468
    public virtual void PasswordValidated()
    {
    }

    // PlayerC.PAS:236
    public virtual void SetPlayerGameOverStatus(sbyte player, GameOverStatus status)
    {
        AoWHSMap.Current.Players.GetPlayers(player).SetGameOverStatus(status);
    }

    // PlayerC.PAS:241
    public virtual void GameOver(sbyte player, GameOverStatus status)
    {
        if (AoWHSMap.Current.Players.GetPlayers(player).GameOverStatus != status && AoWHSMap.Current.Players.GetPlayers(player).Status != PlayerStatus.Terminated)
        {
            AoWHSMap.Current.LockGameOverEvent();
            SetPlayerGameOverStatus(player, status);
            if (status == GameOverStatus.Lost)
            {
                if (!AoWHSMap.Current.AlliedVictory)
                {
                    if (AoWHSMap.Current.Players.ActivePlayers.Count == 1)
                    {
                        GameOver((sbyte)AoWHSMap.Current.Players.ActivePlayers[0], GameOverStatus.Won);
                    }
                }
                else
                {
                    if (AoWHSMap.Current.Players.GetActivePlayersAllied())
                    {
                        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                        {
                            if (I != player && AoWHSMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing && AoWHSMap.Current.Players.GetPlayers(I).GameOverStatus != GameOverStatus.Lost)
                            {
                                SetPlayerGameOverStatus((sbyte)I, GameOverStatus.Won);
                            }
                        }
                    }
                }
            }
            else
            {
                if (status == GameOverStatus.Won)
                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        if (I != player && AoWHSMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing && AoWHSMap.Current.Players.GetPlayers(I).GameOverStatus != GameOverStatus.Lost)
                        {
                            if (AoWHSMap.Current.Players.GetDiplomaticRelation(player, (sbyte)I) == DiplomaticRelation.Alliance && AoWHSMap.Current.AlliedVictory)
                            {
                                SetPlayerGameOverStatus((sbyte)I, GameOverStatus.Won);
                            }
                            else
                            {
                                SetPlayerGameOverStatus((sbyte)I, GameOverStatus.Lost);
                            }
                        }
                    }
                }
            }

            AoWHSMap.Current.UnlockGameOverEvent();
        }
    }

    // PlayerC.PAS:274
    public virtual void GameOverEventHandled(sbyte player)
    {
        AoWHSMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
    }

    // PlayerC.PAS:388
    public virtual void ExitGame(ExitGameMode mode, ExitGameCallBack done)
    {
        MessageEventLog log = default;
        if (AoWHSMap.Current.Combat == null)
        {
            if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network && AoWHSMap.Current.GameStatus != AoWHSMapGameStatus.Terminated && AoWHSMap.Current.GameType != GameType.Tutorial && AoWHSMap.Current.Players.LocalPlayers.Count > 1)
            {
                if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous || AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer)
                {
                    _exitMode = mode;
                    _exitCallback = done;
                    log = new MessageEventLog(null);
                    log.Title = AbilityRules.TranslateResourceString(ConfirmText);
                    log.Buttons = MessageEventLogButtons.YesNoCancel;
                    log.Layout = MessageEventLogLayout.Normal;
                    var yourAboutToExitAMultiPlayerGame = AbilityRules.TranslateResourceString(s_yourAboutToExitMultiPlayerGameText);
                    log.Message.Add(yourAboutToExitAMultiPlayerGame);
                    var translatedText = AbilityRules.TranslateResourceString(s_doYouWantToContinueExitingText);
                    log.Message.Add(translatedText);
                    AoWHSMap.Current.TriggerExecuteEventLog(log, false, ExitNetworkGameCallback);
                    log.Release();
                }
                else
                {
                    log = new MessageEventLog(null);
                    log.Title = AbilityRules.TranslateResourceString(GameText.MessageText);
                    log.Buttons = MessageEventLogButtons.Ok;
                    log.Layout = MessageEventLogLayout.Normal;
                    var pleaseWaitTillItIsYourTurn = AbilityRules.TranslateResourceString(s_pleaseWaitTillItIsYourTurnText);
                    log.Message.Add(pleaseWaitTillItIsYourTurn);
                    AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
                    log.Release();
                }
            }
            else
            {
                if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Modified) && AoWHSMap.Current.GameStatus != AoWHSMapGameStatus.Terminated && AoWHSMap.Current.GameType != GameType.Tutorial)
                {
                    if (!(_exitCallback != null))
                    {
                        if (mode != ExitGameMode.Reload)
                        {
                            _exitMode = mode;
                            _exitCallback = done;
                            log = new MessageEventLog(null);
                            log.Title = AbilityRules.TranslateResourceString(ConfirmText);
                            log.Buttons = MessageEventLogButtons.YesNoCancel;
                            if (GameTables.GameEngine.Campaign == null && !IsEmpty(AoWHSMap.Current.FileName) && !DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
                            {
                                log.Layout = MessageEventLogLayout.Normal;
                                // The message names the file the save writes (own-formats.md 4.2).
                                var fileName = ExtractFileName(FileBoundary.WriteName(AoWHSMap.Current.FileName));
                                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_saveGameFormat), [fileName]);
                                log.Message.Add(format);
                            }
                            else
                            {
                                if (GameTables.GameEngine.Campaign != null && !IsEmpty(GameTables.GameEngine.Campaign.FileName) && !DocumentKind.CampaignMap.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
                                {
                                    // { Original tests the map extension when saving a campaign. }
                                    log.Layout = MessageEventLogLayout.Normal;
                                    var fileName = ExtractFileName(FileBoundary.WriteName(GameTables.GameEngine.Campaign.FileName));
                                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_saveGameFormat), [fileName]);
                                    log.Message.Add(format);
                                }
                                else
                                {
                                    log.Layout = MessageEventLogLayout.Normal;
                                    var doYouWantToSaveThisGame = AbilityRules.TranslateResourceString(s_doYouWantToSaveThisGameText);
                                    log.Message.Add(doYouWantToSaveThisGame);
                                }
                            }

                            AoWHSMap.Current.TriggerExecuteEventLog(log, false, SaveGameOnExitCallback);
                            log.Release();
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
                    AoWHSMap.Current.ExecuteExitGame();
                }
            }
        }
        else
        {
            if (done != null)
            {
                done(mode, false);
            }
        }
    }

    // PlayerC.PAS:153
    public virtual void EndTurn(sbyte player)
    {
        EngineMessage message = default;
        int X = default;
        int Y = default;
        int L = default;
        Player P;
        AoWHSMap.Current.LockRelationsChanged();
        if (player == AoWHSMap.Current.SeatedPlayer)
        {
            P = AoWHSMap.Current.Players.GetPlayers(player);
            P.SetEndTurn(true);
            if (P != null)
            {
                AoWHSMap.Current.GetViewCenter(ref X, ref Y, ref L);
                P.ViewX = (byte)X;
                P.ViewY = (byte)Y;
                P.ViewL = (byte)L;
            }
        }

        EngineMessage.InitializeMessage(ref message, 537001991, this);
        message.Payload = player;
        AoWHSMap.Current.SendMessage(ref message, this);
        AoWHSMap.Current.UnlockRelationsChanged();
        AoWHSMap.Current.TriggerRelationsChanged();
    }

    // PlayerC.PAS:279
    public virtual void SetSeatedPlayer(sbyte player)
    {
        if (player != AoWHSMap.Current.SeatedPlayer)
        {
            AoWHSMap.Current.SeatedPlayer = player;
            AoWHSMap.Current.SeatedPlayerChanged();
            AoWHSMap.Current.ResetSeatedPlayerView();
        }
    }

    // PlayerC.PAS:518
    public virtual void ValidateDisconnectedPlayers()
    {
        int player;
        int count;
        PlayerConnectionEventLog log;
        for (int I = _disconnectedPlayers.Count - 1; I >= 0; I--)
        {
            player = _disconnectedPlayers[I].First;
            if (AoWHSMap.Current.Players.GetPlayers(player).GameOverStatus == GameOverStatus.None)
            {
                if (AoWHSMap.Current.TokenManager.PlayerReady(AoWHSMap.Current.CurrentPlayer) && AoWHSMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Count == 0)
                {
                    count = _disconnectedPlayers[I].Second;
                    if (count > 0)
                    {
                        count -= 1;
                        _disconnectedPlayers.List[I].Second = count;
                    }

                    if (count == 0)
                    {
                        log = new PlayerConnectionEventLog(null);
                        log.Setup((sbyte)player, 2);
                        AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
                        log.Release();
                        _disconnectedPlayers.Delete(I);
                    }
                }
            }
            else
            {
                _disconnectedPlayers.Delete(I);
            }
        }
    }

    // PlayerC.PAS:556
    public virtual void ConnectionLost()
    {
        PlayerConnectionEventLog log;
        log = new PlayerConnectionEventLog(null);
        log.Setup(-1, 2);
        AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
        log.Release();
    }

    // PlayerC.PAS:289
    public virtual bool LockAI() => AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local && !AoWHSMap.Current.RequestCombatType && AoWHSMap.Current.AILockCount != 0;

    // PlayerC.PAS:49
    public void GetSyncValue(ref int value)
    {
        value = AoWHSMap.Current.DefendersSeed;
    }

    // PlayerC.PAS:54
    public void OutOfSync(object sender)
    {
        PlayerConnectionEventLog log;
        if (AoWHSMap.Current.TokenManager.TokenExecutor.ActiveTacticalBattle.Active)
        {
            if (!_outOfSyncShown)
            {
                log = new PlayerConnectionEventLog(null);
                log.Setup(-1, 3);
                AoWHSMap.Current.TriggerExecuteEventLog(log, AoWHSMap.Current.Combat != null, null);
                log.Release();
                _outOfSyncShown = true;
            }
            else
            {
                AoWHSMap.Current.TokenManager.TokenExecutor.Lock();
            }
        }
    }

    // PlayerC.PAS:295
    public void ListPlayerInfo(StringCollection strings)
    {
        int race;
        string text;
        PlayerCommunicationControl playerCommunicationControl;
        playerCommunicationControl = AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            text = "Player " + IntToStr(I);
            race = AoWHSMap.Current.Players.GetPlayers(I).PlayerRace;
            if (race != -1)
            {
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[race]);
                text = text + "(" + raceNamesPlural + ")";
            }

            text = text + " at system " + IntToStr(playerCommunicationControl.Players[I].SystemId);
            if (strings != null)
            {
                strings.Add(text);
            }

            GameTables.GameEngine.OutputDebugString(text);
        }
    }

    // PlayerC.PAS:313
    public void ExitSaveGameCallback(EventLog log)
    {
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            if (GameTables.GameEngine.Campaign != null)
            {
                GameTables.GameEngine.SaveCampaign(((SaveGameEventLog)log).FileName);
            }
            else
            {
                GameTables.GameEngine.SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
            }

            AoWHSMap.Current.ExecuteExitGame();
            if (_exitCallback != null)
            {
                _exitCallback(_exitMode, true);
            }
        }
        else
        {
            if (_exitCallback != null)
            {
                _exitCallback(_exitMode, false);
            }
        }

        _exitCallback = null;
    }

    // PlayerC.PAS:326
    public void SaveGameOnExitCallback(EventLog log)
    {
        SaveGameEventLog saveLog = default;
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            if (GameTables.GameEngine.Campaign == null && (IsEmpty(AoWHSMap.Current.FileName) || DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName))) || GameTables.GameEngine.Campaign != null && (IsEmpty(GameTables.GameEngine.Campaign.FileName) || DocumentKind.CampaignMap.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName))))
            {
                saveLog = new SaveGameEventLog(null);
                if (!IsEmpty(AoWHSMap.Current.FileName))
                {
                    saveLog.DefaultName = ExtractFileName(AoWHSMap.Current.FileName);
                    if (Pos(".", saveLog.DefaultName) != 0)
                    {
                        saveLog.DefaultName = RtlUnit.Copy(saveLog.DefaultName, 1, Pos(".", saveLog.DefaultName) - 1);
                    }
                }
                else
                {
                    saveLog.DefaultName = AoWHSMap.Current.MapName;
                }

                saveLog.Directory = GameTables.GameEngine.StartupDirectory + "Save\\";
                AoWHSMap.Current.TriggerExecuteEventLog(saveLog, false, ExitSaveGameCallback);
                saveLog.Release();
            }
            else
            {
                GameEngine engine = GameTables.GameEngine;
                // The save writes the own sibling of an original's name, which stays untouched
                // (own-formats.md 4.2).
                if (engine.Campaign != null)
                {
                    engine.SaveCampaign(FileBoundary.WriteName(engine.Campaign.FileName));
                }
                else
                {
                    engine.SaveHsm(FileBoundary.WriteName(AoWHSMap.Current.FileName), null, GameProfile.WorldMap);
                }

                AoWHSMap.Current.ExecuteExitGame();
                if (_exitCallback != null)
                {
                    _exitCallback(_exitMode, true);
                }

                _exitCallback = null;
            }
        }
        else
        {
            if (((MessageEventLog)log).PressedButton == PressedEventLogButton.No)
            {
                AoWHSMap.Current.ExecuteExitGame();
                if (_exitCallback != null)
                {
                    _exitCallback(_exitMode, true);
                }

                _exitCallback = null;
            }
            else
            {
                if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Cancel)
                {
                    if (_exitCallback != null)
                    {
                        _exitCallback(_exitMode, false);
                    }

                    _exitCallback = null;
                }
            }
        }
    }

    // PlayerC.PAS:372
    public void ExitNetworkGameCallback(EventLog log)
    {
        TerminatePlayerTokenEvent tokenEvent;
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            if (AoWHSMap.Current.PlayMode == PlayMode.Simultaneous || AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer)
            {
                tokenEvent = new TerminatePlayerTokenEvent(null);
                tokenEvent.Player = AoWHSMap.Current.SeatedPlayer;
                tokenEvent.ExitCallback = _exitCallback;
                tokenEvent.ExitMode = _exitMode;
                AoWHSMap.Current.TokenManager.GetTokenControl(AoWHSMap.Current.SeatedPlayer).AddEvent(tokenEvent);
                tokenEvent.Release();
            }
        }
    }

    // PlayerC.PAS:471
    public void PasswordTryAgainCallback(EventLog log)
    {
        PasswordEventLog passwordLog = default;
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            passwordLog = new PasswordEventLog(null);
            AoWHSMap.Current.TriggerExecuteEventLog(passwordLog, false, ValidatePasswordCallback);
            passwordLog.Release();
        }
        else
        {
            PasswordFailed();
        }
    }

    // PlayerC.PAS:484
    public void ValidatePasswordCallback(EventLog log)
    {
        string password = "";
        MessageEventLog messageLog = default;
        password = AbilityRules.Encode(((PasswordEventLog)log).Password, 0);
        if (((PasswordEventLog)log).PressedButton != PressedEventLogButton.Ok || password != _password)
        {
            messageLog = new MessageEventLog(null);
            messageLog.Title = AbilityRules.TranslateResourceString(GameText.WarningText);
            messageLog.Buttons = MessageEventLogButtons.YesNo;
            var passwordFailed = AbilityRules.TranslateResourceString(s_passwordFailedText);
            messageLog.Message.Text = passwordFailed;
            messageLog.Message.Add("");
            var translatedText = AbilityRules.TranslateResourceString(s_doYouWantToTryAgainText);
            messageLog.Message.Add(translatedText);
            AoWHSMap.Current.TriggerExecuteEventLog(messageLog, false, PasswordTryAgainCallback);
            messageLog.Release();
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
        if (AoWHSMap.Current.SeatedPlayer != -1)
        {
            SetSeatedPlayer(-1);
        }

        _password = password;
        log = new PasswordEventLog(null);
        AoWHSMap.Current.TriggerExecuteEventLog(log, false, ValidatePasswordCallback);
        log.Release();
    }

    // PlayerC.PAS:551
    public void PlayerDisconnected(sbyte player)
    {
        if (_disconnectedPlayers.IndexOfItemA(player) == -1)
        {
            _disconnectedPlayers.AddAbility(player, 150);
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
