// Translated from Sign.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Commands;

// FText, FXhx, FYhx, FLhx
public partial class UpdateSignTokenEvent : TokenEvent
{
    public UpdateSignTokenEvent(ECustomNode owner) : base(owner)
    {
        FText = new StringList();
    }

    // Sign.pas:202
    public new const int ClassID = 131343;

    public StringList FText;
    public sbyte FXhx;
    public sbyte FYhx;
    public sbyte FLhx;

    // Sign.pas:199
    public override void Dispose()
    {
        FText?.Dispose();
        base.Dispose();
    }

    // Sign.pas:205
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FText, 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 18);
    }

    // Sign.pas:196

    // Sign.pas:214
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result;
        Sign Sign;
        Sign = (Sign)AoWHSMap.Current.MapContainer.GetField(FXhx, FYhx, FLhx).FindOwnedChild(131330);
        if (Sign != null)
        {
            Sign.SetText(FText);
            Sign.FTextSeen.Clear();
            Sign.FTextSeen[AoWHSMap.Current.FCurrentPlayer] = true;
            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    // Sign.pas:54

}
