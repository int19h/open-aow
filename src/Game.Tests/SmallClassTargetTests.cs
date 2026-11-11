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
        var events = new EventStore<HSMap>();
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
        public override void Activate(AIMoveExecutor executer)
        {
            log.Add("activate");
            base.Activate(executer);
        }

        public override void Deactivate() { log.Add("deactivate"); base.Deactivate(); }

        public override void Dispose() { log.Add("destroy"); base.Dispose(); }
    }

    [Fact]
    public void MoveControlReplacementCleansUpBeforeConstruction()
    {
        var log = new List<string>();
        using var executer = new AIMoveExecutor(null!);
        executer.ActivateMEC(() => new TrackedControl(log));
        log.Clear();
        executer.ActivateMEC(() =>
        {
            Assert.Null(executer.FControl);
            log.Add("construct");
            return new TrackedControl(log);
        });
        Assert.Equal(["deactivate", "destroy", "construct", "activate"], log);
        Assert.Same(executer, executer.FControl.FExecuter);
        Assert.Equal(0, executer.FControl.FState);
        log.Clear();
        Assert.Throws<InvalidDataException>(() => executer.ActivateMEC(() => throw new InvalidDataException("control")));
        Assert.Equal(["deactivate", "destroy"], log);
        Assert.Null(executer.FControl);
        executer.ActivateMEC(null!);
        Assert.Equal(["deactivate", "destroy"], log);
    }
}
