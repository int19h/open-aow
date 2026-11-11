// Translated from Engine.pas.

namespace Aow.Engine.Nodes;

// { Engine.TEObjectEditorList: the editor descriptors an engine registers, one per editor class id
//   (an EditorClass per editor class since 9.4-4, a list of editor metaclasses before) }
public partial class EngineObjectEditorList
{
    private readonly List<EditorClass> FList = new();

    public int Count { get => FList.Count; }
    public EditorClass this[int Index] { get => FList[Index]; }

    // Engine.pas:8356
    public void RegisterClass(EditorClass AClass)
    {
        if (FindEditor(AClass.EditorClassID) == null)
        {
            FList.Add(AClass);
        }
        else
        {
            throw new InvalidOperationException("EObjectEditor Double Defined!");
        }
    }

    // Engine.pas:8364
    public EditorClass FindEditor(int AClassID)
    {
        EditorClass Result;
        int I;
        I = 0;
        while (I != Count && AClassID != FList[I].EditorClassID)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = FList[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }
}
