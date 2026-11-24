#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.Session;

// local player
public sealed class PbemPlayerControl(CustomNode? owner) : TurnPlayerControl(owner)
{
    public new const int ClassId = 131_667;
    private PlayerId? _localPlayer = PlayerId.Independent;

    // PlayerC.PAS:984
    public override void NewTurn(PlayerId? player)
    {
        if (StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status.Contains(PbemPlayerStatuses.NewTurn) || StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human || StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Terminated)
        {
            base.NewTurn(player);
            Player? players = StrategicMap.Current.Players.GetPlayers(player);
            StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status & ~(PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) | PbemPlayerStatuses.Playing;
        }
        else
        {
            if (StrategicMap.Current.Players.LocalPlayers.Count > 1 || player != _localPlayer)
            {
                StrategicMap.Current.SetCurrentPlayer(player);
                Player? players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status | PbemPlayerStatuses.NewTurn;
                SendEmailGameEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    Mode = 2,
                    SendPlayer = player
                };
                StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: SendEmailGameCallback);
                _ = log.Release();
            }
            else
            {
                base.NewTurn(player);
                Player? players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status & ~(PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) | PbemPlayerStatuses.Playing;
            }
        }
    }

    // PlayerC.PAS:1194
    public override void StartNewMap()
    {
        DefaultStartMap();
        NewDay();
        int i = 0;
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
                int item = StrategicMap.Current.TurnOrder[i];
                condition = StrategicMap.Current.Players.GetPlayers(item).PlayerType != PlayerType.Human;
            }

            if (!condition)
            {
                break;
            }

            i += 1;
        }

        int player = StrategicMap.Current.TurnOrder[i];
        Player? players = StrategicMap.Current.Players.GetPlayers(player);
        bool test = GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) is not null;
        if (test)
        {
            test = CompareText(b: GetEngine().Registry.GetLocalEmailAddress(), a: StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.EmailAddress) == 0;
        }

        if (test)
        {
            _localPlayer = PlayerId.FromOriginal((sbyte)player);
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status | PbemPlayerStatuses.NewTurn;
        }
        else
        {
            StrategicMap.Current.SetSeatedPlayer(null);
        }

        NewTurn(PlayerId.FromOriginal((sbyte)StrategicMap.Current.TurnOrder[1]));
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
    public override void ExitGame(ExitGameMode mode, ExitGameCallBack? done)
    {
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatuses.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
            {
                SaveGameEventLog log = new(null, holder: CreationHolder())
                {
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_saveEmailGameText)
                };
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name))
                {
                    log.DefaultName = $"{StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.EmailGame.Name}.asg";
                }

                log.Directory = $"{GetEngine().StartupDirectory}EmailIn\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ExitGameSaveAsCallback);
                _ = log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                GetEngine().SaveHsm(StrategicMap.Current.FileName, progress: null, profile: GameProfile.WorldMap);
                StrategicMap.Current.ExecuteExitGame();
            }
        }
        else
        {
            StrategicMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1053
    public override void EndTurn(PlayerId? player)
    {
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(StrategicMap.Current.FileName)))
            {
                SaveGameEventLog saveLog = new(null, holder: CreationHolder())
                {
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_saveEmailGameText)
                };
                if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name))
                {
                    saveLog.DefaultName = $"{StrategicMap.Current.Players.GetPlayers(player).PbemSettings.EmailGame.Name}.asg";
                }

                saveLog.Directory = $"{GetEngine().StartupDirectory}EmailIn\\";
                StrategicMap.Current.TriggerTypedExecuteEventLog(saveLog, immediate: false, done: EndTurnGameSavedCallback);
                _ = saveLog.Release();
            }
            else
            {
                Player? players = StrategicMap.Current.Players.GetPlayers(player);
                StrategicMap.Current.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status | PbemPlayerStatuses.TurnDone) & ~PbemPlayerStatuses.Playing;
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap.Current.FileName = FileBoundary.WriteName(StrategicMap.Current.FileName);
                bool saved;
                try
                {
                    GetEngine().SaveHsm(StrategicMap.Current.FileName, progress: null, profile: GameProfile.WorldMap);
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
                    MessageEventLog messageLog = new(null, holder: CreationHolder());
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat) + ".", arguments: [StrategicMap.Current.FileName]);
                    messageLog.Message.Text = format;
                    StrategicMap.Current.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
                    _ = messageLog.Release();
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
    public void TerminateEmailGame() => StrategicMap.Current.ExitGame(ExitGameMode.Exit, done: null);

    // PlayerC.PAS:890
    public void TerminateEmailGameCallback(EventLog log) => TerminateEmailGame();

    // PlayerC.PAS:893
    public void SendEmailGameAutomatedCallback(SmtpEventLog log)
    {
        _ = RtlHost.Files.DeleteFile(log.FileName);
        if (!log.Success)
        {
            SendEmailGameEventLog sendLog = new(null)
            {
                Holder = CreationHolder(),
                Mode = 1,
                SendPlayer = StrategicMap.Current.CurrentPlayer
            };
            StrategicMap.Current.TriggerExecuteEventLog(sendLog, immediate: false, done: SendEmailGameCallback);
            _ = sendLog.Release();
        }
        else
        {
            TerminateEmailGame();
        }
    }

    // PlayerC.PAS:909
    public void SendEmailGameManualCallback(SaveGameEventLog log)
    {
        bool failed;
        if (!IsEmpty(log.FileName))
        {
            try
            {
                GetEngine().SaveHsm(log.FileName, progress: null, profile: GameProfile.WorldMap);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GameSavedFormat), arguments: [log.FileName]);
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

        MessageEventLog messageLog;
        if (failed)
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder());
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat) + ".", arguments: [log.FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
            _ = messageLog.Release();
        }
        else
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder());
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameSuccessfullySavedText) + ".", arguments: [log.FileName]);
            messageLog.Message.Text = format;
            StrategicMap.Current.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
            _ = messageLog.Release();
        }
    }

    // PlayerC.PAS:938
    public void SendEmailGameCallback(EventLog log)
        /* TODO: Implement Windows.GetTempPath in the platform or editor service. */
        => throw new NotImplementedException();

    // PlayerC.PAS:1018
    public void TriggerGameRestartedEvents(PlayerId? player)
    {
        StrategicMap.Current.SynchronizeBegin();
        RestartGameEventLog log = new(null, holder: CreationHolder())
        {
            Player = player
        };
        string name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameRestartEventLogFormat), arguments: [name]);
        log.Message.Text = format;
        EventLog.DistributeEventLog(log, execute: false);
        _ = log.Release();
        StrategicMap.Current.SynchronizeEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallback(SaveGameEventLog log)
    {
        bool failed;
        if (!IsEmpty(log.FileName))
        {
            try
            {
                Player? players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status = (players.PbemSettings.Status | PbemPlayerStatuses.TurnDone) & ~PbemPlayerStatuses.Playing;
                GetEngine().SaveHsm(log.FileName, progress: null, profile: GameProfile.WorldMap);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GameSavedFormat), arguments: [log.FileName]);
                StrategicMap.Current.ShowMessage(format);
                StrategicMap.Current.SynchronizeBegin();
                base.EndTurn(StrategicMap.Current.CurrentPlayer);
                StrategicMap.Current.SynchronizeEnd();
                failed = false;
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [log.FileName]);
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
        Player? players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        _ = GetEngine().EmailGames.UpdateGame(players.PbemSettings.EmailGame);
        Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status | PbemPlayerStatuses.Playing;
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatuses.NewTurn))
        {
            StrategicMap.Current.SynchronizeBegin();
            DefaultStartMap();
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(StrategicMap.Current.CurrentPlayer));
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
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(StrategicMap.Current.CurrentPlayer));
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
        _localPlayer = StrategicMap.Current.CurrentPlayer;
        Player? players = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
        if (GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) is null)
        {
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            bool restarted = GetEngine().EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame);
            Player? players2 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            Player? players3 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
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
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer);
            MessageEventLog log;
            if (GetEngine().EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame))
            {
                SetSeatedPlayer(null);
                log = new MessageEventLog(null, holder: CreationHolder())
                {
                    Buttons = MessageEventLogButtons.YesNo,
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarningText)
                };
                string pbemRestartCheat = AbilityRules.TranslateResourceString(GetEngine(), S: s_pbemRestartCheatText);
                log.Message.Text = pbemRestartCheat;
                _ = log.Message.Add("");
                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWantToContinueText);
                _ = log.Message.Add(translatedText);
                StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ContinueRestartedGameCallback);
                _ = log.Release();
            }
            else
            {
                SetSeatedPlayer(null);
                if ((StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PbemSettings.Status & (PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) == PbemPlayerStatuses.TurnDone)
                {
                    log = new MessageEventLog(null, holder: CreationHolder())
                    {
                        Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarningText),
                        Buttons = MessageEventLogButtons.YesNo
                    };
                    string pbemGameEnded = AbilityRules.TranslateResourceString(GetEngine(), S: s_pbemGameEndedText);
                    log.Message.Text = pbemGameEnded;
                    _ = log.Message.Add("");
                    string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_doYouWantToContinueText);
                    _ = log.Message.Add(translatedText);
                    StrategicMap.Current.TriggerTypedExecuteEventLog(log, immediate: false, done: ContinueTurnDoneGameCallback);
                    _ = log.Release();
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
                GetEngine().SaveHsm(log.FileName, progress: null, profile: GameProfile.WorldMap);
                StrategicMap.Current.ExecuteExitGame();
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [log.FileName]);
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
}
