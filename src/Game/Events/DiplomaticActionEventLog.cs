// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Events;

// Action
public partial class DiplomaticActionEventLog : EventLog
{
    public DiplomaticActionEventLog(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1199
    public new const int ClassID = 131347;

    public DiplomaticAction FAction;

    // Diplomacy.pas:1170
    public override void Dispose()
    {
        DiplomaticAction Action;
        Action = FAction;
        if (Action != null)
        {
            Action.Release();
            FAction = null;
        }

        base.Dispose();
    }

    // Diplomacy.pas:1202
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FAction, null, 20);
    }

    // Diplomacy.pas:1182
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        FAction.MsgProc(ref Msg, Sender);
    }

    // Diplomacy.pas:1265
    public override string GetText() => FAction.Proposal.LogText(FAction);

    // Diplomacy.pas:1228
    public override int GetTextColor()
    {
        int Result;
        int Race;
        if (FAction.Proposal.SignedAsOtherPlayer)
        {
            bool condition = FAction.FOtherPlayer > 0;
            if (condition)
            {
                var count = AoWHSMap.Current.FPlayers.Count;
                condition = FAction.FOtherPlayer < count;
            }

            if (condition)
            {
                Race = AoWHSMap.Current.FPlayers.GetPlayers(FAction.FOtherPlayer).FPlayerRace;
            }
            else
            {
                Race = 12;
            }

            Result = P5RaceColors[Race];
        }
        else
        {
            if (FAction.FReaction == DiplomaticActionReaction.Accept)
            {
                bool condition = FAction.FTarget > 0;
                if (condition)
                {
                    var count = AoWHSMap.Current.FPlayers.Count;
                    condition = FAction.FTarget < count;
                }

                if (condition)
                {
                    Race = AoWHSMap.Current.FPlayers.GetPlayers(FAction.FTarget).FPlayerRace;
                }
                else
                {
                    Race = 12;
                }

                Result = P5RaceColors[Race];
            }
            else
            {
                if (AoWHSMap.Current.FSeatedPlayer != FAction.FSource)
                {
                    bool condition = FAction.FSource > 0;
                    if (condition)
                    {
                        var count = AoWHSMap.Current.FPlayers.Count;
                        condition = FAction.FSource < count;
                    }

                    if (condition)
                    {
                        Race = AoWHSMap.Current.FPlayers.GetPlayers(FAction.FSource).FPlayerRace;
                    }
                    else
                    {
                        Race = 12;
                    }

                    Result = P5RaceColors[Race];
                }
                else
                {
                    bool condition = FAction.FTarget > 0;
                    if (condition)
                    {
                        var count = AoWHSMap.Current.FPlayers.Count;
                        condition = FAction.FTarget < count;
                    }

                    if (condition)
                    {
                        Race = AoWHSMap.Current.FPlayers.GetPlayers(FAction.FTarget).FPlayerRace;
                    }
                    else
                    {
                        Race = 12;
                    }

                    Result = P5RaceColors[Race];
                }
            }
        }

        return Result;
    }

    // Diplomacy.pas:1333
    public override int GetLeftIcon()
    {
        int Result;
        DiplomaticAction Action;
        Action = FAction;
        if (!Action.Proposal.SignedAsOtherPlayer)
        {
            bool condition = Action.FSource > 0;
            if (condition)
            {
                var count = AoWHSMap.Current.FPlayers.Count;
                condition = Action.FSource < count;
            }

            if (condition)
            {
                Result = AoWHSMap.Current.FPlayers.GetPlayers(Action.FSource).FPlayerRace;
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            bool condition = Action.FOtherPlayer > 0;
            if (condition)
            {
                var count = AoWHSMap.Current.FPlayers.Count;
                condition = Action.FOtherPlayer < count;
            }

            if (condition)
            {
                Result = AoWHSMap.Current.FPlayers.GetPlayers(Action.FOtherPlayer).FPlayerRace;
            }
            else
            {
                Result = -1;
            }
        }

        return Result;
    }

    // Diplomacy.pas:1379
    // { DCC32 folds JMP-to-RET; keep the original branch over the 11-byte arms. }
    public override int GetRightIcon()
    {
        int Result;
        DiplomaticAction Action;
        Action = FAction;
        bool condition = Action.FTarget > 0;
        if (condition)
        {
            var count = AoWHSMap.Current.FPlayers.Count;
            condition = Action.FTarget < count;
        }

        if (condition)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(Action.FTarget).FPlayerRace;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Diplomacy.pas:1225
    public override bool GetVisible() => true;

    // Diplomacy.pas:1208
    public override void PlayLoggedSFX(EventLogbook Logbook) => AudioPlayback.PlaySample(FAction.Proposal.LoggedSample);

    // Diplomacy.pas:1217
    public override void NotifyLogged(EventLogbook Logbook)
    {
        DiplomaticAction Action;
        base.NotifyLogged(Logbook);
        Action = FAction;
        if (Action != null)
        {
            Action.FEventLogID = FID;
        }
    }

    // Diplomacy.pas:1414
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        bool Result = default;
        bool Show;
        Show = Trigger == ExecuteTrigger.User;
        if (AoWHSMap.Current.FSeatedPlayer == FAction.FTarget || AoWHSMap.Current.FSeatedPlayer == FAction.FSource)
        {
            Show = Show || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && Trigger == ExecuteTrigger.NewTurn;
            Show = Show || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy);
        }

        Show = Show && (Trigger == ExecuteTrigger.User || FAction.FReaction != DiplomaticActionReaction.Ignore);
        if (Show)
        {
            AoWHSMap.Current.TriggerExecuteEventLog(this, Mode, Done);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:1188
    public void SetDiplomaticAction(DiplomaticAction Action)
    {
        DiplomaticAction Old;
        Old = FAction;
        if (Old != Action)
        {
            if (Old != null)
            {
                Old.Release();
            }

            if (Action != null)
            {
                FAction = (DiplomaticAction)Action.Copy(GameTables.AoWEngine);
            }
            else
            {
                FAction = null;
            }
        }
    }

    public override int GetMiddleIcon() => FAction.Proposal.MiddleIcon(FAction);

    private static FixedArray_int_13 P5RaceColors = FixedArray_int_13.From([14693424, 6726911, 11520812, 16702087, 6352705, 1441277, 10863076, 16252925, 11759804, 5263593, 4351675, 7960728, 11053224]);

    internal static bool P5DiplomaticExecute(DiplomaticActionEventLog log, bool immediate, ExecuteTrigger trigger, object data, ExecuteDoneEvent done)
    {
        bool result = trigger == ExecuteTrigger.User;
        if (log.FAction.FTarget == AoWHSMap.Current.FSeatedPlayer || log.FAction.FSource == AoWHSMap.Current.FSeatedPlayer)
        {
            result = result || AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy) && trigger == ExecuteTrigger.NewTurn;
            result = result || AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Diplomacy);
        }
        result = result && (trigger == ExecuteTrigger.User || log.FAction.FReaction != DiplomaticActionReaction.Ignore);
        if (!result) return false;
        AoWHSMap.Current.TriggerExecuteEventLog(log, immediate, done);
        return true;
    }

}
