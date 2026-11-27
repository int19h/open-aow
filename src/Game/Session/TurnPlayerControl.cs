#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Session;

public partial class TurnPlayerControl(CustomNode? owner) : PlayerControl(owner)
{
    public new const int ClassId = 131_664;

    // PlayerC.PAS:589
    public override void NewTurn(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.LockMapChangedEvent();
        strategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(player));
        NewTurnEventLog log;
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (engineStrategicMap.RequestCombatType)
        {
            if (engineStrategicMap.SeatedPlayer is not null)
            {
                engineStrategicMap.SetSeatedPlayer(null);
            }

            StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
            if (getPlayersStrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null, holder: CreationHolder())
                {
                    TurnPlayer = getPlayersStrategicMap.CurrentPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_itsYourTurnText)
                };
                GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: ShowNewTurnCallback);
                _ = log.Release();
            }
            else
            {
                log = new NewTurnEventLog(null, holder: CreationHolder())
                {
                    TurnPlayer = GetEngine().StrategicMap.CurrentPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.None
                };
                if (player == PlayerId.Independent)
                {
                    log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_independentsAreMovingText);
                }
                else
                {
                    string name = GetEngine().StrategicMap.Players.GetPlayers(player).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_playerIsMovingFormat), arguments: [name]);
                }

                GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: true, done: null);
                _ = log.Release();
            }
        }
        else
        {
            StrategicMap resetTurnTimerStrategicMap = GetEngine().StrategicMap;
            if (resetTurnTimerStrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human && player == resetTurnTimerStrategicMap.SeatedPlayer)
            {
                if (resetTurnTimerStrategicMap.GameType != GameType.Tutorial || resetTurnTimerStrategicMap.Day != 1)
                {
                    resetTurnTimerStrategicMap.ResetTurnTimer();
                    resetTurnTimerStrategicMap.EnableTurnTimer();
                    resetTurnTimerStrategicMap.ResetSeatedPlayerView();
                    log = new NewTurnEventLog(null, holder: CreationHolder())
                    {
                        TurnPlayer = player,
                        Mode = NewTurnEventLogMode.Dialog,
                        DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                        Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_itsYourTurnText)
                    };
                    GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: null);
                    _ = log.Release();
                    ReplayLogbook();
                }
                else
                {
                    GetEngine().StrategicMap.TutorialControl.ShowMessage(50, immediate: false, onceOnly: true);
                }
            }
        }

        base.NewTurn(player);
        StrategicMap playersStrategicMap = GetEngine().StrategicMap;
        if (playersStrategicMap.Players.GetPlayers(player).PlayerType == PlayerType.Human && playersStrategicMap.ConnectionType != PlayerConnectionType.Email && GetEngine().Registry.GetAutoSave())
        {
            AutoSave();
        }

        StrategicMap unlockMapChangedEventStrategicMap = GetEngine().StrategicMap;
        unlockMapChangedEventStrategicMap.UnlockMapChangedEvent();
        unlockMapChangedEventStrategicMap.InvalidateMap();
        StrategicMap activatePlayerStrategicMap = GetEngine().StrategicMap;
        if (activatePlayerStrategicMap.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            _ = activatePlayerStrategicMap.AIExecutor.ActivatePlayer(player);
            if (player == PlayerId.Independent)
            {
                string independents = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.IndependentsText);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_areMovingFormat), arguments: [independents]);
                GetEngine().StrategicMap.ShowMessage(format);
            }
            else
            {
                Player? players = GetEngine().StrategicMap.Players.GetPlayers(player);
                string raceNamesPlural = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_areMovingFormat), arguments: [raceNamesPlural]);
                GetEngine().StrategicMap.ShowMessage(format);
            }
        }
        else
        {
            // A driven human keeps the new-turn sound and the day-one book. The executor call is
            // the one a CPU seat gets above.
            if (HeadlessHooks.DriveHumanSeats)
            {
                _ = GetEngine().StrategicMap.AIExecutor.ActivatePlayer(player);
            }

            _ = AudioPlayback.PlaySample("SFX\\EVENTS\\NEWTURN.WAV");
            StrategicMap triggerExecuteEventLogStrategicMap = GetEngine().StrategicMap;
            if (triggerExecuteEventLogStrategicMap.Day == 1 && (triggerExecuteEventLogStrategicMap.RequestCombatType || player == triggerExecuteEventLogStrategicMap.SeatedPlayer))
            {
                if (triggerExecuteEventLogStrategicMap.GameType is GameType.Custom or GameType.Tutorial)
                {
                    ShowBookEventLog bookLog = new(null, holder: CreationHolder());
                    triggerExecuteEventLogStrategicMap.TriggerExecuteEventLog(bookLog, immediate: false, done: null);
                    _ = bookLog.Release();
                }

                StrategicMap listResearchSpellsStrategicMap = GetEngine().StrategicMap;
                if (listResearchSpellsStrategicMap.Players.GetPlayers(player).MagicControl.ResearchSpellId == 0)
                {
                    SpellList spells = new();
                    listResearchSpellsStrategicMap.Players.GetPlayers(player).MagicControl.ListResearchSpells(spells, spellTypes: SpellTypes.Combat | SpellTypes.Unit | SpellTypes.Global);
                    if (spells.GetCount() > 0)
                    {
                        PlayerMagicEventLog magicLog = new(null, holder: CreationHolder())
                        {
                            Mode = PlayerMagicEventLog.EventMode.StartResearch
                        };
                        GetEngine().StrategicMap.TriggerExecuteEventLog(magicLog, immediate: false, done: null);
                        _ = magicLog.Release();
                    }
                }

                GetEngine().StrategicMap.TutorialControl.ShowMessage(52, immediate: false, onceOnly: true);
            }
        }
    }

    // PlayerC.PAS:875
    public override void ValidateTurnTiming()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.TurnTimerEnabled)
        {
            if (strategicMap.CurrentPlayer == strategicMap.SeatedPlayer)
            {
                if (strategicMap.TurnTimeLeft <= 0)
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
        }
    }

    // PlayerC.PAS:569
    public override void Setup()
    {
        base.Setup();
        GetEngine().StrategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = true;
    }

    // PlayerC.PAS:766
    public override void StartNewMap()
    {
        base.StartNewMap();
        DefaultStartMap();
        NewDay();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (!strategicMap.RequestCombatType)
        {
            int item = strategicMap.Players.LocalHumanPlayers[0];
            strategicMap.SetSeatedPlayer(PlayerId.FromOriginal((sbyte)item));
            GetEngine().StrategicMap.ResetSeatedPlayerView();
        }

        NewTurn(PlayerId.FromOriginal((sbyte)GetEngine().StrategicMap.TurnOrder[1]));
    }

    // PlayerC.PAS:835
    public override void StartLoadedMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (!IsEmpty(strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).Settings))
        {
            ValidatePassword(strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).Settings);
        }
        else
        {
            ContinueLoadedMap();
        }
    }

    // PlayerC.PAS:832
    public override void PasswordValidated() => ContinueLoadedMap();

    // PlayerC.PAS:854
    public override void SetPlayerGameOverStatus(PlayerId? player, GameOverStatus status)
    {
        base.SetPlayerGameOverStatus(player, status);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (!strategicMap.RequestCombatType || strategicMap.Players.GetPlayers(player).PlayerType != PlayerType.Human)
        {
            strategicMap.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            strategicMap.Players.Update();
            strategicMap.TriggerPlayerTerminatedEvent(player);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.CurrentPlayer == player && engineStrategicMap.Players.GetPlayers(player).GetLocal())
            {
                _ = AbilityRules.EndPlayerTurn(engineStrategicMap.CurrentPlayer, engine: GetEngine());
            }

            StrategicMap triggerGameTerminatedEventStrategicMap = GetEngine().StrategicMap;
            if (triggerGameTerminatedEventStrategicMap.Players.LocalHumanPlayers.Count == 0)
            {
                triggerGameTerminatedEventStrategicMap.TriggerGameTerminatedEvent();
            }
        }
    }

    // PlayerC.PAS:868
    public override void GameOver(PlayerId? player, GameOverStatus status)
    {
        GetEngine().StrategicMap.LockGameOverEvent();
        base.GameOver(player, status);
        GetEngine().StrategicMap.UnlockGameOverEvent();
    }

    // PlayerC.PAS:842
    public override void GameOverEventHandled(PlayerId? player)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.RequestCombatType)
        {
            strategicMap.Players.GetPlayers(player).Status = PlayerStatus.Terminated;
            strategicMap.Players.Update();
            strategicMap.TriggerPlayerTerminatedEvent(player);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            if (engineStrategicMap.Players.LocalHumanPlayers.Count == 0)
            {
                engineStrategicMap.TriggerGameTerminatedEvent();
            }
            else
            {
                _ = AbilityRules.EndPlayerTurn(GetEngine().StrategicMap.CurrentPlayer, engine: GetEngine());
            }
        }
    }

    // PlayerC.PAS:715
    public override void EndTurn(PlayerId? player)
    {
        base.EndTurn(player);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (player == strategicMap.SeatedPlayer)
        {
            strategicMap.ZeroTurnTimer();
            if (strategicMap.Players.GetPlayers(player).Status == PlayerStatus.Playing)
            {
                strategicMap.ClearExecuteEventLogBuffer();
            }

            GetEngine().StrategicMap.Selection.UnselectLocation();
        }

        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (player == engineStrategicMap.CurrentPlayer)
        {
            if (HeadlessHooks.HandSeatToAI(engineStrategicMap.Players.GetPlayers(player).PlayerType))
            {
                engineStrategicMap.AIExecutor.DeactivatePlayer(player);
            }

            EndTurnEventLog log = new(null, holder: CreationHolder());
            StrategicMap addEventStrategicMap = GetEngine().StrategicMap;
            addEventStrategicMap.Players.GetPlayers(addEventStrategicMap.CurrentPlayer).EventLogbook.AddEvent(log);
            _ = log.Release();
            StrategicMap indexOfStrategicMap = GetEngine().StrategicMap;
            int index = indexOfStrategicMap.TurnOrder.IndexOf((byte)PlayerIdExtensions.ToOriginal(indexOfStrategicMap.CurrentPlayer));
            bool found = false;
            while (!found)
            {
                index += 1;
                if (index == GetEngine().StrategicMap.TurnOrder.Count)
                {
                    NewDay();
                    index = 0;
                }

                StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
                sbyte nextPlayer = (sbyte)getPlayersStrategicMap.TurnOrder[index];
                found = getPlayersStrategicMap.Players.GetPlayers(nextPlayer).Status == PlayerStatus.Playing;
                if (!found)
                {
                    base.NewTurn(PlayerId.FromOriginal(nextPlayer));
                    base.EndTurn(PlayerId.FromOriginal(nextPlayer));
                }
            }

            StrategicMap newTurnStrategicMap = GetEngine().StrategicMap;
            if (newTurnStrategicMap.TurnOrder.Count != 0)
            {
                NewTurn(PlayerId.FromOriginal((sbyte)newTurnStrategicMap.TurnOrder[index]));
            }
            else
            {
                NewTurn(PlayerId.Independent);
            }
        }
    }

    // PlayerC.PAS:693
    public override void SetSeatedPlayer(PlayerId? player)
    {
        GetEngine().StrategicMap.LockMapChangedEvent();
        base.SetSeatedPlayer(player);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.UnlockMapChangedEvent();
        strategicMap.InvalidateMap();
    }

    // PlayerC.PAS:779
    public virtual void ContinueLoadedMap()
    {
        DefaultStartMap();
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(PlayerIdExtensions.ToOriginal(strategicMap.CurrentPlayer));
        NewTurnEventLog log;
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (engineStrategicMap.RequestCombatType)
        {
            engineStrategicMap.SetSeatedPlayer(null);
            StrategicMap getPlayersStrategicMap = GetEngine().StrategicMap;
            if (getPlayersStrategicMap.Players.GetPlayers(getPlayersStrategicMap.CurrentPlayer).PlayerType == PlayerType.Human)
            {
                log = new NewTurnEventLog(null, holder: CreationHolder())
                {
                    TurnPlayer = getPlayersStrategicMap.CurrentPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.YourTurnContinuesText)
                };
                GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
                _ = log.Release();
            }
            else
            {
                StrategicMap currentPlayerStrategicMap = GetEngine().StrategicMap;
                log = new NewTurnEventLog(null, holder: CreationHolder())
                {
                    TurnPlayer = currentPlayerStrategicMap.CurrentPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.None
                };
                if (currentPlayerStrategicMap.CurrentPlayer == PlayerId.Independent)
                {
                    log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_independentsAreMovingText);
                }
                else
                {
                    StrategicMap getNameStrategicMap = GetEngine().StrategicMap;
                    string name = getNameStrategicMap.Players.GetPlayers(getNameStrategicMap.CurrentPlayer).GetName();
                    log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_playerIsMovingFormat), arguments: [name]);
                }

                GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: true, done: null);
                _ = log.Release();
            }
        }
        else
        {
            StrategicMap setSeatedPlayerStrategicMap = GetEngine().StrategicMap;
            int item = setSeatedPlayerStrategicMap.Players.LocalHumanPlayers[0];
            setSeatedPlayerStrategicMap.SetSeatedPlayer(PlayerId.FromOriginal((sbyte)item));
            GetEngine().StrategicMap.ResetSeatedPlayerView();
            StrategicMap playersStrategicMap = GetEngine().StrategicMap;
            if (playersStrategicMap.Players.GetPlayers(playersStrategicMap.CurrentPlayer).PlayerType == PlayerType.Human && playersStrategicMap.CurrentPlayer == playersStrategicMap.SeatedPlayer)
            {
                log = new NewTurnEventLog(null, holder: CreationHolder())
                {
                    TurnPlayer = playersStrategicMap.SeatedPlayer,
                    Mode = NewTurnEventLogMode.Dialog,
                    DialogStyle = NewTurnEventLogDialogStyles.Closeable,
                    Text = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.YourTurnContinuesText)
                };
                GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate: false, done: StartGameCallback);
                _ = log.Release();
            }
        }

        StrategicMap handSeatToAIStrategicMap = GetEngine().StrategicMap;
        if (HeadlessHooks.HandSeatToAI(handSeatToAIStrategicMap.Players.GetPlayers(handSeatToAIStrategicMap.CurrentPlayer).PlayerType))
        {
            _ = handSeatToAIStrategicMap.AIExecutor.ActivatePlayer(handSeatToAIStrategicMap.CurrentPlayer);
        }

        GetEngine().OutputDebugString($"Random Seed: {GetEngine().StrategicMap.DefendersSeed.ToString(CultureInfo.InvariantCulture)}");
    }

    // PlayerC.PAS:683
    public void ReplayLogbook()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        EventLogbook book = strategicMap.Players.GetPlayers(strategicMap.CurrentPlayer).EventLogbook;
        int index = book.FindFirstLog(EndTurnEventLog.ClassId, player: strategicMap.CurrentPlayer, day: strategicMap.Day - 1);
        for (int i = index + 1, last = book.GetCount() - 1; i <= last; i++)
        {
            _ = book[i].Execute(false, trigger: ExecuteTrigger.NewTurn, done: null);
        }
    }

    // PlayerC.PAS:701
    public void ShowNewTurnCallback(EventLog log)
    {
        SetSeatedPlayer(GetEngine().StrategicMap.CurrentPlayer);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.ResetTurnTimer();
        strategicMap.EnableTurnTimer();
        if (strategicMap.CurrentPlayer is not null)
        {
            ReplayLogbook();
        }
    }

    // PlayerC.PAS:709
    public void StartGameCallback(EventLog log)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.SeatedPlayer is null)
        {
            SetSeatedPlayer(strategicMap.CurrentPlayer);
        }

        GetEngine().StrategicMap.EnableTurnTimer();
    }

    // PlayerC.PAS:758
    public void DefaultStartMap()
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.Players.Update();
        strategicMap.RequestCombatType = strategicMap.ConnectionType == PlayerConnectionType.Email || (strategicMap.ConnectionType == PlayerConnectionType.Local && strategicMap.Players.LocalHumanPlayers.Count > 1);
        strategicMap.ShowIndependentMoves = !strategicMap.RequestCombatType;
    }

    private static readonly string s_itsYourTurnText = "It's your turn!";
    private static readonly string s_independentsAreMovingText = "Independents are moving";
    private static readonly string s_playerIsMovingFormat = "%s is moving";
    private static readonly string s_areMovingFormat = "%s are moving";
}
