using Aow.Game.AI;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Registration order of city and place production controls on one player.
[Collection("Engine")]
[EngineState]
public sealed class ProductionControlOrderTests
{
    [Fact]
    public void BothKindsKeepRegistrationOrderThroughTheReaders()
    {
        StrategicMap previous = StrategicMap.Current;
        StrategicMap.Current = null;
        Player player = new(null) { Holder = EngineFixture.Engine };
        AutoProduceAIPlayerAction action = new(null) { Holder = EngineFixture.Engine };
        try
        {
            CityProductionControl city = new(null) { Holder = EngineFixture.Engine };
            ProductionPlaceControl place = new(null) { Holder = EngineFixture.Engine };
            CityProductionControl later = new(null) { Holder = EngineFixture.Engine };
            Assert.Equal(0, player.ProductionControls.Add(city));
            Assert.Equal(1, player.ProductionControls.Add(place));
            Assert.Equal(2, player.ProductionControls.Add(later));
            Assert.Same(city, player.ProductionControls[0]);
            Assert.Same(place, player.ProductionControls[1]);
            Assert.Same(later, player.ProductionControls[2]);
            Assert.Equal(0, player.ProductionControls.IndexOf(city));
            Assert.Equal(1, player.ProductionControls.IndexOf(place));
            Assert.Equal(2, player.ProductionControls.IndexOf(later));

            player.AIBudget.UpdateProductionUpkeep();
            Assert.Equal(0, player.AIBudget.ProductionGoldUpkeep);
            Assert.Equal(0, player.AIBudget.ProductionManaUpkeep);

            action.Control = player.AIControl;
            Assert.False(action.TransporterProductionControlExists());
            Assert.False(action.ProducingTransporter());
            Assert.False(action.ResearchingTransporter());

            List<ProductionControl> seen = [];
            for (int i = player.ProductionControls.Count - 1; i >= 0; i--)
            {
                seen.Add(player.ProductionControls[i]);
            }

            Assert.Same(later, seen[0]);
            Assert.Same(place, seen[1]);
            Assert.Same(city, seen[2]);

            city.Status = ProductionControlStatus.Producing;
            place.Status = ProductionControlStatus.Producing;
            Assert.False(action.ExecuteProduce(city, 0));
            Assert.False(action.ExecuteResearch(place, 0));
            Assert.Equal(1, player.ProductionControls.Remove(place));
            Assert.Equal(-1, player.ProductionControls.IndexOf(place));
            Assert.Same(city, player.ProductionControls[0]);
            Assert.Same(later, player.ProductionControls[1]);
            Assert.Equal(2, player.ProductionControls.Add(place));
            Assert.Same(place, player.ProductionControls[2]);
        }
        finally
        {
            action.Dispose();
            player.Dispose();
            StrategicMap.Current = previous;
        }
    }
}
