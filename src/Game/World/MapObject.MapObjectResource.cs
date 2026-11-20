#nullable disable
using Aow.Engine;
using Aow.Formats.Tree;

namespace Aow.Game.World;

public partial class MapObject
{
    private int _unlinkedResourceId;

    public int ResourceId
    {
        get => ResourceLinked ? _resource.ResourceId : _unlinkedResourceId;
        set
        {
            if (ResourceLinked)
            {
                throw new InvalidOperationException("Cannot set ResourceID when resource is linked!");
            }

            _unlinkedResourceId = value;
            _resource = null;
        }
    }

    public virtual bool LinkToResource(GameEngine engine)
    {
        if (!ResourceLinked)
        {
            MapObjectResource resource = GetResourceList(engine).FindResource<MapObjectResource>(_unlinkedResourceId);
            if (resource is not null)
            {
                _resource = resource;
                ResourceLinked = true;
            }
        }
        return ResourceLinked;
    }

    public override void PersistFields(IPersister persister)
    {
        // The original stores nothing in a text stream, the translate stream's included.
        if (PersistSupport.Translating(persister))
        {
            return;
        }

        if (persister.Loading)
        {
            ResourceLinked = false;
            _unlinkedResourceId = persister.Int32("ResourceId", value: _unlinkedResourceId);
            _resource = null;
            _ = LinkToResource(PersistSupport.Engine(persister));
            if (!ResourceLinked)
            {
                throw new MapObjectResourceMissingException($"Resource Missing {GetType().Name} {_unlinkedResourceId}");
            }
        }
        else
        {
            int id = ResourceId;
            _ = persister.Int32("ResourceId", value: id);
        }
    }
}
