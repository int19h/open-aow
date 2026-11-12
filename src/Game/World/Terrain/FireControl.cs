// Translated from AoWE.pas.
using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// Resources
public partial class FireControl : IDisposable
{
    // Structur.PAS:3720
    // Resources
    public FireControl()
    {
        _resources = new List<Fire.Resource>();
    }

    private readonly List<Fire.Resource> _resources;

    // Structur.PAS:3726
    public virtual void Dispose()
    {
        _resources?.Clear();
    }

    // Structur.PAS:3732
    public Fire.Resource GetRandomFireResource(FireType fireType)
    {
        Fire.Resource result;
        List<Fire.Resource> resources;
        resources = new List<Fire.Resource>();
        for (int I = 0, last = _resources.Count - 1; I <= last; I++)
        {
            if (_resources[I].FireType == fireType)
            {
                resources.Add(_resources[I]);
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
            fire.Player = StrategicMap.Current.CurrentPlayer;
            if (!StrategicMap.Current.PlaceHex(fire, X, Y, L))
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
        _resources.Add(resource);
    }

    // Structur.PAS:3776
    public void UnregisterFireResource(Fire.Resource resource)
    {
        _resources.Remove(resource);
    }

    // intf_P1_11.inc:62

}
