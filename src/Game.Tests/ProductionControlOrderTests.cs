using Aow.Game.Ai;
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
        var previous = AoWHSMap.Current;
        AoWHSMap.Current = null!;
        var player = new Player(null!);
        var action = new AutoProduceAIPA(null!);
        try
        {
            var city = new CityProductionControl(null!);
            var place = new ProductionPlaceControl(null!);
            var later = new CityProductionControl(null!);
            Assert.Equal(0, player.FProductionControls.Add(city));
            Assert.Equal(1, player.FProductionControls.Add(place));
            Assert.Equal(2, player.FProductionControls.Add(later));
            Assert.Same(city, player.FProductionControls.GetPC(0));
            Assert.Same(place, player.FProductionControls.GetPC(1));
            Assert.Same(later, player.FProductionControls.GetPC(2));
            Assert.Equal(0, player.FProductionControls.IndexOf(city));
            Assert.Equal(1, player.FProductionControls.IndexOf(place));
            Assert.Equal(2, player.FProductionControls.IndexOf(later));

            player.FAIBudget.UpdateProductionUpkeep();
            Assert.Equal(0, player.FAIBudget.FProductionGoldUpkeep);
            Assert.Equal(0, player.FAIBudget.FProductionManaUpkeep);

            action.FControl = player.FAIControl;
            Assert.False(action.TransporterProductionControlExists());
            Assert.False(action.ProducingTransporter());
            Assert.False(action.ResearchingTransporter());

            var seen = new List<ProductionControl>();
            for (int i = player.FProductionControls.Count - 1; i >= 0; i--)
            {
                seen.Add(player.FProductionControls.GetPC(i));
            }

            Assert.Same(later, seen[0]);
            Assert.Same(place, seen[1]);
            Assert.Same(city, seen[2]);

            city.FStatus = ProductionControl.Status.Producing;
            place.FStatus = ProductionControl.Status.Producing;
            Assert.False(action.ExecuteProduce(city, 0));
            Assert.False(action.ExecuteResearch(place, 0));
            Assert.Equal(1, player.FProductionControls.Remove(place));
            Assert.Equal(-1, player.FProductionControls.IndexOf(place));
            Assert.Same(city, player.FProductionControls.GetPC(0));
            Assert.Same(later, player.FProductionControls.GetPC(1));
            Assert.Equal(2, player.FProductionControls.Add(place));
            Assert.Same(place, player.FProductionControls.GetPC(2));
        }
        finally
        {
            action.Dispose();
            player.Dispose();
            AoWHSMap.Current = previous;
        }
    }
}
