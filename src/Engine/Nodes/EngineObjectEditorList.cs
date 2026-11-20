#nullable disable
// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
//   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
public class EngineObjectEditorList
{
    private readonly List<EditorClass> _list = [];

    public int Count => _list.Count;
    public EditorClass this[int index] => _list[index];

    // Engine.pas:8356
    public void RegisterClass(EditorClass classValue)
    {
        if (FindEditor(classValue.EditorClassId) is null)
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
        int i = 0;
        while (i != Count && classId != _list[i].EditorClassId)
        {
            i += 1;
        }

        return i != Count ? _list[i] : null;
    }
}
