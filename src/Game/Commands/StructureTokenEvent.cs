// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class StructureTokenEvent : TokenEvent
{
    public StructureTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135440;

    private MapCoordinate Location { get; set; }

    public sbyte Player { get; private set; }

    private int _structureClassId;

    public int Action { get; set; }

    private bool _razed;

    public int Param { get; set; }

    private int _param2;

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
        _param2 = persister.Int32("Param2", value: _param2);
    }

    // Structur.PAS:3077
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = tokenEvent.ExecuteCount != 0 || Validate()
            ? ((Structure)StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindOwnedChild(_structureClassId)).ExecuteTokenEvent(this, tokenEvent)
            : ExecuteResult.DoneHalt;
        return result;
    }

    // Structur.PAS:3065
    public override bool Validate()
    {
        bool result;
        Structure structure;
        MapField mapField;
        mapField = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        structure = (Structure)mapField.FindOwnedChild(_structureClassId);
        result = structure != null && structure.EngineClass.ClassId == _structureClassId && mapField.GetPlayer() == Player;

        return result;
    }

    // Structur.PAS:2537
    public void Initialize(int classId, sbyte x, sbyte y, sbyte mapLevel)
    {
        MapField mapField;
        Structure structure;
        {
            Location = Location with { X = x };
            Location = Location with { Y = y };
        }
        Location = Location with { MapLevel = mapLevel };
        mapField = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        structure = (Structure)mapField.FindOwnedChild(classId);
        Assert(classId == structure.EngineClass.ClassId, "Invalid position (StructureTE)", "Structur.PAS", 3045);
        _structureClassId = classId;
        _razed = structure.GetRazed();
        Player = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetPlayer();
    }

    // NetworkE.pas:230

}
