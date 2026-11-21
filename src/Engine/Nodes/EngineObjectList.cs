#nullable disable warnings
// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// The child slots of a TENode or a TEChangeNotifyNode, in stored order. An empty slot holds null,
// and the slot index is the stored slot. The storage and the sorts are the TList ones, so the
// order of equal keys and the spare slot past Count stay as the original keeps them.
public sealed class EngineObjectList : DelphiList<EngineObject>
{
    // Engine.pas:7146
    public EngineObject? FindObject(int classId)
    {
        int i = 0;
        bool found = false;
        while (!found && i < Count)
        {
            found = List[i] is not null && classId == List[i].EngineClass.ClassId;
            i += 1;
        }

        return found ? List[i - 1] : null;
    }

    // Engine.pas:7161
    public int FindObjectIndex(int classId)
    {
        int i = 0;
        bool found = false;
        while (!found && i < Count)
        {
            found = List[i] is not null && classId == List[i].EngineClass.ClassId;
            i += 1;
        }

        return found ? i - 1 : -1;
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
        int i = 0;
        bool ok = true;
        while (i < Count)
        {
            ok = List[i] is null || classId == List[i].EngineClass.ClassId;
            i += 1;
        }

        return ok && Count != 0;
    }
}
