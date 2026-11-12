// Translated from Sign.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Commands;

// Text, HexX, HexY, HexLevel
public partial class UpdateSignTokenEvent : TokenEvent
{
    public UpdateSignTokenEvent(ECustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

    // Sign.pas:202
    public new const int ClassId = 131343;

    public StringList Text { get; }

    public sbyte HexX;
    public sbyte HexY;
    public sbyte HexLevel;

    // Sign.pas:199
    public override void Dispose()
    {
        Text?.Dispose();
        base.Dispose();
    }

    // Sign.pas:205
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStrings(Text, 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexX), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexY), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref HexLevel), 18);
    }

    // Sign.pas:196

    // Sign.pas:214
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        Sign sign;
        sign = (Sign)StrategicMap.Current.MapContainer.GetField(HexX, HexY, HexLevel).FindOwnedChild(131330);
        if (sign != null)
        {
            sign.SetText(Text);
            sign.TextSeen.Clear();
            sign.TextSeen[StrategicMap.Current.CurrentPlayer] = true;
            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // Sign.pas:54

}
