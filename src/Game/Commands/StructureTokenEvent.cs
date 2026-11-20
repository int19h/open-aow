// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Commands;

public class StructureTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_440;

    private MapCoordinate Location { get; set; }

    public sbyte Player { get; private set; }

    private int _structureClassId;

    public int Action { get; set; }

    private bool _razed;

    public int Param { get; set; }

    // Structur.PAS:3051
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Action = persister.Int32("Action", value: Action);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        _structureClassId = persister.Int32("StructureClassId", value: _structureClassId);
        _razed = persister.Boolean("Razed", value: _razed);
        Param = persister.Int32("Param", value: Param);
        // Delphi TStructureTE.FParam2 (id 23) is always 0 (omitted) and never read.
        persister.Skip("Param2");
    }

    // Structur.PAS:3077
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        return tokenEvent.ExecuteCount != 0 || Validate()
            ? StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Structure>(_structureClassId).ExecuteTokenEvent(this, executor: tokenEvent)
            : ExecuteResult.DoneHalt;
    }

    // Structur.PAS:3065
    public override bool Validate()
    {
        MapField mapField = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        Structure structure = mapField.FindOwnedChild<Structure>(_structureClassId);
        return structure is not null && structure.EngineClass.ClassId == _structureClassId && mapField.GetPlayer() == Player;
    }

    // Structur.PAS:2537
    public void Initialize(int classId, sbyte x, sbyte y, sbyte mapLevel)
    {
        {
            Location = Location with { X = x };
            Location = Location with { Y = y };
        }
        Location = Location with { MapLevel = mapLevel };
        MapField mapField = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        Structure structure = mapField.FindOwnedChild<Structure>(classId);
        Assert(classId == structure.EngineClass.ClassId, message: "Invalid position (StructureTE)", file: "Structur.PAS", line: 3045);
        _structureClassId = classId;
        _razed = structure.GetRazed();
        Player = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetPlayer();
    }
}
