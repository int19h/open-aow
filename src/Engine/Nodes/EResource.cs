// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
// FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
public partial class EResource : EObject
{
    public new const int ClassID = 1024;

    public EResource(ECustomNode owner) : base(owner)
    { }

    internal ECustomNode FResourceList;
    internal int FResourceID;
    internal int FEditID;
    internal int FEditIndex;

    // Engine.pas:8513
    public override void SetOwner(ECustomNode AOwner)
    {
        base.SetOwner(AOwner);
        FResourceList = AOwner;
    }

    // Engine.pas:8542
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FResourceID, 1);
        Stream.rwInteger(ref FEditID, 2);
        Stream.rwInteger(ref FEditIndex, 3);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("ResourceID", ref FResourceID);
        persister.Int("EditID", ref FEditID);
        persister.Int("EditIndex", ref FEditIndex);
    }

    // Engine.pas:8519
    public override void SetArrayOwner(ECustomNode AOwner, int Index)
    {
        base.SetArrayOwner(AOwner, Index);
        FResourceList = AOwner;
    }

    // Engine.pas:8533
    public virtual LibraryImage GetCellImage() => null;

    // Engine.pas:8538
    public virtual void SetCellImage(LibraryImage Value)
    {
    }

    // Engine.pas:8550
    public virtual int ResourceUserClassID() => 0;

    // Engine.pas:8555
    public virtual EObject CreateResourceUser()
    {
        EObject Result;
        int UserClassID;
        UserClassID = ResourceUserClassID();
        if (UserClassID != 0)
        {
            Result = GetEngine().CreateEObject(UserClassID, null);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:8525
    public virtual void Draw(Canvas Canvas, Rect Rect, Game.Draw.Images.BltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color)
    {
        Rect R = default;
        LibraryDrawing.BltLibraryImage(Canvas, GetCellImage(), Rect, Mode, ScaleX, ScaleY, BkColor, Color, false, ref R);
    }

    public ECustomNode ResourceList { get => FResourceList; set => FResourceList = value; }
    public int EditID { get => FEditID; set => FEditID = value; }
    public int EditIndex { get => FEditIndex; set => FEditIndex = value; }
    public int ResourceID { get => FResourceID; set => FResourceID = value; }
    public LibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

    // Engine.pas:8508
    public virtual string ResourceEditName() => GetType().Name;

}
