using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.World;

public partial class MapObject
{
    private int unlinkedResourceId;

    protected internal int GetResourceID() => GetResourceLinked() ? FResource.ResourceID : unlinkedResourceId;

    protected internal void SetResourceID(int value)
    {
        if (GetResourceLinked()) throw new InvalidOperationException("Cannot set ResourceID when resource is linked!");
        unlinkedResourceId = value;
        FResource = null;
    }

    public virtual bool LinkToResource(GameEngine engine)
    {
        if (!GetResourceLinked())
        {
            var resource = GetResourceList(engine).FindResource(unlinkedResourceId);
            if (resource != null)
            {
                FResource = (MapObjectResource)resource;
                SetResourceLinked(true);
            }
        }
        return GetResourceLinked();
    }

    public override void ReadWrite(EStorageStream stream)
    {
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.smText, StorageModeElement.Compare)) != default) return;
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            SetResourceLinked(false);
            stream.rwInteger(ref unlinkedResourceId, 1);
            FResource = null;
            LinkToResource(stream.Engine);
            if (!GetResourceLinked()) throw new MapObjectResourceMissingException($"Resource Missing {GetType().Name} {unlinkedResourceId}");
        }
        else
        {
            int id = GetResourceID();
            stream.rwInteger(ref id, 1);
        }
    }
}
