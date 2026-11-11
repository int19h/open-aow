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
        FDisconnectedPlayers = new DoubleItemList();
    }

    public ExitGameCallBack FExitCallBack;
    public ExitGameMode FExitMode;
    public string FPassword = "";
    public DoubleItemList FDisconnectedPlayers;
    public bool FOutOfSyncShown;

    // PlayerC.PAS:7
    public override void Dispose()
    {
        if (AoWHSMap.Current != null && AoWHSMap.Current.FTokenManager != null)
        {
            var tokenExecuter = AoWHSMap.Current.FTokenManager.TokenExecuter;
            tokenExecuter.OnGetSyncValue = null;
            tokenExecuter.OnSyncError = null;
        }

        FDisconnectedPlayers?.Dispose();
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
        EngineMessage Msg = default;
        TextEventLog Log = default;
        MessageEventLog MessageLog;
        AoWHSMap.Current.FDay += 1;
        AoWHSMap.Current.FPlayers.BeginUpdate();
        AoWHSMap.Current.LockRelationsChanged();
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            Log = new NewDayEventLog(null);
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(NewDayRStr), [AoWHSMap.Current.FDay]);
            if (AoWHSMap.Current.FDayLimit != 0 && AoWHSMap.Current.FDayLimit - AoWHSMap.Current.FDay <= 10)
            {
                var fmtDaysLeftStr = DurationText.FmtDaysLeftStr(AoWHSMap.Current.FDayLimit - AoWHSMap.Current.FDay);
                Log.FText = Log.FText + " (" + fmtDaysLeftStr + ")";
            }

            Log.FPlayer = (sbyte)I;
            AoWHSMap.Current.FPlayers.GetPlayers(I).FEventLogbook.AddEvent(Log);
            Log.Release();
        }

        if (AoWHSMap.Current.FTurnOrderMode == AoWHSMap.TurnOrderMode.Changing)
        {
            AoWHSMap.Current.GenerateNewTurnOrder();
        }

        if (AoWHSMap.Current.FDay == 1)
        {
            AoWHSMap.Current.FRaces.ResetPlayerRelations();
            if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local)
            {
                Randomize();
                AoWHSMap.Current.FInitialRandomSeed = RandSeed;
            }

            AoWHSMap.Current.FDefendersSeed = AoWHSMap.Current.FInitialRandomSeed;
        }

        try
        {
            EngineMessage.InitMsg(ref Msg, 537001985, this);
            AoWHSMap.Current.SendMsg(ref Msg, this);
        }
        catch
        {
        }

        try
        {
            AoWHSMap.Current.FNewDayEventsStore.Raise(this);
        }
        catch
        {
        }

        if (AoWHSMap.Current.FDayLimit == AoWHSMap.Current.FDay)
        {
            MessageLog = new MessageEventLog(null);
            MessageLog.FTitle = AbilityRules.TranslateRStr(GameText.InfoRStr);
            MessageLog.FButtons = MessageEventLog.Buttons.Ok;
            MessageLog.FLayout = MessageEventLog.Layout.Normal;
            var turnLimitReached = AbilityRules.TranslateRStr(TurnLimitReachedRStr);
            MessageLog.FMessage.Text = turnLimitReached;
            AoWHSMap.Current.TriggerExecuteEventLog(MessageLog, false, null);
            MessageLog.Release();
            AoWHSMap.Current.GameOver(AoWHSMap.Current.FSeatedPlayer, GameOverStatus.Lost);
            for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
            {
                AoWHSMap.Current.GameOver((sbyte)I, GameOverStatus.Lost);
            }
        }

        AoWHSMap.Current.UnlockRelationsChanged();
        AoWHSMap.Current.TriggerRelationsChanged();
        AoWHSMap.Current.FPlayers.EndUpdate();
    }

    // PlayerC.PAS:177
    public virtual void NewTurn(sbyte Player)
    {
        EngineMessage Msg = default;
        if (AoWHSMap.Current.FDay != 1)
        {
            var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            var upkeep = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetUpkeep();
            AoWHSMap.Current.FPlayers.GetPlayers(Player).SetGems(players.FGems + fPlayersPlayers.FIncome - upkeep);
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FGems < 0)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).SetGems(0);
                var players2 = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                players2.FMoraleValue = (sbyte)(players2.FMoraleValue - 5);
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMoraleValue < -50)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(Player).FMoraleValue = -50;
                }
            }
            else
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FMoraleValue = 0;
            }

            AoWHSMap.Current.FPlayers.GetPlayers(Player).SetEndTurn(false);
        }

        var players3 = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        players3.FDay += 1;
        AoWHSMap.Current.LockUpdateVisibility();
        EngineMessage.InitMsg(ref Msg, 537001986, this);
        Msg.Payload = Player;
        AoWHSMap.Current.SendMsg(ref Msg, this);
        AoWHSMap.Current.FNewTurnEventsStore.Raise(Player);
        EngineMessage.InitMsg(ref Msg, 537001987, this);
        Msg.Payload = Player;
        AoWHSMap.Current.SendMsg(ref Msg, this);
        AoWHSMap.Current.UnLockUpdateVisibility();
        if (AoWHSMap.Current.FDebugSave)
        {
            try
            {
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
                {
                    if (Player == 0)
                    {
                        GameTables.AoWEngine.SaveHSM(GameTables.AoWEngine.StartupDirectory + "DEBUG\\" + AoWHSMap.Current.FMapName + IntToStr(AoWHSMap.Current.FDay) + "_p" + IntToStr(AoWHSMap.Current.FSeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
                    }
                }
                else
                {
                    GameTables.AoWEngine.SaveHSM(GameTables.AoWEngine.StartupDirectory + "DEBUG\\" + AoWHSMap.Current.FMapName + IntToStr(AoWHSMap.Current.FDay) + "-" + IntToStr(Player) + "_p" + IntToStr(AoWHSMap.Current.FSeatedPlayer) + FileBoundary.WriteExtension(DocumentKind.Save), null, GameProfile.WorldMap);
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
        string FileName = "";
        if (AoWHSMap.Current.FGameType == GameType.Campaign)
        {
            try
            {
                // The autosave takes the extension the host writes (own-formats.md 4.2).
                FileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.CampaignSave);
                GameTables.AoWEngine.SaveCampaign(GameTables.AoWEngine.StartupDirectory + "Save\\" + FileName);
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr), [FileName]);
                AoWHSMap.Current.ShowWarning(format);
            }
        }
        else
        {
            try
            {
                FileName = "autosave" + FileBoundary.WriteExtension(DocumentKind.Save);
                GameTables.AoWEngine.SaveHSM(GameTables.AoWEngine.StartupDirectory + "Save\\" + FileName, null, GameProfile.WorldMap);
            }
            catch
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CouldNotSaveXRStr), [FileName]);
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
        LocalPlayerCommunicationControl PCC = default;
        if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            if (AoWHSMap.Current.FPlayerCommunicator.PCC == null)
            {
                throw new InvalidOperationException("Please setup DirectPlayPCC");
            }

            var tokenExecuter = AoWHSMap.Current.FTokenManager.TokenExecuter;
            tokenExecuter.OnGetSyncValue = this.GetSyncValue;
            tokenExecuter.OnSyncError = OutOfSync;
        }
        else
        {
            if ((AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || Set<PlayerConnectionType>.Of(PlayerConnectionType.Local, PlayerConnectionType.Email).Contains(AoWHSMap.Current.FConnectionType)) && AoWHSMap.Current.FPlayerCommunicator.PCC == null)
            {
                PCC = new LocalPlayerCommunicationControl();
                PCC.Setup(AoWHSMap.Current.FPlayers.Count);
                for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human)
                    {
                        PCC.SetConnectionMode(I, ConnectionMode.Local);
                    }
                    else
                    {
                        PCC.SetConnectionMode(I, ConnectionMode.Global);
                    }
                }

                AoWHSMap.Current.FPlayerCommunicator.ActivatePCC(PCC);
                PCC.Release();
                AoWHSMap.Current.FPlayers.Update();
            }
        }
    }

    // PlayerC.PAS:224
    public virtual void StartNewMap()
    {
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I == 0 || AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus = Player.Status.Playing;
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
    public virtual void SetPlayerGameOverStatus(sbyte Player, GameOverStatus Status)
    {
        AoWHSMap.Current.FPlayers.GetPlayers(Player).SetGameOverStatus(Status);
    }

    // PlayerC.PAS:241
    public virtual void GameOver(sbyte Player, GameOverStatus Status)
    {
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FGameOverStatus != Status && AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus != Session.Player.Status.Terminated)
        {
            AoWHSMap.Current.LockGameOverEvent();
            SetPlayerGameOverStatus(Player, Status);
            if (Status == GameOverStatus.Lost)
            {
                if (!AoWHSMap.Current.FAlliedVictory)
                {
                    if (AoWHSMap.Current.FPlayers.FActivePlayers.Count == 1)
                    {
                        GameOver((sbyte)AoWHSMap.Current.FPlayers.FActivePlayers[0], GameOverStatus.Won);
                    }
                }
                else
                {
                    if (AoWHSMap.Current.FPlayers.GetActivePlayersAllied())
                    {
                        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                        {
                            if (I != Player && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Session.Player.Status.Playing && AoWHSMap.Current.FPlayers.GetPlayers(I).FGameOverStatus != GameOverStatus.Lost)
                            {
                                SetPlayerGameOverStatus((sbyte)I, GameOverStatus.Won);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Status == GameOverStatus.Won)
                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        if (I != Player && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Session.Player.Status.Playing && AoWHSMap.Current.FPlayers.GetPlayers(I).FGameOverStatus != GameOverStatus.Lost)
                        {
                            if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, (sbyte)I) == DiplomaticRelation.Alliance && AoWHSMap.Current.FAlliedVictory)
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
    public virtual void GameOverEventHandled(sbyte Player)
    {
        AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus = Session.Player.Status.Terminated;
    }

    // PlayerC.PAS:388
    public virtual void ExitGame(ExitGameMode Mode, ExitGameCallBack Done)
    {
        MessageEventLog Log = default;
        if (AoWHSMap.Current.FCombat == null)
        {
            if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network && AoWHSMap.Current.FGameStatus != AoWHSMap.GameStatus.Terminated && AoWHSMap.Current.FGameType != GameType.Tutorial && AoWHSMap.Current.FPlayers.FLocalPlayers.Count > 1)
            {
                if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous || AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    FExitMode = Mode;
                    FExitCallBack = Done;
                    Log = new MessageEventLog(null);
                    Log.FTitle = AbilityRules.TranslateRStr(ConfirmRStr);
                    Log.FButtons = MessageEventLog.Buttons.YesNoCancel;
                    Log.FLayout = MessageEventLog.Layout.Normal;
                    var yourAboutToExitAMultiPlayerGame = AbilityRules.TranslateRStr(YourAboutToExitAMultiPlayerGameRStr);
                    Log.FMessage.Add(yourAboutToExitAMultiPlayerGame);
                    var aoWETranslateRStr = AbilityRules.TranslateRStr(DoYouWantToContinueExitingRStr);
                    Log.FMessage.Add(aoWETranslateRStr);
                    AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ExitNetworkGameCallBack);
                    Log.Release();
                }
                else
                {
                    Log = new MessageEventLog(null);
                    Log.FTitle = AbilityRules.TranslateRStr(GameText.MessageRStr);
                    Log.FButtons = MessageEventLog.Buttons.Ok;
                    Log.FLayout = MessageEventLog.Layout.Normal;
                    var pleaseWaitTillItIsYourTurn = AbilityRules.TranslateRStr(PleaseWaitTillItIsYourTurnRStr);
                    Log.FMessage.Add(pleaseWaitTillItIsYourTurn);
                    AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
                    Log.Release();
                }
            }
            else
            {
                if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Modified) && AoWHSMap.Current.FGameStatus != AoWHSMap.GameStatus.Terminated && AoWHSMap.Current.FGameType != GameType.Tutorial)
                {
                    if (!(FExitCallBack != null))
                    {
                        if (Mode != ExitGameMode.Reload)
                        {
                            FExitMode = Mode;
                            FExitCallBack = Done;
                            Log = new MessageEventLog(null);
                            Log.FTitle = AbilityRules.TranslateRStr(ConfirmRStr);
                            Log.FButtons = MessageEventLog.Buttons.YesNoCancel;
                            if (GameTables.AoWEngine.FCampaign == null && !IsEmpty(AoWHSMap.Current.FileName) && !DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
                            {
                                Log.FLayout = MessageEventLog.Layout.Normal;
                                // The message names the file the save writes (own-formats.md 4.2).
                                var fileName = ExtractFileName(FileBoundary.WriteName(AoWHSMap.Current.FileName));
                                var format = AbilityRules.Format(AbilityRules.TranslateRStr(SaveGameToXRStr), [fileName]);
                                Log.FMessage.Add(format);
                            }
                            else
                            {
                                if (GameTables.AoWEngine.FCampaign != null && !IsEmpty(GameTables.AoWEngine.FCampaign.FFileName) && !DocumentKind.CampaignMap.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName)))
                                {
                                    // { Original tests the map extension when saving a campaign. }
                                    Log.FLayout = MessageEventLog.Layout.Normal;
                                    var fileName = ExtractFileName(FileBoundary.WriteName(GameTables.AoWEngine.FCampaign.FFileName));
                                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(SaveGameToXRStr), [fileName]);
                                    Log.FMessage.Add(format);
                                }
                                else
                                {
                                    Log.FLayout = MessageEventLog.Layout.Normal;
                                    var doYouWantToSaveThisGame = AbilityRules.TranslateRStr(DoYouWantToSaveThisGameRStr);
                                    Log.FMessage.Add(doYouWantToSaveThisGame);
                                }
                            }

                            AoWHSMap.Current.TriggerExecuteEventLog(Log, false, SaveGameOnExitCallBack);
                            Log.Release();
                        }
                        else
                        {
                            Done(Mode, true);
                        }
                    }
                    else
                    {
                        FExitCallBack(FExitMode, false);
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
            if (Done != null)
            {
                Done(Mode, false);
            }
        }
    }

    // PlayerC.PAS:153
    public virtual void EndTurn(sbyte Player)
    {
        EngineMessage Msg = default;
        int X = default;
        int Y = default;
        int L = default;
        Player P;
        AoWHSMap.Current.LockRelationsChanged();
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            P = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            P.SetEndTurn(true);
            if (P != null)
            {
                AoWHSMap.Current.GetViewCenter(ref X, ref Y, ref L);
                P.FViewX = (byte)X;
                P.FViewY = (byte)Y;
                P.FViewL = (byte)L;
            }
        }

        EngineMessage.InitMsg(ref Msg, 537001991, this);
        Msg.Payload = Player;
        AoWHSMap.Current.SendMsg(ref Msg, this);
        AoWHSMap.Current.UnlockRelationsChanged();
        AoWHSMap.Current.TriggerRelationsChanged();
    }

    // PlayerC.PAS:279
    public virtual void SetSeatedPlayer(sbyte Player)
    {
        if (Player != AoWHSMap.Current.FSeatedPlayer)
        {
            AoWHSMap.Current.FSeatedPlayer = Player;
            AoWHSMap.Current.SeatedPlayerChanged();
            AoWHSMap.Current.ResetSeatedPlayerView();
        }
    }

    // PlayerC.PAS:518
    public virtual void ValidateDisconnectedPlayers()
    {
        int Player;
        int Count;
        PlayerConnectionEventLog Log;
        for (int I = FDisconnectedPlayers.Count - 1; I >= 0; I--)
        {
            Player = FDisconnectedPlayers[I].A;
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FGameOverStatus == GameOverStatus.None)
            {
                if (AoWHSMap.Current.FTokenManager.PlayerReady(AoWHSMap.Current.FCurrentPlayer) && AoWHSMap.Current.FTokenManager.TokenExecuter.ActiveTC.Count == 0)
                {
                    Count = FDisconnectedPlayers[I].B;
                    if (Count > 0)
                    {
                        Count -= 1;
                        FDisconnectedPlayers.List[I].B = Count;
                    }

                    if (Count == 0)
                    {
                        Log = new PlayerConnectionEventLog(null);
                        Log.Setup((sbyte)Player, 2);
                        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
                        Log.Release();
                        FDisconnectedPlayers.Delete(I);
                    }
                }
            }
            else
            {
                FDisconnectedPlayers.Delete(I);
            }
        }
    }

    // PlayerC.PAS:556
    public virtual void ConnectionLost()
    {
        PlayerConnectionEventLog Log;
        Log = new PlayerConnectionEventLog(null);
        Log.Setup(-1, 2);
        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
        Log.Release();
    }

    // PlayerC.PAS:289
    public virtual bool LockAI() => AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local && !AoWHSMap.Current.FRequestCombatType && AoWHSMap.Current.FAILockCount != 0;

    // PlayerC.PAS:49
    public void GetSyncValue(ref int Value)
    {
        Value = AoWHSMap.Current.FDefendersSeed;
    }

    // PlayerC.PAS:54
    public void OutOfSync(object Sender)
    {
        PlayerConnectionEventLog Log;
        if (AoWHSMap.Current.FTokenManager.TokenExecuter.ActiveTC.Active)
        {
            if (!FOutOfSyncShown)
            {
                Log = new PlayerConnectionEventLog(null);
                Log.Setup(-1, 3);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, AoWHSMap.Current.FCombat != null, null);
                Log.Release();
                FOutOfSyncShown = true;
            }
            else
            {
                AoWHSMap.Current.FTokenManager.TokenExecuter.Lock();
            }
        }
    }

    // PlayerC.PAS:295
    public void ListPlayerInfo(StringCollection Strings)
    {
        int Race;
        string Text;
        PlayerCommunicationControl PCC;
        PCC = AoWHSMap.Current.FPlayerCommunicator.PCC;
        for (int I = 0, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            Text = "Player " + IntToStr(I);
            Race = AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerRace;
            if (Race != -1)
            {
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Race]);
                Text = Text + "(" + raceNamesPlural + ")";
            }

            Text = Text + " at system " + IntToStr(PCC.Players[I].SystemID);
            if (Strings != null)
            {
                Strings.Add(Text);
            }

            GameTables.AoWEngine.OutputDebugStr(Text);
        }
    }

    // PlayerC.PAS:313
    public void ExitSaveGameCallBack(EventLog Log)
    {
        if (!IsEmpty(((SaveGameEventLog)Log).FFileName))
        {
            if (GameTables.AoWEngine.FCampaign != null)
            {
                GameTables.AoWEngine.SaveCampaign(((SaveGameEventLog)Log).FFileName);
            }
            else
            {
                GameTables.AoWEngine.SaveHSM(((SaveGameEventLog)Log).FFileName, null, GameProfile.WorldMap);
            }

            AoWHSMap.Current.ExecuteExitGame();
            if (FExitCallBack != null)
            {
                FExitCallBack(FExitMode, true);
            }
        }
        else
        {
            if (FExitCallBack != null)
            {
                FExitCallBack(FExitMode, false);
            }
        }

        FExitCallBack = null;
    }

    // PlayerC.PAS:326
    public void SaveGameOnExitCallBack(EventLog Log)
    {
        SaveGameEventLog SaveLog = default;
        if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Yes)
        {
            if (GameTables.AoWEngine.FCampaign == null && (IsEmpty(AoWHSMap.Current.FileName) || DocumentKind.Map.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName))) || GameTables.AoWEngine.FCampaign != null && (IsEmpty(GameTables.AoWEngine.FCampaign.FFileName) || DocumentKind.CampaignMap.HasExtension(ExtractFileExt(AoWHSMap.Current.FileName))))
            {
                SaveLog = new SaveGameEventLog(null);
                if (!IsEmpty(AoWHSMap.Current.FileName))
                {
                    SaveLog.FDefaultName = ExtractFileName(AoWHSMap.Current.FileName);
                    if (Pos(".", SaveLog.FDefaultName) != 0)
                    {
                        SaveLog.FDefaultName = RtlUnit.Copy(SaveLog.FDefaultName, 1, Pos(".", SaveLog.FDefaultName) - 1);
                    }
                }
                else
                {
                    SaveLog.FDefaultName = AoWHSMap.Current.FMapName;
                }

                SaveLog.FDirectory = GameTables.AoWEngine.StartupDirectory + "Save\\";
                AoWHSMap.Current.TriggerExecuteEventLog(SaveLog, false, ExitSaveGameCallBack);
                SaveLog.Release();
            }
            else
            {
                GameEngine aoWEngine = GameTables.AoWEngine;
                // The save writes the own sibling of an original's name, which stays untouched
                // (own-formats.md 4.2).
                if (aoWEngine.FCampaign != null)
                {
                    aoWEngine.SaveCampaign(FileBoundary.WriteName(aoWEngine.FCampaign.FFileName));
                }
                else
                {
                    aoWEngine.SaveHSM(FileBoundary.WriteName(AoWHSMap.Current.FileName), null, GameProfile.WorldMap);
                }

                AoWHSMap.Current.ExecuteExitGame();
                if (FExitCallBack != null)
                {
                    FExitCallBack(FExitMode, true);
                }

                FExitCallBack = null;
            }
        }
        else
        {
            if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.No)
            {
                AoWHSMap.Current.ExecuteExitGame();
                if (FExitCallBack != null)
                {
                    FExitCallBack(FExitMode, true);
                }

                FExitCallBack = null;
            }
            else
            {
                if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Cancel)
                {
                    if (FExitCallBack != null)
                    {
                        FExitCallBack(FExitMode, false);
                    }

                    FExitCallBack = null;
                }
            }
        }
    }

    // PlayerC.PAS:372
    public void ExitNetworkGameCallBack(EventLog Log)
    {
        TerminatePlayerTokenEvent TE;
        if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Yes)
        {
            if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous || AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                TE = new TerminatePlayerTokenEvent(null);
                TE.FPlayer = AoWHSMap.Current.FSeatedPlayer;
                TE.FExitCallBack = FExitCallBack;
                TE.FExitMode = FExitMode;
                AoWHSMap.Current.FTokenManager.GetTokenControl(AoWHSMap.Current.FSeatedPlayer).AddEvent(TE);
                TE.Release();
            }
        }
    }

    // PlayerC.PAS:471
    public void PasswordTryAgainCallBack(EventLog Log)
    {
        PasswordEventLog PasswordLog = default;
        if (((MessageEventLog)Log).FPressedButton == PressedEventLogButton.Yes)
        {
            PasswordLog = new PasswordEventLog(null);
            AoWHSMap.Current.TriggerExecuteEventLog(PasswordLog, false, ValidatePasswordCallBack);
            PasswordLog.Release();
        }
        else
        {
            PasswordFailed();
        }
    }

    // PlayerC.PAS:484
    public void ValidatePasswordCallBack(EventLog Log)
    {
        string Password = "";
        MessageEventLog MessageLog = default;
        Password = AbilityRules.Encode(((PasswordEventLog)Log).FPassword, 0);
        if (((PasswordEventLog)Log).FPressedButton != PressedEventLogButton.Ok || Password != FPassword)
        {
            MessageLog = new MessageEventLog(null);
            MessageLog.FTitle = AbilityRules.TranslateRStr(GameText.WarningRStr);
            MessageLog.FButtons = MessageEventLog.Buttons.YesNo;
            var passwordFailed = AbilityRules.TranslateRStr(PasswordFailedRStr);
            MessageLog.FMessage.Text = passwordFailed;
            MessageLog.FMessage.Add("");
            var aoWETranslateRStr = AbilityRules.TranslateRStr(DoYouWantToTryAgainRStr);
            MessageLog.FMessage.Add(aoWETranslateRStr);
            AoWHSMap.Current.TriggerExecuteEventLog(MessageLog, false, PasswordTryAgainCallBack);
            MessageLog.Release();
        }
        else
        {
            PasswordValidated();
        }
    }

    // PlayerC.PAS:504
    public void ValidatePassword(string Password)
    {
        PasswordEventLog Log = default;
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            SetSeatedPlayer(-1);
        }

        FPassword = Password;
        Log = new PasswordEventLog(null);
        AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ValidatePasswordCallBack);
        Log.Release();
    }

    // PlayerC.PAS:551
    public void PlayerDisconnected(sbyte Player)
    {
        if (FDisconnectedPlayers.IndexOfItemA(Player) == -1)
        {
            FDisconnectedPlayers.AddAB(Player, 150);
        }
    }

    private static readonly string NewDayRStr = "Day %d dawns";
    private static readonly string TurnLimitReachedRStr = "Sorry, turn limit reached!";
    internal static readonly string ConfirmRStr = "Confirm";
    private static readonly string YourAboutToExitAMultiPlayerGameRStr = "You're about to exit a multi-player game!";
    private static readonly string DoYouWantToContinueExitingRStr = "Do you want to continue exiting?";
    private static readonly string PleaseWaitTillItIsYourTurnRStr = "Please, wait till it is your turn.";
    private static readonly string SaveGameToXRStr = "Save game to %s?";
    private static readonly string DoYouWantToSaveThisGameRStr = "Do you want to save this game?";
    private static readonly string PasswordFailedRStr = "Password failed!";
    private static readonly string DoYouWantToTryAgainRStr = "Do you want to try again?";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
