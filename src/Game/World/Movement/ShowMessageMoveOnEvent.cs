// Translated from MoveOnEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Movement;

public partial class ShowMessageMoveOnEvent : MoveOnEvent
{
    public ShowMessageMoveOnEvent(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
    }

    // MoveOnEvents.pas:346
    public new const int ClassID = 132429;

    public StringList FMessage;

    // MoveOnEvents.pas:340
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    // MoveOnEvents.pas:349
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FMessage, 40);
    }

    // MoveOnEvents.pas:334

    // MoveOnEvents.pas:355
    public override void MovedOnTrigger(sbyte Player)
    {
        ShowMessageEventLog Log;
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Log = new ShowMessageEventLog(null);
            var text = FMessage.Text;
            Log.FMessage.Text = text;
            AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
            Log.Release();
        }

        SetTriggerPlayer(Player, false);
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:375
        public new const int ClassID = 132430;

        // MoveOnEvents.pas:378
        public override int ResourceUserClassID() => 132429;

        // MoveOnEvents.pas:369
        public override string ResourceEditName() => AbilityRules.TranslateRStr(ShowMessageEventRStr);

        private static readonly string ShowMessageEventRStr = "Show Message Event";

    }

    // MoveOnEvents.pas:93

}
