// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

public partial class TurnPlayerControl : PlayerControl
{
    public new const int ClassID = 131664;

    public TurnPlayerControl(ECustomNode owner) : base(owner)
    { }

    // PlayerC.PAS:589
    public override void NewTurn(sbyte Player)
    {
        NewTurnEventLog Log = default;
        ShowBookEventLog BookLog = default;
        SpellList Spells = default;
        PlayerMagicEventLog MagicLog = default;
        AoWHSMap.Current.LockMapChangedEvent();
        AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(Player);
        if (AoWHSMap.Current.FRequestCombatType)
        {
            if (AoWHSMap.Current.FSeatedPlayer != -1)
            {
                AoWHSMap.Current.SetSeatedPlayer(-1);
            }

            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = AoWHSMap.Current.FCurrentPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                Log.FText = AbilityRules.TranslateRStr(ItsYourTurnRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, ShowNewTurnCallBack);
                Log.Release();
            }
            else
            {
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = AoWHSMap.Current.FCurrentPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (Player == 0)
                {
                    Log.FText = AbilityRules.TranslateRStr(IndependentsAreMovingRStr);
                }
                else
                {
                    var name = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
                    Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(PlayerIsMovingRStr), [name]);
                }

                AoWHSMap.Current.TriggerExecuteEventLog(Log, true, null);
                Log.Release();
            }
        }
        else
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human && Player == AoWHSMap.Current.FSeatedPlayer)
            {
                if (AoWHSMap.Current.FGameType != GameType.Tutorial || AoWHSMap.Current.FDay != 1)
                {
                    AoWHSMap.Current.ResetTurnTimer();
                    AoWHSMap.Current.EnableTurnTimer();
                    AoWHSMap.Current.ResetSeatedPlayerView();
                    Log = new NewTurnEventLog(null);
                    Log.FTurnPlayer = Player;
                    Log.FMode = NewTurnEventLog.Mode.Dialog;
                    Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                    Log.FText = AbilityRules.TranslateRStr(ItsYourTurnRStr);
                    AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
                    Log.Release();
                    ReplayLogbook();
                }
                else
                {
                    AoWHSMap.Current.FTutorialControl.ShowMessage(50, false, true);
                }
            }
        }

        base.NewTurn(Player);
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human && AoWHSMap.Current.FConnectionType != PlayerConnectionType.Email && GameTables.AoWEngine.FRegistry.GetAutoSave())
        {
            AutoSave();
        }

        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.FAIExecuter.ActivatePlayer(Player);
            if (Player == 0)
            {
                var independents = AbilityRules.TranslateRStr(GameTables.IndependentsRStr);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(AreMovingRStr), [independents]);
                AoWHSMap.Current.ShowMessage(format);
            }
            else
            {
                var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players.FPlayerRace]);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(AreMovingRStr), [raceNamesPlural]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }
        else
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
            if (AoWHSMap.Current.FDay == 1 && (AoWHSMap.Current.FRequestCombatType || Player == AoWHSMap.Current.FSeatedPlayer))
            {
                if (Set<GameType>.Of(GameType.Custom, GameType.Tutorial).Contains(AoWHSMap.Current.FGameType))
                {
                    BookLog = new ShowBookEventLog(null);
                    AoWHSMap.Current.TriggerExecuteEventLog(BookLog, false, null);
                    BookLog.Release();
                }

                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchSpellID == 0)
                {
                    Spells = new SpellList();
                    AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.ListResearchSpells(ref Spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    if (Spells.GetCount() > 0)
                    {
                        MagicLog = new PlayerMagicEventLog(null);
                        MagicLog.FMode = PlayerMagicEventLog.PlayerMagicEventMode.StartResearch;
                        AoWHSMap.Current.TriggerExecuteEventLog(MagicLog, false, null);
                        MagicLog.Release();
                    }

                    Spells?.Dispose();
                }

                AoWHSMap.Current.FTutorialControl.ShowMessage(52, false, true);
            }
        }
    }

    // PlayerC.PAS:875
    public override void ValidateTurnTiming()
    {
        if (AoWHSMap.Current.GetTurnTimerEnabled())
        {
            if (AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                if (AoWHSMap.Current.GetTurnTimeLeft() <= 0)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None && !AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FSingleProcessCycle)
                    {
                        if (AoWHSMap.Current.FTokenManager.PlayerReady(AoWHSMap.Current.FSeatedPlayer))
                        {
                            AbilityRules.EndPlayerTurn(AoWHSMap.Current.FSeatedPlayer);
                        }
                    }
                }
            }
        }
    }

    // PlayerC.PAS:569
    public override void Setup()
    {
        base.Setup();
        AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = true;
    }

    // PlayerC.PAS:766
    public override void StartNewMap()
    {
        base.StartNewMap();
        DefaultStartMap();
        NewDay();
        if (!AoWHSMap.Current.FRequestCombatType)
        {
            var item = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
            AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
            AoWHSMap.Current.ResetSeatedPlayerView();
        }

        NewTurn((sbyte)AoWHSMap.Current.FTurnOrder[1]);
    }

    // PlayerC.PAS:835
    public override void StartLoadedMap()
    {
        if (!IsEmpty(AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FSettings))
        {
            ValidatePassword(AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FSettings);
        }
        else
        {
            StartLoadedMapEx();
        }
    }

    // PlayerC.PAS:832
    public override void PasswordValidated()
    {
        StartLoadedMapEx();
    }

    // PlayerC.PAS:854
    public override void SetPlayerGameOverStatus(sbyte Player, GameOverStatus Status)
    {
        base.SetPlayerGameOverStatus(Player, Status);
        if (!AoWHSMap.Current.FRequestCombatType || AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus = Session.Player.Status.Terminated;
            AoWHSMap.Current.FPlayers.Update();
            AoWHSMap.Current.TriggerPlayerTerminatedEvent(Player);
            if (AoWHSMap.Current.FCurrentPlayer == Player && AoWHSMap.Current.FPlayers.GetPlayers(Player).GetLocal())
            {
                AbilityRules.EndPlayerTurn(AoWHSMap.Current.FCurrentPlayer);
            }

            if (AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count == 0)
            {
                AoWHSMap.Current.TriggerGameTerminatedEvent();
            }
        }
    }

    // PlayerC.PAS:868
    public override void GameOver(sbyte Player, GameOverStatus Status)
    {
        AoWHSMap.Current.LockGameOverEvent();
        base.GameOver(Player, Status);
        AoWHSMap.Current.UnlockGameOverEvent();
    }

    // PlayerC.PAS:842
    public override void GameOverEventHandled(sbyte Player)
    {
        if (AoWHSMap.Current.FRequestCombatType)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus = Session.Player.Status.Terminated;
            AoWHSMap.Current.FPlayers.Update();
            AoWHSMap.Current.TriggerPlayerTerminatedEvent(Player);
            if (AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count == 0)
            {
                AoWHSMap.Current.TriggerGameTerminatedEvent();
            }
            else
            {
                AbilityRules.EndPlayerTurn(AoWHSMap.Current.FCurrentPlayer);
            }
        }
    }

    // PlayerC.PAS:715
    public override void EndTurn(sbyte Player)
    {
        EndTurnEventLog Log;
        int Index;
        bool Found;
        sbyte NextPlayer;
        base.EndTurn(Player);
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            AoWHSMap.Current.ZeroTurnTimer();
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FStatus == Session.Player.Status.Playing)
            {
                AoWHSMap.Current.ClearExecuteEventLogBuffer();
            }

            AoWHSMap.Current.Selection.UnselectLocation();
        }

        if (Player == AoWHSMap.Current.FCurrentPlayer)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
            {
                AoWHSMap.Current.FAIExecuter.DeactivatePlayer(Player);
            }

            Log = new EndTurnEventLog(null);
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
            Index = AoWHSMap.Current.FTurnOrder.IndexOf((byte)AoWHSMap.Current.FCurrentPlayer);
            Found = false;
            while (!Found)
            {
                Index += 1;
                if (Index == AoWHSMap.Current.FTurnOrder.Count)
                {
                    NewDay();
                    Index = 0;
                }

                NextPlayer = (sbyte)AoWHSMap.Current.FTurnOrder[Index];
                Found = AoWHSMap.Current.FPlayers.GetPlayers(NextPlayer).FStatus == Session.Player.Status.Playing;
                if (!Found)
                {
                    base.NewTurn(NextPlayer);
                    base.EndTurn(NextPlayer);
                }
            }

            if (AoWHSMap.Current.FTurnOrder.Count != 0)
            {
                NewTurn((sbyte)AoWHSMap.Current.FTurnOrder[Index]);
            }
            else
            {
                NewTurn(0);
            }
        }
    }

    // PlayerC.PAS:693
    public override void SetSeatedPlayer(sbyte Player)
    {
        AoWHSMap.Current.LockMapChangedEvent();
        base.SetSeatedPlayer(Player);
        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

    // PlayerC.PAS:779
    public virtual void StartLoadedMapEx()
    {
        NewTurnEventLog Log = default;
        DefaultStartMap();
        AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(AoWHSMap.Current.FCurrentPlayer);
        if (AoWHSMap.Current.FRequestCombatType)
        {
            AoWHSMap.Current.SetSeatedPlayer(-1);
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPlayerType == PlayerType.Human)
            {
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = AoWHSMap.Current.FCurrentPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                Log.FText = AbilityRules.TranslateRStr(GameText.YourTurnContinuesRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, StartGameCallBack);
                Log.Release();
            }
            else
            {
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = AoWHSMap.Current.FCurrentPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (AoWHSMap.Current.FCurrentPlayer == 0)
                {
                    Log.FText = AbilityRules.TranslateRStr(IndependentsAreMovingRStr);
                }
                else
                {
                    var name = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).GetName();
                    Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(PlayerIsMovingRStr), [name]);
                }

                AoWHSMap.Current.TriggerExecuteEventLog(Log, true, null);
                Log.Release();
            }
        }
        else
        {
            var item = AoWHSMap.Current.FPlayers.FLocalHumanPlayers[0];
            AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
            AoWHSMap.Current.ResetSeatedPlayerView();
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPlayerType == PlayerType.Human && AoWHSMap.Current.FCurrentPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                Log = new NewTurnEventLog(null);
                Log.FTurnPlayer = AoWHSMap.Current.FSeatedPlayer;
                Log.FMode = NewTurnEventLog.Mode.Dialog;
                Log.FDialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                Log.FText = AbilityRules.TranslateRStr(GameText.YourTurnContinuesRStr);
                AoWHSMap.Current.TriggerExecuteEventLog(Log, false, StartGameCallBack);
                Log.Release();
            }
        }

        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FPlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.FAIExecuter.ActivatePlayer(AoWHSMap.Current.FCurrentPlayer);
        }

        GameTables.AoWEngine.OutputDebugStr("Random Seed: " + IntToStr(AoWHSMap.Current.FDefendersSeed));
    }

    // PlayerC.PAS:683
    public void ReplayLogbook()
    {
        EventLogbook Book;
        int Index;
        Book = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FCurrentPlayer).FEventLogbook;
        Index = Book.FindFirstLogEx(131811, AoWHSMap.Current.FCurrentPlayer, AoWHSMap.Current.FDay - 1);
        for (int I = Index + 1, last = Book.GetCount() - 1; I <= last; I++)
        {
            Book.GetLogs(I).Execute(false, ExecuteTrigger.NewTurn, null);
        }
    }

    // PlayerC.PAS:701
    public void ShowNewTurnCallBack(EventLog Log)
    {
        SetSeatedPlayer(AoWHSMap.Current.FCurrentPlayer);
        AoWHSMap.Current.ResetTurnTimer();
        AoWHSMap.Current.EnableTurnTimer();
        if (AoWHSMap.Current.FCurrentPlayer != -1)
        {
            ReplayLogbook();
        }
    }

    // PlayerC.PAS:709
    public void StartGameCallBack(EventLog Log)
    {
        if (AoWHSMap.Current.FSeatedPlayer == -1)
        {
            SetSeatedPlayer(AoWHSMap.Current.FCurrentPlayer);
        }

        AoWHSMap.Current.EnableTurnTimer();
    }

    // PlayerC.PAS:758
    public void DefaultStartMap()
    {
        AoWHSMap.Current.FPlayers.Update();
        AoWHSMap.Current.FRequestCombatType = AoWHSMap.Current.FConnectionType == PlayerConnectionType.Email || (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Local && AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count > 1);
        AoWHSMap.Current.FShowIndependentMoves = !AoWHSMap.Current.FRequestCombatType;
    }

    private static readonly string ItsYourTurnRStr = "It's your turn!";
    private static readonly string IndependentsAreMovingRStr = "Independents are moving";
    private static readonly string PlayerIsMovingRStr = "%s is moving";
    private static readonly string AreMovingRStr = "%s are moving";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
