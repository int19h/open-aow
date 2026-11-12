// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

// location, destination, transport status, transport points, player, grouped
public partial class AIMoveExecutorArmy : AIMoveExecutorUnitList
{
    public AIMoveExecutorArmy(ECustomNode owner) : base(owner)
    { }

    public MapCoordinate Location;
    private MapCoordinate _destination;

    public AIMoveExecutorTransportStatus TransportStatus { get; private set; }

    public MapCoordinate BoardingPoint;
    public MapCoordinate ArrivalPoint;
    public MapCoordinate TransportSource;
    public MapCoordinate TransportDestination;

    public sbyte Player { get; private set; }

    // AIGroup.pas:4527
    public void Setup(AIMoveExecutorUnit transporter)
    {
        Location.X = transporter.Location.X;
        Location.Y = transporter.Location.Y;
        Location.MapLevel = transporter.Location.MapLevel;
        Location.Tag = 0;
        _destination.X = transporter.Destination.X;
        _destination.Y = transporter.Destination.Y;
        _destination.MapLevel = transporter.Destination.MapLevel;
        _destination.Tag = 0;
        Player = transporter.Troop.Player;
        TransportStatus = transporter.TransportStatus;
        BoardingPoint = transporter.BoardingPoint;
        ArrivalPoint = transporter.ArrivalPoint;
        TransportSource = transporter.TransportSource;
        TransportDestination = transporter.TransportDestination;
        AddChild(transporter);
    }

    // AIGroup.pas:4547
    public bool CanAdd(AIMoveExecutorUnit unitEntry) => Unsafe.As<MapCoordinate, int>(ref Location) == Unsafe.As<MapCoordinate, int>(ref unitEntry.Location) && Unsafe.As<MapCoordinate, int>(ref _destination) == Unsafe.As<MapCoordinate, int>(ref unitEntry.Destination) && TransportStatus == unitEntry.TransportStatus && (TransportStatus == AIMoveExecutorTransportStatus.Done || (Unsafe.As<MapCoordinate, int>(ref BoardingPoint) == Unsafe.As<MapCoordinate, int>(ref unitEntry.BoardingPoint) && Unsafe.As<MapCoordinate, int>(ref ArrivalPoint) == Unsafe.As<MapCoordinate, int>(ref unitEntry.ArrivalPoint) && Unsafe.As<MapCoordinate, int>(ref TransportSource) == Unsafe.As<MapCoordinate, int>(ref unitEntry.TransportSource) && Unsafe.As<MapCoordinate, int>(ref TransportDestination) == Unsafe.As<MapCoordinate, int>(ref unitEntry.TransportDestination)));

    // AIGroup.pas:4559
    public void RetrieveArmy(byte selection, out ArmyHexSprite army, out byte armySelection)
    {
        int index;
        army = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetArmyHexSprite();
        if (army != null)
        {
            armySelection = 0;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if ((selection & 1) != 0)
                {
                    var units = GetUnits(I);
                    index = army.Army.FindChildIndex(units.Troop);
                    if (index != -1)
                    {
                        armySelection = (byte)(armySelection | (1 << index));
                    }
                    else
                    {
                        throw new KeyNotFoundException("Unit missing!");
                    }
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
        for (int I = Count - 1; I >= 0; I--)
        {
            GetUnits(I).ResetTransporterSettings();
        }
    }

    // AIGroup.pas:4594
    public bool WaitingForTransport() => Unsafe.As<MapCoordinate, int>(ref Location) == Unsafe.As<MapCoordinate, int>(ref BoardingPoint);

    // AIGroup.pas:4599
    public int Moves(byte selection)
    {
        int result = 65535;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                if (result > GetUnits(I).Troop.GetMoves())
                {
                    result = GetUnits(I).Troop.GetMoves();
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        if (result == 65535)
        {
            result = -1;
        }

        return result;
    }

    // AIGroup.pas:4613
    public int MovePoints(byte selection)
    {
        int result = 65535;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                if (result > GetUnits(I).Troop.GetMovePoints())
                {
                    result = GetUnits(I).Troop.GetMovePoints();
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        if (result == 65535)
        {
            result = -1;
        }

        return result;
    }

    // AIGroup.pas:4627
    public byte ValidTerrain(byte selection, sbyte terrainType, sbyte overlayType)
    {
        bool all;
        byte result = 0;
        all = true;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (((1 << I) & selection) != 0)
            {
                if (!GetUnits(I).Troop.ValidTerrain(terrainType, overlayType))
                {
                    all = false;
                }
                else
                {
                    result = (byte)(result | (1 << I));
                }
            }
        }

        if (all)
        {
            result = selection;
        }

        return result;
    }

    // Engine.pas:631

}
