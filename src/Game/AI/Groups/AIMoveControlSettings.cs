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
        TransportPath = new MovePath();
        RefCount = 1;
        Clear();
    }

    public int RefCount { get; private set; }
    public sbyte Player { get; private set; }
    public int SourceX { get; private set; }
    public int SourceY { get; private set; }
    public int SourceL { get; private set; }
    public int DestinationX { get; private set; }
    public int DestinationY { get; private set; }
    public int DestinationL { get; private set; }

    public MovePointTable MovePointTable;

    public event NotifyEvent<object> OnDone;

    internal void RaiseDone(object sender)
    {
        if (OnDone != null)
        {
            OnDone(sender);
        }
    }

    public Troop Transporter { get; set; }
    public int MovePointsMax { get; private set; }
    public int QuickMovePointsMax { get; private set; }
    public Set<MoveModeElement> MoveMode { get; set; }
    public ArmyHexSprite ArmyHexSprite { get; private set; }
    public int Selection { get; private set; }
    public AIMoveControlStatus Status { get; set; }
    public int MovePoints { get; set; }
    public MovePath TransportPath { get; }

    public MapCoordinate BoardingPoint;
    public MapCoordinate ArrivalPoint;
    public MapCoordinate TransportSource;
    public MapCoordinate TransportDestination;

    // AIGroup.pas:3849
    public override void Dispose()
    {
        TransportPath?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:3874
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:3880
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:3855
    public override void Clear()
    {
        base.Clear();
        MovePointsMax = 175;
        QuickMovePointsMax = 300;
        BoardingPoint.X = -1;
        ArrivalPoint.X = -1;
        TransportSource.X = -1;
        // { Original bug: clears the destination Y coordinate instead of X. }
        TransportDestination.Y = -1;
        MovePoints = -1;
        TransportPath.Clear();
        DestinationX = -1;
        DestinationY = -1;
        DestinationL = -1;
        Selection = 0;
    }

    // AIGroup.pas:3887
    public void SetSourcePosition(int X, int Y, int L)
    {
        SourceX = X;
        SourceY = Y;
        SourceL = L;
    }

    // AIGroup.pas:3894
    public void SetDestinationPosition(int X, int Y, int L)
    {
        DestinationX = X;
        DestinationY = Y;
        DestinationL = L;
    }

    // AIGroup.pas:3901
    public void Setup(ArmyHexSprite army, byte selection, int X, int Y, int L)
    {
        var hexLevel = army.GetHexLevel();
        var hexY = army.GetHexY();
        SetSourcePosition(army.GetHexX(), hexY, hexLevel);
        SetDestinationPosition(X, Y, L);
        ArmyHexSprite = army;
        Selection = selection;
        Player = army.Army.Player;
        army.Army.CreateMovePointTable(selection, Bytes.Of(ref MovePointTable));
    }

    // intf_P5_06.inc:154

}
