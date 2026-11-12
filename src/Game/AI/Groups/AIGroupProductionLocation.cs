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
        RefCount = 1;
    }

    public int RefCount;
    public ProductionControl ProductionControl;
    public MapCoordinate Location;
    public int MovePoints;
    public int TransporterId;
    public int PathStrength;

    // AIGroup.pas:892
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:897
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

    // AIGroup.pas:903
    public virtual bool Validate(AIGroupControl control) => true;

    // intf_P5_07.inc:190

}
