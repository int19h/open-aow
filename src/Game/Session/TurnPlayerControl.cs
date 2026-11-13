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
        StrategicMap.Current.LockMapChangedEvent();
        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(player);
        if (StrategicMap.Current.RequestCombatType)
        {
            if (StrategicMap.Current.SeatedPlayer != -1)
            {
                StrategicMap.Current.SetSeatedPlayer(-1);
            }

            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = StrategicMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_itsYourTurnText);
                StrategicMap.Current.TriggerExecuteEventLog(log, false, ShowNewTurnCallback);
                log.Release();
            }
            else
            {
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = StrategicMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (player == 0)
                {
                    log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_independentsAreMovingText);
                }
                else
                {
                    var name = StrategicMap.Current.Players.GetPlayers(player).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_playerIsMovingFormat), [name]);
                }

                StrategicMap.Current.TriggerExecuteEventLog(log, true, null);
                log.Release();
            }
        }
        else
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && player == StrategicMap.Current.SeatedPlayer)
            {
                if (StrategicMap.Current.GameType != GameType.Tutorial || StrategicMap.Current.Day != 1)
                {
                    StrategicMap.Current.ResetTurnTimer();
                    StrategicMap.Current.EnableTurnTimer();
                    StrategicMap.Current.ResetSeatedPlayerView();
                    log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                    log.TurnPlayer = player;
                    log.Mode = NewTurnEventLogMode.Dialog;
                    log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                    log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_itsYourTurnText);
                    StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
                    log.Release();
                    ReplayLogbook();
                }
                else
                {
                    StrategicMap.Current.TutorialControl.ShowMessage(50, false, true);
                }
            }
        }

        base.NewTurn(player);
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human && StrategicMap.Current.ConnectionType != PlayerConnectionType.Email && GetEngine().Registry.GetAutoSave())
        {
            AutoSave();
        }

        StrategicMap.Current.UnlockMapChangedEvent();
        StrategicMap.Current.InvalidateMap();
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            StrategicMap.Current.AIExecutor.ActivatePlayer(player);
            if (player == 0)
            {
                var independents = AbilityRules.TranslateResourceString(GetEngine(), GameTables.IndependentsText);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_areMovingFormat), [independents]);
                StrategicMap.Current.ShowMessage(format);
            }
            else
            {
                var players = StrategicMap.Current.Players.GetPlayers(player);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_areMovingFormat), [raceNamesPlural]);
                StrategicMap.Current.ShowMessage(format);
            }
        }
        else
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
            if (StrategicMap.Current.Day == 1 && (StrategicMap.Current.RequestCombatType || player == StrategicMap.Current.SeatedPlayer))
            {
                if (Set<GameType>.Of(GameType.Custom, GameType.Tutorial).Contains(StrategicMap.Current.GameType))
                {
                    bookLog = new ShowBookEventLog(null) { Holder = CreationHolder() };
                    StrategicMap.Current.TriggerExecuteEventLog(bookLog, false, null);
                    bookLog.Release();
                }

                if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                {
                    spells = new SpellList();
                    StrategicMap.Current.Players.GetPlayers(player).MagicControl.ListResearchSpells(ref spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                    if (spells.GetCount() > 0)
                    {
                        magicLog = new PlayerMagicEventLog(null) { Holder = CreationHolder() };
                        magicLog.Mode = PlayerMagicEventLog.EventMode.StartResearch;
                        StrategicMap.Current.TriggerExecuteEventLog(magicLog, false, null);
                        magicLog.Release();
                    }

                    spells?.Dispose();
                }

                StrategicMap.Current.TutorialControl.ShowMessage(52, false, true);
            }
        }
    }

    // PlayerC.PAS:875
    public override void ValidateTurnTiming()
    {
        if (StrategicMap.Current.GetTurnTimerEnabled())
        {
            if (StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
            {
                if (StrategicMap.Current.GetTurnTimeLeft() <= 0)
                {
                    if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None && !StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).SingleProcessCycle)
                    {
                        if (StrategicMap.Current.TokenManager.PlayerReady(StrategicMap.Current.SeatedPlayer))
                        {
                            AbilityRules.EndPlayerTurn(StrategicMap.Current.SeatedPlayer, engine: GetEngine());
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
        StrategicMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = true;
    }

    // PlayerC.PAS:766
    public override void StartNewMap()
    {
        base.StartNewMap();
        DefaultStartMap();
        NewDay();
        if (!StrategicMap.Current.RequestCombatType)
        {
            var item = StrategicMap.Current.Players.LocalHumanPlayers[0];
            StrategicMap.Current.SetSeatedPlayer((sbyte)item);
            StrategicMap.Current.ResetSeatedPlayerView();
        }

        NewTurn((sbyte)StrategicMap.Current.TurnOrder[1]);
    }

    // PlayerC.PAS:835
    public override void StartLoadedMap()
    {
        if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).Settings))
        {
            ValidatePassword(StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).Settings);
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
        if (!StrategicMap.Current.RequestCombatType || StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            StrategicMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            StrategicMap.Current.Players.Update();
            StrategicMap.Current.TriggerPlayerTerminatedEvent(player);
            if (StrategicMap.Current.CurrentPlayer == player && StrategicMap.Current.Players.GetPlayers(player).GetLocal())
            {
                AbilityRules.EndPlayerTurn(StrategicMap.Current.CurrentPlayer, engine: GetEngine());
            }

            if (StrategicMap.Current.Players.LocalHumanPlayers.Count == 0)
            {
                StrategicMap.Current.TriggerGameTerminatedEvent();
            }
        }
    }

    // PlayerC.PAS:868
    public override void GameOver(sbyte player, GameOverStatus status)
    {
        StrategicMap.Current.LockGameOverEvent();
        base.GameOver(player, status);
        StrategicMap.Current.UnlockGameOverEvent();
    }

    // PlayerC.PAS:842
    public override void GameOverEventHandled(sbyte player)
    {
        if (StrategicMap.Current.RequestCombatType)
        {
            StrategicMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            StrategicMap.Current.Players.Update();
            StrategicMap.Current.TriggerPlayerTerminatedEvent(player);
            if (StrategicMap.Current.Players.LocalHumanPlayers.Count == 0)
            {
                StrategicMap.Current.TriggerGameTerminatedEvent();
            }
            else
            {
                AbilityRules.EndPlayerTurn(StrategicMap.Current.CurrentPlayer, engine: GetEngine());
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
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            StrategicMap.Current.ZeroTurnTimer();
            if (StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                StrategicMap.Current.ClearExecuteEventLogBuffer();
            }

            StrategicMap.Current.Selection.UnselectLocation();
        }

        if (player == StrategicMap.Current.CurrentPlayer)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                StrategicMap.Current.AIExecutor.DeactivatePlayer(player);
            }

            log = new EndTurnEventLog(null) { Holder = CreationHolder() };
            StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).EventLogbook.AddEvent(log);
            log.Release();
            index = StrategicMap.Current.TurnOrder.IndexOf((byte)StrategicMap.Current.CurrentPlayer);
            found = false;
            while (!found)
            {
                index += 1;
                if (index == StrategicMap.Current.TurnOrder.Count)
                {
                    NewDay();
                    index = 0;
                }

                nextPlayer = (sbyte)StrategicMap.Current.TurnOrder[index];
                found = StrategicMap.Current.Players.GetPlayers(nextPlayer).Status == PlayerStatus.Playing;
                if (!found)
                {
                    base.NewTurn(nextPlayer);
                    base.EndTurn(nextPlayer);
                }
            }

            if (StrategicMap.Current.TurnOrder.Count != 0)
            {
                NewTurn((sbyte)StrategicMap.Current.TurnOrder[index]);
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
        StrategicMap.Current.LockMapChangedEvent();
        base.SetSeatedPlayer(player);
        StrategicMap.Current.UnlockMapChangedEvent();
        StrategicMap.Current.InvalidateMap();
    }

    // PlayerC.PAS:779
    public virtual void ContinueLoadedMap()
    {
        NewTurnEventLog log = default;
        DefaultStartMap();
        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
        if (StrategicMap.Current.RequestCombatType)
        {
            StrategicMap.Current.SetSeatedPlayer(-1);
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = StrategicMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GetEngine(), GameText.YourTurnContinuesText);
                StrategicMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
                log.Release();
            }
            else
            {
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = StrategicMap.Current.CurrentPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of();
                if (StrategicMap.Current.CurrentPlayer == 0)
                {
                    log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_independentsAreMovingText);
                }
                else
                {
                    var name = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_playerIsMovingFormat), [name]);
                }

                StrategicMap.Current.TriggerExecuteEventLog(log, true, null);
                log.Release();
            }
        }
        else
        {
            var item = StrategicMap.Current.Players.LocalHumanPlayers[0];
            StrategicMap.Current.SetSeatedPlayer((sbyte)item);
            StrategicMap.Current.ResetSeatedPlayerView();
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PlayerType == PlayerType.Human && StrategicMap.Current.CurrentPlayer == StrategicMap.Current.SeatedPlayer)
            {
                log = new NewTurnEventLog(null) { Holder = CreationHolder() };
                log.TurnPlayer = StrategicMap.Current.SeatedPlayer;
                log.Mode = NewTurnEventLogMode.Dialog;
                log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
                log.Text = AbilityRules.TranslateResourceString(GetEngine(), GameText.YourTurnContinuesText);
                StrategicMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
                log.Release();
            }
        }

        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).PlayerType != PlayerType.Human)
        {
            StrategicMap.Current.AIExecutor.ActivatePlayer(StrategicMap.Current.CurrentPlayer);
        }

        GetEngine().OutputDebugString("Random Seed: " + IntToStr(StrategicMap.Current.DefendersSeed));
    }

    // PlayerC.PAS:683
    public void ReplayLogbook()
    {
        EventLogbook book;
        int index;
        book = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.CurrentPlayer).EventLogbook;
        index = book.FindFirstLog(131811, StrategicMap.Current.CurrentPlayer, StrategicMap.Current.Day - 1);
        for (int I = index + 1, last = book.GetCount() - 1; I <= last; I++)
        {
            book.GetLogs(I).Execute(false, ExecuteTrigger.NewTurn, null);
        }
    }

    // PlayerC.PAS:701
    public void ShowNewTurnCallback(EventLog log)
    {
        SetSeatedPlayer(StrategicMap.Current.CurrentPlayer);
        StrategicMap.Current.ResetTurnTimer();
        StrategicMap.Current.EnableTurnTimer();
        if (StrategicMap.Current.CurrentPlayer != -1)
        {
            ReplayLogbook();
        }
    }

    // PlayerC.PAS:709
    public void StartGameCallback(EventLog log)
    {
        if (StrategicMap.Current.SeatedPlayer == -1)
        {
            SetSeatedPlayer(StrategicMap.Current.CurrentPlayer);
        }

        StrategicMap.Current.EnableTurnTimer();
    }

    // PlayerC.PAS:758
    public void DefaultStartMap()
    {
        StrategicMap.Current.Players.Update();
        StrategicMap.Current.RequestCombatType = StrategicMap.Current.ConnectionType == PlayerConnectionType.Email || (StrategicMap.Current.ConnectionType == PlayerConnectionType.Local && StrategicMap.Current.Players.LocalHumanPlayers.Count > 1);
        StrategicMap.Current.ShowIndependentMoves = !StrategicMap.Current.RequestCombatType;
    }

    private static readonly string s_itsYourTurnText = "It's your turn!";
    private static readonly string s_independentsAreMovingText = "Independents are moving";
    private static readonly string s_playerIsMovingFormat = "%s is moving";
    private static readonly string s_areMovingFormat = "%s are moving";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
