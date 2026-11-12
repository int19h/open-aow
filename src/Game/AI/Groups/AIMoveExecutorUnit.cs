// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

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
    public MapCoordinate ArrivalPoint;
    public MapCoordinate TransportSource;
    public MapCoordinate TransportDestination;

    public AIMoveExecutorUnitStatus Status { get; set; }
    public byte MoveResult { get; set; }

    // AIGroup.pas:4280
    public void Initialize(Troop troop, sbyte X, sbyte Y, sbyte L)
    {
        Troop = troop;
        UnitId = Troop.UnitId;
        Location.X = X;
        Location.Y = Y;
        Location.MapLevel = L;
        Location.Tag = 0;
        _origin.X = X;
        _origin.Y = Y;
        _origin.MapLevel = L;
        _origin.Tag = 0;
        Destination.X = -1;
        Destination.Y = -1;
        Destination.MapLevel = -1;
        Destination.Tag = 0;
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
                Troop.GetLocation(out Location.X, out Location.Y, out Location.MapLevel);
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
        BoardingPoint.X = -1;
        ArrivalPoint.X = -1;
        TransportSource.X = -1;
        TransportDestination.X = -1;
    }

    // Engine.pas:524

}
