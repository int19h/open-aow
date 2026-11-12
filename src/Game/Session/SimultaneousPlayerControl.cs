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
        AoWHSMap.Current.Selection.UnselectLocation();
        base.NewDay();
        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
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
        if (player == AoWHSMap.Current.SeatedPlayer)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (AoWHSMap.Current.Day == 1)
                {
                    if (AoWHSMap.Current.GameType == GameType.Custom)
                    {
                        log = new ShowBookEventLog(null);
                        AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
                        log.Release();
                    }

                    if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                    {
                        spells = new SpellList();
                        AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ListResearchSpells(ref spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                        if (spells.GetCount() > 0)
                        {
                            log = new PlayerMagicEventLog(null);
                            ((PlayerMagicEventLog)log).Mode = PlayerMagicEventLog.EventMode.StartResearch;
                            AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
                            log.Release();
                        }

                        spells?.Dispose();
                    }
                }
                else
                {
                    AoWHSMap.Current.ResetTurnTimer();
                    AoWHSMap.Current.EnableTurnTimer();
                }
            }
        }

        base.NewTurn(player);
        if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                AoWHSMap.Current.AIExecutor.ActivatePlayer(player);
            }
        }
    }

    // PlayerC.PAS:1663
    public override void ValidateTurnTiming()
    {
        if (AoWHSMap.Current.GetTurnTimerEnabled() && AoWHSMap.Current.GetTurnTimeLeft() <= 0)
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

    // PlayerC.PAS:1299
    public override void Setup()
    {
        base.Setup();
        AoWHSMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = false;
    }

    // PlayerC.PAS:1364
    public override void StartNewMap()
    {
        NewTurnEventLog log = default;
        base.StartNewMap();
        AoWHSMap.Current.ShowIndependentMoves = false;
        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I == 0 || AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Independent)
            {
                AoWHSMap.Current.Players.GetPlayers(I).Status = PlayerStatus.Playing;
            }
        }

        AoWHSMap.Current.Players.Update();
        AoWHSMap.Current.CurrentPlayer = 0;
        var item = AoWHSMap.Current.Players.LocalHumanPlayers[0];
        AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
        AoWHSMap.Current.SeatedPlayerChanged();
        Assert(AoWHSMap.Current.SeatedPlayer != 0, "", "PlayerC.PAS", 1584);
        AoWHSMap.Current.ResetSeatedPlayerView();
        AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(AoWHSMap.Current.CurrentPlayer);
        log = new NewTurnEventLog(null);
        log.TurnPlayer = AoWHSMap.Current.SeatedPlayer;
        log.Mode = NewTurnEventLogMode.Dialog;
        log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        log.Text = AbilityRules.TranslateResourceString(s_letGameBeginText);
        AoWHSMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
        log.Release();
        if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network)
        {
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
        else
        {
            AoWHSMap.Current.AIExecutor.Lock();
        }

        NewDay();
    }

    // PlayerC.PAS:1639
    public override void StartLoadedMap()
    {
        var item = AoWHSMap.Current.Players.LocalHumanPlayers[0];
        if (!IsEmpty(AoWHSMap.Current.Players.GetPlayers(item).Settings))
        {
            var fLocalHumanPlayersItem = AoWHSMap.Current.Players.LocalHumanPlayers[0];
            ValidatePassword(AoWHSMap.Current.Players.GetPlayers(fLocalHumanPlayersItem).Settings);
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
        AoWHSMap.Current.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
        AoWHSMap.Current.TokenManager.TokenExecutor.GetTacticalBattle(player).Active = false;
        AoWHSMap.Current.Players.Update();
        AoWHSMap.Current.TriggerPlayerTerminatedEvent(player);
        if (AoWHSMap.Current.Players.LocalHumanPlayers.Count == 0)
        {
            AoWHSMap.Current.TriggerGameTerminatedEvent();
        }
        else
        {
            EndTurn(player);
        }
    }

    // PlayerC.PAS:1646
    public override void GameOverEventHandled(sbyte player)
    {
        AoWHSMap.Current.SetSeatedPlayer(-1);
        AoWHSMap.Current.TriggerGameTerminatedEvent();
    }

    // PlayerC.PAS:1305
    public override void EndTurn(sbyte player)
    {
        int I;
        bool allDone;
        EndTurnEventLog log;
        EngineMessage message = default;
        AoWHSMap.Current.Players.GetPlayers(player).SetEndTurn(true);
        allDone = true;
        I = 1;
        while (allDone && I != AoWHSMap.Current.Players.Count)
        {
            allDone = AoWHSMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Playing || AoWHSMap.Current.Players.GetPlayers(I).SingleProcessCycle;
            I += 1;
        }

        if (allDone)
        {
            if (AoWHSMap.Current.SeatedPlayer == player)
            {
                AoWHSMap.Current.ClearExecuteEventLogBuffer();
            }

            AoWHSMap.Current.LockRelationsChanged();
            I = 0;
            int last = AoWHSMap.Current.Players.Count - 1;
            for (; I <= last; I += 1)
            {
                if (AoWHSMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    if (AoWHSMap.Current.Players.GetPlayers(I).PlayerType != PlayerType.Human)
                    {
                        AoWHSMap.Current.AIExecutor.DeactivatePlayer((sbyte)I);
                    }

                    log = new EndTurnEventLog(null);
                    AoWHSMap.Current.Players.GetPlayers(I).EventLogbook.AddEvent(log);
                    log.Release();
                }

                EngineMessage.InitializeMessage(ref message, 537001991, this);
                message.Payload = I;
                AoWHSMap.Current.SendMessage(ref message, this);
            }

            AoWHSMap.Current.UnlockRelationsChanged();
            NewDay();
        }
    }

    // PlayerC.PAS:1354
    public void StartGameCallback(EventLog log)
    {
        if (AoWHSMap.Current.ConnectionType != PlayerConnectionType.Network)
        {
            AoWHSMap.Current.AIExecutor.Unlock();
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
    }

    // PlayerC.PAS:1603
    public void ContinueLoadedMap()
    {
        NewTurnEventLog log = default;
        Player player = default;
        AoWHSMap.Current.ShowIndependentMoves = false;
        var item = AoWHSMap.Current.Players.LocalHumanPlayers[0];
        AoWHSMap.Current.SetSeatedPlayer((sbyte)item);
        AoWHSMap.Current.SeatedPlayerChanged();
        AoWHSMap.Current.ResetSeatedPlayerView();
        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            player = AoWHSMap.Current.Players.GetPlayers(I);
            if (player.Status == PlayerStatus.Playing && player.PlayerType != PlayerType.Human)
            {
                AoWHSMap.Current.AIExecutor.ActivatePlayer((sbyte)I);
            }
        }

        for (int I = 0, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Players.GetPlayers(I).Status != PlayerStatus.Playing)
            {
                AoWHSMap.Current.TokenManager.TokenControls[I].Active = false;
            }
        }

        AoWHSMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(AoWHSMap.Current.CurrentPlayer);
        log = new NewTurnEventLog(null);
        log.TurnPlayer = AoWHSMap.Current.SeatedPlayer;
        log.Mode = NewTurnEventLogMode.Dialog;
        log.DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable);
        log.Text = AbilityRules.TranslateResourceString(s_gameContinuesText);
        AoWHSMap.Current.TriggerExecuteEventLog(log, false, StartGameCallback);
        log.Release();
        if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network)
        {
            AoWHSMap.Current.ResetTurnTimer();
            AoWHSMap.Current.EnableTurnTimer();
        }
        else
        {
            AoWHSMap.Current.AIExecutor.Lock();
        }
    }

    private static readonly string s_letGameBeginText = "Let the game begin";
    private static readonly string s_gameContinuesText = "The game continues";

    // intf_P1_04.inc:26

    // intf_P1_04.inc:26

}
