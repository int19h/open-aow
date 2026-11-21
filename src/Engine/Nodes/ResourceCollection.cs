#nullable disable warnings
// Translated from Engine.pas.
using static Aow.Rtl.DelphiRandom;

namespace Aow.Engine.Nodes;

// resources with unique ids
public class ResourceCollection(CustomNode? owner) : EngineNode(owner), IChildList<EngineResource>
{
    public new const int ClassId = 1_025;

    // The resource in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a resource, so the read never fails.
    public EngineResource this[int index] => GetChild<EngineResource>(index);

    // A resource list takes resources only. A list of one kind of resource narrows that through
    // AcceptsResource, and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is EngineResource resource && AcceptsResource(resource);

    protected virtual bool AcceptsResource(EngineResource resource) => true;

    int IChildSink<EngineResource>.AddChild(EngineResource child) => AddChild(child);

    // Engine.pas:8573
    protected internal EngineResource GetResource(int index) => this[index];

    // Engine.pas:8578
    public virtual void ResourceChanged()
    {
    }

    // Engine.pas:8582
    public EngineResource? FindResource(int id)
    {
        bool found = false;
        int i = 0;
        while (!found && i != Count)
        {
            found = GetChild(i) is not null && id == this[i].ResourceId;
            i += 1;
        }

        return found ? this[i - 1] : null;
    }

    // The id selects the resource. A resource of another class throws, the same way the call site did.
    public T FindResource<T>(int id) where T : EngineResource => (T)FindResource(id);

    // Engine.pas:8600
    public void ValidateResourceId(int index)
    {
        if (index < Count)
        {
            int id = GetResource(index).ResourceId;
            EngineResource resource = GetResource(index);
            resource.ResourceId ^= 0xFFFF;

            if (FindResource(id) is not null)
            {
                int tries = 10;
                bool found = false;
                int newId = 0;
                while (tries > 0 && !found)
                {
                    newId = Random(268435455);
                    found = FindResource(newId) is null;
                    tries -= 1;
                }

                while (!found)
                {
                    newId = tries;
                    found = FindResource(newId) is null;
                    tries += 1;
                }

                GetResource(index).ResourceId = newId;
            }
            else
            {
                GetResource(index).ResourceId = id;
            }
        }
    }
}
