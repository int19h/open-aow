// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// the child list of a TENode
public partial class EngineObjectList : DelphiList
{
    // Engine.pas:7136
    protected internal EngineObject GetEngineObject(int index) => (EngineObject)this[index];

    // Engine.pas:7141
    protected internal void SetEngineObject(int index, EngineObject value)
    {
        this[index] = value;
    }

    // Engine.pas:7146
    public EngineObject FindObject(int classId)
    {
        EngineObject result;
        int I;
        bool found;
        I = 0;
        found = false;
        while (!found && I < Count)
        {
            found = List[I] != null && classId == ((EngineObject)List[I]).EngineClass.ClassId;
            I += 1;
        }

        if (found)
        {
            result = (EngineObject)List[I - 1];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:7161
    public int FindObjectIndex(int classId)
    {
        int result;
        int I;
        bool found;
        I = 0;
        found = false;
        while (!found && I < Count)
        {
            found = List[I] != null && classId == ((EngineObject)List[I]).EngineClass.ClassId;
            I += 1;
        }

        if (found)
        {
            result = I - 1;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:7176
    public void ClearList()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            // { the original tests List^[Count], one past the end, not List^[I] }
            if (List[Count] != null)
            {
                ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            }

            Delete(I);
        }
    }

    // Engine.pas:7188
    public bool AllHaveClassId(int classId)
    {
        int I;
        bool ok;
        I = 0;
        ok = true;
        while (I < Count)
        {
            ok = List[I] == null || classId == ((EngineObject)List[I]).EngineClass.ClassId;
            I += 1;
        }

        return ok && Count != 0;
    }

    public sealed class ObjectsAccessor(EngineObjectList target)
    {
        public EngineObject this[int index] { get => target.GetEngineObject(index); set => target.SetEngineObject(index, value); }
    }

    public ObjectsAccessor EObjects => new ObjectsAccessor(this);
}
