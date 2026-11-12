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

    public Troop Troop;
    public int UnitId;
    public MapCoordinate Location;
    public MapCoordinate Origin;
    public MapCoordinate Destination;
    public int TransporterId;
    public AIMoveExecutorTransportStatus TransportStatus;
    public MapCoordinate BoardingPoint;
    public MapCoordinate ArrivalPoint;
    public MapCoordinate TransportSource;
    public MapCoordinate TransportDestination;
    public AIMoveExecutorUnitStatus Status;
    public byte MoveResult;

    // AIGroup.pas:4280
    public void Initialize(Troop troop, sbyte X, sbyte Y, sbyte L)
    {
        Troop = troop;
        UnitId = Troop.UnitId;
        Location.X = X;
        Location.Y = Y;
        Location.MapLevel = L;
        Location.Tag = 0;
        Origin.X = X;
        Origin.Y = Y;
        Origin.MapLevel = L;
        Origin.Tag = 0;
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
        bool result = AoWHSMap.Current.UnitControl.FindUnit(UnitId) == Troop;
        if (result && !Troop.UnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (Status == AIMoveExecutorUnitStatus.Moving)
            {
                Troop.GetLocation(ref Location.X, ref Location.Y, ref Location.MapLevel);
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
