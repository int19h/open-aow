using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// One explicit registration per editor class keeps descriptor identity in the editor list.
public sealed class EditorClass
{
    private static readonly Catalog<Type, EditorClass> catalog = new("editor class");
    private readonly Action<EObject, GameEngine, EditDoneEvent> edit;

    private EditorClass(EClass eclass, int editorClassId, Action<EObject, GameEngine, EditDoneEvent> edit)
    {
        Class = eclass;
        EditorClassID = editorClassId;
        this.edit = edit;
    }

    public EClass Class { get; }
    public string ClassName => Class.ClassName;
    public int EditorClassID { get; }

    public void Edit(EObject Obj, GameEngine AEngine, EditDoneEvent Done) => edit(Obj, AEngine, Done);

    public static EditorClass Register<T>() where T : EObjectEditor, IEObjectEditorClass =>
        catalog.Add(typeof(T), new EditorClass(EClass.Of<T>(), T.EditorClassID, static (obj, engine, done) => T.Edit(obj, engine, done)));

    public static EditorClass Of<T>() where T : EObjectEditor, IEObjectEditorClass => catalog[typeof(T)];
    public static EditorClass Of(Type type) => catalog[type];

    public static IReadOnlyList<EditorClass> All => catalog.All;
}
