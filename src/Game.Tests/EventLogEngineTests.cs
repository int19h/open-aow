using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// A new event log takes its day and player from the map of the engine it reaches (issue 335). The
// holder reaches the constructor before it reads the map. A log that reaches no engine takes day 0
// and the Independent player, the port's choice where the original leaves player index 0.
[Collection("Engine")]
[EngineState]
public sealed class EventLogEngineTests
{
    [Fact]
    public void ALogTakesTheDayOfTheMapOfItsEngine()
    {
        GameEngine engine = EngineFixture.Engine;
        StrategicMap engineMap = new(null, holder: engine) { Day = 7 };
        using IDisposable scope = EngineFixture.UseMap(engineMap);
        StrategicMap.Current = new StrategicMap(null, holder: engine) { Day = 3 };

        EventLog log = new(null, holder: engine);

        Assert.Equal(7, log.Day);
    }

    [Fact]
    public void ALogThatReachesNoEngineTakesDayZeroAndTheIndependentPlayer()
    {
        using IDisposable scope = EngineFixture.UseMap(new StrategicMap(null, holder: EngineFixture.Engine) { Day = 7 });

        EventLog log = new(null, holder: null!);

        Assert.Equal(0, log.Day);
        Assert.Equal(PlayerId.Independent, log.Player);
    }
}
