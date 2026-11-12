// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// resources with unique ids
public partial class EResourceList : ECustomOwner
{
    public new const int ClassId = 1025;

    public EResourceList(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:8573
    protected internal EngineResource GetResource(int index) => (EngineResource)_children.List[index];

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
        while (!found && I != this._children.Count)
        {
            found = this._children.List[I] != null && id == ((EngineResource)this._children.List[I])._resourceId;
            I += 1;
        }

        if (found)
        {
            result = (EngineResource)this._children.List[I - 1];
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
        if (index < _children.Count)
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
