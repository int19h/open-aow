// Translated from MoveOnEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Audio;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Movement;

public partial class ChangeSongMoveOnEvent : MoveOnEvent
{
    public ChangeSongMoveOnEvent(ECustomNode owner) : base(owner)
    { }

    // MoveOnEvents.pas:277
    public new const int ClassID = 132427;

    public string FSongFileName = "";

    // MoveOnEvents.pas:280
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FSongFileName, 40);
        }
    }

    // MoveOnEvents.pas:286
    public override void MovedOnTrigger(sbyte Player)
    {
        // Use the map's seated player.
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            if (CompareText(PlayingSongFileName(), FSongFileName) != 0)
            {
                AudioPlayback.PlaySongEx(FSongFileName, 2, 0, false);
            }
        }

        SetTriggerPlayer(Player, false);
    }

    private static string PlayingSongFileName()
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.GetPlayingSongFileName();
        }
        return "";
    }

    public new partial class Resource : MoveOnEventResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // MoveOnEvents.pas:301
        public new const int ClassID = 132428;

        // MoveOnEvents.pas:304
        public override int ResourceUserClassID() => 132427;

        // MoveOnEvents.pas:295
        public override string ResourceEditName() => AbilityRules.TranslateRStr(ChangeSongEventRStr);

        private static readonly string ChangeSongEventRStr = "Change Song Event";

    }

}
