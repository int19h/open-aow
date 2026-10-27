using Aow.Units.AoWE;
using Aow.Units.HSEngine;
using Aow.Units.Terrain;

namespace Aow.Units.AoWTer;

public partial class TAoWTerrain
{
    // AoWTer.pas:45. Message $1105 carries a TFire whose byte $1E is FAge.
    public void ValidateDenseVegetationBurned(object info)
    {
        if (((TFire)info).FAge < 2 || (short)FResource.TerrainList.Overlay[0] != 1) return;
        var resource = FindCompatibleResource(((TTerrainMOR)FResource).TerrainType, 3, 3);
        if (resource == null) return;
        UpdateTerrain((TMapLevel)Owner.Owner, GetXhx(), GetYhx());
        var owner = Owner;
        RemoveFromMap();
        SetResource(resource);
        Owner = owner;
        PlaceOnMap();
        if (FResource.Level <= 15) FResource.Engine.HSMap.StaticSceneInvalid = true;
        OverrideTerrain();
    }
}
