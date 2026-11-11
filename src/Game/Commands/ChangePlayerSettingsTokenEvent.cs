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

    public new const int ClassID = 135974;

    public sbyte FPlayer;
    public string FSettings = "";

    // MoveT.PAS:1891
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 30);
        Stream.rwString(ref FSettings, 31);
    }

    // MoveT.PAS:1898
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FSettings = FSettings;
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
