// Translated from Sign.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Commands;

// Text, HexX, HexY, HexLevel
public partial class UpdateSignTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // Sign.pas:202
    public new const int ClassId = 131343;

    public DelphiStringList Text { get; } = new DelphiStringList();

    public MapCoordinate Location { get; set; }

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
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
    }

    // Sign.pas:196

    // Sign.pas:214
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Sign sign = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Sign>(131330);
        ExecuteResult result;
        if (sign is not null)
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
