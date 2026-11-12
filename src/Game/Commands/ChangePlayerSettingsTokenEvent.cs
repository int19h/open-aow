// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// player, settings string
public partial class ChangePlayerSettingsTokenEvent : TokenEvent
{
    public ChangePlayerSettingsTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135974;

    public sbyte Player;
    public string Settings = "";

    // MoveT.PAS:1891
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 30);
        stream.String(ref Settings, 31);
    }

    // MoveT.PAS:1898
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.Players.GetPlayers(Player).Settings = Settings;
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
