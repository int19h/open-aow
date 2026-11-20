#nullable disable
// Translated from AoWE.pas.
using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

// Resources
public partial class FireControl : IDisposable
{
    // Structur.PAS:3720
    // Resources
    public FireControl() => _resources = [];

    private readonly List<Fire.Resource> _resources;

    // Structur.PAS:3726
    public virtual void Dispose() => _resources?.Clear();

    // Structur.PAS:3732
    public Fire.Resource GetRandomFireResource(FireType fireType)
    {
        List<Fire.Resource> resources = [];
        for (int i = 0, last = _resources.Count - 1; i <= last; i++)
        {
            if (_resources[i].FireType == fireType)
            {
                resources.Add(_resources[i]);
            }
        }

        Fire.Resource result = resources.Count != 0 ? resources[Random(resources.Count)] : null;

        resources?.Clear();
        return result;
    }

    // Structur.PAS:3747
    public Fire PlaceFire(int X, int Y, int L, FireType fireType)
    {
        Fire.Resource resource = GetRandomFireResource(fireType);
        Fire result;
        if (resource is not null)
        {
            Fire fire = resource.CreateResourceUser<Fire>();
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
    public void RegisterFireResource(Fire.Resource resource) => _resources.Add(resource);

    // Structur.PAS:3776
    public void UnregisterFireResource(Fire.Resource resource) => _resources.Remove(resource);

    // intf_P1_11.inc:62

}
