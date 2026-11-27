using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Second-engine ownership (issue 335). A flag message takes its day from the map of its creator
// engine and lands on that same map, while the legacy global holds another map.
[Collection("Engine")]
[EngineState]
public sealed class EventLogDestinationTests
{
    [Fact]
    public void AFlagMessageLandsOnTheMapOfItsCreatorEngine()
    {
        GameEngine engine = EngineFixture.Engine;
        StrategicMap engineMap = new(null, holder: engine) { Day = 7, SeatedPlayer = PlayerId.Player1 };
        using IDisposable scope = EngineFixture.UseMap(engineMap);
        StrategicMap globalMap = new(null, holder: engine) { Day = 3, SeatedPlayer = PlayerId.Player1 };
        StrategicMap.Current = globalMap;
        List<EventLog> engineLogs = [];
        List<EventLog> globalLogs = [];
        engineMap.ExecutingEventLog += notification => engineLogs.Add(notification.Info.Log);
        globalMap.ExecutingEventLog += notification => globalLogs.Add(notification.Info.Log);
        FlagMessageEvent flag = new(null!) { Holder = engine };

        Assert.True(flag.Trigger(PlayerId.Player1));

        Assert.Equal(7, Assert.Single(engineLogs).Day);
        Assert.Empty(globalLogs);
    }
}
