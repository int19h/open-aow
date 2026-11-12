using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// One explicit registration per editor class keeps descriptor identity in the editor list.
public sealed class EditorClass
{
    private static readonly Catalog<Type, EditorClass> s_catalog = new("editor class");
    private readonly Action<EngineObject, GameEngine, EditDoneEvent> _edit;

    private EditorClass(EngineClass engineClass, int editorClassId, Action<EngineObject, GameEngine, EditDoneEvent> edit)
    {
        Class = engineClass;
        EditorClassId = editorClassId;
        this._edit = edit;
    }

    public EngineClass Class { get; }
    public string StorageKey => Class.StorageKey;
    public int EditorClassId { get; }

    public void Edit(EngineObject objectValue, GameEngine engine, EditDoneEvent done) => _edit(objectValue, engine, done);

    public static EditorClass Register<T>() where T : EngineObjectEditor, IEngineObjectEditorClass =>
        s_catalog.Add(typeof(T), new EditorClass(EngineClass.Of<T>(), T.EditorClassId, static (objectValue, engine, done) => T.Edit(objectValue, engine, done)));

    public static EditorClass Of<T>() where T : EngineObjectEditor, IEngineObjectEditorClass => s_catalog[typeof(T)];
    public static EditorClass Of(Type type) => s_catalog[type];

    public static IReadOnlyList<EditorClass> All => s_catalog.All;
}
