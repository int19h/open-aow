// Translated from Engine.pas.
using Aow.Game;

namespace Aow.Engine.Nodes;

// editors are classes looked up by the class id of the object (TEObject.Editor). everything is
// called on the class. The base editor edits nothing: Edit reports a cancel at once
public partial class EngineObjectEditor(CustomNode owner) : EngineObject(owner), IEngineObjectEditorClass
{

    // Engine.pas:3036
    // { TEObjectEditor: stubs }
    public static int EditorClassId => 0;

    // Engine.pas:3041
    public static void Edit(EngineObject objectValue, GameEngine engine, Action<EditResult, EngineObject> done) => done(EditResult.Cancel, objectValue);

    [Flags]
    public enum EditModes : byte
    {
        None = 0,
        Edit = 1 << 0,
        Cut = 1 << 1,
        Copy = 1 << 2,
        Paste = 1 << 3,
        Delete = 1 << 4,
    }
}
