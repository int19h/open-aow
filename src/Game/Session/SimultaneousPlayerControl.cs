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

public partial class SimultaneousPlayerControl : PlayerControl
{
    public new const int ClassId = 131665;

    public SimultaneousPlayerControl(ECustomNode owner) : base(owner)
    { }

    // PlayerC.PAS:1343
    public override void NewDay()
    {
        StrategicMap.Current.Selection.UnselectLocation();
        base.NewDay();
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            NewTurn((sbyte)I);
        }

        AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
        if (GameTables.GameEngine.Registry.GetAutoSave())
        {
            AutoSave();
        }
    }

    // PlayerC.PAS:1258
    public override void NewTurn(sbyte player)
    {
        EventLog log;
        SpellList spells = default;
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (StrategicMap.Current.Day == 1)
                {
                    if (StrategicMap.Current.GameType == GameType.Custom)
                    {
                        log = new ShowBookEventLog(null);
                        StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
                        log.Release();
                    }

                    if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                    {
                        spells = new SpellList();
                        StrategicMap.Current.Players.GetPlayers(player).MagicControl.ListResearchSpells(ref spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                        if (spells.GetCount() > 0)
                        {
                            log = new PlayerMagicEventLog(null);
                            ((PlayerMagicEventLog)log).Mode = PlayerMagicEventLog.EventMode.StartResearch;
                            StrategicMap.Current.TriggerExecuteEventLog(log, false, null);
                            log.Release();
                        }

                        spells?.Dispose();
                    }
                }
                else
                {
                    StrategicMap.Current.ResetTurnTimer();
                    StrategicMap.Current.EnableTurnTimer();
                }
            }
        }

        base.NewTurn(player);
        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                StrategicMap.Current.AIExecutor.ActivatePlayer(player);
            }
        }
    }

    // PlayerC.PAS:1663
    public override void ValidateTurnTiming()
    {
        if (StrategicMap.Current.GetTurnTimerEnabled() && StrategicMap.Current.GetTurnTimeLeft() <= 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None && !StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).SingleProcessCycle)
            {
                if (StrategicMap.Current.TokenManager.PlayerReady(StrategicMap.Current.SeatedPlayer))
                {
                    AbilityRules.EndPlayerTurn(StrategicMap.Current.SeatedPlayer);
                }
            }
        }
    }

    // PlayerC.PAS:1299
    public override void Setup()
    {
        base.Setup();
        StrategicMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = false;
    }

    // PlayerC.PAS:1364
    public override void StartNewMap()
    {
        NewTurnEventLog log = default;
        base.StartNewMap();
        StrategicMap.Current.ShowIndependentMoves = false;
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I == 0 || StrategicMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                StrategicMap.Current.Players.GetPlayers(I).Status = PlayerStatus.Playing;
            }
        }

        StrategicMap.Current.Players.Update();
        StrategicMap.Current.CurrentPlayer = 0;
        var item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        StrategicMap.Current.SetSeatedPlayer((sbyte)item);
        StrategicMap.Current.NotifySeatedPlayerChanged();
        Assert(StrategicMap.Current.SeatedPlayer != 0, "", "PlayerC.PAS", 1584);
        StrategicMap.Current.ResetSeatedPlayerView();
        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
        log = new NewTurnEventLog(null);
        log.TurnPlayer = StrategicMap.Current.SeatedPlayer;
        log.Mode = NewTurnEventLogMode.Dialog;
        log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        log.Text = AbilityRules.TranslateResourceString(s_letGameBeginText);
        StrategicMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
        log.Release();
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network)
        {
            StrategicMap.Current.ResetTurnTimer();
            StrategicMap.Current.EnableTurnTimer();
        }
        else
        {
            StrategicMap.Current.AIExecutor.Lock();
        }

        NewDay();
    }

    // PlayerC.PAS:1639
    public override void StartLoadedMap()
    {
        var item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(item).Settings))
        {
            var fLocalHumanPlayersItem = StrategicMap.Current.Players.LocalHumanPlayers[0];
            ValidatePassword(StrategicMap.Current.Players.GetPlayers(fLocalHumanPlayersItem).Settings);
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:1636
    public override void PasswordValidated()
    {
        ContinueLoadedMap();
    }

    // PlayerC.PAS:1652
    public override void SetPlayerGameOverStatus(sbyte player, GameOverStatus status)
    {
        base.SetPlayerGameOverStatus(player, status);
        StrategicMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
        StrategicMap.Current.TokenManager.TokenExecutor.GetTacticalBattle(player).Active = false;
        StrategicMap.Current.Players.Update();
        StrategicMap.Current.TriggerPlayerTerminatedEvent(player);
        if (StrategicMap.Current.Players.LocalHumanPlayers.Count == 0)
        {
            StrategicMap.Current.TriggerGameTerminatedEvent();
        }
        else
        {
            EndTurn(player);
        }
    }

    // PlayerC.PAS:1646
    public override void GameOverEventHandled(sbyte player)
    {
        StrategicMap.Current.SetSeatedPlayer(-1);
        StrategicMap.Current.TriggerGameTerminatedEvent();
    }

    // PlayerC.PAS:1305
    public override void EndTurn(sbyte player)
    {
        int I;
        bool allDone;
        EndTurnEventLog log;
        EngineMessage message = default;
        StrategicMap.Current.Players.GetPlayers(player).SetEndTurn(true);
        allDone = true;
        I = 1;
        while (allDone && I != StrategicMap.Current.Players.Count)
        {
            allDone = StrategicMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Playing || StrategicMap.Current.Players.GetPlayers(I).SingleProcessCycle;
            I += 1;
        }

        if (allDone)
        {
            if (StrategicMap.Current.SeatedPlayer == player)
            {
                StrategicMap.Current.ClearExecuteEventLogBuffer();
            }

            StrategicMap.Current.LockRelationsChanged();
            I = 0;
            int last = StrategicMap.Current.Players.Count - 1;
            for (; I <= last; I += 1)
            {
                if (StrategicMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    if (StrategicMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Human)
                    {
                        StrategicMap.Current.AIExecutor.DeactivatePlayer((sbyte)I);
                    }

                    log = new EndTurnEventLog(null);
                    StrategicMap.Current.Players.GetPlayers(I).EventLogbook.AddEvent(log);
                    log.Release();
                }

                EngineMessage.InitializeMessage(ref message, 537001991, this);
                message.Payload = I;
                StrategicMap.Current.SendMessage(ref message, this);
            }

            StrategicMap.Current.UnlockRelationsChanged();
            NewDay();
        }
    }

    // PlayerC.PAS:1354
    public void StartGameCallback(EventLog log)
    {
        if (StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
        {
            StrategicMap.Current.AIExecutor.Unlock();
            StrategicMap.Current.ResetTurnTimer();
            StrategicMap.Current.EnableTurnTimer();
        }
    }

    // PlayerC.PAS:1603
    public void ContinueLoadedMap()
    {
        NewTurnEventLog log = default;
        Player player = default;
        StrategicMap.Current.ShowIndependentMoves = false;
        var item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        StrategicMap.Current.SetSeatedPlayer((sbyte)item);
        StrategicMap.Current.NotifySeatedPlayerChanged();
        StrategicMap.Current.ResetSeatedPlayerView();
        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            player = StrategicMap.Current.Players.GetPlayers(I);
            if (player.Status == PlayerStatus.Playing && player.PlayerType != PlayerType.Human)
            {
                StrategicMap.Current.AIExecutor.ActivatePlayer((sbyte)I);
            }
        }

        for (int I = 0, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (StrategicMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Playing)
            {
                StrategicMap.Current.TokenManager.TokenControls[I].Active = false;
            }
        }

        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
        log = new NewTurnEventLog(null);
        log.TurnPlayer = StrategicMap.Current.SeatedPlayer;
        log.Mode = NewTurnEventLogMode.Dialog;
        log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        log.Text = AbilityRules.TranslateResourceString(s_gameContinuesText);
        StrategicMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
        log.Release();
        if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network)
        {
            StrategicMap.Current.ResetTurnTimer();
            StrategicMap.Current.EnableTurnTimer();
        }
        else
        {
            StrategicMap.Current.AIExecutor.Lock();
        }
    }

    private static readonly string s_letGameBeginText = "Let the game begin";
    private static readonly string s_gameContinuesText = "The game continues";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
