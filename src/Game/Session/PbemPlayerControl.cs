// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// local player
public partial class PbemPlayerControl : TurnPlayerControl
{
    public new const int ClassId = 131667;

    public PbemPlayerControl(ECustomNode owner) : base(owner)
    { }

    private sbyte _localPlayer;

    // PlayerC.PAS:984
    public override void NewTurn(sbyte player)
    {
        SendEmailGameEventLog log = default;
        if (StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn) || StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human || StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Terminated)
        {
            base.NewTurn(player);
            var players = StrategicMap.Current.Players.GetPlayers(player);
            StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
        }
        else
        {
            if (StrategicMap.Current.Players.LocalPlayers.Count > 1 || player != _localPlayer)
            {
                StrategicMap.Current.SetCurrentPlayer(player);
                var players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn);
                log = new SendEmailGameEventLog(null);
                log.Mode = 2;
                log.SendPlayer = player;
                StrategicMap.Current.TriggerExecuteEventLog(log, false, SendEmailGameCallback);
                log.Release();
            }
            else
            {
                base.NewTurn(player);
                var players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
            }
        }
    }

    // PlayerC.PAS:1194
    public override void StartNewMap()
    {
        int I;
        int player;
        DefaultStartMap();
        NewDay();
        I = 0;
        int last = StrategicMap.Current.Players.Count - 1;
        for (; I <= last; I += 1)
        {
            if (I == 0 || StrategicMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                StrategicMap.Current.Players.GetPlayers(I).Status = PlayerStatus.Playing;
            }
        }

        I = 0;
        while (true)
        {
            bool condition = I < StrategicMap.Current.TurnOrder.Count;
            if (condition)
            {
                var item = StrategicMap.Current.TurnOrder[I];
                condition = StrategicMap.Current.Players.GetPlayers(item).PlayerType != PlayerType.Human;
            }

            if (!condition)
                break;
            I += 1;
        }

        player = StrategicMap.Current.TurnOrder[I];
        var players = StrategicMap.Current.Players.GetPlayers(player);
        bool test = GameTables.GameEngine.EmailGames.FindEmailGame(players.PbemSettings.EmailGame) != null;
        if (test)
        {
            var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
            test = CompareText(StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.EmailAddress, localEmailAddress) == 0;
        }

        if (test)
        {
            _localPlayer = (sbyte)player;
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn);
        }
        else
        {
            StrategicMap.Current.SetSeatedPlayer(-1);
        }

        NewTurn((sbyte)StrategicMap.Current.TurnOrder[1]);
    }

    // PlayerC.PAS:1187
    public override void StartLoadedMap()
    {
        if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).Settings))
        {
            ValidatePassword(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).Settings);
        }
        else
        {
            ValidateLoadedMap();
        }
    }

    // PlayerC.PAS:1184
    public override void PasswordValidated()
    {
        ValidateLoadedMap();
    }

    // PlayerC.PAS:1230
    public override void ExitGame(ExitGameMode mode, ExitGameCallBack done)
    {
        SaveGameEventLog log = default;
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
            {
                log = new SaveGameEventLog(null);
                log.Title = AbilityRules.TranslateResourceString(s_saveEmailGameText);
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name))
                {
                    log.DefaultName = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name + ".asg";
                }

                log.Directory = GameTables.GameEngine.StartupDirectory + "EmailIn\\";
                StrategicMap.Current.TriggerExecuteEventLog(log, false, ExitGameSaveAsCallback);
                log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                GameTables.GameEngine.SaveHsm(StrategicMap.Current.FileName, null, GameProfile.WorldMap);
                StrategicMap.Current.ExecuteExitGame();
            }
        }
        else
        {
            StrategicMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1053
    public override void EndTurn(sbyte player)
    {
        SaveGameEventLog saveLog = default;
        MessageEventLog messageLog = default;
        bool saved = default;
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
            {
                saveLog = new SaveGameEventLog(null);
                saveLog.Title = AbilityRules.TranslateResourceString(s_saveEmailGameText);
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name))
                {
                    saveLog.DefaultName = StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name + ".asg";
                }

                saveLog.Directory = GameTables.GameEngine.StartupDirectory + "EmailIn\\";
                StrategicMap.Current.TriggerExecuteEventLog(saveLog, false, EndTurnGameSavedCallback);
                saveLog.Release();
            }
            else
            {
                var players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                try
                {
                    GameTables.GameEngine.SaveHsm(StrategicMap.Current.FileName, null, GameProfile.WorldMap);
                    saved = true;
                }
                catch
                {
                    saved = false;
                }

                if (saved)
                {
                    base.EndTurn(StrategicMap.Current.CurrentPlayer);
                }
                else
                {
                    messageLog = new MessageEventLog(null);
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat) + ".", [StrategicMap.Current.FileName]);
                    messageLog.Message.Text = format;
                    StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
                    messageLog.Release();
                }
            }
        }
        else
        {
            // { Original uses current player, ignoring Player here. }
            base.EndTurn(StrategicMap.Current.CurrentPlayer);
        }
    }

    // PlayerC.PAS:887
    public void TerminateEmailGame()
    {
        StrategicMap.Current.ExitGame(ExitGameMode.Exit, null);
    }

    // PlayerC.PAS:890
    public void TerminateEmailGameCallback(EventLog log)
    {
        TerminateEmailGame();
    }

    // PlayerC.PAS:893
    public void SendEmailGameAutomatedCallback(EventLog log)
    {
        SendEmailGameEventLog sendLog = default;
        DeleteFile(((SmtpEventLog)log).FileName);
        if (!((SmtpEventLog)log).Success)
        {
            sendLog = new SendEmailGameEventLog(null);
            sendLog.Mode = 1;
            sendLog.SendPlayer = StrategicMap.Current.CurrentPlayer;
            StrategicMap.Current.TriggerExecuteEventLog(sendLog, false, SendEmailGameCallback);
            sendLog.Release();
        }
        else
        {
            TerminateEmailGame();
        }
    }

    // PlayerC.PAS:909
    public void SendEmailGameManualCallback(EventLog log)
    {
        bool failed = default;
        MessageEventLog messageLog = default;
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            try
            {
                GameTables.GameEngine.SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.GameSavedFormat), [((SaveGameEventLog)log).FileName]);
                StrategicMap.Current.ShowMessage(format);
                failed = false;
            }
            catch
            {
                failed = true;
            }
        }
        else
        {
            failed = true;
        }

        if (failed)
        {
            messageLog = new MessageEventLog(null);
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat) + ".", [((SaveGameEventLog)log).FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
            messageLog.Release();
        }
        else
        {
            messageLog = new MessageEventLog(null);
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameSuccessfullySavedText) + ".", [((SaveGameEventLog)log).FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
            messageLog.Release();
        }
    }

    // PlayerC.PAS:938
    public void SendEmailGameCallback(EventLog log)
    {
        /* TODO: Implement Windows.GetTempPath in the platform or editor service. */
        throw new NotImplementedException();
    }

    // PlayerC.PAS:1018
    public void TriggerGameRestartedEvents(sbyte player)
    {
        RestartGameEventLog log;
        StrategicMap.Current.SynchronizeBegin();
        log = new RestartGameEventLog(null);
        log._player = player;
        var name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameRestartEventLogFormat), [name]);
        log.Message.Text = format;
        EventLog.DistributeEventLog(log, false);
        log.Release();
        StrategicMap.Current.SynchronizeEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallback(EventLog log)
    {
        bool failed = default;
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            try
            {
                var players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                GameTables.GameEngine.SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.GameSavedFormat), [((SaveGameEventLog)log).FileName]);
                StrategicMap.Current.ShowMessage(format);
                StrategicMap.Current.SynchronizeBegin();
                base.EndTurn(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.SynchronizeEnd();
                failed = false;
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [((SaveGameEventLog)log).FileName]);
                StrategicMap.Current.ShowWarning(format);
                failed = true;
            }
        }
        else
        {
            failed = true;
        }

        if (failed)
        {
            StrategicMap.Current.TriggerEndTurnEvent(StrategicMap.Current.CurrentPlayer);
        }
    }

    // PlayerC.PAS:1094
    public void StartLoadedEmailGame()
    {
        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.LastDay = StrategicMap.Current.Day;
        var players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        GameTables.GameEngine.EmailGames.UpdateGame(players.PbemSettings.EmailGame);
        var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn))
        {
            StrategicMap.Current.SynchronizeBegin();
            DefaultStartMap();
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
            NewTurn(StrategicMap.Current.CurrentPlayer);
            StrategicMap.Current.SynchronizeEnd();
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:1111
    public void ContinueRestartedGameCallback(EventLog log)
    {
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            StrategicMap.Current.SynchronizeBegin();
            TriggerGameRestartedEvents(StrategicMap.Current.CurrentPlayer);
            StrategicMap.Current.SynchronizeEnd();
            StartLoadedEmailGame();
        }
        else
        {
            StrategicMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1123
    public void ContinueTurnDoneGameCallback(EventLog log)
    {
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            StrategicMap.Current.SynchronizeBegin();
            DefaultStartMap();
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
            EndTurn(StrategicMap.Current.CurrentPlayer);
            StrategicMap.Current.SynchronizeEnd();
        }
        else
        {
            StrategicMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1136
    public void ValidateLoadedMap()
    {
        bool restarted = default;
        MessageEventLog log = default;
        _localPlayer = StrategicMap.Current.CurrentPlayer;
        var players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        if (GameTables.GameEngine.EmailGames.FindEmailGame(players.PbemSettings.EmailGame) == null)
        {
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            restarted = GameTables.GameEngine.EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame);
            var players2 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            var players3 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            var emailGame = GameTables.GameEngine.EmailGames.CreateEmailGame(players3.PbemSettings.EmailGame.Name, players2.PbemSettings.EmailGame.EmailAddress);
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.SetGame(emailGame);
            if (restarted)
            {
                TriggerGameRestartedEvents(StrategicMap.Current.CurrentPlayer);
            }

            StartLoadedEmailGame();
        }
        else
        {
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            if (GameTables.GameEngine.EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame))
            {
                SetSeatedPlayer(-1);
                log = new MessageEventLog(null);
                log.Buttons = MessageEventLogButtons.YesNo;
                log.Title = AbilityRules.TranslateResourceString(GameText.WarningText);
                var pbemRestartCheat = AbilityRules.TranslateResourceString(s_pbemRestartCheatText);
                log.Message.Text = pbemRestartCheat;
                log.Message.Add("");
                var translatedText = AbilityRules.TranslateResourceString(s_doYouWantToContinueText);
                log.Message.Add(translatedText);
                StrategicMap.Current.TriggerExecuteEventLog(log, false, ContinueRestartedGameCallback);
                log.Release();
            }
            else
            {
                SetSeatedPlayer(-1);
                if ((StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status & Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone)) == Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone))
                {
                    log = new MessageEventLog(null);
                    log.Title = AbilityRules.TranslateResourceString(GameText.WarningText);
                    log.Buttons = MessageEventLogButtons.YesNo;
                    var pbemGameEnded = AbilityRules.TranslateResourceString(s_pbemGameEndedText);
                    log.Message.Text = pbemGameEnded;
                    log.Message.Add("");
                    var translatedText = AbilityRules.TranslateResourceString(s_doYouWantToContinueText);
                    log.Message.Add(translatedText);
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, ContinueTurnDoneGameCallback);
                    log.Release();
                }
                else
                {
                    StartLoadedEmailGame();
                }
            }
        }
    }

    // PlayerC.PAS:1219
    public void ExitGameSaveAsCallback(EventLog log)
    {
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            try
            {
                GameTables.GameEngine.SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
                StrategicMap.Current.ExecuteExitGame();
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [((SaveGameEventLog)log).FileName]);
                StrategicMap.Current.ShowWarning(format);
            }
        }
    }

    private static readonly string s_doYouWantToContinueText = "Do you want to continue?";
    private static readonly string s_gameRestartEventLogFormat = "Game is restarted for unknown reason, contact %s for more information";
    private static readonly string s_gameSuccessfullySavedText = "Game successfully saved";
    private static readonly string s_pbemGameEndedText = "This game's turn has been ended. Restarting is only allowed for resend purposes.";
    private static readonly string s_pbemRestartCheatText = "This game isn't the original game. Restarting it qualifies you as a cheater!";
    private static readonly string s_saveEmailGameText = "Save email game...";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
