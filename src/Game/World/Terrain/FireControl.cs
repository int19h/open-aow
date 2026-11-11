// Translated from AoWE.pas.
using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// FResources
public partial class FireControl : IDisposable
{
    // Structur.PAS:3720
    // FResources
    public FireControl()
    {
        FResources = new List<Fire.Resource>();
    }

    public List<Fire.Resource> FResources;

    // Structur.PAS:3726
    public virtual void Dispose()
    {
        FResources?.Clear();
    }

    // Structur.PAS:3732
    public Fire.Resource GetRndFireResource(FireType FireType)
    {
        Fire.Resource Result;
        List<Fire.Resource> Resources;
        Resources = new List<Fire.Resource>();
        for (int I = 0, last = FResources.Count - 1; I <= last; I++)
        {
            if (FResources[I].FFireType == FireType)
            {
                Resources.Add(FResources[I]);
            }
        }

        if (Resources.Count != 0)
        {
            var random = Random(Resources.Count);
            Result = Resources[random];
        }
        else
        {
            Result = null;
        }

        Resources?.Clear();
        return Result;
    }

    // Structur.PAS:3747
    public Fire PlaceFire(int X, int Y, int L, FireType FireType)
    {
        Fire Result;
        Fire.Resource Resource;
        Fire Fire;
        Resource = GetRndFireResource(FireType);
        if (Resource != null)
        {
            Fire = (Fire)Resource.CreateResourceUser();
            Fire.FPlayer = AoWHSMap.Current.FCurrentPlayer;
            if (!AoWHSMap.Current.PlaceHX(Fire, X, Y, L))
            {
                Fire?.Dispose();
                Result = null;
            }
            else
            {
                Result = Fire;
                Fire.TriggerDamage();
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Structur.PAS:3771
    public void RegisterFireResource(Fire.Resource Resource)
    {
        FResources.Add(Resource);
    }

    // Structur.PAS:3776
    public void UnregisterFireResource(Fire.Resource Resource)
    {
        FResources.Remove(Resource);
    }

    // intf_P1_11.inc:62

}
