using Aow.Engine.Nodes;
using Aow.Game.Combat;

namespace Aow.Game.Tests;

// Combat helpers whose creator reaches no engine (issue 335). They read the map and the pack of
// their creator's engine. A creator with no engine has neither, so a read with no guard throws, as
// the original threw on a null global. A strategic map cannot be built without an engine, so the
// creator here is a bare node outside every tree.
[Collection("Engine")]
[EngineState]
public sealed class EngineLessCombatConstructionTests
{
    private sealed class DetachedNode() : EngineObject(null);

    [Fact]
    public void TargetSelectionOnAnEngineLessCreatorThrowsAtItsFirstEngineRead()
    {
        DetachedNode creator = new();
        Assert.Null(creator.GetEngine());
        using TargetSelectionControl selection = new(creator);
        Assert.Same(creator, selection.Creator);
        _ = Assert.Throws<NullReferenceException>(() => selection.Initialize(creator));
        _ = Assert.Throws<NullReferenceException>(selection.Activate);
    }

    [Fact]
    public void GeneratorOnAnEngineLessCreatorThrowsAtConstruction()
    {
        DetachedNode creator = new();
        Assert.Null(creator.GetEngine());
        _ = Assert.Throws<NullReferenceException>(() => new RandomBattleMapGenerator(creator));
    }
}
