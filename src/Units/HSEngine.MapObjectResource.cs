using Aow.Rtl;
using Aow.Units.Engine;
using static Aow.Units.Engine.EngineUnit;

namespace Aow.Units.HSEngine;

public partial class TMapObject
{
    private int unlinkedResourceId;

    protected internal int GetResourceID() => GetResourceLinked() ? FResource.ResourceID : unlinkedResourceId;

    protected internal void SetResourceID(int value)
    {
        if (GetResourceLinked()) throw new InvalidOperationException("Cannot set ResourceID when resource is linked!");
        unlinkedResourceId = value;
        FResource = null;
    }

    public virtual bool LinkToResource(TEngine engine)
    {
        if (!GetResourceLinked())
        {
            var resource = GetResourceList(engine).FindResource(unlinkedResourceId);
            if (resource != null)
            {
                FResource = (TMapObjectResource)resource;
                SetResourceLinked(true);
            }
        }
        return GetResourceLinked();
    }

    public override void ReadWrite(TEStorageStream stream)
    {
        if ((stream.StorageMode() & TSet<TStorageModeElement>.Of(smText, TStorageModeElement.smCompare)) != default) return;
        if (stream.StorageMode().Contains(TStorageModeElement.smRead))
        {
            SetResourceLinked(false);
            stream.rwInteger(ref unlinkedResourceId, 1);
            FResource = null;
            LinkToResource(stream.Engine);
            if (!GetResourceLinked()) throw new EMapObjectResourceMissing($"Resource Missing {GetType().Name} {unlinkedResourceId}");
        }
        else
        {
            int id = GetResourceID();
            stream.rwInteger(ref id, 1);
        }
    }
}
