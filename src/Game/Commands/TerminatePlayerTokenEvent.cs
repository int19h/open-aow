// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class TerminatePlayerTokenEvent : TokenEvent
{
    public TerminatePlayerTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131836;

    public sbyte Player;
    public ExitGameCallBack ExitCallback;
    public ExitGameMode ExitMode;

    // MoveT.PAS:3249
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 20);
    }

    // MoveT.PAS:3255
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        MessageEventLog log = default;
        AoWHSMap.Current.Players.GetPlayers(Player).PlayerType = PlayerType.Cpu3;
        var remotePlayerCommunicationControl = (RemotePlayerCommunicationControl)AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl;
        remotePlayerCommunicationControl.SetConnectionMode(Player, ConnectionMode.Global);
        remotePlayerCommunicationControl.SetSystemId(Player, 0);

        if (!AoWHSMap.Current.Players.GetPlayers(Player).SingleProcessCycle)
        {
            AoWHSMap.Current.PlayerControl.EndTurn(Player);
        }

        AoWHSMap.Current.Players.Update();
        if (AoWHSMap.Current.SeatedPlayer != Player)
        {
            log = new MessageEventLog(null);
            log.Buttons = MessageEventLogButtons.Ok;
            log.Layout = MessageEventLogLayout.Normal;
            var name = AoWHSMap.Current.Players.GetPlayers(Player).GetName();
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_leftGameFormat), [name]);
            log.Message.Add(format);
            // { Append the departure message, as in the original. }
            AoWHSMap.Current.TriggerExecuteEventLog(log, false, null);
            log.Release();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
            if (ExitCallback != null)
            {
                ExitCallback(ExitMode, true);
            }
        }

        return ExecuteResult.Done;
    }

    private static readonly string s_leftGameFormat = "%s left the game!";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
