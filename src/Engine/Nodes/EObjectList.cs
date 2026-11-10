// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// the child list of a TENode
public partial class EObjectList : DelphiList
{
    // Engine.pas:7136
    protected internal EObject GetEObject(int Index) => (EObject)this[Index];

    // Engine.pas:7141
    protected internal void SetEObject(int Index, EObject Value)
    {
        this[Index] = Value;
    }

    // Engine.pas:7146
    public EObject FindObject(int AClassID)
    {
        EObject Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = List[I] != null && AClassID == ((EObject)List[I]).EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = (EObject)List[I - 1];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:7161
    public int FindObjectIndex(int AClassID)
    {
        int Result;
        int I;
        bool Found;
        I = 0;
        Found = false;
        while (!Found && I < Count)
        {
            Found = List[I] != null && AClassID == ((EObject)List[I]).EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = I - 1;
        }
        else
        {
            Result = -1;
        }

        return Result;
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
    public bool AllHaveCID(int AClassID)
    {
        int I;
        bool OK;
        I = 0;
        OK = true;
        while (I < Count)
        {
            OK = List[I] == null || AClassID == ((EObject)List[I]).EClass.ClassID;
            I += 1;
        }

        return OK && Count != 0;
    }

    public sealed class EObjectsAccessor(EObjectList target)
    {
        public EObject this[int Index] { get => target.GetEObject(Index); set => target.SetEObject(Index, value); }
    }

    public EObjectsAccessor EObjects => new EObjectsAccessor(this);
}
