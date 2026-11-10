using Aow.Game;

namespace Aow.Engine.Nodes;

// The class-side members of an editor class that the engine's editor list reads without an
// instance (Engine.pas TEObjectEditor class methods). A class with its own values re-lists the
// interface and hides the inherited statics with `new`, a class without one inherits them.
public interface IEObjectEditorClass
{
    static abstract int EditorClassID { get; }

    static abstract void Edit(EObject Obj, GameEngine AEngine, EditDoneEvent Done);
}
