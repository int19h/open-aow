// Translated from AoWE.pas.

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// Action
public partial class DiplomaticActionEventLog : EventLog
{
    public DiplomaticActionEventLog(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1199
    public new const int ClassId = 131347;

    public DiplomaticAction Action;

    // Diplomacy.pas:1170
    public override void Dispose()
    {
        DiplomaticAction action;
        action = Action;
        if (action != null)
        {
            action.Release();
            Action = null;
        }

        base.Dispose();
    }

    // Diplomacy.pas:1202
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Created(ref Action, null, 20);
    }

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
        int result;
        int race;
        if (Action.Proposal.SignedAsOtherPlayer)
        {
            bool condition = Action.OtherPlayer > 0;
            if (condition)
            {
                var count = StrategicMap.Current.Players.Count;
                condition = Action.OtherPlayer < count;
            }

            if (condition)
            {
                race = StrategicMap.Current.Players.GetPlayers(Action.OtherPlayer).PlayerRace;
            }
            else
            {
                race = 12;
            }

            result = RaceColors[race];
        }
        else
        {
            if (Action.Reaction == DiplomaticActionReaction.Accept)
            {
                bool condition = Action.Target > 0;
                if (condition)
                {
                    var count = StrategicMap.Current.Players.Count;
                    condition = Action.Target < count;
                }

                if (condition)
                {
                    race = StrategicMap.Current.Players.GetPlayers(Action.Target).PlayerRace;
                }
                else
                {
                    race = 12;
                }

                result = RaceColors[race];
            }
            else
            {
                if (StrategicMap.Current.SeatedPlayer != Action.Source)
                {
                    bool condition = Action.Source > 0;
                    if (condition)
                    {
                        var count = StrategicMap.Current.Players.Count;
                        condition = Action.Source < count;
                    }

                    if (condition)
                    {
                        race = StrategicMap.Current.Players.GetPlayers(Action.Source).PlayerRace;
                    }
                    else
                    {
                        race = 12;
                    }

                    result = RaceColors[race];
                }
                else
                {
                    bool condition = Action.Target > 0;
                    if (condition)
                    {
                        var count = StrategicMap.Current.Players.Count;
                        condition = Action.Target < count;
                    }

                    if (condition)
                    {
                        race = StrategicMap.Current.Players.GetPlayers(Action.Target).PlayerRace;
                    }
                    else
                    {
                        race = 12;
                    }

                    result = RaceColors[race];
                }
            }
        }

        return result;
    }

    // Diplomacy.pas:1333
    public override int GetLeftIcon()
    {
        int result;
        DiplomaticAction action;
        action = Action;
        if (!action.Proposal.SignedAsOtherPlayer)
        {
            bool condition = action.Source > 0;
            if (condition)
            {
                var count = StrategicMap.Current.Players.Count;
                condition = action.Source < count;
            }

            if (condition)
            {
                result = StrategicMap.Current.Players.GetPlayers(action.Source).PlayerRace;
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            bool condition = action.OtherPlayer > 0;
            if (condition)
            {
                var count = StrategicMap.Current.Players.Count;
                condition = action.OtherPlayer < count;
            }

            if (condition)
            {
                result = StrategicMap.Current.Players.GetPlayers(action.OtherPlayer).PlayerRace;
            }
            else
            {
                result = -1;
            }
        }

        return result;
    }

    // Diplomacy.pas:1379
    // { DCC32 folds JMP-to-RET; keep the original branch over the 11-byte arms. }
    public override int GetRightIcon()
    {
        int result;
        DiplomaticAction action;
        action = Action;
        bool condition = action.Target > 0;
        if (condition)
        {
            var count = StrategicMap.Current.Players.Count;
            condition = action.Target < count;
        }

        if (condition)
        {
            result = StrategicMap.Current.Players.GetPlayers(action.Target).PlayerRace;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Diplomacy.pas:1225
    public override bool GetVisible() => true;

    // Diplomacy.pas:1208
    public override void PlayLoggedSfx(EventLogbook logbook) => AudioPlayback.PlaySample(Action.Proposal.LoggedSample);

    // Diplomacy.pas:1217
    public override void NotifyLogged(EventLogbook logbook)
    {
        DiplomaticAction action;
        base.NotifyLogged(logbook);
        action = Action;
        if (action != null)
        {
            action.EventLogId = Id;
        }
    }

    // Diplomacy.pas:1414
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent done)
    {
        bool result = default;
        bool show;
        show = trigger == ExecuteTrigger.User;
        if (StrategicMap.Current.SeatedPlayer == Action.Target || StrategicMap.Current.SeatedPlayer == Action.Source)
        {
            show = show || StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && trigger == ExecuteTrigger.NewTurn;
            show = show || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy);
        }

        show = show && (trigger == ExecuteTrigger.User || Action.Reaction != DiplomaticActionReaction.Ignore);
        if (show)
        {
            StrategicMap.Current.TriggerExecuteEventLog(this, mode, done);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:1188
    public void SetDiplomaticAction(DiplomaticAction action)
    {
        DiplomaticAction old;
        old = Action;
        if (old != action)
        {
            if (old != null)
            {
                old.Release();
            }

            if (action != null)
            {
                Action = (DiplomaticAction)action.Copy(GetEngine());
            }
            else
            {
                Action = null;
            }
        }
    }

    public override int GetMiddleIcon() => Action.Proposal.MiddleIcon(Action);

    private static ReadOnlySpan<int> RaceColors => [14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224];

    internal static bool DiplomaticExecute(DiplomaticActionEventLog log, bool immediate, ExecuteTrigger trigger, object data, ExecuteDoneEvent done)
    {
        bool result = trigger == ExecuteTrigger.User;
        if (log.Action.Target == StrategicMap.Current.SeatedPlayer || log.Action.Source == StrategicMap.Current.SeatedPlayer)
        {
            result = result || StrategicMap.Current.PlayMode == PlayMode.Turn && log.GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && trigger == ExecuteTrigger.NewTurn;
            result = result || StrategicMap.Current.PlayMode == PlayMode.Simultaneous && log.GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy);
        }
        result = result && (trigger == ExecuteTrigger.User || log.Action.Reaction != DiplomaticActionReaction.Ignore);
        if (!result) return false;
        StrategicMap.Current.TriggerExecuteEventLog(log, immediate, done);
        return true;
    }

}
