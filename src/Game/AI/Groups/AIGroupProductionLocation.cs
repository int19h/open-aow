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
        FRefCount = 1;
    }

    public int FRefCount;
    public ProductionControl FProductionControl;
    public MapCoordinate FLocation;
    public int FMovePoints;
    public int FTransporterID;
    public int FPathStrength;

    // AIGroup.pas:892
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:897
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

    // AIGroup.pas:903
    public virtual bool Validate(AIGroupControl Control) => true;

    // intf_P5_07.inc:190

}
