// Translated from MoveOnEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

public partial class PlaySampleMoveOnEvent : MoveOnEvent
{
    public PlaySampleMoveOnEvent(ECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:307
    public new const int ClassID = 132445;

    public string FSampleFileName = "";

    // MoveOnEvents.pas:310
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSampleFileName, 40);
        }
    }

    // MoveOnEvents.pas:316
    public override void MovedOnTrigger(sbyte Player)
    {
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            AudioPlayback.PlaySample(FSampleFileName);
        }

        SetTriggerPlayer(Player, false);
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:328
        public new const int ClassID = 132446;

        // MoveOnEvents.pas:331
        public override int ResourceUserClassID() => 132445;

        // MoveOnEvents.pas:322
        public override string ResourceEditName() => AbilityRules.TranslateRStr(PlaySampleEventRStr);

        private static readonly string PlaySampleEventRStr = "Play Sample";

    }

}
