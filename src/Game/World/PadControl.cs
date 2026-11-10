// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using Aow.Rtl;

namespace Aow.Game.World;

// { Interface declaration run owned by P5. }
// pad resources, terrain types
public partial class PadControl : EControlStyleObject
{
    public DelphiList FPadResources;
    public BitList FTerrainTypes;

    // MoveT.PAS:3622
    public override void Dispose()
    {
        FTerrainTypes?.Dispose();
        FPadResources?.Dispose();
        base.Dispose();
    }

    // MoveT.PAS:3614
    public PadControl(ECustomNode owner) : base(owner)
    {
        FTerrainTypes = new BitList();
        FPadResources = new DelphiList();
        SetControlStyle(Set<EControlStyleElement>.Of(EControlStyleElement.FixedChild));
    }

    // MoveT.PAS:3629
    public MapObjectResource GetPadResource(int Size)
    {
        MapObjectResource Result;
        if (Size < FPadResources.Count)
        {
            Result = (MapObjectResource)FPadResources.List[Size];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // MoveT.PAS:3635
    public void SetPadResource(int Size, MapObjectResource Resource)
    {
        while (FPadResources.Count <= Size)
        {
            FPadResources.Add(null);
        }

        FPadResources.List[Size] = Resource;
    }

    // MoveT.PAS:3641
    public void RegisterPadResource(MapObjectResource Resource, int Size)
    {
        if (GetPadResource(Size) == null)
        {
            SetPadResource(Size, Resource);
        }
    }

    // MoveT.PAS:3646
    public void UnregisterPadResource(MapObjectResource Resource, int Size)
    {
        if (Resource == GetPadResource(Size))
        {
            SetPadResource(Size, null);
        }
    }

    // MoveT.PAS:3651
    public bool CanPlacePad(sbyte X, sbyte Y, sbyte L, int Size)
    {
        bool Result;
        MapObjectResource Resource;
        ILTerrainMO Pad;
        Resource = GetPadResource(Size);
        if (Resource != null)
        {
            Pad = (ILTerrainMO)Resource.CreateResourceUser();
            Result = Pad.CanPlace(AoWHSMap.Current.MapContainer, X, Y, L);
            Pad?.Dispose();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MoveT.PAS:3666
    public ILTerrainMO PlacePad(sbyte X, sbyte Y, sbyte L, int Size)
    {
        ILTerrainMO Result;
        MapObjectResource Resource;
        Resource = GetPadResource(Size);
        if (Resource != null)
        {
            Result = (ILTerrainMO)Resource.PlaceResourceUser(AoWHSMap.Current, X, Y, L);
        }
        else
        {
            Result = null;
        }

        return Result;
    }
}
