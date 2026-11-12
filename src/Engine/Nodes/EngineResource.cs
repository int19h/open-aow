// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
// FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
public partial class EngineResource : EngineObject
{
    public new const int ClassId = 1024;

    public EngineResource(ECustomNode owner) : base(owner)
    { }

    internal ECustomNode _resourceList;
    internal int _resourceId;
    internal int _editId;
    internal int _editIndex;

    // Engine.pas:8513
    public override void SetOwner(ECustomNode owner)
    {
        base.SetOwner(owner);
        _resourceList = owner;
    }

    // Engine.pas:8542
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _resourceId, 1);
        stream.rwInteger(ref _editId, 2);
        stream.rwInteger(ref _editIndex, 3);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("ResourceId", ref _resourceId);
        persister.Int32("EditId", ref _editId);
        persister.Int32("EditIndex", ref _editIndex);
    }

    // Engine.pas:8519
    public override void SetArrayOwner(ECustomNode owner, int index)
    {
        base.SetArrayOwner(owner, index);
        _resourceList = owner;
    }

    // Engine.pas:8533
    public virtual LibraryImage GetCellImage() => null;

    // Engine.pas:8538
    public virtual void SetCellImage(LibraryImage value)
    {
    }

    // Engine.pas:8550
    public virtual int ResourceUserClassId() => 0;

    // Engine.pas:8555
    public virtual EngineObject CreateResourceUser()
    {
        EngineObject result;
        int userClassId;
        userClassId = ResourceUserClassId();
        if (userClassId != 0)
        {
            result = GetEngine().CreateEngineObject(userClassId, null);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:8525
    public virtual void Draw(Canvas canvas, Rect rectangle, Game.Draw.Images.BlitMode mode, int scaleX, int scaleY, int bkColor, int color)
    {
        Rect R = default;
        LibraryDrawing.BlitLibraryImage(canvas, GetCellImage(), rectangle, mode, scaleX, scaleY, bkColor, color, false, ref R);
    }

    public ECustomNode ResourceList { get => _resourceList; set => _resourceList = value; }
    public int EditId { get => _editId; set => _editId = value; }
    public int EditIndex { get => _editIndex; set => _editIndex = value; }
    public int ResourceId { get => _resourceId; set => _resourceId = value; }
    public LibraryImage CellImage { get => GetCellImage(); set => SetCellImage(value); }

    // Engine.pas:8508
    public virtual string ResourceEditName() => GetType().Name;

}
