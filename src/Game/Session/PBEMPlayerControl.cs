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
public partial class PBEMPlayerControl : TurnPlayerControl
{
    public new const int ClassID = 131667;

    public PBEMPlayerControl(ECustomNode owner) : base(owner)
    { }

    public sbyte FLocalPlayer;

    // PlayerC.PAS:984
    public override void NewTurn(sbyte Player)
    {
        SendEmailGameEventLog Log = default;
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus.Contains(PBEMPlayerStatusElement.NewTurn) || AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human || AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus == Session.Player.Status.Terminated)
        {
            base.NewTurn(Player);
            var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus = players.FPBEMSettings.FStatus - Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.NewTurn, PBEMPlayerStatusElement.TurnDone) + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.Playing);
        }
        else
        {
            if (AoWHSMap.Current.FPlayers.FLocalPlayers.Count > 1 || Player != FLocalPlayer)
            {
                AoWHSMap.Current.SetCurrentPlayer(Player);
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus = players.FPBEMSettings.FStatus + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.NewTurn);
                Log = new SendEmailGameEventLog(null);
                Log.FMode = 2;
                Log.FSendPlayer = Player;
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, SendEmailGameCallBack);
                Log.Release();
            }
            else
            {
                base.NewTurn(Player);
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus = players.FPBEMSettings.FStatus - Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.NewTurn, PBEMPlayerStatusElement.TurnDone) + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.Playing);
            }
        }
    }

    // PlayerC.PAS:1194
    public override void StartNewMap()
    {
        int I;
        int Player;
        DefaultStartMap();
        NewDay();
        I = 0;
        int last = AoWHSMap.Current.FPlayers.Count - 1;
        for (; I <= last; I += 1)
        {
            if (I == 0 || AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus = Session.Player.Status.Playing;
            }
        }

        I = 0;
        while (true)
        {
            bool condition = I < AoWHSMap.Current.FTurnOrder.Count;
            if (condition)
            {
                var item = AoWHSMap.Current.FTurnOrder[I];
                condition = AoWHSMap.Current.FPlayers.GetPlayers(item).FPlayerType != PlayerType.Human;
            }

            if (!condition)
                break;
            I += 1;
        }

        Player = AoWHSMap.Current.FTurnOrder[I];
        var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        bool test = GameTables.AoWEngine.FEmailGames.FindEmailGame(players.FPBEMSettings.FEmailGame) != null;
        if (test)
        {
            var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
            test = CompareText(AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FEmailGame.FEmailAddress, localEmailAddress) == 0;
        }

        if (test)
        {
            FLocalPlayer = (sbyte)Player;
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus = fPlayersPlayers.FPBEMSettings.FStatus + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.NewTurn);
        }
        else
        {
            AoWHSMap.Current.SetSeatedPlayer(-1);
        }

        NewTurn((sbyte)AoWHSMap.Current.FTurnOrder[1]);
    }

    // PlayerC.PAS:1187
    public override void StartLoadedMap()
    {
        if (!IsEmpty(AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FSettings))
        {
            ValidatePassword(AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FSettings);
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
    public override void ExitGame(ExitGameMode Mode, ExitGameCallBack Done)
    {
        SaveGameEventLog Log = default;
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FStatus.Contains(PBEMPlayerStatusElement.Playing))
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
            {
                Log = new SaveGameEventLog(null);
                Log.FTitle = AbilityRules.TranslateRStr(SaveEmailGameRStr);
                if (!IsEmpty(AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FEmailGame.FName))
                {
                    Log.FDefaultName = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FEmailGame.FName + ".asg";
                }

                Log.FDirectory = GameTables.AoWEngine.StartupDirectory + "EmailIn\\";
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ExitGameSaveAsCallBack);
                Log.Release();
            }
            else
            {
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                AoWHSMap.Current.FileName = FileBoundary.WriteName(AoWHSMap.Current.FileName);
                GameTables.AoWEngine.SaveHSM(AoWHSMap.Current.FileName, null, GameProfile.WorldMap);
                AoWHSMap.Current.ExecuteExitGame();
            }
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1053
    public override void EndTurn(sbyte Player)
    {
        SaveGameEventLog SaveLog = default;
        MessageEventLog MessageLog = default;
        bool Saved = default;
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human && AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus == Session.Player.Status.Playing)
        {
            if (DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
            {
                SaveLog = new SaveGameEventLog(null);
                SaveLog.FTitle = AbilityRules.TranslateRStr(SaveEmailGameRStr);
                if (!IsEmpty(AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FEmailGame.FName))
                {
                    SaveLog.FDefaultName = AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FEmailGame.FName + ".asg";
                }

                SaveLog.FDirectory = GameTables.AoWEngine.StartupDirectory + "EmailIn\\";
                AoWHSMap.Current.TriggerExecuteEventLog(SaveLog, false, EndTurnGameSavedCallBack);
                SaveLog.Release();
            }
            else
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FPBEMSettings.FStatus = players.FPBEMSettings.FStatus + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.TurnDone) - Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.Playing);
                // A game file outside EmailIn takes the name the host writes (own-formats.md 4.2).
                AoWHSMap.Current.FileName = FileBoundary.WriteName(AoWHSMap.Current.FileName);
                try
                {
                    GameTables.AoWEngine.SaveHSM(AoWHSMap.Current.FileName, null, GameProfile.WorldMap);
                    Saved = true;
                }
                catch
                {
                    Saved = false;
                }

                if (Saved)
                {
                    base.EndTurn(AoWHSMap.Current.FCurrentPlayer);
                }
                else
                {
                    MessageLog = new MessageEventLog(null);
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr) + ".", [AoWHSMap.Current.FileName]);
                    MessageLog.FMessage.Text = format;
                    AoWHSMap.Current.TriggerExecuteEventLog(MessageLog, false, TerminateEmailGameCallBack);
                    MessageLog.Release();
                }
            }
        }
        else
        {
            // { Original uses current player, ignoring Player here. }
            base.EndTurn(AoWHSMap.Current.FCurrentPlayer);
        }
    }

    // PlayerC.PAS:887
    public void TerminateEmailGame()
    {
        AoWHSMap.Current.ExitGame(ExitGameMode.Exit, null);
    }

    // PlayerC.PAS:890
    public void TerminateEmailGameCallBack(EventLog Log)
    {
        TerminateEmailGame();
    }

    // PlayerC.PAS:893
    public void SendEmailGameAutomatedCallBack(EventLog Log)
    {
        SendEmailGameEventLog SendLog = default;
        DeleteFile(((SMTPEventLog)Log).FFileName);
        if (!((SMTPEventLog)Log).FSuccess)
        {
            SendLog = new SendEmailGameEventLog(null);
            SendLog.FMode = 1;
            SendLog.FSendPlayer = AoWHSMap.Current.FCurrentPlayer;
            AoWHSMap.Current.TriggerExecuteEventLog(SendLog, false, SendEmailGameCallBack);
            SendLog.Release();
        }
        else
        {
            TerminateEmailGame();
        }
    }

    // PlayerC.PAS:909
    public void SendEmailGameManualCallBack(EventLog Log)
    {
        bool Failed = default;
        MessageEventLog MessageLog = default;
        if (!IsEmpty(((SaveGameEventLog)Log).FFileName))
        {
            try
            {
                GameTables.AoWEngine.SaveHSM(((SaveGameEventLog)Log).FFileName, null, GameProfile.WorldMap);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.GameSavedRStr), [((SaveGameEventLog)Log).FFileName]);
                AoWHSMap.Current.ShowMessage(format);
                Failed = false;
            }
            catch
            {
                Failed = true;
            }
        }
        else
        {
            Failed = true;
        }

        if (Failed)
        {
            MessageLog = new MessageEventLog(null);
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr) + ".", [((SaveGameEventLog)Log).FFileName]);
            MessageLog.FMessage.Text = format;
            AoWHSMap.Current.TriggerExecuteEventLog(MessageLog, false, TerminateEmailGameCallBack);
            MessageLog.Release();
        }
        else
        {
            MessageLog = new MessageEventLog(null);
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameSuccessfullySavedRStr) + ".", [((SaveGameEventLog)Log).FFileName]);
            MessageLog.FMessage.Text = format;
            AoWHSMap.Current.TriggerExecuteEventLog(MessageLog, false, TerminateEmailGameCallBack);
            MessageLog.Release();
        }
    }

    // PlayerC.PAS:938
    public void SendEmailGameCallBack(EventLog Log)
    {
        /* TODO: Implement Windows.GetTempPath in the platform or editor service. */
        throw new NotImplementedException();
    }

    // PlayerC.PAS:1018
    public void TriggerGameRestartedEvents(sbyte Player)
    {
        RestartGameEventLog Log;
        AoWHSMap.Current.SynchroniseBegin();
        Log = new RestartGameEventLog(null);
        Log.FPlayer = Player;
        var name = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameRestartEventLogXRStr), [name]);
        Log.FMessage.Text = format;
        EventLog.DistributeEventLog(Log, false);
        Log.Release();
        AoWHSMap.Current.SynchroniseEnd();
    }

    // PlayerC.PAS:1031
    public void EndTurnGameSavedCallBack(EventLog Log)
    {
        bool Failed = default;
        if (!IsEmpty(((SaveGameEventLog)Log).FFileName))
        {
            try
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
                AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FStatus = players.FPBEMSettings.FStatus + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.TurnDone) - Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.Playing);
                GameTables.AoWEngine.SaveHSM(((SaveGameEventLog)Log).FFileName, null, GameProfile.WorldMap);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.GameSavedRStr), [((SaveGameEventLog)Log).FFileName]);
                AoWHSMap.Current.ShowMessage(format);
                AoWHSMap.Current.SynchroniseBegin();
                base.EndTurn(AoWHSMap.Current.FCurrentPlayer);
                AoWHSMap.Current.SynchroniseEnd();
                Failed = false;
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr), [((SaveGameEventLog)Log).FFileName]);
                AoWHSMap.Current.ShowWarning(format);
                Failed = true;
            }
        }
        else
        {
            Failed = true;
        }

        if (Failed)
        {
            AoWHSMap.Current.TriggerEndTurnEvent(AoWHSMap.Current.FCurrentPlayer);
        }
    }

    // PlayerC.PAS:1094
    public void StartLoadedEmailGame()
    {
        AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FEmailGame.FLastDay = AoWHSMap.Current.FDay;
        var players = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
        GameTables.AoWEngine.FEmailGames.UpdateGame(players.FPBEMSettings.FEmailGame);
        var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
        AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FStatus = fPlayersPlayers.FPBEMSettings.FStatus + Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.Playing);
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FStatus.Contains(PBEMPlayerStatusElement.NewTurn))
        {
            AoWHSMap.Current.SynchroniseBegin();
            DefaultStartMap();
            AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(AoWHSMap.Current.FCurrentPlayer);
            NewTurn(AoWHSMap.Current.FCurrentPlayer);
            AoWHSMap.Current.SynchroniseEnd();
        }
        else
        {
            StartLoadedMapEx();
        }
    }

    // PlayerC.PAS:1111
    public void ContinueRestartedGameCallBack(EventLog Log)
    {
        if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Yes)
        {
            AoWHSMap.Current.SynchroniseBegin();
            TriggerGameRestartedEvents(AoWHSMap.Current.FCurrentPlayer);
            AoWHSMap.Current.SynchroniseEnd();
            StartLoadedEmailGame();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1123
    public void ContinueTurnDoneGameCallBack(EventLog Log)
    {
        if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Yes)
        {
            AoWHSMap.Current.SynchroniseBegin();
            DefaultStartMap();
            AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(AoWHSMap.Current.FCurrentPlayer);
            EndTurn(AoWHSMap.Current.FCurrentPlayer);
            AoWHSMap.Current.SynchroniseEnd();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
        }
    }

    // PlayerC.PAS:1136
    public void ValidateLoadedMap()
    {
        bool Restarted = default;
        MessageEventLog Log = default;
        FLocalPlayer = AoWHSMap.Current.FCurrentPlayer;
        var players = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
        if (GameTables.AoWEngine.FEmailGames.FindEmailGame(players.FPBEMSettings.FEmailGame) == null)
        {
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
            Restarted = GameTables.AoWEngine.FEmailGames.EmailGameRestarted(fPlayersPlayers.FPBEMSettings.FEmailGame);
            var players2 = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
            var players3 = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
            var emailGame = GameTables.AoWEngine.FEmailGames.CreateEmailGame(players3.FPBEMSettings.FEmailGame.FName, players2.FPBEMSettings.FEmailGame.FEmailAddress);
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.SetGame(emailGame);
            if (Restarted)
            {
                TriggerGameRestartedEvents(AoWHSMap.Current.FCurrentPlayer);
            }

            StartLoadedEmailGame();
        }
        else
        {
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer);
            if (GameTables.AoWEngine.FEmailGames.EmailGameRestarted(fPlayersPlayers.FPBEMSettings.FEmailGame))
            {
                SetSeatedPlayer(-1);
                Log = new MessageEventLog(null);
                Log.FButtons = MessageEventLog.Buttons.YesNo;
                Log.FTitle = AbilityRules.TranslateRStr(GameText.WarningRStr);
                var pbemRestartCheat = AbilityRules.TranslateRStr(PBEMRestartCheatRStr);
                Log.FMessage.Text = pbemRestartCheat;
                Log.FMessage.Add("");
                var aoWETranslateRStr = AbilityRules.TranslateRStr(DoYouWantToContinueRStr);
                Log.FMessage.Add(aoWETranslateRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ContinueRestartedGameCallBack);
                Log.Release();
            }
            else
            {
                SetSeatedPlayer(-1);
                if ((AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPBEMSettings.FStatus & Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.NewTurn, PBEMPlayerStatusElement.TurnDone)) == Set<PBEMPlayerStatusElement>.Of(PBEMPlayerStatusElement.TurnDone))
                {
                    Log = new MessageEventLog(null);
                    Log.FTitle = AbilityRules.TranslateRStr(GameText.WarningRStr);
                    Log.FButtons = MessageEventLog.Buttons.YesNo;
                    var pbemGameEnded = AbilityRules.TranslateRStr(PBEMGameEndedRStr);
                    Log.FMessage.Text = pbemGameEnded;
                    Log.FMessage.Add("");
                    var aoWETranslateRStr = AbilityRules.TranslateRStr(DoYouWantToContinueRStr);
                    Log.FMessage.Add(aoWETranslateRStr);
                    AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ContinueTurnDoneGameCallBack);
                    Log.Release();
                }
                else
                {
                    StartLoadedEmailGame();
                }
            }
        }
    }

    // PlayerC.PAS:1219
    public void ExitGameSaveAsCallBack(EventLog Log)
    {
        if (!IsEmpty(((SaveGameEventLog)Log).FFileName))
        {
            try
            {
                GameTables.AoWEngine.SaveHSM(((SaveGameEventLog)Log).FFileName, null, GameProfile.WorldMap);
                AoWHSMap.Current.ExecuteExitGame();
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr), [((SaveGameEventLog)Log).FFileName]);
                AoWHSMap.Current.ShowWarning(format);
            }
        }
    }

    private static readonly string DoYouWantToContinueRStr = "Do you want to continue?";
    private static readonly string GameRestartEventLogXRStr = "Game is restarted for unknown reason, contact %s for more information";
    private static readonly string GameSuccessfullySavedRStr = "Game successfully saved";
    private static readonly string PBEMGameEndedRStr = "This game's turn has been ended. Restarting is only allowed for resend purposes.";
    private static readonly string PBEMRestartCheatRStr = "This game isn't the original game. Restarting it qualifies you as a cheater!";
    private static readonly string SaveEmailGameRStr = "Save email game...";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
