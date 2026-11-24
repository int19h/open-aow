#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// references, player, source, destination, move table, completion event, transporter, search
// limits, mode, army, selection, cycles, status, cost, path, transport locations
public class AIMoveControlSettings : EngineObject
{
    // AIGroup.pas:3841
    public AIMoveControlSettings(CustomNode? owner) : base(owner)
    {
        TransportPath = [];
        RefCount = 1;
        Clear();
    }

    public AIMoveControlSettings(CustomNode? owner, NotifyEvent<object>? done) : this(owner) => _done = done;

    public int RefCount { get; private set; }
    public PlayerId? Player { get; private set; } = PlayerId.Independent;
    public MapCoordinate Source { get; private set; }
    public MapCoordinate Destination { get; private set; }

    public MovePointTable MovePointTable { get; private set; }

    private readonly NotifyEvent<object> _done;

    internal void Complete(object sender) => _done?.Invoke(sender);

    public Troop? Transporter { get; set; }
    public int MovePointsMax { get; private set; }
    public int QuickMovePointsMax { get; private set; }
    public MoveModes MoveMode { get; set; }
    public ArmyHexSprite ArmyHexSprite { get; private set; }
    public int Selection { get; private set; }
    public AIMoveControlStatus Status { get; set; }
    public int MovePoints { get; set; }
    public MovePath TransportPath { get; }

    public MapCoordinate BoardingPoint { get; internal set; }
    public MapCoordinate ArrivalPoint { get; internal set; }
    public MapCoordinate TransportSource { get; internal set; }
    public MapCoordinate TransportDestination { get; internal set; }

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
        BoardingPoint = BoardingPoint with { X = -1 };
        ArrivalPoint = ArrivalPoint with { X = -1 };
        TransportSource = TransportSource with { X = -1 };
        // { Original bug: clears the destination Y coordinate instead of X. }
        TransportDestination = TransportDestination with { Y = -1 };
        MovePoints = -1;
        TransportPath.Clear();
        Destination = new MapCoordinate(-1, -1, -1);
        Selection = 0;
    }

    // AIGroup.pas:3887
    public void SetSourcePosition(MapCoordinate source) => Source = source;

    public void SetSourcePosition(int x, int y, int level)
        => SetSourcePosition(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level));

    // AIGroup.pas:3894
    public void SetDestinationPosition(MapCoordinate destination) => Destination = destination;

    public void SetDestinationPosition(int x, int y, int level)
        => SetDestinationPosition(new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level));

    // AIGroup.pas:3901
    public void Setup(ArmyHexSprite? army, int selection, int X, int Y, int L)
    {
        int hexLevel = army.HexLevel;
        int hexY = army.HexY;
        SetSourcePosition(army.HexX, hexY, hexLevel);
        SetDestinationPosition(X, Y, L);
        ArmyHexSprite = army;
        Selection = selection;
        Player = army.Army.Player;
        _ = MovePointTable;
        MovePointTable movePointTable = army.Army.CreateMovePointTable(selection);
        MovePointTable = movePointTable;
    }
}
