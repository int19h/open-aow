using Aow.Engine;

using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Notify lists fire from the last subscriber to the first on the live array.
// Engine.pas:5046 reads FCount once, then indexes FList on the way down.
[Collection("Engine")]
[EngineState]
public sealed class EventListTests
{
    [Fact]
    public void RaiseRunsFromTheLastSubscriberToTheFirst()
    {
        EventStore<string> store = new();
        List<string> log = [];
        void HandlerA(string sender) => log.Add("A" + sender);
        void HandlerB(string sender) => log.Add("B" + sender);
        void HandlerC(string sender) => log.Add("C" + sender);
        store.Add(HandlerA);
        store.Add(HandlerB);
        store.Add(HandlerC);

        store.Raise("1");

        Assert.Equal(["C1", "B1", "A1"], log);
    }

    [Fact]
    public void HandlerThatRemovesItselfStillRunsTheSubscribersBelowIt()
    {
        EventStore<string> store = new();
        List<string> log = [];
        void HandlerA(string sender) => log.Add("A");
        void HandlerB(string sender) => log.Add("B");
        void HandlerC(string sender)
        {
            log.Add("C");
            store.Remove(HandlerC);
        }

        store.Add(HandlerA);
        store.Add(HandlerB);
        store.Add(HandlerC);
        store.Raise("");

        Assert.Equal(["C", "B", "A"], log);
        Assert.Equal(2, store.Count);
    }

    [Fact]
    public void RemovingALowerSubscriberRecallsTheCurrentHandler()
    {
        EventStore<string> store = new();
        List<string> log = [];
        void HandlerA(string sender) => log.Add("A");
        void HandlerB(string sender) => log.Add("B");
        void HandlerC(string sender)
        {
            log.Add("C");
            store.Remove(HandlerA);
        }

        store.Add(HandlerA);
        store.Add(HandlerB);
        store.Add(HandlerC);
        store.Raise("");

        Assert.Equal(["C", "C", "B"], log);
        Assert.Equal(2, store.Count);
    }

    [Fact]
    public void SubscriberAddedDuringTheRaiseWaitsForTheNextRaise()
    {
        EventStore<string> store = new();
        List<string> log = [];
        void Added(string sender) => log.Add("added");
        void First(string sender) => log.Add("first");
        void Last(string sender)
        {
            log.Add("last");
            store.Add(Added);
        }

        store.Add(First);
        store.Add(Last);
        store.Raise("");

        Assert.Equal(["last", "first"], log);
        Assert.Equal(3, store.Count);
        log.Clear();
        store.Raise("");
        Assert.Equal(["added", "last", "first"], log);
    }

    [Fact]
    public void DuplicateMethodGroupRemovedOnceStaysSubscribedOnce()
    {
        EventStore<string> store = new();
        int runs = 0;
        void Handler(string sender) => runs++;
        store.Add(Handler);
        store.Add(Handler);
        Assert.Equal(2, store.Count);

        store.Remove(Handler);

        Assert.Equal(1, store.Count);
        store.Raise("");
        Assert.Equal(1, runs);
    }

    [Fact]
    public void IntegerPayloadReachesTheSubscriber()
    {
        EventStore<sbyte> store = new();
        sbyte seen = -1;
        store.Add(player => seen = player);

        store.Raise(7);

        Assert.Equal(7, seen);
    }

    [Fact]
    public void PlayerGemsChangeReachesATypedSubscriber()
    {
        StrategicMap previous = StrategicMap.Current;
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        StrategicMap.Current = map;
        Player player = new(null) { Holder = EngineFixture.Engine };
        try
        {
            sbyte seen = -2;
            player.PlayerIndex = 2;
            player.GemsChanged += source => seen = source.PlayerIndex;

            player.SetGems(9);

            Assert.Equal(9, player.Gems);
            Assert.Equal(2, seen);
        }
        finally
        {
            player.Dispose();
            StrategicMap.Current = previous;
            map.Dispose();
        }
    }
}
