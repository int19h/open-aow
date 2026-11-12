// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// resources with unique ids
public partial class EResourceList : ECustomOwner, IChildList<EngineResource>
{
    public new const int ClassId = 1025;

    // The resource in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a resource, so the cast never fails.
    public EngineResource this[int index] => (EngineResource)GetChild(index);

    // A resource list takes resources only. A list of one kind of resource narrows that through
    // AcceptsResource, and cannot widen it.
    protected sealed override bool AcceptsChild(EngineObject child) => child is EngineResource resource && AcceptsResource(resource);

    protected virtual bool AcceptsResource(EngineResource resource) => true;

    int IChildSink<EngineResource>.AddChild(EngineResource child) => AddChild(child);

    public EResourceList(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:8573
    protected internal EngineResource GetResource(int index) => this[index];

    // Engine.pas:8578
    public virtual void ResourceChanged()
    {
    }

    // Engine.pas:8582
    public EngineResource FindResource(int id)
    {
        EngineResource result;
        int I;
        bool found;
        found = false;
        I = 0;
        while (!found && I != this.Count)
        {
            found = this.GetChild(I) != null && id == this[I]._resourceId;
            I += 1;
        }

        if (found)
        {
            result = this[I - 1];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:8600
    public void ValidateResourceId(int index)
    {
        int id;
        int newId;
        int tries;
        bool found;
        if (index < Count)
        {
            id = GetResource(index)._resourceId;
            var resource = GetResource(index);
            resource._resourceId = resource._resourceId ^ 65535;

            if (FindResource(id) != null)
            {
                tries = 10;
                found = false;
                newId = 0;
                while (tries > 0 && !found)
                {
                    newId = Random(268435455);
                    found = FindResource(newId) == null;
                    tries -= 1;
                }

                while (!found)
                {
                    newId = tries;
                    found = FindResource(newId) == null;
                    tries += 1;
                }

                GetResource(index)._resourceId = newId;
            }
            else
            {
                GetResource(index)._resourceId = id;
            }
        }
    }

    public sealed class ResourcesAccessor(EResourceList target)
    {
        public EngineResource this[int index] { get => target.GetResource(index); }
    }

    public ResourcesAccessor Resources => new ResourcesAccessor(this);

}
