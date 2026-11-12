// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

// references, production control, location, moves, transporter, path strength
public partial class AIGroupProductionLocation : EngineObject
{
    // AIGroup.pas:887
    public AIGroupProductionLocation(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
    }

    private int _refCount;

    public ProductionControl ProductionControl { get; set; }
    public MapCoordinate Location { get; set; }
    public int MovePoints { get; set; }
    public int TransporterId { get; set; }
    public int PathStrength { get; set; }

    // AIGroup.pas:892
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:897
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:903
    public virtual bool Validate(AIGroupControl control) => true;

    // intf_P5_07.inc:190

}
