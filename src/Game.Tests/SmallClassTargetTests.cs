using Aow.Engine;
using Aow.Game.AI.Groups;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

public sealed class SmallClassTargetTests
{
    [Fact]
    public void MapLoadedHandlersRemoveByStaticMethodIdentity()
    {
        var events = new EventStore<GameMap>();
        events.Add(BehaviorRegistrar.MapLoaded);
        events.Add(CityRegistrar.MapLoaded);
        events.Add(IndependentRegistrar.MapLoaded);
        events.Add(MainRegistrar.MapLoaded);
        Assert.Equal(4, events.Count);
        events.Remove(BehaviorRegistrar.MapLoaded);
        events.Remove(CityRegistrar.MapLoaded);
        events.Remove(IndependentRegistrar.MapLoaded);
        events.Remove(MainRegistrar.MapLoaded);
        Assert.Equal(0, events.Count);
    }

    private sealed class TrackedControl(List<string> log) : AIMoveExecutorControl(null!)
    {
        public override void Activate(AIMoveExecutor executor)
        {
            log.Add("activate");
            base.Activate(executor);
        }

        public override void Deactivate() { log.Add("deactivate"); base.Deactivate(); }

        public override void Dispose() { log.Add("destroy"); base.Dispose(); }
    }

    [Fact]
    public void MoveControlReplacementCleansUpBeforeConstruction()
    {
        var log = new List<string>();
        using var executor = new AIMoveExecutor(null!) { Holder = EngineFixture.Engine };
        executor.ActivateMoveExecutorControl(() => new TrackedControl(log));
        log.Clear();
        executor.ActivateMoveExecutorControl(() =>
        {
            Assert.Null(executor.Control);
            log.Add("construct");
            return new TrackedControl(log);
        });
        Assert.Equal(["deactivate", "destroy", "construct", "activate"], log);
        Assert.Same(executor, executor.Control.Executor);
        Assert.Equal(0, executor.Control.State);
        log.Clear();
        Assert.Throws<InvalidDataException>(() => executor.ActivateMoveExecutorControl(() => throw new InvalidDataException("control")));
        Assert.Equal(["deactivate", "destroy"], log);
        Assert.Null(executor.Control);
        executor.ActivateMoveExecutorControl(null!);
        Assert.Equal(["deactivate", "destroy"], log);
    }
}
