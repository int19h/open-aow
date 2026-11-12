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
        if (AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn) || AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human || AoWHSMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Terminated)
        {
            base.NewTurn(player);
            var players = AoWHSMap.Current.Players.GetPlayers(player);
            AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
        }
        else
        {
            if (AoWHSMap.Current.Players.LocalPlayers.Count > 1 || player != _localPlayer)
            {
                AoWHSMap.Current.SetCurrentPlayer(player);
                var players = AoWHSMap.Current.Players.GetPlayers(player);
                AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn);
                log = new SendEmailGameEventLog(null);
                log.Mode = 2;
                log.SendPlayer = player;
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, SendEmailGameCallback);
                log.Release();
            }
            else
            {
                base.NewTurn(player);
                var players = AoWHSMap.Current.Players.GetPlayers(player);
                AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
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
        int last = AoWHSMap.Current.Players.Count - 1;
        for (; I <= last; I += 1)
        {
            if (I == 0 || AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.Players.GetPlayers(I).Status = PlayerStatus.Playing;
            }
        }

        I = 0;
        while (true)
        {
            bool condition = I < AoWHSMap.Current.TurnOrder.Count;
            if (condition)
            {
                var item = AoWHSMap.Current.TurnOrder[I];
                condition = AoWHSMap.Current.Players.GetPlayers(item).PlayerType != PlayerType.Human;
            }

            if (!condition)
                break;
            I += 1;
        }

        player = AoWHSMap.Current.TurnOrder[I];
        var players = AoWHSMap.Current.Players.GetPlayers(player);
        bool test = GameTables.GameEngine.EmailGames.FindEmailGame(players.PbemSettings.EmailGame) != null;
        if (test)
        {
            var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
            test = CompareText(AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.EmailAddress, localEmailAddress) == 0;
        }

        if (test)
        {
            _localPlayer = (sbyte)player;
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(player);
            AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn);
        }
        else
        {
            AoWHSMap.Current.SetSeatedPlayer(-1);
        }

        NewTurn((sbyte)AoWHSMap.Current.TurnOrder[1]);
    }

    // PlayerC.PAS:1187
    public override void StartLoadedMap()
    {
        if (!IsEmpty(AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).Settings))
        {
            ValidatePassword(AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).Settings);
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
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
            {
                log = new SaveGameEventLog(null);
                log.Title = AbilityRules.TranslateResourceString(s_saveEmailGameText);
                if (!IsEmpty(AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name))
                {
                    log.DefaultName = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name + ".asg";
                }

                log.Directory = GameTables.GameEngine.StartupDirectory + "EmailIn\\";
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, ExitGameSaveAsCallback);
                log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                AoWHSMap.Current.FileName = FileBoundary.WriteName(AoWHSMap.Current.FileName);
                GameTables.GameEngine.SaveHsm(AoWHSMap.Current.FileName, null, GameProfile.WorldMap);
                AoWHSMap.Current.ExecuteExitGame();
            }
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1053
    public override void EndTurn(sbyte player)
    {
        SaveGameEventLog saveLog = default;
        MessageEventLog messageLog = default;
        bool saved = default;
        if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && AoWHSMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
            {
                saveLog = new SaveGameEventLog(null);
                saveLog.Title = AbilityRules.TranslateResourceString(s_saveEmailGameText);
                if (!IsEmpty(AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name))
                {
                    saveLog.DefaultName = AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name + ".asg";
                }

                saveLog.Directory = GameTables.GameEngine.StartupDirectory + "EmailIn\\";
                AoWHSMap.Current.TriggerExecuteEventLog(saveLog, false, EndTurnGameSavedCallback);
                saveLog.Release();
            }
            else
            {
                var players = AoWHSMap.Current.Players.GetPlayers(player);
                AoWHSMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                AoWHSMap.Current.FileName = FileBoundary.WriteName(AoWHSMap.Current.FileName);
                try
                {
                    GameTables.GameEngine.SaveHsm(AoWHSMap.Current.FileName, null, GameProfile.WorldMap);
                    saved = true;
                }
                catch
                {
                    saved = false;
                }

                if (saved)
                {
                    base.EndTurn(AoWHSMap.Current.CurrentPlayer);
                }
                else
                {
                    messageLog = new MessageEventLog(null);
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat) + ".", [AoWHSMap.Current.FileName]);
                    messageLog.Message.Text = format;
                    AoWHSMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
                    messageLog.Release();
                }
            }
        }
        else
        {
            // { Original uses current player, ignoring Player here. }
            base.EndTurn(AoWHSMap.Current.CurrentPlayer);
        }
    }

    // PlayerC.PAS:887
    public void TerminateEmailGame()
    {
        AoWHSMap.Current.ExitGame(ExitGameMode.Exit, null);
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
            sendLog.SendPlayer = AoWHSMap.Current.CurrentPlayer;
            AoWHSMap.Current.TriggerExecuteEventLog(sendLog, false, SendEmailGameCallback);
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
                AoWHSMap.Current.ShowMessage(format);
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
            AoWHSMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
            messageLog.Release();
        }
        else
        {
            messageLog = new MessageEventLog(null);
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameSuccessfullySavedText) + ".", [((SaveGameEventLog)log).FileName]);
            messageLog.Message.Text = format;
            AoWHSMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
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
        AoWHSMap.Current.SynchronizeBegin();
        log = new RestartGameEventLog(null);
        log._player = player;
        var name = AoWHSMap.Current.Players.GetPlayers(player).GetName();
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_gameRestartEventLogFormat), [name]);
        log.Message.Text = format;
        EventLog.DistributeEventLog(log, false);
        log.Release();
        AoWHSMap.Current.SynchronizeEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallback(EventLog log)
    {
        bool failed = default;
        if (!IsEmpty(((SaveGameEventLog)log).FileName))
        {
            try
            {
                var players = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
                AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                GameTables.GameEngine.SaveHsm(((SaveGameEventLog)log).FileName, null, GameProfile.WorldMap);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.GameSavedFormat), [((SaveGameEventLog)log).FileName]);
                AoWHSMap.Current.ShowMessage(format);
                AoWHSMap.Current.SynchronizeBegin();
                base.EndTurn(AoWHSMap.Current.CurrentPlayer);
                AoWHSMap.Current.SynchronizeEnd();
                failed = false;
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [((SaveGameEventLog)log).FileName]);
                AoWHSMap.Current.ShowWarning(format);
                failed = true;
            }
        }
        else
        {
            failed = true;
        }

        if (failed)
        {
            AoWHSMap.Current.TriggerEndTurnEvent(AoWHSMap.Current.CurrentPlayer);
        }
    }

    // PlayerC.PAS:1094
    public void StartLoadedEmailGame()
    {
        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.EmailGame.LastDay = AoWHSMap.Current.Day;
        var players = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
        GameTables.GameEngine.EmailGames.UpdateGame(players.PbemSettings.EmailGame);
        var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn))
        {
            AoWHSMap.Current.SynchronizeBegin();
            DefaultStartMap();
            AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(AoWHSMap.Current.CurrentPlayer);
            NewTurn(AoWHSMap.Current.CurrentPlayer);
            AoWHSMap.Current.SynchronizeEnd();
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
            AoWHSMap.Current.SynchronizeBegin();
            TriggerGameRestartedEvents(AoWHSMap.Current.CurrentPlayer);
            AoWHSMap.Current.SynchronizeEnd();
            StartLoadedEmailGame();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1123
    public void ContinueTurnDoneGameCallback(EventLog log)
    {
        if (((MessageEventLog)log).PressedButton == PressedEventLogButton.Yes)
        {
            AoWHSMap.Current.SynchronizeBegin();
            DefaultStartMap();
            AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(AoWHSMap.Current.CurrentPlayer);
            EndTurn(AoWHSMap.Current.CurrentPlayer);
            AoWHSMap.Current.SynchronizeEnd();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1136
    public void ValidateLoadedMap()
    {
        bool restarted = default;
        MessageEventLog log = default;
        _localPlayer = AoWHSMap.Current.CurrentPlayer;
        var players = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
        if (GameTables.GameEngine.EmailGames.FindEmailGame(players.PbemSettings.EmailGame) == null)
        {
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
            restarted = GameTables.GameEngine.EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame);
            var players2 = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
            var players3 = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
            var emailGame = GameTables.GameEngine.EmailGames.CreateEmailGame(players3.PbemSettings.EmailGame.Name, players2.PbemSettings.EmailGame.EmailAddress);
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.SetGame(emailGame);
            if (restarted)
            {
                TriggerGameRestartedEvents(AoWHSMap.Current.CurrentPlayer);
            }

            StartLoadedEmailGame();
        }
        else
        {
            var fPlayersPlayers = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer);
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
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, ContinueRestartedGameCallback);
                log.Release();
            }
            else
            {
                SetSeatedPlayer(-1);
                if ((AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PbemSettings.Status & Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone)) == Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone))
                {
                    log = new MessageEventLog(null);
                    log.Title = AbilityRules.TranslateResourceString(GameText.WarningText);
                    log.Buttons = MessageEventLogButtons.YesNo;
                    var pbemGameEnded = AbilityRules.TranslateResourceString(s_pbemGameEndedText);
                    log.Message.Text = pbemGameEnded;
                    log.Message.Add("");
                    var translatedText = AbilityRules.TranslateResourceString(s_doYouWantToContinueText);
                    log.Message.Add(translatedText);
                    AoWHSMap.Current.TriggerExecuteEventLog(log, false, ContinueTurnDoneGameCallback);
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
                AoWHSMap.Current.ExecuteExitGame();
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CouldNotSaveFormat), [((SaveGameEventLog)log).FileName]);
                AoWHSMap.Current.ShowWarning(format);
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
