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
    public new const int ClassId = 131664;

    public TurnPlayerControl(ECustomNode owner) : base(owner)
    { }

    // PlayerC.PAS:589
    public override void NewTurn(sbyte player)
    {
        NewTurnEventLog log = default;
        ShowBookEventLog bookLog = default;
        SpellList spells = default;
        PlayerMagicEventLog magicLog = default;
        AoWHSMap.Current.LockMapChangedEvent();
        AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(player);
        if (AoWHSMap.Current.RequestCombatType)
        {
            if (AoWHSMap.Current.SeatedPlayer != -1)
            {
                AoWHSMap.Current.SetSeatedPlayer(-1);
            }

            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null);
                log.TurnPlayer = AoWHSMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(s_itsYourTurnText);
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, ShowNewTurnCallback);
                log.Release();
            }
            else
            {
                log = new NewTurnEventLog(null);
                log.TurnPlayer = AoWHSMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (player == 0)
                {
                    log.Text = AbilityRules.TranslateResourceString(s_independentsAreMovingText);
                }
                else
                {
                    var name = AoWHSMap.Current.Players.GetPlayers(player).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_playerIsMovingFormat), [name]);
                }

                AoWHSMap.Current.TriggerExecuteEventLog(log, true, null);
                log.Release();
            }
        }
        else
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && player == AoWHSMap.Current.SeatedPlayer)
            {
                if (AoWHSMap.Current.GameType != GameType.Tutorial || AoWHSMap.Current.Day != 1)
                {
                    AoWHSMap.Current.ResetTurnTimer();
                    AoWHSMap.Current.EnableTurnTimer();
                    AoWHSMap.Current.ResetSeatedPlayerView();
                    log = new NewTurnEventLog(null);
                    log.TurnPlayer = player;
                    log.Mode = NewTurnEventLogMode.Dialog;
                    log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                    log.Text = AbilityRules.TranslateResourceString(s_itsYourTurnText);
                    AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
                    log.Release();
                    ReplayLogbook();
                }
                else
                {
                    AoWHSMap.Current.TutorialControl.ShowMessage(50, false, true);
                }
            }
        }

        base.NewTurn(player);
        if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && AoWHSMap.Current.ConnectionType != PlayerConnectionType.Email && GameTables.GameEngine.Registry.GetAutoSave())
        {
            AutoSave();
        }

        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
        if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.AIExecutor.ActivatePlayer(player);
            if (player == 0)
            {
                var independents = AbilityRules.TranslateResourceString(GameTables.s_independentsText);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_areMovingFormat), [independents]);
                AoWHSMap.Current.ShowMessage(format);
            }
            else
            {
                var players = AoWHSMap.Current.Players.GetPlayers(player);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[players.PlayerRace]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_areMovingFormat), [raceNamesPlural]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }
        else
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
            if (AoWHSMap.Current.Day == 1 && (AoWHSMap.Current.RequestCombatType || player == AoWHSMap.Current.SeatedPlayer))
            {
                if (Set<GameType>.Of(GameType.Custom, GameType.Tutorial).Contains(AoWHSMap.Current.GameType))
                {
                    bookLog = new ShowBookEventLog(null);
                    AoWHSMap.Current.TriggerExecuteEventLog(bookLog, false, null);
                    bookLog.Release();
                }

                if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                {
                    spells = new SpellList();
                    AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ListResearchSpells(ref spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    if (spells.GetCount() > 0)
                    {
                        magicLog = new PlayerMagicEventLog(null);
                        magicLog.Mode = PlayerMagicEventLog.EventMode.StartResearch;
                        AoWHSMap.Current.TriggerExecuteEventLog(magicLog, false, null);
                        magicLog.Release();
                    }

                    spells?.Dispose();
                }

                AoWHSMap.Current.TutorialControl.ShowMessage(52, false, true);
            }
        }
    }

    // PlayerC.PAS:875
    public override void ValidateTurnTiming()
    {
        if (AoWHSMap.Current.GetTurnTimerEnabled())
        {
            if (AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer)
            {
                if (AoWHSMap.Current.GetTurnTimeLeft() <= 0)
                {
                    if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None && !AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).SingleProcessCycle)
                    {
                        if (AoWHSMap.Current.TokenManager.PlayerReady(AoWHSMap.Current.SeatedPlayer))
                        {
                            AbilityRules.EndPlayerTurn(AoWHSMap.Current.SeatedPlayer);
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
        AoWHSMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
    }

    // PlayerC.PAS:766
    public override void StartNewMap()
    {
        base.StartNewMap();
        DefaultStartMap();
        NewDay();
        if (!AoWHSMap.Current.RequestCombatType)
        {
            var item = AoWHSMap.Current.Players.LocalHumanPlayers[0];
            AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
            AoWHSMap.Current.ResetSeatedPlayerView();
        }

        NewTurn((sbyte)AoWHSMap.Current.TurnOrder[1]);
    }

    // PlayerC.PAS:835
    public override void StartLoadedMap()
    {
        if (!IsEmpty(AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).Settings))
        {
            ValidatePassword(AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).Settings);
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:832
    public override void PasswordValidated()
    {
        ContinueLoadedMap();
    }

    // PlayerC.PAS:854
    public override void SetPlayerGameOverStatus(sbyte player, GameOverStatus status)
    {
        base.SetPlayerGameOverStatus(player, status);
        if (!AoWHSMap.Current.RequestCombatType || AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            AoWHSMap.Current.Players.Update();
            AoWHSMap.Current.TriggerPlayerTerminatedEvent(player);
            if (AoWHSMap.Current.CurrentPlayer == player && AoWHSMap.Current.Players.GetPlayers(player).GetLocal())
            {
                AbilityRules.EndPlayerTurn(AoWHSMap.Current.CurrentPlayer);
            }

            if (AoWHSMap.Current.Players.LocalHumanPlayers.Count == 0)
            {
                AoWHSMap.Current.TriggerGameTerminatedEvent();
            }
        }
    }

    // PlayerC.PAS:868
    public override void GameOver(sbyte player, GameOverStatus status)
    {
        AoWHSMap.Current.LockGameOverEvent();
        base.GameOver(player, status);
        AoWHSMap.Current.UnlockGameOverEvent();
    }

    // PlayerC.PAS:842
    public override void GameOverEventHandled(sbyte player)
    {
        if (AoWHSMap.Current.RequestCombatType)
        {
            AoWHSMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            AoWHSMap.Current.Players.Update();
            AoWHSMap.Current.TriggerPlayerTerminatedEvent(player);
            if (AoWHSMap.Current.Players.LocalHumanPlayers.Count == 0)
            {
                AoWHSMap.Current.TriggerGameTerminatedEvent();
            }
            else
            {
                AbilityRules.EndPlayerTurn(AoWHSMap.Current.CurrentPlayer);
            }
        }
    }

    // PlayerC.PAS:715
    public override void EndTurn(sbyte player)
    {
        EndTurnEventLog log;
        int index;
        bool found;
        sbyte nextPlayer;
        base.EndTurn(player);
        if (player == AoWHSMap.Current.SeatedPlayer)
        {
            AoWHSMap.Current.ZeroTurnTimer();
            if (AoWHSMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                AoWHSMap.Current.ClearExecuteEventLogBuffer();
            }

            AoWHSMap.Current.Selection.UnselectLocation();
        }

        if (player == AoWHSMap.Current.CurrentPlayer)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                AoWHSMap.Current.AIExecutor.DeactivatePlayer(player);
            }

            log = new EndTurnEventLog(null);
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).EventLogbook.AddEvent(log);
            log.Release();
            index = AoWHSMap.Current.TurnOrder.IndexOf((byte)AoWHSMap.Current.CurrentPlayer);
            found = false;
            while (!found)
            {
                index += 1;
                if (index == AoWHSMap.Current.TurnOrder.Count)
                {
                    NewDay();
                    index = 0;
                }

                nextPlayer = (sbyte)AoWHSMap.Current.TurnOrder[index];
                found = AoWHSMap.Current.Players.GetPlayers(nextPlayer).Status == PlayerStatus.Playing;
                if (!found)
                {
                    base.NewTurn(nextPlayer);
                    base.EndTurn(nextPlayer);
                }
            }

            if (AoWHSMap.Current.TurnOrder.Count != 0)
            {
                NewTurn((sbyte)AoWHSMap.Current.TurnOrder[index]);
            }
            else
            {
                NewTurn(0);
            }
        }
    }

    // PlayerC.PAS:693
    public override void SetSeatedPlayer(sbyte player)
    {
        AoWHSMap.Current.LockMapChangedEvent();
        base.SetSeatedPlayer(player);
        AoWHSMap.Current.UnlockMapChangedEvent();
        AoWHSMap.Current.InvalidateMap();
    }

    // PlayerC.PAS:779
    public virtual void ContinueLoadedMap()
    {
        NewTurnEventLog log = default;
        DefaultStartMap();
        AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(AoWHSMap.Current.CurrentPlayer);
        if (AoWHSMap.Current.RequestCombatType)
        {
            AoWHSMap.Current.SetSeatedPlayer(-1);
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null);
                log.TurnPlayer = AoWHSMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GameText.s_yourTurnContinuesText);
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
                log.Release();
            }
            else
            {
                log = new NewTurnEventLog(null);
                log.TurnPlayer = AoWHSMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (AoWHSMap.Current.CurrentPlayer == 0)
                {
                    log.Text = AbilityRules.TranslateResourceString(s_independentsAreMovingText);
                }
                else
                {
                    var name = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(s_playerIsMovingFormat), [name]);
                }

                AoWHSMap.Current.TriggerExecuteEventLog(log, true, null);
                log.Release();
            }
        }
        else
        {
            var item = AoWHSMap.Current.Players.LocalHumanPlayers[0];
            AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
            AoWHSMap.Current.ResetSeatedPlayerView();
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PlayerType == PlayerType.Human && AoWHSMap.Current.CurrentPlayer == AoWHSMap.Current.SeatedPlayer)
            {
                log = new NewTurnEventLog(null);
                log.TurnPlayer = AoWHSMap.Current.SeatedPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GameText.s_yourTurnContinuesText);
                AoWHSMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
                log.Release();
            }
        }

        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).PlayerType != PlayerType.Human)
        {
            AoWHSMap.Current.AIExecutor.ActivatePlayer(AoWHSMap.Current.CurrentPlayer);
        }

        GameTables.GameEngine.OutputDebugString("Random Seed: " + IntToStr(AoWHSMap.Current.DefendersSeed));
    }

    // PlayerC.PAS:683
    public void ReplayLogbook()
    {
        EventLogbook book;
        int index;
        book = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.CurrentPlayer).EventLogbook;
        index = book.FindFirstLog(131811, AoWHSMap.Current.CurrentPlayer, AoWHSMap.Current.Day - 1);
        for (int I = index + 1, last = book.GetCount() - 1; I <= last; I++)
        {
            book.GetLogs(I).Execute(false, ExecuteTrigger.NewTurn, null);
        }
    }

    // PlayerC.PAS:701
    public void ShowNewTurnCallback(EventLog log)
    {
        SetSeatedPlayer(AoWHSMap.Current.CurrentPlayer);
        AoWHSMap.Current.ResetTurnTimer();
        AoWHSMap.Current.EnableTurnTimer();
        if (AoWHSMap.Current.CurrentPlayer != -1)
        {
            ReplayLogbook();
        }
    }

    // PlayerC.PAS:709
    public void StartGameCallback(EventLog log)
    {
        if (AoWHSMap.Current.SeatedPlayer == -1)
        {
            SetSeatedPlayer(AoWHSMap.Current.CurrentPlayer);
        }

        AoWHSMap.Current.EnableTurnTimer();
    }

    // PlayerC.PAS:758
    public void DefaultStartMap()
    {
        AoWHSMap.Current.Players.Update();
        AoWHSMap.Current.RequestCombatType = AoWHSMap.Current.ConnectionType == PlayerConnectionType.Email || (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Local && AoWHSMap.Current.Players.LocalHumanPlayers.Count > 1);
        AoWHSMap.Current.ShowIndependentMoves = !AoWHSMap.Current.RequestCombatType;
    }

    private static readonly string s_itsYourTurnText = "It's your turn!";
    private static readonly string s_independentsAreMovingText = "Independents are moving";
    private static readonly string s_playerIsMovingFormat = "%s is moving";
    private static readonly string s_areMovingFormat = "%s are moving";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
