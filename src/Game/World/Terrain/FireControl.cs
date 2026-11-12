// Translated from AoWE.pas.
using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// Resources
public partial class FireControl : IDisposable
{
    // Structur.PAS:3720
    // Resources
    public FireControl()
    {
        Resources = new List<Fire.Resource>();
    }

    public List<Fire.Resource> Resources;

    // Structur.PAS:3726
    public virtual void Dispose()
    {
        Resources?.Clear();
    }

    // Structur.PAS:3732
    public Fire.Resource GetRandomFireResource(FireType fireType)
    {
        Fire.Resource result;
        List<Fire.Resource> resources;
        resources = new List<Fire.Resource>();
        for (int I = 0, last = Resources.Count - 1; I <= last; I++)
        {
            if (Resources[I].FireType == fireType)
            {
                resources.Add(Resources[I]);
            }
        }

        if (resources.Count != 0)
        {
            var random = Random(resources.Count);
            result = resources[random];
        }
        else
        {
            result = null;
        }

        resources?.Clear();
        return result;
    }

    // Structur.PAS:3747
    public Fire PlaceFire(int X, int Y, int L, FireType fireType)
    {
        Fire result;
        Fire.Resource resource;
        Fire fire;
        resource = GetRandomFireResource(fireType);
        if (resource != null)
        {
            fire = (Fire)resource.CreateResourceUser();
            fire.Player = AoWHSMap.Current.CurrentPlayer;
            if (!AoWHSMap.Current.PlaceHex(fire, X, Y, L))
            {
                fire?.Dispose();
                result = null;
            }
            else
            {
                result = fire;
                fire.TriggerDamage();
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Structur.PAS:3771
    public void RegisterFireResource(Fire.Resource resource)
    {
        Resources.Add(resource);
    }

    // Structur.PAS:3776
    public void UnregisterFireResource(Fire.Resource resource)
    {
        Resources.Remove(resource);
    }

    // intf_P1_11.inc:62

}
