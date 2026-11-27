#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

public sealed class SimultaneousPlayerControl(CustomNode? owner) : PlayerControl(owner)
{
    public new const int ClassId = 131_665;

    // PlayerC.PAS:1343
    public override void NewDay()
    {
        GetEngine().StrategicMap.Selection.UnselectLocation();
        base.NewDay();
        for (int i = 0, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
        {
            NewTurn(PlayerId.FromOriginal((sbyte)i));
        }

        _ = AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
        if (GetEngine().Registry.GetAutoSave())
        {
            AutoSave();
        }
    }

    // PlayerC.PAS:1258
    public override void NewTurn(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (player == strategicMap.SeatedPlayer)
        {
            if (strategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (strategicMap.Day == 1)
                {
                    if (strategicMap.GameType == GameType.Custom)
                    {
                        ShowBookEventLog log = new(null) { Holder = CreationHolder() };
                        strategicMap.TriggerExecuteEventLog(log, immediate: false, done: null);
                        _ = log.Release();
                    }

                    StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                    if (engineStrategicMap.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                    {
                        SpellList spells = new();
                        engineStrategicMap.Players.GetPlayers(player).MagicControl.ListResearchSpells(spells, spellTypes: SpellTypes.Combat | SpellTypes.Unit | SpellTypes.Global);
                        if (spells.GetCount() > 0)
                        {
                            PlayerMagicEventLog research = new(null)
                            {
                                Holder = CreationHolder(),
                                Mode = PlayerMagicEventLog.EventMode.StartResearch
                            };
                            GetEngine().StrategicMap.TriggerExecuteEventLog(research, immediate: false, done: null);
                            _ = research.Release();
                        }
                    }
                }
                else
                {
                    StrategicMap resetTurnTimerStrategicMap = GetEngine().StrategicMap;
                    resetTurnTimerStrategicMap.ResetTurnTimer();
                    resetTurnTimerStrategicMap.EnableTurnTimer();
                }
            }
        }

        base.NewTurn(player);
        StrategicMap handSeatToAIStrategicMap = GetEngine().StrategicMap;
        if (HeadlessHooks.HandSeatToAI(handSeatToAIStrategicMap.Players.GetPlayers(player).PlayerType))
        {
            if (handSeatToAIStrategicMap.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                _ = handSeatToAIStrategicMap.AIExecutor.ActivatePlayer(player);
            }
        }
    }

    // PlayerC.PAS:1663
    public override void ValidateTurnTiming()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.TurnTimerEnabled && strategicMap.TurnTimeLeft <= 0)
        {
            if (strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).GameOverStatus == GameOverStatus.None && !strategicMap.Players.GetPlayers(strategicMap.SeatedPlayer).SingleProcessCycle)
            {
                if (strategicMap.TokenManager.PlayerReady(strategicMap.SeatedPlayer))
                {
                    _ = AbilityRules.EndPlayerTurn(strategicMap.SeatedPlayer, engine: GetEngine());
                }
            }
        }
    }

    // PlayerC.PAS:1299
    public override void Setup()
    {
        base.Setup();
        GetEngine().StrategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = false;
    }

    // PlayerC.PAS:1364
    public override void StartNewMap()
    {
        base.StartNewMap();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.ShowIndependentMoves = false;
        for (int i = 0, last = strategicMap.Players.Count - 1; i <= last; i++)
        {
            if (i == 0 || strategicMap.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                strategicMap.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }

        strategicMap.Players.Update();
        strategicMap.CurrentPlayer = PlayerId.Independent;
        int item = strategicMap.Players.LocalHumanPlayers[0];
        strategicMap.SetSeatedPlayer(PlayerId.FromOriginal((sbyte)item));
        GetEngine().StrategicMap.NotifySeatedPlayerChanged();
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        Assert(engineStrategicMap.SeatedPlayer != PlayerId.Independent, message: "", file: "PlayerC.PAS", line: 1584);
        engineStrategicMap.ResetSeatedPlayerView();
        StrategicMap activateTacticalBattleStrategicMap = GetEngine().StrategicMap;
        activateTacticalBattleStrategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(activateTacticalBattleStrategicMap.CurrentPlayer));
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = GetEngine().StrategicMap.SeatedPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = NewTurnEventLogDialogStyles.Closeable,
            Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_letGameBeginText)
        };
        GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
        _ = log.Release();
        StrategicMap resetTurnTimerStrategicMap = GetEngine().StrategicMap;
        if (resetTurnTimerStrategicMap.ConnectionType == PlayerConnectionType.Network)
        {
            resetTurnTimerStrategicMap.ResetTurnTimer();
            resetTurnTimerStrategicMap.EnableTurnTimer();
        }
        else
        {
            resetTurnTimerStrategicMap.AIExecutor.Lock();
        }

        NewDay();
    }

    // PlayerC.PAS:1639
    public override void StartLoadedMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        int item = strategicMap.Players.LocalHumanPlayers[0];
        if (!IsEmpty(strategicMap.Players.GetPlayers(item).Settings))
        {
            byte fLocalHumanPlayersItem = strategicMap.Players.LocalHumanPlayers[0];
            ValidatePassword(strategicMap.Players.GetPlayers(fLocalHumanPlayersItem).Settings);
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:1636
    public override void PasswordValidated() => ContinueLoadedMap();

    // PlayerC.PAS:1652
    public override void SetPlayerGameOverStatus(PlayerId? player, GameOverStatus status)
    {
        base.SetPlayerGameOverStatus(player, status);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
        strategicMap.TokenManager.TokenExecutor.GetTacticalBattle(player).Active = false;
        strategicMap.Players.Update();
        strategicMap.TriggerPlayerTerminatedEvent(player);
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (engineStrategicMap.Players.LocalHumanPlayers.Count == 0)
        {
            engineStrategicMap.TriggerGameTerminatedEvent();
        }
        else
        {
            EndTurn(player);
        }
    }

    // PlayerC.PAS:1646
    public override void GameOverEventHandled(PlayerId? player)
    {
        GetEngine().StrategicMap.SetSeatedPlayer(null);
        GetEngine().StrategicMap.TriggerGameTerminatedEvent();
    }

    // PlayerC.PAS:1305
    public override void EndTurn(PlayerId? player)
    {
        EngineMessage message = default;
        GetEngine().StrategicMap.Players.GetPlayers(player).SetEndTurn(true);
        bool allDone = true;
        int i = 1;
        while (allDone && i != GetEngine().StrategicMap.Players.Count)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            allDone = strategicMap.Players.GetPlayers(i).Status != PlayerStatus.Playing || strategicMap.Players.GetPlayers(i).SingleProcessCycle;
            i += 1;
        }

        if (allDone)
        {
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.SeatedPlayer == player)
            {
                engineStrategicMap.ClearExecuteEventLogBuffer();
            }

            StrategicMap lockRelationsChangedStrategicMap = GetEngine().StrategicMap;
            lockRelationsChangedStrategicMap.LockRelationsChanged();
            i = 0;
            int last = lockRelationsChangedStrategicMap.Players.Count - 1;
            for (; i <= last; i += 1)
            {
                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                if (getPlayersStrategicMap.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    if (HeadlessHooks.HandSeatToAI(getPlayersStrategicMap.Players.GetPlayers(i).PlayerType))
                    {
                        getPlayersStrategicMap.AIExecutor.DeactivatePlayer(PlayerId.FromOriginal((sbyte)i));
                    }

                    EndTurnEventLog log = new(null) { Holder = CreationHolder() };
                    GetEngine().StrategicMap.Players.GetPlayers(i).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }

                EngineMessage.InitializeMessage(out message, messageId: 537001991, sender: this);
                message.Payload = i;
                GetEngine().StrategicMap.SendMessage(ref message, sender: this);
            }

            GetEngine().StrategicMap.UnlockRelationsChanged();
            NewDay();
        }
    }

    // PlayerC.PAS:1354
    public void StartGameCallback(EventLog log)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.ConnectionType != PlayerConnectionType.Network)
        {
            strategicMap.AIExecutor.Unlock();
            strategicMap.ResetTurnTimer();
            strategicMap.EnableTurnTimer();
        }
    }

    // PlayerC.PAS:1603
    public void ContinueLoadedMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.ShowIndependentMoves = false;
        int item = strategicMap.Players.LocalHumanPlayers[0];
        strategicMap.SetSeatedPlayer(PlayerId.FromOriginal((sbyte)item));
        GetEngine().StrategicMap.NotifySeatedPlayerChanged();
        GetEngine().StrategicMap.ResetSeatedPlayerView();
        for (int i = 0, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
        {
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            Player? player = engineStrategicMap.Players.GetPlayers(i);
            if (player.Status == PlayerStatus.Playing && HeadlessHooks.HandSeatToAI(player.PlayerType))
            {
                _ = engineStrategicMap.AIExecutor.ActivatePlayer(PlayerId.FromOriginal((sbyte)i));
            }
        }

        StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
        for (int i = 0, last = getPlayersStrategicMap.Players.Count - 1; i <= last; i++)
        {
            if (getPlayersStrategicMap.Players.GetPlayers(i).Status != PlayerStatus.Playing)
            {
                getPlayersStrategicMap.TokenManager.TokenControls[PlayerId.FromOriginal(i)].Active = false;
            }
        }

        getPlayersStrategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(getPlayersStrategicMap.CurrentPlayer));
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = GetEngine().StrategicMap.SeatedPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = NewTurnEventLogDialogStyles.Closeable,
            Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_gameContinuesText)
        };
        GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
        _ = log.Release();
        StrategicMap resetTurnTimerStrategicMap = GetEngine().StrategicMap;
        if (resetTurnTimerStrategicMap.ConnectionType == PlayerConnectionType.Network)
        {
            resetTurnTimerStrategicMap.ResetTurnTimer();
            resetTurnTimerStrategicMap.EnableTurnTimer();
        }
        else
        {
            resetTurnTimerStrategicMap.AIExecutor.Lock();
        }
    }

    private static readonly string s_letGameBeginText = "Let the game begin";
    private static readonly string s_gameContinuesText = "The game continues";
}
