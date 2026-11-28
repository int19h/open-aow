#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Terrain;

namespace Aow.Game.World;

// { Interface declaration run owned by P5. }
// pad resources, terrain types
public sealed class PadControl : ControlStyleObject
{
    public List<MapObjectResource?> PadResources { get; }
    public BitList TerrainTypes { get; }

    // MoveT.PAS:3622
    public override void Dispose()
    {
        PadResources?.Clear();
        base.Dispose();
    }

    // MoveT.PAS:3614
    public PadControl(CustomNode owner) : base(owner)
    {
        TerrainTypes = [];
        PadResources = [];
        SetControlStyle(ControlStyles.FixedChild);
    }

    // MoveT.PAS:3629
    public MapObjectResource? GetPadResource(int size) => size < PadResources.Count ? PadResources[size] : null;

    // MoveT.PAS:3635
    public void SetPadResource(int size, MapObjectResource? resource)
    {
        while (PadResources.Count <= size)
        {
            PadResources.Add(null);
        }

        PadResources[size] = resource;
    }

    // MoveT.PAS:3641
    public void RegisterPadResource(MapObjectResource resource, int size)
    {
        if (GetPadResource(size) is null)
        {
            SetPadResource(size, resource);
        }
    }

    // MoveT.PAS:3646
    public void UnregisterPadResource(MapObjectResource resource, int size)
    {
        if (resource == GetPadResource(size))
        {
            SetPadResource(size, resource: null);
        }
    }

    // MoveT.PAS:3651
    public bool CanPlacePad(int X, int Y, int L, int size)
    {
        MapObjectResource? resource = GetPadResource(size);
        bool result;
        if (resource is not null)
        {
            ImageLibraryTerrainMapObject pad = resource.CreateResourceUser<ImageLibraryTerrainMapObject>();
            result = pad.CanPlace(GetEngine().StrategicMap.MapContainer, X, Y, L);
            pad?.Dispose();
        }
        else
        {
            return false;
        }

        return result;
    }

    // MoveT.PAS:3666
    public ImageLibraryTerrainMapObject? PlacePad(int X, int Y, int L, int size)
    {
        MapObjectResource? resource = GetPadResource(size);
        return resource?.PlaceResourceUser<ImageLibraryTerrainMapObject>(GetEngine().StrategicMap, X, Y, L);
    }
}
