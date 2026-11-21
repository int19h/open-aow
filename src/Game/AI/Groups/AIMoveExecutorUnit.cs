#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.AI.Groups;

// unit, ID, location, origin, destination, transporter ID, transport status, transport points,
// status, move result
public sealed class AIMoveExecutorUnit(CustomNode owner) : EngineObject(owner)
{
    public Troop Troop { get; private set; }
    public int UnitId { get; private set; }

    public MapCoordinate Location { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private MapCoordinate _origin;

    public MapCoordinate Destination { get; private set; }

    public int TransporterId { get; set; }
    public AIMoveExecutorTransportStatus TransportStatus { get; set; }

    public MapCoordinate BoardingPoint { get; internal set; }
    // A path step with its cost (TransportMoveExecutorControl's meeting point). AIMoveExecutorArmy.CanAdd
    // compares it with the cost, as the Delphi four-byte comparison did.
    public PathStep ArrivalPoint { get; internal set; }
    public MapCoordinate TransportSource { get; internal set; }
    public MapCoordinate TransportDestination { get; internal set; }

    public AIMoveExecutorUnitStatus Status { get; set; }
    public byte MoveResult { get; set; }

    // AIGroup.pas:4280
    public void Initialize(Troop troop, sbyte X, sbyte Y, sbyte L)
    {
        Troop = troop;
        UnitId = Troop.UnitId;
        Location = new MapCoordinate(X, Y, L);
        _origin = new MapCoordinate(X, Y, L);
        Destination = new MapCoordinate(-1, -1, -1);
        ResetTransporterSettings();
        Status = AIMoveExecutorUnitStatus.Idle;
        MoveResult = 0;
    }

    // AIGroup.pas:4301
    public bool Validate()
    {
        bool result = StrategicMap.Current.UnitControl.FindUnit(UnitId) == Troop;
        if (result && !Troop.UnitStatus.Contains(UnitStatuses.Killed))
        {
            if (Status == AIMoveExecutorUnitStatus.Moving)
            {
                MapCoordinate coordinate105 = Troop.GetLocation();
                sbyte locationX = coordinate105.X;
                sbyte locationY = coordinate105.Y;
                sbyte locationMapLevel = coordinate105.MapLevel;
                Location = new MapCoordinate(locationX, locationY, locationMapLevel);
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
}
