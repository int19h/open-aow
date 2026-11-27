using Aow.Game.Draw.Animations;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Second-engine ownership (issue 335, decision of 2026-11-24). A storm looks for its target on the
// map of its own engine, the engine its diplomacy and damage helpers read, while the legacy global
// holds another map with no level to scan.
[Collection("Engine")]
[EngineState]
public sealed class StormEngineOwnershipTests
{
    [Fact]
    public void TargetSearchScansTheMapOfTheStormsEngine()
    {
        GameEngine engine = EngineFixture.Engine;
        StrategicMap engineMap = new(null, holder: engine);
        MapContainer engineContainer = new(null) { Holder = engine };
        MapLevel level = new(engineContainer);
        level.CreateMapFieldList(1, 1);
        engineContainer.Map = engineMap;
        engineMap.MapContainer = engineContainer;
        using IDisposable scope = EngineFixture.UseMap(engineMap);

        StrategicMap globalMap = new(null, holder: engine);
        MapContainer globalContainer = new(null) { Holder = engine, Map = globalMap };
        globalMap.MapContainer = globalContainer;
        StrategicMap.Current = globalMap;

        Storm storm = new(null!) { Holder = engine };
        bool found = storm.GetBestTarget(PlayerId.Independent, X: 0, Y: 0, L: 0, range: 1, target: out MapCoordinate target, info: out _);

        Assert.False(found);
        Assert.Equal(MapCoordinate.None, target);
        Assert.Same(engineMap, engine.StrategicMap);
        Assert.Same(globalMap, StrategicMap.Current);
    }
}
