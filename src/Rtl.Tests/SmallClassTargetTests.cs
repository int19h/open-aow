using Aow.Units.AoWE;
using Aow.Units.Engine;
using ObjectList = Aow.Units.EList.TObjectList;
using ListObject = Aow.Units.EList.TListObject;

namespace Aow.Rtl.Tests;

public sealed class SmallClassTargetTests
{
    [Fact]
    public void MapLoadedHandlersRemoveByStaticMethodIdentity()
    {
        var events = new TEventList();
        events.Add(Aow.Units.BehaviorAG.TRegistrator.MapLoaded);
        events.Add(Aow.Units.City.TRegistrator.MapLoaded);
        events.Add(Aow.Units.IndependentAG.TRegistrator.MapLoaded);
        events.Add(Aow.Units.MainAG.TRegistrator.MapLoaded);
        Assert.Equal(4, events.Count);
        Assert.Equal(0, events.Remove(Aow.Units.BehaviorAG.TRegistrator.MapLoaded));
        Assert.Equal(0, events.Remove(Aow.Units.City.TRegistrator.MapLoaded));
        Assert.Equal(0, events.Remove(Aow.Units.IndependentAG.TRegistrator.MapLoaded));
        Assert.Equal(0, events.Remove(Aow.Units.MainAG.TRegistrator.MapLoaded));
        Assert.Equal(0, events.Count);
        events.Destroy();
    }

    private sealed class PreviousItem : ListObject
    {
        internal bool Destroyed;
        public override void Destroy() { Destroyed = true; base.Destroy(); }
    }

    [Fact]
    public void ListStreamKeepsNilSlotsAndReplacesOwnedItems()
    {
        using var input = new TMemoryStream();
        byte[] encoded = [3, 0, 0, 0, 0, 1, 0];
        input.WriteBuffer(encoded);
        input.Position = 0;
        var previous = new PreviousItem();
        var list = new ObjectList();
        list.Add(previous);
        try
        {
            list.LoadFromStream(input);
            Assert.True(previous.Destroyed);
            Assert.Equal(3, list.Count);
            Assert.IsType<ListObject>(list[0]);
            Assert.Null(list[1]);
            Assert.IsType<ListObject>(list[2]);
            Assert.NotSame(list[0], list[2]);
            using var output = new TMemoryStream();
            list.SaveToStream(output);
            Assert.Equal(encoded.Length, output.Size);
            output.Position = 0;
            var actual = new byte[encoded.Length];
            output.ReadBuffer(actual);
            Assert.Equal(encoded, actual);
        }
        finally { list.Destroy(); }
    }

    private sealed class TrackedControl(List<string> log) : TAIMoveExecuterControl(null!)
    {
        public override void Activate(TAIMoveExecuter executer)
        {
            log.Add("activate");
            base.Activate(executer);
        }
        public override void Deactivate() { log.Add("deactivate"); base.Deactivate(); }
        public override void Destroy() { log.Add("destroy"); base.Destroy(); }
    }

    [Fact]
    public void MoveControlReplacementCleansUpBeforeConstruction()
    {
        var log = new List<string>();
        var executer = new TAIMoveExecuter(null!);
        try
        {
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
        finally { executer.Destroy(); }
    }
}
