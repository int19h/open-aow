// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class GameOverTE : TokenEvent
{
    public GameOverTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135441;

    public sbyte FPlayer;
    public GameOverStatus FGameOverStatus;

    // MoveT.PAS:3233
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 20);
        Stream.rwData(Bytes.Of(ref FGameOverStatus, 1), 1, 21);
    }

    // MoveT.PAS:3240
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        AoWHSMap.Current.FPlayerControl.GameOver(FPlayer, FGameOverStatus);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
