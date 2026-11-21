#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class TerminatePlayerTokenEvent(CustomNode? owner) : TokenEvent(owner)
{
    public new const int ClassId = 131_836;

    public sbyte Player { get; set; }

    public ExitGameCallBack? ExitCallback { get; init; }
    public ExitGameMode ExitMode { get; init; }

    // MoveT.PAS:3249
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.UnsignedByte(name: "Player", value: Player);
    }

    // MoveT.PAS:3255
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        StrategicMap.Current.Players.GetPlayers(Player).PlayerType = PlayerType.Cpu3;
        RemotePlayerCommunicationControl? remotePlayerCommunicationControl = (RemotePlayerCommunicationControl?)StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        remotePlayerCommunicationControl.SetConnectionMode(Player, mode: ConnectionMode.Global);
        remotePlayerCommunicationControl.SetSystemId(Player, systemId: 0);

        if (!StrategicMap.Current.Players.GetPlayers(Player).SingleProcessCycle)
        {
            StrategicMap.Current.PlayerControl.EndTurn(Player);
        }

        StrategicMap.Current.Players.Update();
        if (StrategicMap.Current.SeatedPlayer != Player)
        {
            MessageEventLog log = new(null, holder: CreationHolder())
            {
                Buttons = MessageEventLogButtons.Ok,
                Layout = MessageEventLogLayout.Normal
            };
            string name = StrategicMap.Current.Players.GetPlayers(Player).GetName();
            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_leftGameFormat), arguments: [name]);
            _ = log.Message.Add(format);
            // { Append the departure message, as in the original. }
            StrategicMap.Current.TriggerExecuteEventLog(log, immediate: false, done: null);
            _ = log.Release();
        }
        else
        {
            StrategicMap.Current.ExecuteExitGame();
            ExitCallback?.Invoke(ExitMode, Exited: true);
        }

        return ExecuteResult.Done;
    }

    private static readonly string s_leftGameFormat = "%s left the game!";
}
