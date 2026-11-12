// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using Aow.Rtl;

namespace Aow.Game.World;

// { Interface declaration run owned by P5. }
// pad resources, terrain types
public partial class PadControl : ControlStyleObject
{
    public List<MapObjectResource> PadResources;
    public BitList TerrainTypes;

    // MoveT.PAS:3622
    public override void Dispose()
    {
        TerrainTypes?.Dispose();
        PadResources?.Clear();
        base.Dispose();
    }

    // MoveT.PAS:3614
    public PadControl(ECustomNode owner) : base(owner)
    {
        TerrainTypes = new BitList();
        PadResources = new List<MapObjectResource>();
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.FixedChild));
    }

    // MoveT.PAS:3629
    public MapObjectResource GetPadResource(int size)
    {
        MapObjectResource result;
        if (size < PadResources.Count)
        {
            result = PadResources[size];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // MoveT.PAS:3635
    public void SetPadResource(int size, MapObjectResource resource)
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
        if (GetPadResource(size) == null)
        {
            SetPadResource(size, resource);
        }
    }

    // MoveT.PAS:3646
    public void UnregisterPadResource(MapObjectResource resource, int size)
    {
        if (resource == GetPadResource(size))
        {
            SetPadResource(size, null);
        }
    }

    // MoveT.PAS:3651
    public bool CanPlacePad(sbyte X, sbyte Y, sbyte L, int size)
    {
        bool result;
        MapObjectResource resource;
        ImageLibraryTerrainMapObject pad;
        resource = GetPadResource(size);
        if (resource != null)
        {
            pad = (ImageLibraryTerrainMapObject)resource.CreateResourceUser();
            result = pad.CanPlace(AoWHSMap.Current.MapContainer, X, Y, L);
            pad?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MoveT.PAS:3666
    public ImageLibraryTerrainMapObject PlacePad(sbyte X, sbyte Y, sbyte L, int size)
    {
        ImageLibraryTerrainMapObject result;
        MapObjectResource resource;
        resource = GetPadResource(size);
        if (resource != null)
        {
            result = (ImageLibraryTerrainMapObject)resource.PlaceResourceUser(AoWHSMap.Current, X, Y, L);
        }
        else
        {
            result = null;
        }

        return result;
    }
}
