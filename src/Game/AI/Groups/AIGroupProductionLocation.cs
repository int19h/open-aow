#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

// references, production control, location, moves, transporter, path strength
public sealed class AIGroupProductionLocation(CustomNode owner) : EngineObject(owner), IOwned<AIGroupProductionLocationList>, IOwnedSink<AIGroupProductionLocationList>
{
    // A reference list holds the location and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list.
    public new AIGroupProductionLocationList Owner => GetOwner<AIGroupProductionLocationList>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is AIGroupProductionLocationList;

    void IOwnedSink<AIGroupProductionLocationList>.SetOwner(AIGroupProductionLocationList owner) => SetOwner(owner);

    private int _refCount = 1;

    public ProductionControl ProductionControl { get; init; }
    public MapCoordinate Location { get; init; }
    public int MovePoints { get; init; }
    public int TransporterId { get; init; }
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
    public bool Validate(AIGroupControl control) => true;
}
