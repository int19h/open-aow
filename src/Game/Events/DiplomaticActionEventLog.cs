#nullable disable warnings
// Translated from AoWE.pas.

using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Graphics;

namespace Aow.Game.Events;

// Action
public partial class DiplomaticActionEventLog(CustomNode? owner) : EventLog(owner)
{

    // Diplomacy.pas:1199
    public new const int ClassId = 131_347;

    public DiplomaticAction? Action { get; set; }

    // Diplomacy.pas:1170
    public override void Dispose()
    {
        DiplomaticAction? action = Action;
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
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        Action.MessageProc(ref message, sender);
    }

    // Diplomacy.pas:1265
    public override string GetText() => Action.Proposal.LogText(Action);

    // Diplomacy.pas:1228
    public override Color GetTextColor()
    {
        int race;
        if (Action.Proposal.SignedAsOtherPlayer)
        {
            bool condition = PlayerIdExtensions.ToOriginal(Action.OtherPlayer) > 0;
            if (condition)
            {
                int count = StrategicMap.Current.Players.Count;
                condition = PlayerIdExtensions.ToOriginal(Action.OtherPlayer) < count;
            }

            race = condition ? StrategicMap.Current.Players.GetPlayers(Action.OtherPlayer).PlayerRace : 12;

            return RaceColors[race];
        }
        else
        {
            if (Action.Reaction == DiplomaticActionReaction.Accept)
            {
                bool condition = PlayerIdExtensions.ToOriginal(Action.Target) > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = PlayerIdExtensions.ToOriginal(Action.Target) < count;
                }

                race = condition ? StrategicMap.Current.Players.GetPlayers(Action.Target).PlayerRace : 12;

                return RaceColors[race];
            }
            else
            {
                if (StrategicMap.Current.SeatedPlayer != Action.Source)
                {
                    bool condition = PlayerIdExtensions.ToOriginal(Action.Source) > 0;
                    if (condition)
                    {
                        int count = StrategicMap.Current.Players.Count;
                        condition = PlayerIdExtensions.ToOriginal(Action.Source) < count;
                    }

                    race = condition ? StrategicMap.Current.Players.GetPlayers(Action.Source).PlayerRace : 12;

                    return RaceColors[race];
                }
                else
                {
                    bool condition = PlayerIdExtensions.ToOriginal(Action.Target) > 0;
                    if (condition)
                    {
                        int count = StrategicMap.Current.Players.Count;
                        condition = PlayerIdExtensions.ToOriginal(Action.Target) < count;
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
            DiplomaticAction? action = Action;
            if (!action.Proposal.SignedAsOtherPlayer)
            {
                bool condition = PlayerIdExtensions.ToOriginal(action.Source) > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = PlayerIdExtensions.ToOriginal(action.Source) < count;
                }

                return condition ? StrategicMap.Current.Players.GetPlayers(action.Source).PlayerRace : -1;
            }
            else
            {
                bool condition = PlayerIdExtensions.ToOriginal(action.OtherPlayer) > 0;
                if (condition)
                {
                    int count = StrategicMap.Current.Players.Count;
                    condition = PlayerIdExtensions.ToOriginal(action.OtherPlayer) < count;
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
            DiplomaticAction? action = Action;
            bool condition = PlayerIdExtensions.ToOriginal(action.Target) > 0;
            if (condition)
            {
                int count = StrategicMap.Current.Players.Count;
                condition = PlayerIdExtensions.ToOriginal(action.Target) < count;
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
        DiplomaticAction? action = Action;
        action?.EventLogId = Id;
    }

    // Diplomacy.pas:1414
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent? done)
    {
        bool show = trigger == ExecuteTrigger.User;
        if (StrategicMap.Current.SeatedPlayer == Action.Target || StrategicMap.Current.SeatedPlayer == Action.Source)
        {
            show = show || (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.Diplomacy) && trigger == ExecuteTrigger.NewTurn);
            show = show || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.Diplomacy));
        }

        show = show && (trigger == ExecuteTrigger.User || Action.Reaction != DiplomaticActionReaction.Ignore);
        if (show)
        {
            StrategicMap.Current.TriggerExecuteEventLog(this, immediate: mode, done);
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
        DiplomaticAction? old = Action;
        if (old != action)
        {
            _ = (old?.Release());

            Action = action?.Copy<DiplomaticAction>(GetEngine());
        }
    }

    public override int MiddleIcon => Action.Proposal.MiddleIcon(Action);

    private static readonly Color[] RaceColors = [new(48, 52, 224), new(255, 164, 102), new(44, 203, 175), new(135, 218, 254), new(65, 239, 96), new(253, 253, 21), new(228, 193, 165), new(253, 255, 247), new(188, 112, 179), new(233, 80, 80), new(187, 102, 66), new(152, 120, 121), new(168, 168, 168)];

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi P5DiplomaticExecute signature keeps data.")]
    internal static bool DiplomaticExecute(DiplomaticActionEventLog log, bool immediate, ExecuteTrigger trigger, object data, ExecuteDoneEvent done)
    {
        bool result = trigger == ExecuteTrigger.User;
        if (log.Action.Target == StrategicMap.Current.SeatedPlayer || log.Action.Source == StrategicMap.Current.SeatedPlayer)
        {
            result = result || (StrategicMap.Current.PlayMode == PlayMode.Turn && log.GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.Diplomacy) && trigger == ExecuteTrigger.NewTurn);
            result = result || (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && log.GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.Diplomacy));
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
