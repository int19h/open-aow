#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Commands;

public class StructureTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_440;

    private MapCoordinate Location { get; set; }

    public PlayerId? Player { get; private set; } = PlayerId.Independent;

    private int _structureClassId;

    public int Action { get; set; }

    private bool _razed;

    public int Param { get; set; }

    // Structur.PAS:3051
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Action = persister.Int32("Action", value: Action);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Byte);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
        _structureClassId = persister.Int32("StructureClassId", value: _structureClassId);
        _razed = persister.Boolean("Razed", value: _razed);
        Param = persister.Int32("Param", value: Param);
        // Delphi TStructureTE.FParam2 (id 23) is always 0 (omitted) and never read.
        persister.Skip("Param2");
    }

    // Structur.PAS:3077
    // Validate runs on the first execution only, as the original's short circuit has it.
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        bool valid = tokenEvent.ExecuteCount != 0;
        if (!valid)
        {
            valid = Validate();
            tokenEvent.ReportValidated(this, isValid: valid);
        }

        return valid
            ? GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild<Structure>(_structureClassId).ExecuteTokenEvent(this, executor: tokenEvent)
            : ExecuteResult.DoneHalt;
    }

    // Structur.PAS:3065
    public override bool Validate()
    {
        MapField mapField = GetEngine().StrategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        Structure structure = mapField.FindOwnedChild<Structure>(_structureClassId);
        return structure is not null && structure.EngineClass.ClassId == _structureClassId && mapField.GetPlayer() == Player;
    }

    // Structur.PAS:2537
    public void Initialize(int classId, int x, int y, int mapLevel)
    {
        {
            Location = Location with { X = (sbyte)x };
            Location = Location with { Y = (sbyte)y };
        }
        Location = Location with { MapLevel = (sbyte)mapLevel };
        StrategicMap strategicMap = GetEngine().StrategicMap;
        MapField mapField = strategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        Structure structure = mapField.FindOwnedChild<Structure>(classId);
        Assert(classId == structure.EngineClass.ClassId, message: "Invalid position (StructureTE)", file: "Structur.PAS", line: 3045);
        _structureClassId = classId;
        _razed = structure.GetRazed();
        Player = strategicMap.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetPlayer();
    }
}
