// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// player, settings string
public class ChangePlayerSettingsTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135974;

    public sbyte Player { get; set; }
    public string Settings { get; set; } = "";

    // MoveT.PAS:1891
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Settings = persister.String("Settings", value: Settings);
    }

    // MoveT.PAS:1898
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.Players.GetPlayers(Player).Settings = Settings;
        return ExecuteResult.Done;
    }
}
