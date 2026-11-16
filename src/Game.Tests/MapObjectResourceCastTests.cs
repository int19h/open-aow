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
        Structure structure = new(null!);
        Assert.Null(structure.Resource);

        StructureResource structureResource = new(null!);
        structure.SetResource(structureResource);
        Assert.Same(structureResource, structure.Resource);

        TerrainMapObjectResource wrongResource = new(null!);
        structure.SetResource(wrongResource);
        Assert.Throws<InvalidCastException>(() => structure.Resource);
    }

    [Fact]
    public void PlayerAndCropStructuresReturnCovariantResources()
    {
        PlayerStructure playerStructure = new(null!);
        PlayerStructureResource playerResource = new(null!);
        playerStructure.SetResource(playerResource);
        Assert.Same(playerResource, playerStructure.Resource);

        PlayerCropStructure cropStructure = new(null!);
        PlayerCropStructureResource cropResource = new(null!);
        cropStructure.SetResource(cropResource);
        Assert.Same(cropResource, cropStructure.Resource);

        TerrainMapObject terrainObject = new(null!);
        TerrainMapObjectResource terrainResource = new(null!);
        terrainObject.SetResource(terrainResource);
        Assert.Same(terrainResource, terrainObject.Resource);
    }

    [Fact]
    public void FindChildAndFindResourceReturnTypedResultAndThrowOnMismatch()
    {
        MapObjectResourceList list = new(null!);
        Assert.Null(list.FindChild<StructureResource>(StructureResource.ClassId));
        Assert.Null(list.FindResource<StructureResource>(60));

        StructureResource structureResource = new(null!) { ResourceId = 60 };
        list.AddChild(structureResource);
        Assert.Same(structureResource, list.FindChild<StructureResource>(StructureResource.ClassId));
        Assert.Same(structureResource, list.FindResource<StructureResource>(60));

        Assert.Throws<InvalidCastException>(() => list.FindChild<PlayerStructureResource>(StructureResource.ClassId));
        Assert.Throws<InvalidCastException>(() => list.FindResource<PlayerStructureResource>(60));
    }
}
