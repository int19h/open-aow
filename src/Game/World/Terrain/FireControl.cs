#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

// Resources
public sealed class FireControl : IDisposable
{
    // Structur.PAS:3720
    // Resources
    public FireControl() => _resources = [];

    private readonly List<Fire.Resource> _resources;

    // Structur.PAS:3726
    public void Dispose() => _resources?.Clear();

    // Structur.PAS:3732
    public Fire.Resource? GetRandomFireResource(FireType fireType)
    {
        List<Fire.Resource> resources = [];
        resources.AddRange(_resources.Where(item => item.FireType == fireType));
        Fire.Resource? result = resources.Count != 0 ? resources[Random(resources.Count)] : null;

        resources?.Clear();
        return result;
    }

    // Structur.PAS:3747
    public Fire? PlaceFire(int X, int Y, int L, FireType fireType)
    {
        Fire.Resource? resource = GetRandomFireResource(fireType);
        Fire result;
        if (resource is not null)
        {
            Fire fire = resource.CreateResourceUser<Fire>();
            fire.Player = StrategicMap.Current.CurrentPlayer;
            if (!StrategicMap.Current.PlaceHex(fire, X, Y, L))
            {
                fire?.Dispose();
                return null;
            }
            else
            {
                result = fire;
                fire.TriggerDamage();
            }
        }
        else
        {
            return null;
        }

        return result;
    }

    // Structur.PAS:3771
    public void RegisterFireResource(Fire.Resource resource) => _resources.Add(resource);

    // Structur.PAS:3776
    public void UnregisterFireResource(Fire.Resource resource) => _resources.Remove(resource);
}
