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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(player).PbemSettings.Status.Contains(PbemPlayerStatuses.NewTurn) || strategicMap.Players.GetPlayers(player).PlayerType != PlayerType.Human || strategicMap.Players.GetPlayers(player).Status == PlayerStatus.Terminated)
        {
            base.NewTurn(player);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            Player? players = engineStrategicMap.Players.GetPlayers(player);
            engineStrategicMap.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status & ~(PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) | PbemPlayerStatuses.Playing;
        }
        else
        {
            StrategicMap setCurrentPlayerStrategicMap = GetEngine().StrategicMap;
            if (setCurrentPlayerStrategicMap.Players.LocalPlayers.Count > 1 || player != _localPlayer)
            {
                setCurrentPlayerStrategicMap.SetCurrentPlayer(player);
                Player? players = setCurrentPlayerStrategicMap.Players.GetPlayers(player);
                setCurrentPlayerStrategicMap.Players.GetPlayers(player).PbemSettings.Status = players.PbemSettings.Status | PbemPlayerStatuses.NewTurn;
                SendEmailGameEventLog log = new(null, holder: CreationHolder())
                {
                    Mode = 2,
                    SendPlayer = player
                };
                setCurrentPlayerStrategicMap.TriggerExecuteEventLog(log, immediate: false, done: SendEmailGameCallback);
                _ = log.Release();
            }
            else
            {
                base.NewTurn(player);
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                Player? players = getPlayersStrategicMap.Players.GetPlayers(player);
                getPlayersStrategicMap.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status & ~(PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) | PbemPlayerStatuses.Playing;
            }
        }
    }

    // PlayerC.PAS:1194
    public override void StartNewMap()
    {
        DefaultStartMap();
        NewDay();
        int i = 0;
        StrategicMap strategicMap = GetEngine().StrategicMap;
        int last = strategicMap.Players.Count - 1;
        for (; i <= last; i += 1)
        {
            if (i == 0 || strategicMap.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                strategicMap.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }

        i = 0;
        while (true)
        {
            bool condition = i < strategicMap.TurnOrder.Count;
            if (condition)
            {
                int item = strategicMap.TurnOrder[i];
                condition = strategicMap.Players.GetPlayers(item).PlayerType != PlayerType.Human;
            }

            if (!condition)
            {
                break;
            }

            i += 1;
        }

        int player = strategicMap.TurnOrder[i];
        Player? players = strategicMap.Players.GetPlayers(player);
        bool test = GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) is not null;
        if (test)
        {
            test = CompareText(b: GetEngine().Registry.GetLocalEmailAddress(), a: GetEngine().StrategicMap.Players.GetPlayers(player).PbemSettings.EmailGame.EmailAddress) == 0;
        }

        if (test)
        {
            _localPlayer = PlayerId.FromOriginal((sbyte)player);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(player);
            engineStrategicMap.Players.GetPlayers(player).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status | PbemPlayerStatuses.NewTurn;
        }
        else
        {
            GetEngine().StrategicMap.SetSeatedPlayer(null);
        }

        NewTurn(PlayerId.FromOriginal((sbyte)GetEngine().StrategicMap.TurnOrder[1]));
    }

    // PlayerC.PAS:1187
    public override void StartLoadedMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (!IsEmpty(strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).Settings))
        {
            ValidatePassword(strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).Settings);
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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatuses.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(strategicMap.FileName)))
            {
                SaveGameEventLog log = new(null, holder: CreationHolder())
                {
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_saveEmailGameText)
                };
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (!IsEmpty(engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer).PbemSettings.EmailGame.Name))
                {
                    log.DefaultName = $"{engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer).PbemSettings.EmailGame.Name}.asg";
                }

                log.Directory = $"{GetEngine().StartupDirectory}EmailIn\\";
                engineStrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ExitGameSaveAsCallback);
                _ = log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                StrategicMap writeNameStrategicMap = GetEngine().StrategicMap;
                writeNameStrategicMap.FileName = FileBoundary.WriteName(writeNameStrategicMap.FileName);
                GetEngine().SaveHsm(GetEngine().StrategicMap.FileName, progress: null, profile: GameProfile.WorldMap);
                GetEngine().StrategicMap.ExecuteExitGame();
            }
        }
        else
        {
            GetEngine().StrategicMap.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1053
    public override void EndTurn(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human && strategicMap.Players.GetPlayers(player).Status == PlayerStatus.Playing)
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(strategicMap.FileName)))
            {
                SaveGameEventLog saveLog = new(null, holder: CreationHolder())
                {
                    Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_saveEmailGameText)
                };
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (!IsEmpty(engineStrategicMap.Players.GetPlayers(player).PbemSettings.EmailGame.Name))
                {
                    saveLog.DefaultName = $"{engineStrategicMap.Players.GetPlayers(player).PbemSettings.EmailGame.Name}.asg";
                }

                saveLog.Directory = $"{GetEngine().StartupDirectory}EmailIn\\";
                engineStrategicMap.TriggerTypedExecuteEventLog(saveLog, immediate: false, done: EndTurnGameSavedCallback);
                _ = saveLog.Release();
            }
            else
            {
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                Player? players = getPlayersStrategicMap.Players.GetPlayers(player);
                getPlayersStrategicMap.Players.GetPlayers(player).PbemSettings.Status = (players.PbemSettings.Status | PbemPlayerStatuses.TurnDone) & ~PbemPlayerStatuses.Playing;
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                getPlayersStrategicMap.FileName = FileBoundary.WriteName(getPlayersStrategicMap.FileName);
                bool saved;
                try
                {
                    GetEngine().SaveHsm(GetEngine().StrategicMap.FileName, progress: null, profile: GameProfile.WorldMap);
                    saved = true;
                }
                catch
                {
                    saved = false;
                }

                if (saved)
                {
                    base.EndTurn(GetEngine().StrategicMap.CurrentPlayer);
                }
                else
                {
                    MessageEventLog messageLog = new(null, holder: CreationHolder());
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat) + ".", arguments: [GetEngine().StrategicMap.FileName]);
                    messageLog.Message.Text = format;
                    GetEngine().StrategicMap.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
                    _ = messageLog.Release();
                }
            }
        }
        else
        {
            // { Original uses current player, ignoring Player here. }
            base.EndTurn(GetEngine().StrategicMap.CurrentPlayer);
        }
    }

    // PlayerC.PAS:887
    public void TerminateEmailGame() => GetEngine().StrategicMap.ExitGame(ExitGameMode.Exit, done: null);

    // PlayerC.PAS:890
    public void TerminateEmailGameCallback(EventLog log) => TerminateEmailGame();

    // PlayerC.PAS:893
    public void SendEmailGameAutomatedCallback(SmtpEventLog log)
    {
        _ = RtlHost.Files.DeleteFile(log.FileName);
        if (!log.Success)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            SendEmailGameEventLog sendLog = new(null, holder: CreationHolder())
            {
                Mode = 1,
                SendPlayer = strategicMap.CurrentPlayer
            };
            strategicMap.TriggerExecuteEventLog(sendLog, immediate: false, done: SendEmailGameCallback);
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
                GetEngine().StrategicMap.ShowMessage(format);
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
            GetEngine().StrategicMap.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
            _ = messageLog.Release();
        }
        else
        {
            messageLog = new MessageEventLog(null, holder: CreationHolder());
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameSuccessfullySavedText) + ".", arguments: [log.FileName]);
            messageLog.Message.Text = format;
            GetEngine().StrategicMap.TriggerExecuteEventLog(messageLog, immediate: false, done: TerminateEmailGameCallback);
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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.SynchronizeBegin();
        RestartGameEventLog log = new(null, holder: CreationHolder())
        {
            Player = player
        };
        string name = strategicMap.Players.GetPlayers(player).GetName();
        string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_gameRestartEventLogFormat), arguments: [name]);
        log.Message.Text = format;
        EventLog.DistributeEventLog(log, execute: false);
        _ = log.Release();
        GetEngine().StrategicMap.SynchronizeEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallback(SaveGameEventLog log)
    {
        bool failed;
        if (!IsEmpty(log.FileName))
        {
            try
            {
                StrategicMap strategicMap = GetEngine().StrategicMap;
                Player? players = strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer);
                strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).PbemSettings.Status = (players.PbemSettings.Status | PbemPlayerStatuses.TurnDone) & ~PbemPlayerStatuses.Playing;
                GetEngine().SaveHsm(log.FileName, progress: null, profile: GameProfile.WorldMap);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GameSavedFormat), arguments: [log.FileName]);
                GetEngine().StrategicMap.ShowMessage(format);
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                engineStrategicMap.SynchronizeBegin();
                base.EndTurn(engineStrategicMap.CurrentPlayer);
                GetEngine().StrategicMap.SynchronizeEnd();
                failed = false;
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [log.FileName]);
                GetEngine().StrategicMap.ShowWarning(format);
                failed = true;
            }
        }
        else
        {
            failed = true;
        }

        if (failed)
        {
            StrategicMap triggerEndTurnEventStrategicMap = GetEngine().StrategicMap;
            triggerEndTurnEventStrategicMap.TriggerEndTurnEvent(triggerEndTurnEventStrategicMap.CurrentPlayer);
        }
    }

    // PlayerC.PAS:1094
    public void StartLoadedEmailGame()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).PbemSettings.EmailGame.LastDay = strategicMap.Day;
        Player? players = strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer);
        _ = GetEngine().EmailGames.UpdateGame(players.PbemSettings.EmailGame);
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer);
        engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer).PbemSettings.Status = fPlayersPlayers.PbemSettings.Status | PbemPlayerStatuses.Playing;
        if (engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer).PbemSettings.Status.Contains(PbemPlayerStatuses.NewTurn))
        {
            engineStrategicMap.SynchronizeBegin();
            DefaultStartMap();
            engineStrategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(engineStrategicMap.CurrentPlayer));
            NewTurn(GetEngine().StrategicMap.CurrentPlayer);
            GetEngine().StrategicMap.SynchronizeEnd();
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
            StrategicMap strategicMap = GetEngine().StrategicMap;
            strategicMap.SynchronizeBegin();
            TriggerGameRestartedEvents(strategicMap.CurrentPlayer);
            GetEngine().StrategicMap.SynchronizeEnd();
            StartLoadedEmailGame();
        }
        else
        {
            GetEngine().StrategicMap.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1123
    public void ContinueTurnDoneGameCallback(MessageEventLog log)
    {
        if (log.PressedButton == PressedEventLogButton.Yes)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            strategicMap.SynchronizeBegin();
            DefaultStartMap();
            strategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(strategicMap.CurrentPlayer));
            EndTurn(GetEngine().StrategicMap.CurrentPlayer);
            GetEngine().StrategicMap.SynchronizeEnd();
        }
        else
        {
            GetEngine().StrategicMap.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1136
    public void ValidateLoadedMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        _localPlayer = strategicMap.CurrentPlayer;
        Player? players = strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer);
        if (GetEngine().EmailGames.FindEmailGame(players.PbemSettings.EmailGame) is null)
        {
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            Player? fPlayersPlayers = engineStrategicMap.Players.GetPlayers(engineStrategicMap.CurrentPlayer);
            bool restarted = GetEngine().EmailGames.EmailGameRestarted(fPlayersPlayers.PbemSettings.EmailGame);
            StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
            Player? players2 = getPlayersStrategicMap.Players.GetPlayers(getPlayersStrategicMap.CurrentPlayer);
            Player? players3 = getPlayersStrategicMap.Players.GetPlayers(getPlayersStrategicMap.CurrentPlayer);
            EmailGame emailGame = GetEngine().EmailGames.CreateEmailGame(players3.PbemSettings.EmailGame.Name, players2.PbemSettings.EmailGame.EmailAddress);
            StrategicMap setGameStrategicMap = GetEngine().StrategicMap;
            setGameStrategicMap.Players.GetPlayers(setGameStrategicMap.CurrentPlayer).PbemSettings.SetGame(emailGame);
            if (restarted)
            {
                TriggerGameRestartedEvents(GetEngine().StrategicMap.CurrentPlayer);
            }

            StartLoadedEmailGame();
        }
        else
        {
            StrategicMap playersStrategicMap = GetEngine().StrategicMap;
            Player? fPlayersPlayers = playersStrategicMap.Players.GetPlayers(playersStrategicMap.CurrentPlayer);
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
                GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ContinueRestartedGameCallback);
                _ = log.Release();
            }
            else
            {
                SetSeatedPlayer(null);
                StrategicMap currentPlayerStrategicMap = GetEngine().StrategicMap;
                if ((currentPlayerStrategicMap.Players.GetPlayers(currentPlayerStrategicMap.CurrentPlayer).PbemSettings.Status & (PbemPlayerStatuses.NewTurn | PbemPlayerStatuses.TurnDone)) == PbemPlayerStatuses.TurnDone)
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
                    GetEngine().StrategicMap.TriggerTypedExecuteEventLog(log, immediate: false, done: ContinueTurnDoneGameCallback);
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
                GetEngine().StrategicMap.ExecuteExitGame();
            }
            catch
            {
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CouldNotSaveFormat), arguments: [log.FileName]);
                GetEngine().StrategicMap.ShowWarning(format);
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
