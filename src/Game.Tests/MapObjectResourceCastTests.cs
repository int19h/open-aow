using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class MapObjectResourceCastTests
{
    [Fact]
    public void CovariantResourceReturnsTypedResourceAndThrowsOnMismatch()
    {
        Structure structure = new(null!) { Holder = EngineFixture.Engine };
        Assert.Null(structure.Resource);

        StructureResource structureResource = new(null!) { Holder = EngineFixture.Engine };
        structure.SetResource(structureResource);
        Assert.Same(structureResource, structure.Resource);

        TerrainMapObjectResource wrongResource = new(null!) { Holder = EngineFixture.Engine };
        structure.SetResource(wrongResource);
        _ = Assert.Throws<InvalidCastException>(() => structure.Resource);
    }

    [Fact]
    public void PlayerAndCropStructuresReturnCovariantResources()
    {
        PlayerStructure playerStructure = new(null!) { Holder = EngineFixture.Engine };
        PlayerStructureResource playerResource = new(null!) { Holder = EngineFixture.Engine };
        playerStructure.SetResource(playerResource);
        Assert.Same(playerResource, playerStructure.Resource);

        PlayerCropStructure cropStructure = new(null!) { Holder = EngineFixture.Engine };
        PlayerCropStructureResource cropResource = new(null!) { Holder = EngineFixture.Engine };
        cropStructure.SetResource(cropResource);
        Assert.Same(cropResource, cropStructure.Resource);

        TerrainMapObject terrainObject = new(null!) { Holder = EngineFixture.Engine };
        TerrainMapObjectResource terrainResource = new(null!) { Holder = EngineFixture.Engine };
        terrainObject.SetResource(terrainResource);
        Assert.Same(terrainResource, terrainObject.Resource);
    }

    [Fact]
    public void FindChildAndFindResourceReturnTypedResultAndThrowOnMismatch()
    {
        MapObjectResourceList list = new(null!) { Holder = EngineFixture.Engine };
        Assert.Null(list.FindChild<StructureResource>(StructureResource.ClassId));
        Assert.Null(list.FindResource<StructureResource>(60));

        StructureResource structureResource = new(null!) { Holder = EngineFixture.Engine, ResourceId = 60 };
        _ = list.AddChild(structureResource);
        Assert.Same(structureResource, list.FindChild<StructureResource>(StructureResource.ClassId));
        Assert.Same(structureResource, list.FindResource<StructureResource>(60));

        _ = Assert.Throws<InvalidCastException>(() => list.FindChild<PlayerStructureResource>(StructureResource.ClassId));
        _ = Assert.Throws<InvalidCastException>(() => list.FindResource<PlayerStructureResource>(60));
    }

    [Fact]
    public void CreateResourceUserWithoutAUserClassStaysNull()
    {
        MapObjectResource resource = new(null) { Holder = EngineFixture.Engine };
        Assert.Null(resource.CreateResourceUser());
        Assert.Null(resource.CreateResourceUser<MapObject>());
    }
}
