#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from Sign.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Commands;

// Text, HexX, HexY, HexLevel
public class UpdateSignTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // Sign.pas:202
    public new const int ClassId = 131_343;

    public DelphiStringList Text { get; } = [];

    public MapCoordinate Location { get; set; }

    // Sign.pas:199
    public override void Dispose() => base.Dispose();

    // Sign.pas:205
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Lines(name: "Text", lines: Text);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Byte);
    }

    // Sign.pas:214
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        Sign sign = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Sign>(Sign.ClassId);
        if (sign is not null)
        {
            sign.SetText(Text);
            sign.TextSeen.Clear();
            sign.TextSeen[PlayerIdExtensions.ToOriginal(StrategicMap.Current.CurrentPlayer)] = true;
            return ExecuteResult.Done;
        }
        else
        {
            return ExecuteResult.DoneHalt;
        }
    }
}
