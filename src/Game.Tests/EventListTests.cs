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
        var store = new EventStore<string>();
        var log = new List<string>();
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
        var store = new EventStore<string>();
        var log = new List<string>();
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
        var store = new EventStore<string>();
        var log = new List<string>();
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
        var store = new EventStore<string>();
        var log = new List<string>();
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
        var store = new EventStore<string>();
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
        var store = new EventStore<sbyte>();
        sbyte seen = -1;
        store.Add(player => seen = player);

        store.Raise(7);

        Assert.Equal(7, seen);
    }

    [Fact]
    public void PlayerGemsChangeReachesATypedSubscriber()
    {
        var previous = AoWHSMap.Current;
        var map = new AoWHSMap(null!);
        AoWHSMap.Current = map;
        var player = new Player(null!);
        try
        {
            sbyte seen = -2;
            player.PlayerIndex = 2;
            player.FGemsChangedEvents += source => seen = source.PlayerIndex;

            player.SetGems(9);

            Assert.Equal(9, player.Gems);
            Assert.Equal(2, seen);
        }
        finally
        {
            player.Dispose();
            AoWHSMap.Current = previous;
            map.Dispose();
        }
    }
}
