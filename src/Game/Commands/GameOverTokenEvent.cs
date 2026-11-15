// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class GameOverTokenEvent : TokenEvent
{
    public GameOverTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135441;

    public sbyte Player { get; set; }
    public GameOverStatus GameOverStatus { get; set; }

    // MoveT.PAS:3233
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        GameOverStatus = persister.Enum("GameOverStatus", value: GameOverStatus);
    }

    // MoveT.PAS:3240
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.PlayerControl.GameOver(Player, GameOverStatus);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
