// Translated from Engine.pas.
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// resources with unique ids
public partial class EResourceList : ECustomOwner
{
    public new const int ClassID = 1025;

    public EResourceList(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:8573
    protected internal EResource GetResource(int Index) => (EResource)FChildren.List[Index];

    // Engine.pas:8578
    public virtual void ResourceChanged()
    {
    }

    // Engine.pas:8582
    public EResource FindResource(int ID)
    {
        EResource Result;
        int I;
        bool Found;
        Found = false;
        I = 0;
        while (!Found && I != this.FChildren.Count)
        {
            Found = this.FChildren.List[I] != null && ID == ((EResource)this.FChildren.List[I]).FResourceID;
            I += 1;
        }

        if (Found)
        {
            Result = (EResource)this.FChildren.List[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8600
    public void ValidateResourceID(int Index)
    {
        int ID;
        int NewID;
        int Tries;
        bool Found;
        if (Index < FChildren.Count)
        {
            ID = GetResource(Index).FResourceID;
            var resource = GetResource(Index);
            resource.FResourceID = resource.FResourceID ^ 65535;

            if (FindResource(ID) != null)
            {
                Tries = 10;
                Found = false;
                NewID = 0;
                while (Tries > 0 && !Found)
                {
                    NewID = Random(268435455);
                    Found = FindResource(NewID) == null;
                    Tries -= 1;
                }

                while (!Found)
                {
                    NewID = Tries;
                    Found = FindResource(NewID) == null;
                    Tries += 1;
                }

                GetResource(Index).FResourceID = NewID;
            }
            else
            {
                GetResource(Index).FResourceID = ID;
            }
        }
    }

    public sealed class ResourcesAccessor(EResourceList target)
    {
        public EResource this[int Index] { get => target.GetResource(Index); }
    }

    public ResourcesAccessor Resources => new ResourcesAccessor(this);

}
