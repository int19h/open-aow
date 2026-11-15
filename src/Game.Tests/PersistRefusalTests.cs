using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic;
using Aow.Game.Net;
using Aow.Game.Units;

namespace Aow.Game.Tests;

/// <summary>
/// Every product engine class persists through the tree (issue 185 PE-4-a). None is refused.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistRefusalTests
{
    [Fact]
    public void EveryEngineClassPersistsThroughTheTree()
    {
        List<string> found = new();
        foreach (EngineClass? engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(EngineObject).Assembly)
            {
                continue;
            }

            if (EngineObject.TreePersistenceRefusal(engineClass.Type) is string)
            {
                found.Add(engineClass.Type.Name);
            }
        }
        found.Sort(static (a, b) => string.CompareOrdinal(a, b));
        Assert.True(found.Count == 0, "Still refused: " + string.Join(", ", found));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(Enchantment)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(ObservableNode)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(UnitList)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(ArmyView)));
        Assert.Null(EngineObject.TreePersistenceRefusal(typeof(DataPackage)));
        using DataPackage package = new(null!) { Holder = EngineFixture.Engine };
        ObjectNode tree = TreeWriter.Write(package, factory: new EnginePersistableFactory(EngineFixture.Engine));
        Assert.NotNull(tree);
    }
}
