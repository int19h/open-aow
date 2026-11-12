// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// The child slots of a TENode or a TEChangeNotifyNode, in stored order. An empty slot holds null,
// and the slot index is the stored slot. The storage and the sorts are the TList ones, so the
// order of equal keys and the spare slot past Count stay as the original keeps them.
public partial class EngineObjectList : DelphiList<EngineObject>
{
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
            found = List[I] != null && classId == List[I].EngineClass.ClassId;
            I += 1;
        }

        if (found)
        {
            result = List[I - 1];
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
            found = List[I] != null && classId == List[I].EngineClass.ClassId;
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

    // Engine.pas:7176. The original tests List^[Count], one past the end, where it means List^[I].
    // That read is the spare slot or the item just deleted, and it fails when the list is full.
    // Nothing in the game or the editor calls ClearList, so no stored list reaches the old test.
    public void ClearList()
    {
        for (int index = Count - 1; index >= 0; index--)
        {
            List[index]?.Dispose();
            Delete(index);
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
            ok = List[I] == null || classId == List[I].EngineClass.ClassId;
            I += 1;
        }

        return ok && Count != 0;
    }
}
