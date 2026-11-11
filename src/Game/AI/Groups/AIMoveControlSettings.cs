// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// references, player, source, destination, move table, completion event, transporter, search
// limits, mode, army, selection, cycles, status, cost, path, transport locations
public partial class AIMoveControlSettings : EngineObject
{
    // AIGroup.pas:3841
    public AIMoveControlSettings(ECustomNode owner) : base(owner)
    {
        FTransportPath = new MovePath();
        FRefCount = 1;
        Clear();
    }

    public int FRefCount;
    public sbyte FPlayer;
    public int FSourceX;
    public int FSourceY;
    public int FSourceL;
    public int FDestinationX;
    public int FDestinationY;
    public int FDestinationL;
    public SByteArray16By16 FMovePointTable;
    public NotifyEvent<object> FOnDone;
    public Troop FTransporter;
    public int FMovePointsMax;
    public int FQuickMovePointsMax;
    public Set<MoveModeElement> FMoveMode;
    public ArmyHexSprite FArmyHS;
    public int FSelection;
    public int FProcessCycles;
    public AIMoveControlStatus FStatus;
    public int FMovePoints;
    public MovePath FTransportPath;
    public MapCoordinate FBoardingPoint;
    public MapCoordinate FArrivalPoint;
    public MapCoordinate FTransportSource;
    public MapCoordinate FTransportDestination;

    // AIGroup.pas:3849
    public override void Dispose()
    {
        FTransportPath?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:3874
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:3880
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // AIGroup.pas:3855
    public override void Clear()
    {
        base.Clear();
        FMovePointsMax = 175;
        FQuickMovePointsMax = 300;
        FBoardingPoint.X = -1;
        FArrivalPoint.X = -1;
        FTransportSource.X = -1;
        // { Original bug: clears the destination Y coordinate instead of X. }
        FTransportDestination.Y = -1;
        FMovePoints = -1;
        FTransportPath.Clear();
        FDestinationX = -1;
        FDestinationY = -1;
        FDestinationL = -1;
        FSelection = 0;
        FProcessCycles = 0;
    }

    // AIGroup.pas:3887
    public void SetSourcePos(int X, int Y, int L)
    {
        FSourceX = X;
        FSourceY = Y;
        FSourceL = L;
    }

    // AIGroup.pas:3894
    public void SetDestinationPos(int X, int Y, int L)
    {
        FDestinationX = X;
        FDestinationY = Y;
        FDestinationL = L;
    }

    // AIGroup.pas:3901
    public void Setup(ArmyHexSprite Army, byte Selection, int X, int Y, int L)
    {
        var lhx = Army.GetLhx();
        var yhx = Army.GetYhx();
        SetSourcePos(Army.GetXhx(), yhx, lhx);
        SetDestinationPos(X, Y, L);
        FArmyHS = Army;
        FSelection = Selection;
        FPlayer = Army.FArmy.FPlayer;
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref FMovePointTable));
    }

    // intf_P5_06.inc:154

}
