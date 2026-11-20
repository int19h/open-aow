#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

public sealed class SimultaneousPlayerControl(CustomNode owner) : PlayerControl(owner)
{
    public new const int ClassId = 131_665;

    // PlayerC.PAS:1343
    public override void NewDay()
    {
        StrategicMap.Current.Selection.UnselectLocation();
        base.NewDay();
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            NewTurn((sbyte)i);
        }

        _ = AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
        if (GetEngine().Registry.GetAutoSave())
        {
            AutoSave();
        }
    }

    // PlayerC.PAS:1258
    public override void NewTurn(sbyte player)
    {
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (StrategicMap.Current.Day == 1)
                {
                    if (StrategicMap.Current.GameType == GameType.Custom)
                    {
                        ShowBookEventLog log = new(null) { Holder = CreationHolder() };
                        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: null);
                        _ = log.Release();
                    }

                    if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                    {
                        SpellList spells = new();
                        StrategicMap.Current.Players.GetPlayers(player).MagicControl.ListResearchSpells(spells, spellTypes: Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
                        if (spells.GetCount() > 0)
                        {
                            PlayerMagicEventLog research = new(null)
                            {
                                Holder = CreationHolder(),
                                Mode = PlayerMagicEventLog.EventMode.StartResearch
                            };
                            StrategicMap.Current.TriggerExecuteEventLog(research, immediate: false, done: null);
                            _ = research.Release();
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
                _ = StrategicMap.Current.AIExecutor.ActivatePlayer(player);
            }
        }
    }

    // PlayerC.PAS:1663
    public override void ValidateTurnTiming()
    {
        if (StrategicMap.Current.TurnTimerEnabled && StrategicMap.Current.TurnTimeLeft <= 0)
        {
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None && !StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).SingleProcessCycle)
            {
                if (StrategicMap.Current.TokenManager.PlayerReady(StrategicMap.Current.SeatedPlayer))
                {
                    _ = AbilityRules.EndPlayerTurn(StrategicMap.Current.SeatedPlayer, engine: GetEngine());
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
        base.StartNewMap();
        StrategicMap.Current.ShowIndependentMoves = false;
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i == 0 || StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Independent)
            {
                StrategicMap.Current.Players.GetPlayers(i).Status = PlayerStatus.Playing;
            }
        }

        StrategicMap.Current.Players.Update();
        StrategicMap.Current.CurrentPlayer = 0;
        byte item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        StrategicMap.Current.SetSeatedPlayer((sbyte)item);
        StrategicMap.Current.NotifySeatedPlayerChanged();
        Assert(StrategicMap.Current.SeatedPlayer != 0, message: "", file: "PlayerC.PAS", line: 1584);
        StrategicMap.Current.ResetSeatedPlayerView();
        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = StrategicMap.Current.SeatedPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable),
            Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_letGameBeginText)
        };
        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
        _ = log.Release();
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
        byte item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        if (!IsEmpty(StrategicMap.Current.Players.GetPlayers(item).Settings))
        {
            byte fLocalHumanPlayersItem = StrategicMap.Current.Players.LocalHumanPlayers[0];
            ValidatePassword(StrategicMap.Current.Players.GetPlayers(fLocalHumanPlayersItem).Settings);
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:1636
    public override void PasswordValidated() => ContinueLoadedMap();

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
        EngineMessage message = default;
        StrategicMap.Current.Players.GetPlayers(player).SetEndTurn(true);
        bool allDone = true;
        int i = 1;
        while (allDone && i != StrategicMap.Current.Players.Count)
        {
            allDone = StrategicMap.Current.Players.GetPlayers(i).Status != PlayerStatus.Playing || StrategicMap.Current.Players.GetPlayers(i).SingleProcessCycle;
            i += 1;
        }

        if (allDone)
        {
            if (StrategicMap.Current.SeatedPlayer == player)
            {
                StrategicMap.Current.ClearExecuteEventLogBuffer();
            }

            StrategicMap.Current.LockRelationsChanged();
            i = 0;
            int last = StrategicMap.Current.Players.Count - 1;
            for (; i <= last; i += 1)
            {
                if (StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    if (StrategicMap.Current.Players.GetPlayers(i).PlayerType != PlayerType.Human)
                    {
                        StrategicMap.Current.AIExecutor.DeactivatePlayer((sbyte)i);
                    }

                    EndTurnEventLog log = new(null) { Holder = CreationHolder() };
                    StrategicMap.Current.Players.GetPlayers(i).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }

                EngineMessage.InitializeMessage(out message, messageId: 537001991, sender: this);
                message.Payload = i;
                StrategicMap.Current.SendMessage(ref message, sender: this);
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
        StrategicMap.Current.ShowIndependentMoves = false;
        byte item = StrategicMap.Current.Players.LocalHumanPlayers[0];
        StrategicMap.Current.SetSeatedPlayer((sbyte)item);
        StrategicMap.Current.NotifySeatedPlayerChanged();
        StrategicMap.Current.ResetSeatedPlayerView();
        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            Player player = StrategicMap.Current.Players.GetPlayers(i);
            if (player.Status == PlayerStatus.Playing && player.PlayerType != PlayerType.Human)
            {
                _ = StrategicMap.Current.AIExecutor.ActivatePlayer((sbyte)i);
            }
        }

        for (int i = 0, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Players.GetPlayers(i).Status != PlayerStatus.Playing)
            {
                StrategicMap.Current.TokenManager.TokenControls[i].Active = false;
            }
        }

        StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(StrategicMap.Current.CurrentPlayer);
        NewTurnEventLog log = new(null)
        {
            Holder = CreationHolder(),
            TurnPlayer = StrategicMap.Current.SeatedPlayer,
            Mode = NewTurnEventLogMode.Dialog,
            DialogStyle = Set<NewTurnEventLogDialogStyleElement>.Of(NewTurnEventLogDialogStyleElement.Closeable),
            Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_gameContinuesText)
        };
        StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
        _ = log.Release();
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
}
