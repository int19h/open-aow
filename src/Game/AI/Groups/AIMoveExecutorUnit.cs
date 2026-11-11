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

    public Troop FUnit;
    public int FUnitID;
    public MapCoordinate FLocation;
    public MapCoordinate FOrigin;
    public MapCoordinate FDestination;
    public int FTransporterID;
    public AIMoveExecutorTransportStatus FTransportStatus;
    public MapCoordinate FBoardingPoint;
    public MapCoordinate FArrivalPoint;
    public MapCoordinate FTransportSource;
    public MapCoordinate FTransportDestination;
    public AIMoveExecutorUnitStatus FStatus;
    public byte FMoveResult;

    // AIGroup.pas:4280
    public void Initialize(Troop AUnit, sbyte X, sbyte Y, sbyte L)
    {
        FUnit = AUnit;
        FUnitID = FUnit.FUnitID;
        FLocation.X = X;
        FLocation.Y = Y;
        FLocation.L = L;
        FLocation.Flag = 0;
        FOrigin.X = X;
        FOrigin.Y = Y;
        FOrigin.L = L;
        FOrigin.Flag = 0;
        FDestination.X = -1;
        FDestination.Y = -1;
        FDestination.L = -1;
        FDestination.Flag = 0;
        ResetTransporterSettings();
        FStatus = AIMoveExecutorUnitStatus.Idle;
        FMoveResult = 0;
    }

    // AIGroup.pas:4301
    public bool Validate()
    {
        bool Result = AoWHSMap.Current.FUnitControl.FindUnit(FUnitID) == FUnit;
        if (Result && !FUnit.FUnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (FStatus == AIMoveExecutorUnitStatus.Moving)
            {
                FUnit.GetLocation(ref FLocation.X, ref FLocation.Y, ref FLocation.L);
                FStatus = AIMoveExecutorUnitStatus.Ready;
            }
        }
        else
        {
            // { Original returns the identity check even for a killed unit. }
            FUnit = null;
            FStatus = AIMoveExecutorUnitStatus.Lost;
        }

        return Result;
    }

    // AIGroup.pas:4320
    public void ResetTransporterSettings()
    {
        FBoardingPoint.X = -1;
        FArrivalPoint.X = -1;
        FTransportSource.X = -1;
        FTransportDestination.X = -1;
    }

    // Engine.pas:524

}
