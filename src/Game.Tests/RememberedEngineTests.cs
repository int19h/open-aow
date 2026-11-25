using Aow.Engine.Nodes;

namespace Aow.Game.Tests;

// A node that leaves its owner reaches the engine of its latest owner. The holder keeps its
// construction meaning.
public sealed class RememberedEngineTests
{
    [Fact]
    public void ADetachedNodeReachesTheEngineOfItsLatestOwner()
    {
        GameEngine first = new(null);
        GameEngine second = new(null);
        OwningNode firstOwner = new(null) { Holder = first };
        OwningNode secondOwner = new(null) { Holder = second };
        EngineObject node = new(null);
        Assert.Null(node.GetEngine());

        node.SetOwner(firstOwner);
        Assert.Same(first, node.GetEngine());
        node.SetOwner(null);
        Assert.Same(first, node.GetEngine());

        node.SetOwner(secondOwner);
        Assert.Same(second, node.GetEngine());
        node.SetOwner(null);
        Assert.Same(second, node.GetEngine());
        Assert.Null(node.Holder);
    }

    [Fact]
    public void TheOwnerChainWinsOverTheRememberedEngineAndTheHolder()
    {
        GameEngine held = new(null);
        GameEngine owning = new(null);
        OwningNode owner = new(null) { Holder = owning };
        EngineObject node = new(null) { Holder = held };
        Assert.Same(held, node.GetEngine());

        node.SetOwner(owner);
        Assert.Same(owning, node.GetEngine());
        node.SetOwner(null);
        Assert.Same(owning, node.GetEngine());
        Assert.Same(held, node.Holder);
    }
}
