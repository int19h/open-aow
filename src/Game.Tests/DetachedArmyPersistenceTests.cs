using Aow.Engine;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Army.PAS:1898 writes the destination and the user mode when no map is set. An army that reaches
// no engine has no map, so it writes them too instead of failing on the engine read.
public sealed class DetachedArmyPersistenceTests
{
    [Fact]
    public void ADetachedArmyWithNoEngineWritesItsDestinationAndUserMode()
    {
        GameEngine engine = new(null);
        engine.Options &= ~GameEngine.VerifyClasses;
        engine.RegisterEngineClasses([EngineClass.Of<Army>()]);
        Army army = new(null)
        {
            Destination = new MapCoordinate(X: 3, Y: 4, MapLevel: 1),
            UserMode = ArmyUserModes.Guard,
        };
        Assert.Null(army.Owner);
        Assert.Null(army.Holder);
        Assert.Null(army.GetEngine());

        using MemoryStream memory = new();
        engine.StoreEngineObject(memory, objectValue: army, progress: null);
        memory.Position = 0;
        Army loaded = Assert.IsType<Army>(engine.RetrieveEngineObject(memory, progress: null));

        Assert.Equal(army.Destination, loaded.Destination);
        Assert.Equal(army.UserMode, loaded.UserMode);
    }
}
