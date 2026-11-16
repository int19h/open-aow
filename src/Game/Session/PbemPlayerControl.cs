#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// local player
public partial class PbemPlayerControl : TurnPlayerControl
{
    public new const int ClassId = 131667;

    public PbemPlayerControl(CustomNode owner) : base(owner)
    { }

    private sbyte _localPlayer;

    // PlayerC.PAS:984
    public override void NewTurn(sbyte player)
    {
        SendEmailGameEventLog log = default;
        if (StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status.Contains(PbemPlayerStatusElement.NewTurn) || StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human || StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Terminated)
        {
            base.NewTurn(player);
            Player players = StrategicMap.Current.Players.GetPlayers(player);
            StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
        }
        else
        {
            if (StrategicMap.Current.Players.LocalPlayers.Count > 1 || player != _localPlayer)
            {
                StrategicMap.Current.SetCurrentPlayer(player);
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn);
                log = new SendEmailGameEventLog(null) { Holder = CreationHolder() };
                log.Mode = 2;
                log.SendPlayer = player;
                StrategicMap.Current.TriggerExecuteEventLog(log, false, SendEmailGameCallback);
                log.Release();
            }
            else
            {
                base.NewTurn(player);
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone) + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
            }
        }
    }

    // PlayerC.PAS:1194
    public override void StartNewMap()
    {
        int i;
        int player;
        DefaultStartMap();
        NewDay();
        i = 0;
        int last = StrategicMap.Current.Players.Count - 1;
        for (; i <= last; i += 1)
        {
            if (i == 0 || StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                StrategicMap.Current.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }

        i = 0;
        while (true)
        {
            bool condition = i < StrategicMap.Current.TurnOrder.Count;
            if (condition)
            {
                byte item = StrategicMap.Current.TurnOrder[i];
                condition = StrategicMap.Current.Players.GetPlayers(item).PlayerType != PlayerType.Human;
            }

            if (!condition)
            {
                break;
            }

            i += 1;
        }

        player = StrategicMap.Current.TurnOrder[i];
        Player players = StrategicMap.Current.Players.GetPlayers(player);
        bool test = GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) != null;
        if (test)
        {
            test = CompareText(b: GetEngine().Registry.GetLocalEmailAddress(), a: StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.EmailAddress) == 0;
        }

        if (test)
        {
            _localPlayer = (sbyte)player;
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
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
    public override void PasswordValidated() => ValidateLoadedMap();

    // PlayerC.PAS:1230
    public override void ExitGame(ExitGameMode mode, ExitGameCallBack done)
    {
        SaveGameEventLog log = default;
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatusElement.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
            {
                log = new SaveGameEventLog(null, holder: CreationHolder());
                log.Title = AbilityRules.TranslateResourceString(GetEngine(), s_saveEmailGameText);
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name))
                {
                    log.DefaultName = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name + ".asg";
                }

                log.Directory = GetEngine().StartupDirectory + "EmailIn\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ExitGameSaveAsCallback);
                log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                GetEngine().SaveHsm(StrategicMap.Current.FileName, null, GameProfile.WorldMap);
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
                saveLog = new SaveGameEventLog(null, holder: CreationHolder());
                saveLog.Title = AbilityRules.TranslateResourceString(GetEngine(), s_saveEmailGameText);
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name))
                {
                    saveLog.DefaultName = StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name + ".asg";
                }

                saveLog.Directory = GetEngine().StartupDirectory + "EmailIn\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(saveLog, false, EndTurnGameSavedCallback);
                saveLog.Release();
            }
            else
            {
                Player players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                try
                {
                    GetEngine().SaveHsm(StrategicMap.Current.FileName, null, GameProfile.WorldMap);
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
                    messageLog = new MessageEventLog(null, holder: CreationHolder());
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat) + ".", [StrategicMap.Current.FileName]);
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
    public void TerminateEmailGame() => StrategicMap.Current.ExitGame(ExitGameMode.Exit, null);

    // PlayerC.PAS:890
    public void TerminateEmailGameCallback(EventLog log) => TerminateEmailGame();

    // PlayerC.PAS:893
    public void SendEmailGameAutomatedCallback(SmtpEventLog log)
    {
        SendEmailGameEventLog sendLog = default;
        DeleteFile(log.FileName);
        if (!log.Success)
        {
            sendLog = new SendEmailGameEventLog(null) { Holder = CreationHolder() };
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
    public void SendEmailGameManualCallback(SaveGameEventLog log)
    {
        bool failed = default;
        MessageEventLog messageLog = default;
        if (!IsEmpty(log.FileName))
        {
            try
            {
                GetEngine().SaveHsm(log.FileName, null, GameProfile.WorldMap);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.GameSavedFormat), [log.FileName]);
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
            messageLog = new MessageEventLog(null, holder: CreationHolder());
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat) + ".", [log.FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
            messageLog.Release();
        }
        else
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder());
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_gameSuccessfullySavedText) + ".", [log.FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, false, TerminateEmailGameCallback);
            messageLog.Release();
        }
    }

    // PlayerC.PAS:938
    public void SendEmailGameCallback(EventLog log) =>
        /* TODO: Implement Windows.GetTempPath in the platform or editor service. */
        throw new NotImplementedException();

    // PlayerC.PAS:1018
    public void TriggerGameRestartedEvents(sbyte player)
    {
        RestartGameEventLog log;
        StrategicMap.Current.SynchronizeBegin();
        log = new RestartGameEventLog(null, holder: CreationHolder());
        log.Player = player;
        string name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_gameRestartEventLogFormat), [name]);
        log.Message.Text = format;
        EventLog.DistributeEventLog(log, false);
        log.Release();
        StrategicMap.Current.SynchronizeEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallback(SaveGameEventLog log)
    {
        bool failed = default;
        if (!IsEmpty(log.FileName))
        {
            try
            {
                Player players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status = players.PbemSettings.Status + Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone) - Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.Playing);
                GetEngine().SaveHsm(log.FileName, null, GameProfile.WorldMap);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.GameSavedFormat), [log.FileName]);
                StrategicMap.Current.ShowMessage(format);
                StrategicMap.Current.SynchronizeBegin();
                base.EndTurn(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.SynchronizeEnd();
                failed = false;
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat), [log.FileName]);
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
        Player players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        GetEngine().EmailGames.UpdateGame(players.PbemSettings.EmailGame);
        Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
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
    public void ContinueRestartedGameCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
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
    public void ContinueTurnDoneGameCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
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
        Player players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        if (GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) == null)
        {
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            restarted = GetEngine().EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame);
            Player players2 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            Player players3 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            EmailGame emailGame = GetEngine().EmailGames.CreateEmailGame(players3.PbemSettings.EmailGame.Name, players2.PbemSettings.EmailGame.EmailAddress);
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.SetGame(emailGame);
            if (restarted)
            {
                TriggerGameRestartedEvents(StrategicMap.Current.CurrentPlayer);
            }

            StartLoadedEmailGame();
        }
        else
        {
            Player fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            if (GetEngine().EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame))
            {
                SetSeatedPlayer(-1);
                log = new MessageEventLog(null, holder: CreationHolder());
                log.Buttons = MessageEventLogButtons.YesNo;
                log.Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText);
                string pbemRestartCheat = AbilityRules.TranslateResourceString(GetEngine(), s_pbemRestartCheatText);
                log.Message.Text = pbemRestartCheat;
                log.Message.Add("");
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWantToContinueText);
                log.Message.Add(translatedText);
                StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ContinueRestartedGameCallback);
                log.Release();
            }
            else
            {
                SetSeatedPlayer(-1);
                if ((StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status & Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.NewTurn, PbemPlayerStatusElement.TurnDone)) == Set<PbemPlayerStatusElement>.Of(PbemPlayerStatusElement.TurnDone))
                {
                    log = new MessageEventLog(null, holder: CreationHolder());
                    log.Title = AbilityRules.TranslateResourceString(GetEngine(), GameText.WarningText);
                    log.Buttons = MessageEventLogButtons.YesNo;
                    string pbemGameEnded = AbilityRules.TranslateResourceString(GetEngine(), s_pbemGameEndedText);
                    log.Message.Text = pbemGameEnded;
                    log.Message.Add("");
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_doYouWantToContinueText);
                    log.Message.Add(translatedText);
                    StrategicMap.Current.TriggerTypedExecuteEventLog(log, false, ContinueTurnDoneGameCallback);
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
    public void ExitGameSaveAsCallback(SaveGameEventLog log)
    {
        if (!IsEmpty(log.FileName))
        {
            try
            {
                GetEngine().SaveHsm(log.FileName, null, GameProfile.WorldMap);
                StrategicMap.Current.ExecuteExitGame();
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CouldNotSaveFormat), [log.FileName]);
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
