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

    public MapCoordinate FLocation;
    public MapCoordinate FDestination;
    public AIMoveExecutorTransportStatus FTransportStatus;
    public MapCoordinate FBoardingPoint;
    public MapCoordinate FArrivalPoint;
    public MapCoordinate FTransportSource;
    public MapCoordinate FTransportDestination;
    public sbyte FPlayer;
    public bool FGrouped;

    // AIGroup.pas:4527
    public void Setup(AIMoveExecutorUnit Transporter)
    {
        FLocation.X = Transporter.FLocation.X;
        FLocation.Y = Transporter.FLocation.Y;
        FLocation.L = Transporter.FLocation.L;
        FLocation.Flag = 0;
        FDestination.X = Transporter.FDestination.X;
        FDestination.Y = Transporter.FDestination.Y;
        FDestination.L = Transporter.FDestination.L;
        FDestination.Flag = 0;
        FGrouped = false;
        FPlayer = Transporter.FUnit.FPlayer;
        FTransportStatus = Transporter.FTransportStatus;
        FBoardingPoint = Transporter.FBoardingPoint;
        FArrivalPoint = Transporter.FArrivalPoint;
        FTransportSource = Transporter.FTransportSource;
        FTransportDestination = Transporter.FTransportDestination;
        AddChild(Transporter);
    }

    // AIGroup.pas:4547
    public bool CanAdd(AIMoveExecutorUnit UnitEntry) => Unsafe.As<MapCoordinate, int>(ref FLocation) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FLocation) && Unsafe.As<MapCoordinate, int>(ref FDestination) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FDestination) && FTransportStatus == UnitEntry.FTransportStatus && (FTransportStatus == AIMoveExecutorTransportStatus.Done || (Unsafe.As<MapCoordinate, int>(ref FBoardingPoint) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FBoardingPoint) && Unsafe.As<MapCoordinate, int>(ref FArrivalPoint) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FArrivalPoint) && Unsafe.As<MapCoordinate, int>(ref FTransportSource) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FTransportSource) && Unsafe.As<MapCoordinate, int>(ref FTransportDestination) == Unsafe.As<MapCoordinate, int>(ref UnitEntry.FTransportDestination)));

    // AIGroup.pas:4559
    public void RetrieveArmy(byte Selection, ref ArmyHexSprite Army, ref byte ArmySelection)
    {
        int Index;
        Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FLocation.X, FLocation.Y, FLocation.L)).GetArmyHS();
        if (Army != null)
        {
            ArmySelection = 0;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if ((Selection & 1) != 0)
                {
                    var units = GetUnits(I);
                    Index = Army.FArmy.FindChildIndex(units.FUnit);
                    if (Index != -1)
                    {
                        ArmySelection = (byte)(ArmySelection | (1 << Index));
                    }
                    else
                    {
                        throw new KeyNotFoundException("Unit missing!");
                    }
                }

                Selection = (byte)(int)(((uint)Selection) >> 1);
            }
        }
        else
        {
            ArmySelection = 0;
            Army = null;
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
    public bool WaitingForTransport() => Unsafe.As<MapCoordinate, int>(ref FLocation) == Unsafe.As<MapCoordinate, int>(ref FBoardingPoint);

    // AIGroup.pas:4599
    public int Moves(byte Selection)
    {
        int Result = 65535;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                if (Result > GetUnits(I).FUnit.GetMoves())
                {
                    Result = GetUnits(I).FUnit.GetMoves();
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (Result == 65535)
        {
            Result = -1;
        }

        return Result;
    }

    // AIGroup.pas:4613
    public int MovePoints(byte Selection)
    {
        int Result = 65535;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                if (Result > GetUnits(I).FUnit.GetMovePoints())
                {
                    Result = GetUnits(I).FUnit.GetMovePoints();
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (Result == 65535)
        {
            Result = -1;
        }

        return Result;
    }

    // AIGroup.pas:4627
    public byte ValidTerrain(byte Selection, sbyte TerrainType, sbyte OverlayType)
    {
        bool All;
        byte Result = 0;
        All = true;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (((1 << I) & Selection) != 0)
            {
                if (!GetUnits(I).FUnit.ValidTerrain(TerrainType, OverlayType))
                {
                    All = false;
                }
                else
                {
                    Result = (byte)(Result | (1 << I));
                }
            }
        }

        if (All)
        {
            Result = Selection;
        }

        return Result;
    }

    // Engine.pas:631

}
