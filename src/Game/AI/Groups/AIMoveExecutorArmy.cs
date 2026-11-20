#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.AI.Groups;

// location, destination, transport status, transport points, player, grouped
public class AIMoveExecutorArmy(CustomNode owner) : AIMoveExecutorUnitList(owner)
{
    public MapCoordinate Location { get; private set; }

    private MapCoordinate _destination;

    public AIMoveExecutorTransportStatus TransportStatus { get; private set; }

    public MapCoordinate BoardingPoint { get; private set; }
    // A path step with its cost, see AIMoveExecutorUnit.ArrivalPoint.
    public PathStep ArrivalPoint { get; internal set; }
    public MapCoordinate TransportSource { get; private set; }
    public MapCoordinate TransportDestination { get; internal set; }

    public sbyte Player { get; private set; }

    // AIGroup.pas:4527
    public void Setup(AIMoveExecutorUnit transporter)
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
    public void RetrieveArmy(byte selection, out ArmyHexSprite army, out byte armySelection)
    {
        army = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetArmyHexSprite();
        if (army is not null)
        {
            armySelection = 0;
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    AIMoveExecutorUnit units = GetUnits(i);
                    int index = army.Army.FindChildIndex(units.Troop);
                    armySelection = index != -1 ? (byte)(armySelection | (1 << index)) : throw new KeyNotFoundException("Unit missing!");
                }

                selection = (byte)(int)(((uint)selection) >> 1);
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
    public int Moves(byte selection)
    {
        int result = 65535;
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                if (result > GetUnits(i).Troop.GetMoves())
                {
                    result = GetUnits(i).Troop.GetMoves();
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result == 65535 ? -1 : result;
    }

    // AIGroup.pas:4613
    public int MovePoints(byte selection)
    {
        int result = 65535;
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                if (result > GetUnits(i).Troop.GetMovePoints())
                {
                    result = GetUnits(i).Troop.GetMovePoints();
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result == 65535 ? -1 : result;
    }

    // AIGroup.pas:4627
    public byte ValidTerrain(byte selection, sbyte terrainType, sbyte overlayType)
    {
        byte result = 0;
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
                    result = (byte)(result | (1 << i));
                }
            }
        }

        return all ? selection : result;
    }
}
