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
        FResources = new DelphiList();
    }

    public DelphiList FResources;

    // Structur.PAS:3726
    public virtual void Dispose()
    {
        FResources?.Dispose();
    }

    // Structur.PAS:3732
    public Fire.Resource GetRndFireResource(FireType FireType)
    {
        Fire.Resource Result;
        DelphiList Resources;
        Resources = new DelphiList();
        for (int I = 0, last = FResources.Count - 1; I <= last; I++)
        {
            if (((Fire.Resource)FResources.List[I]).FFireType == FireType)
            {
                Resources.Add(FResources.List[I]);
            }
        }

        if (Resources.Count != 0)
        {
            var random = Random(Resources.Count);
            Result = (Fire.Resource)Resources.List[random];
        }
        else
        {
            Result = null;
        }

        Resources?.Dispose();
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
