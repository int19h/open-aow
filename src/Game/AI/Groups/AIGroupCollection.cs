#nullable disable warnings
using System.Collections;

using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

/// <summary>
/// The groups of an AI group list or an AI group manager, in the holder's child slots and their
/// stored order. The holder checks each group that enters and stores the slots. A list holds the
/// groups a player walks. A manager owns the groups it creates. This collection reads them typed
/// and sorts them. A default value has no holder and throws on use, as a missing list does.
/// </summary>
public readonly struct AIGroupCollection : IReadOnlyList<AIGroup>
{

    // Each holder admits only groups (AcceptsChild), so every slot holds a group or is empty.
    internal AIGroupCollection(AIGroupList holder) => Holder = holder;

    internal AIGroupCollection(AIGroupManager holder) => Holder = holder;

    /// <summary>The list or manager that holds the groups.</summary>
    public OwningNode Holder { get; }

    public int Count => Holder.GetCount();

    // The group in slot index, read as the original reads the list, without a range check. The
    // holder admits only groups.
    public AIGroup this[int index] => Holder.GetChild<AIGroup>(index);

    public IEnumerator<AIGroup> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:526
    public AIGroup? GetGroups(int index) => index >= 0 && index < Count ? this[index] : null;

    // AIGroup.pas:533
    public int IndexOfId(int id)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (id == this[result].GroupId)
            {
                return result;
            }
        }

        return -1;
    }

    // AIGroup.pas:544
    public AIGroup? FindId(int id)
    {
        int index = IndexOfId(id);
        return index != -1 ? this[index] : null;
    }

    // AIGroup.pas:559
    public void SortOnProcessPriority() => Holder.SortChildren<AIGroup>(CompareProcessPriority);

    private static int CompareProcessPriority(AIGroup item1, AIGroup item2)
    {
        int number = item2.GetProcessPriority() - item1.GetProcessPriority();
        if (number == 0)
        {
            number = item2.GroupId - item1.GroupId;
        }

        return number;
    }
}
