#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// location, destination, transport status, transport points, player, grouped
public sealed class AIMoveExecutorArmy(CustomNode? owner) : AIMoveExecutorUnitList(owner)
{
    public MapCoordinate Location { get; private set; }

    private MapCoordinate _destination;

    public AIMoveExecutorTransportStatus TransportStatus { get; private set; }

    public MapCoordinate BoardingPoint { get; private set; }
    // A path step with its cost, see AIMoveExecutorUnit.ArrivalPoint.
    public PathStep ArrivalPoint { get; private set; }
    public MapCoordinate TransportSource { get; private set; }
    public MapCoordinate TransportDestination { get; private set; }

    public PlayerId? Player { get; private set; } = PlayerId.Independent;

    // AIGroup.pas:4527
    public void Setup(AIMoveExecutorUnit? transporter)
    {
        Location = new MapCoordinate(X: transporter.Location.X, Y: transporter.Location.Y, MapLevel: transporter.Location.MapLevel);
        _destination = new MapCoordinate(X: transporter.Destination.X, Y: transporter.Destination.Y, MapLevel: transporter.Destination.MapLevel);
        Player = transporter.Troop.Player;
        TransportStatus = transporter.TransportStatus;
        BoardingPoint = transporter.BoardingPoint;
        ArrivalPoint = transporter.ArrivalPoint;
        TransportSource = transporter.TransportSource;
        TransportDestination = transporter.TransportDestination;
        _ = AddChild(transporter);
    }

    // AIGroup.pas:4547
    public bool CanAdd(AIMoveExecutorUnit unitEntry) => Location == unitEntry.Location && _destination == unitEntry.Destination && TransportStatus == unitEntry.TransportStatus && (TransportStatus == AIMoveExecutorTransportStatus.Done || (BoardingPoint == unitEntry.BoardingPoint && ArrivalPoint == unitEntry.ArrivalPoint && TransportSource == unitEntry.TransportSource && TransportDestination == unitEntry.TransportDestination));

    // AIGroup.pas:4559
    public void RetrieveArmy(int selection, out ArmyHexSprite? army, out int armySelection)
    {
        army = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetArmyHexSprite();
        if (army is not null)
        {
            armySelection = 0;
            foreach (AIMoveExecutorUnit unit in this)
            {
                if ((selection & 1) != 0)
                {
                    AIMoveExecutorUnit units = unit;
                    int index = army.Army.FindChildIndex(units.Troop);
                    armySelection = index != -1 ? (armySelection | (1 << index)).TruncateToByte() : throw new KeyNotFoundException("Unit missing!");
                }

                selection >>>= 1;
            }
        }
        else
        {
            armySelection = 0;
            army = null;
        }
    }

    // AIGroup.pas:4587
    public void ResetTransporterSettings()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            GetUnits(i).ResetTransporterSettings();
        }
    }

    // AIGroup.pas:4594
    public bool WaitingForTransport() => Location == BoardingPoint;

    // AIGroup.pas:4599
    public int Moves(int selection)
    {
        int result = 65_535;
        foreach (AIMoveExecutorUnit unit in this)
        {
            if ((selection & 1) != 0)
            {
                if (result > unit.Troop.Moves)
                {
                    result = unit.Troop.Moves;
                }
            }

            selection >>>= 1;
        }

        return result == 65_535 ? -1 : result;
    }

    // AIGroup.pas:4613
    public int MovePoints(int selection)
    {
        int result = 65_535;
        foreach (AIMoveExecutorUnit unit in this)
        {
            if ((selection & 1) != 0)
            {
                if (result > unit.Troop.GetMovePoints())
                {
                    result = unit.Troop.GetMovePoints();
                }
            }

            selection >>>= 1;
        }

        return result == 65_535 ? -1 : result;
    }

    // AIGroup.pas:4627
    public int ValidTerrain(int selection, int terrainType, int overlayType)
    {
        int result = 0;
        bool all = true;
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (((1 << i) & selection) != 0)
            {
                if (!GetUnits(i).Troop.ValidTerrain(terrainType, overlayType))
                {
                    all = false;
                }
                else
                {
                    result = (result | (1 << i)).TruncateToByte();
                }
            }
        }

        return all ? selection : result;
    }
}
