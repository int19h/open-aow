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

public partial class TerminatePlayerTE : TokenEvent
{
    public TerminatePlayerTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131836;

    public sbyte FPlayer;
    public ExitGameCallBack FExitCallBack;
    public ExitGameMode FExitMode;

    // MoveT.PAS:3249
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 20);
    }

    // MoveT.PAS:3255
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        MessageEventLog Log = default;
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType = PlayerType.CPU3;
        var remotePCC = (RemotePCC)AoWHSMap.Current.FPlayerCommunicator.PCC;
        remotePCC.SetConnectionMode(FPlayer, ConnectionMode.Global);
        remotePCC.SetSystemID(FPlayer, 0);

        if (!AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FSingleProcessCycle)
        {
            AoWHSMap.Current.FPlayerControl.EndTurn(FPlayer);
        }

        AoWHSMap.Current.FPlayers.Update();
        if (AoWHSMap.Current.FSeatedPlayer != FPlayer)
        {
            Log = new MessageEventLog(null);
            Log.FButtons = MessageEventLog.Buttons.Ok;
            Log.FLayout = MessageEventLog.Layout.Normal;
            var name = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetName();
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(P5XLeftTheGameRStr), [name]);
            Log.FMessage.Add(format);
            // { Append the departure message, as in the original. }
            AoWHSMap.Current.TriggerExecuteEventLog(Log, false, null);
            Log.Release();
        }
        else
        {
            AoWHSMap.Current.ExecuteExitGame();
            if (FExitCallBack != null)
            {
                FExitCallBack(FExitMode, true);
            }
        }

        return ExecuteResult.Done;
    }

    private static readonly string P5XLeftTheGameRStr = "%s left the game!";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
