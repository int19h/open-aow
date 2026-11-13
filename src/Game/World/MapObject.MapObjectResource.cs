using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.World;

public partial class MapObject
{
    private int _unlinkedResourceId;

    protected internal int GetResourceId() => GetResourceLinked() ? _resource.ResourceId : _unlinkedResourceId;

    protected internal void SetResourceId(int value)
    {
        if (GetResourceLinked()) throw new InvalidOperationException("Cannot set ResourceID when resource is linked!");
        _unlinkedResourceId = value;
        _resource = null;
    }

    public virtual bool LinkToResource(GameEngine engine)
    {
        if (!GetResourceLinked())
        {
            var resource = GetResourceList(engine).FindResource(_unlinkedResourceId);
            if (resource != null)
            {
                _resource = (MapObjectResource)resource;
                SetResourceLinked(true);
            }
        }
        return GetResourceLinked();
    }

    public override void ReadWrite(StorageStream stream)
    {
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.Text, StorageModeElement.Compare)) != default) return;
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            SetResourceLinked(false);
            stream.Int32(ref _unlinkedResourceId, 1);
            _resource = null;
            LinkToResource(stream.Engine);
            if (!GetResourceLinked()) throw new MapObjectResourceMissingException($"Resource Missing {GetType().Name} {_unlinkedResourceId}");
        }
        else
        {
            int id = GetResourceId();
            stream.Int32(ref id, 1);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        if (persister.Loading)
        {
            SetResourceLinked(false);
            _unlinkedResourceId = persister.Int32("ResourceId", value: _unlinkedResourceId);
            _resource = null;
            LinkToResource(PersistSupport.Engine(persister));
            if (!GetResourceLinked()) throw new MapObjectResourceMissingException($"Resource Missing {GetType().Name} {_unlinkedResourceId}");
        }
        else
        {
            int id = GetResourceId();
            id = persister.Int32("ResourceId", value: id);
        }
    }
}
