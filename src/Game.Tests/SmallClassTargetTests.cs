#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.AI.Groups;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

public sealed class SmallClassTargetTests
{
    [Fact]
    public void MapLoadedHandlersRemoveByStaticMethodIdentity()
    {
        // GameEngine.HexSpriteMapLoaded is a plain event (issue 326, decision 4), so a removal
        // matches the static method a registrar subscribed.
        NotifyEvent<GameMap>? events = null;
        events += BehaviorRegistrar.MapLoaded;
        events += CityRegistrar.MapLoaded;
        events += IndependentRegistrar.MapLoaded;
        events += MainRegistrar.MapLoaded;
        Assert.Equal(4, events.GetInvocationList().Length);
        events -= BehaviorRegistrar.MapLoaded;
        events -= CityRegistrar.MapLoaded;
        events -= IndependentRegistrar.MapLoaded;
        events -= MainRegistrar.MapLoaded;
        Assert.Null(events);
    }

    private sealed class TrackedControl(List<string> log) : AIMoveExecutorControl(null!)
    {
        public override void Activate(AIMoveExecutor executor)
        {
            log.Add("activate");
            base.Activate(executor);
        }

        public override void Deactivate()
        {
            log.Add("deactivate");
            base.Deactivate();
        }

        public override void Dispose()
        {
            log.Add("destroy");
            base.Dispose();
        }
    }

    [Fact]
    public void MoveControlReplacementCleansUpBeforeConstruction()
    {
        List<string> log = [];
        using AIMoveExecutor executor = new(null) { Holder = EngineFixture.Engine };
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
        _ = Assert.Throws<InvalidDataException>(() => executor.ActivateMoveExecutorControl(() => throw new InvalidDataException("control")));
        Assert.Equal(["deactivate", "destroy"], log);
        Assert.Null(executor.Control);
        executor.ActivateMoveExecutorControl(null);
        Assert.Equal(["deactivate", "destroy"], log);
    }
}
