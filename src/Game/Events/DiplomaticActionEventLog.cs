#nullable disable
// Translated from AoWE.pas.

using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// Action
public partial class DiplomaticActionEventLog(CustomNode owner) : EventLog(owner)
{

    // Diplomacy.pas:1199
    public new const int ClassId = 131347;

    public DiplomaticAction Action { get; set; }

    // Diplomacy.pas:1170
    public override void Dispose()
    {
        DiplomaticAction action = Action;
        if (action is not null)
        {
            _ = action.Release();
            Action = null;
        }

        base.Dispose();
    }

    // Diplomacy.pas:1202
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Action = persister.Created("Action", objectValue: Action);
    }

    // Diplomacy.pas:1182
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        Action.MessageProc(ref message, sender);
    }

    // Diplomacy.pas:1265
    public override string GetText() => Action.Proposal.LogText(Action);

    // Diplomacy.pas:1228
    public override int GetTextColor()
    {
        int race;
        if (Action.Proposal.SignedAsOtherPlayer)
        {
            bool condition = Action.OtherPlayer > 0;
            if (condition)
            {
                int count = StrategicMap.Current.Players.Count;
                condition = Action.OtherPlayer < count;
            }

            race = condition ? StrategicMap.Current.Players.GetPlayers(Action.OtherPlayer).PlayerRace : 12;

            return RaceColors[race];
        }
        else
        {
            if (Action.Reaction == DiplomaticActionReaction.Accept)
            {
                bool condition = Action.Target > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = Action.Target < count;
                }

                race = condition ? StrategicMap.Current.Players.GetPlayers(Action.Target).PlayerRace : 12;

                return RaceColors[race];
            }
            else
            {
                if (StrategicMap.Current.SeatedPlayer != Action.Source)
                {
                    bool condition = Action.Source > 0;
                    if (condition)
                    {
                        int count = StrategicMap.Current.Players.Count;
                        condition = Action.Source < count;
                    }

                    race = condition ? StrategicMap.Current.Players.GetPlayers(Action.Source).PlayerRace : 12;

                    return RaceColors[race];
                }
                else
                {
                    bool condition = Action.Target > 0;
                    if (condition)
                    {
                        int count = StrategicMap.Current.Players.Count;
                        condition = Action.Target < count;
                    }

                    race = condition ? StrategicMap.Current.Players.GetPlayers(Action.Target).PlayerRace : 12;

                    return RaceColors[race];
                }
            }
        }
    }

    // Diplomacy.pas:1333
    public override int LeftIcon
    {
        get
        {
            DiplomaticAction action = Action;
            if (!action.Proposal.SignedAsOtherPlayer)
            {
                bool condition = action.Source > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = action.Source < count;
                }

                return condition ? StrategicMap.Current.Players.GetPlayers(action.Source).PlayerRace : -1;
            }
            else
            {
                bool condition = action.OtherPlayer > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = action.OtherPlayer < count;
                }

                return condition ? StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).PlayerRace : -1;
            }
        }
    }

    // Diplomacy.pas:1379
    // { DCC32 folds JMP-to-RET; keep the original branch over the 11-byte arms. }
    public override int RightIcon
    {
        get
        {
            DiplomaticAction action = Action;
            bool condition = action.Target > 0;
            if (condition)
            {
                int count = StrategicMap.Current.Players.Count;
                condition = action.Target < count;
            }

            return condition ? StrategicMap.Current.Players.GetPlayers(action.Target).PlayerRace : -1;
        }
    }

    // Diplomacy.pas:1225
    public override bool Visible => true;

    // Diplomacy.pas:1208
    public override void PlayLoggedSfx(EventLogbook logbook) => AudioPlayback.PlaySample(Action.Proposal.LoggedSample);

    // Diplomacy.pas:1217
    public override void NotifyLogged(EventLogbook logbook)
    {
        base.NotifyLogged(logbook);
        DiplomaticAction action = Action;
        action?.EventLogId = Id;
    }

    // Diplomacy.pas:1414
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool show = trigger == ExecuteTrigger.User;
        if (StrategicMap.Current.SeatedPlayer == Action.Target || StrategicMap.Current.SeatedPlayer == Action.Source)
        {
            show = show || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && trigger == ExecuteTrigger.NewTurn);
            show = show || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy));
        }

        show = show && (trigger == ExecuteTrigger.User || Action.Reaction != DiplomaticActionReaction.Ignore);
        if (show)
        {
            StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1188
    public void SetDiplomaticAction(DiplomaticAction action)
    {
        DiplomaticAction old = Action;
        if (old != action)
        {
            _ = (old?.Release());

            Action = action?.Copy<DiplomaticAction>(GetEngine());
        }
    }

    public override int MiddleIcon => Action.Proposal.MiddleIcon(Action);

    private static ReadOnlySpan<int> RaceColors => [14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224];

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi P5DiplomaticExecute signature keeps data.")]
    internal static bool DiplomaticExecute(DiplomaticActionEventLog log, bool immediate, ExecuteTrigger trigger, object data, ExecuteDoneEvent done)
    {
        bool result = trigger == ExecuteTrigger.User;
        if (log.Action.Target == StrategicMap.Current.SeatedPlayer || log.Action.Source == StrategicMap.Current.SeatedPlayer)
        {
            result = result || (StrategicMap.Current.PlayMode == PlayMode.Turn && log.GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && trigger == ExecuteTrigger.NewTurn);
            result = result || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && log.GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy));
        }
        result = result && (trigger == ExecuteTrigger.User || log.Action.Reaction != DiplomaticActionReaction.Ignore);
        if (!result)
        {
            return false;
        }

        StrategicMap.Current.TriggerExecuteEventLog(log, immediate, done);
        return true;
    }
}
