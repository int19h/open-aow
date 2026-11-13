// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.AI.Groups;

// unit, ID, location, origin, destination, transporter ID, transport status, transport points,
// status, move result
public partial class AIMoveExecutorUnit : EngineObject
{
    public AIMoveExecutorUnit(ECustomNode owner) : base(owner)
    { }

    public Troop Troop { get; private set; }
    public int UnitId { get; private set; }

    public MapCoordinate Location;
    private MapCoordinate _origin;
    public MapCoordinate Destination;

    public int TransporterId { get; set; }
    public AIMoveExecutorTransportStatus TransportStatus { get; set; }

    public MapCoordinate BoardingPoint;
    // A path step with its cost (TransportMoveExecutorControl's meeting point). AIMoveExecutorArmy.CanAdd
    // compares it with the cost, as the Delphi four-byte comparison did.
    public PathStep ArrivalPoint;
    public MapCoordinate TransportSource;
    public MapCoordinate TransportDestination;

    public AIMoveExecutorUnitStatus Status { get; set; }
    public byte MoveResult { get; set; }

    // AIGroup.pas:4280
    public void Initialize(Troop troop, sbyte X, sbyte Y, sbyte L)
    {
        Troop = troop;
        UnitId = Troop.UnitId;
        Location = new MapCoordinate(X: X, Y: Y, MapLevel: L);
        _origin = new MapCoordinate(X: X, Y: Y, MapLevel: L);
        Destination = new MapCoordinate(X: -1, Y: -1, MapLevel: -1);
        ResetTransporterSettings();
        Status = AIMoveExecutorUnitStatus.Idle;
        MoveResult = 0;
    }

    // AIGroup.pas:4301
    public bool Validate()
    {
        bool result = StrategicMap.Current.UnitControl.FindUnit(UnitId) == Troop;
        if (result && !Troop.UnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (Status == AIMoveExecutorUnitStatus.Moving)
            {
                Troop.GetLocation(out var locationX, out var locationY, out var locationMapLevel);
                Location = new MapCoordinate(X: locationX, Y: locationY, MapLevel: locationMapLevel);
                Status = AIMoveExecutorUnitStatus.Ready;
            }
        }
        else
        {
            // { Original returns the identity check even for a killed unit. }
            Troop = null;
            Status = AIMoveExecutorUnitStatus.Lost;
        }

        return result;
    }

    // AIGroup.pas:4320
    public void ResetTransporterSettings()
    {
        BoardingPoint = BoardingPoint with { X = -1 };
        ArrivalPoint = ArrivalPoint with { Location = ArrivalPoint.Location with { X = -1 } };
        TransportSource = TransportSource with { X = -1 };
        TransportDestination = TransportDestination with { X = -1 };
    }

    // Engine.pas:524

}
