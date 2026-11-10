// Translated from Engine.pas.
using Aow.Game;

namespace Aow.Engine.Nodes;

// editors are classes looked up by the class id of the object (TEObject.Editor). everything is
// called on the class. The base editor edits nothing: Edit reports a cancel at once
public partial class EObjectEditor : EObject, IEObjectEditorClass
{

    public EObjectEditor(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:3036
    // { TEObjectEditor: stubs }
    public static int EditorClassID => 0;

    // Engine.pas:3041
    public static void Edit(EObject Obj, GameEngine AEngine, EditDoneEvent Done)
    {
        Done(EditResult.Cancel, Obj);
    }

    public enum EditModeElement : byte
    {
        Edit = 0, Cut = 1, Copy = 2, Paste = 3, Delete = 4
    }

}
