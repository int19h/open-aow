using Aow.Game.Engine;
using Aow.Rtl;

namespace Aow.Game.Engine.Nodes;

// The class-side members of an editor class that the engine's editor list reads without an
// instance (Engine.pas TEObjectEditor class methods). A class with its own values re-lists the
// interface and hides the inherited statics with `new`, a class without one inherits them.
public interface IEObjectEditorClass
{
    static abstract int EditorClassID { get; }

    static abstract void Edit(TEObject Obj, Aow.Game.Engine.TEngine AEngine, TEditDoneEvent Done);
}

// One explicit registration per editor class keeps descriptor identity in the editor list.
public sealed class EditorClass
{
    private static readonly Catalog<Type, EditorClass> catalog = new("editor class");
    private readonly Action<TEObject, Aow.Game.Engine.TEngine, TEditDoneEvent> edit;

    private EditorClass(Aow.Game.Engine.EClass eclass, int editorClassId, Action<TEObject, Aow.Game.Engine.TEngine, TEditDoneEvent> edit)
    {
        Class = eclass;
        EditorClassID = editorClassId;
        this.edit = edit;
    }

    public Aow.Game.Engine.EClass Class { get; }
    public string ClassName => Class.ClassName;
    public int EditorClassID { get; }

    public void Edit(TEObject Obj, Aow.Game.Engine.TEngine AEngine, TEditDoneEvent Done) => edit(Obj, AEngine, Done);

    public static EditorClass Register<T>() where T : TEObjectEditor, IEObjectEditorClass =>
        catalog.Add(typeof(T), new EditorClass(Aow.Game.Engine.EClass.Of<T>(), T.EditorClassID, static (obj, engine, done) => T.Edit(obj, engine, done)));

    public static EditorClass Of<T>() where T : TEObjectEditor, IEObjectEditorClass => catalog[typeof(T)];
    public static EditorClass Of(Type type) => catalog[type];

    public static IReadOnlyList<EditorClass> All => catalog.All;
}
