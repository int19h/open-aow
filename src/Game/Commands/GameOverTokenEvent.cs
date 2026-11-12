// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class GameOverTokenEvent : TokenEvent
{
    public GameOverTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135441;

    public sbyte Player;
    public GameOverStatus GameOverStatus;

    // MoveT.PAS:3233
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
        stream.Bytes(Bytes.Of(ref GameOverStatus, 1), 1, 21);
    }

    // MoveT.PAS:3240
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.PlayerControl.GameOver(Player, GameOverStatus);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
