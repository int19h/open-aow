using System;
using System.Collections.Concurrent;

namespace Aow.Units.Engine;

// The class-side members of an editor class that the engine's editor list reads without an
// instance (Engine.pas TEObjectEditor class methods). A class with its own values re-lists the
// interface and hides the inherited statics with `new`, a class without one inherits them.
public interface IEObjectEditorClass
{
    static abstract int EditorClassID { get; }
    static abstract void Edit(TEObject Obj, TEngine AEngine, TEditDoneEvent Done);
}

// One descriptor per editor class: its engine descriptor, its editor class id and its Edit entry.
// Holder<T> is the only constructor, so a type has one descriptor and the editor list compares by
// reference.
public sealed class EditorClass
{
    private static readonly ConcurrentDictionary<Type, EditorClass> byType = new();
    private readonly Action<TEObject, TEngine, TEditDoneEvent> edit;

    private EditorClass(EClass eclass, int editorClassId, Action<TEObject, TEngine, TEditDoneEvent> edit)
    {
        Class = eclass;
        EditorClassID = editorClassId;
        this.edit = edit;
    }

    public EClass Class { get; }
    public int EditorClassID { get; }
    public void Edit(TEObject Obj, TEngine AEngine, TEditDoneEvent Done) => edit(Obj, AEngine, Done);

    public static EditorClass Of<T>() where T : TEObjectEditor, IEObjectEditorClass => Holder<T>.Value;

    // The descriptor of a type known only at run time, read from the same Holder<T> as Of<T>().
    public static EditorClass Of(Type type) => byType.GetOrAdd(type, static t =>
        (EditorClass)typeof(Holder<>).MakeGenericType(t).GetField(nameof(Holder<TEObjectEditor>.Value))!.GetValue(null)!);

    private static class Holder<T> where T : TEObjectEditor, IEObjectEditorClass
    {
        public static readonly EditorClass Value = new(EClass.Of<T>(), T.EditorClassID, static (obj, engine, done) => T.Edit(obj, engine, done));
    }
}
