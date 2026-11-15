// Translated from Sign.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Commands;

// Text, HexX, HexY, HexLevel
public partial class UpdateSignTokenEvent : TokenEvent
{
    public UpdateSignTokenEvent(CustomNode owner) : base(owner)
    {
        Text = new StringList();
    }

    // Sign.pas:202
    public new const int ClassId = 131343;

    public StringList Text { get; }

    public sbyte HexX { get; set; }
    public sbyte HexY { get; set; }
    public sbyte HexLevel { get; set; }

    // Sign.pas:199
    public override void Dispose()
    {
        Text?.Dispose();
        base.Dispose();
    }

    // Sign.pas:205
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.Lines(persister, name: "Text", lines: Text);
        HexX = PersistSupport.UnsignedByte(persister, name: "HexX", value: HexX);
        HexY = PersistSupport.UnsignedByte(persister, name: "HexY", value: HexY);
        HexLevel = PersistSupport.UnsignedByte(persister, name: "HexLevel", value: HexLevel);
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
