#nullable disable
// Translated from Engine.pas.
using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FResourceList, FResourceID, FEditID (the edit id of the resource grid that placed it),
// FEditIndex (its cell in that grid). all three integers are persisted (ids 1.3)
public class EngineResource(CustomNode owner) : EngineObject(owner)
{
    public new const int ClassId = 1024;

    // Engine.pas:8513
    public override void SetOwner(CustomNode owner)
    {
        base.SetOwner(owner);
        ResourceList = owner;
    }

    // Engine.pas:8542
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ResourceId = persister.Int32("ResourceId", ResourceId);
        EditId = persister.Int32("EditId", EditId);
        EditIndex = persister.Int32("EditIndex", EditIndex);
    }

    // Engine.pas:8519
    public override void SetArrayOwner(CustomNode owner, int index)
    {
        base.SetArrayOwner(owner, index);
        ResourceList = owner;
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
        int userClassId = ResourceUserClassId();
        return userClassId != 0 ? GetEngine().CreateEngineObject(userClassId, owner: null) : null;
    }

    // The caller names the user class ResourceUserClassId registers. Another class throws. A zero
    // class id returns null, as CreateResourceUser does.
    public T CreateResourceUser<T>() where T : EngineObject => (T)CreateResourceUser();

    // Engine.pas:8525
    public virtual void Draw(Canvas canvas, Rect rectangle, BlitMode mode, int scaleX, int scaleY, int bkColor, int color)
    {
        Rect r = default;
        LibraryDrawing.BlitLibraryImage(canvas, image: GetCellImage(), rectangle, mode, scaleX, scaleY, bkColor, color, selected: false, drawRectangle: in r);
    }

    public CustomNode ResourceList { get; private set; }
    public int EditId { get; set; }
    public int EditIndex { get; set; }
    public int ResourceId { get; set; }
    public LibraryImage CellImage => GetCellImage();

    // Engine.pas:8508
    public virtual string ResourceEditName() => GetType().Name;

}
