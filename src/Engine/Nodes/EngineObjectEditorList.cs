// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
//   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
public partial class EngineObjectEditorList
{
    private readonly List<EditorClass> _list = new();

    public int Count { get => _list.Count; }
    public EditorClass this[int index] { get => _list[index]; }

    // Engine.pas:8356
    public void RegisterClass(EditorClass classValue)
    {
        if (FindEditor(classValue.EditorClassId) == null)
        {
            _list.Add(classValue);
        }
        else
        {
            throw new InvalidOperationException("EObjectEditor Double Defined!");
        }
    }

    // Engine.pas:8364
    public EditorClass FindEditor(int classId)
    {
        EditorClass result;
        int I;
        I = 0;
        while (I != Count && classId != _list[I].EditorClassId)
        {
            I += 1;
        }

        if (I != Count)
        {
            result = _list[I];
        }
        else
        {
            result = null;
        }

        return result;
    }
}
