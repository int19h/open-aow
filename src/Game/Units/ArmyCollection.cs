using System.Collections;

using Aow.Engine.Nodes;

namespace Aow.Game.Units;

/// <summary>
/// The armies registered with a player, in the army list's child slots and their stored order.
/// The list owns the slots, notifies changes and brackets updates. This collection reads the
/// armies and adds and removes them through that list. A default value has no list and throws on
/// use, as a missing list does.
/// </summary>
public readonly struct ArmyCollection : IReadOnlyList<Army>
{
    internal ArmyCollection(ArmyList list) => Holder = list;

    /// <summary>The list that holds the armies.</summary>
    public ArmyList Holder { get; }

    public int Count => Holder.GetCount();

    // Callers of the original list ask for the count by method.
    public int GetCount() => Holder.GetCount();

    // Player.PAS:6. The army in slot index, read as the original reads the list, without a range
    // check.
    public Army GetArmy(int index) => Holder[index];

    public Army this[int index] => GetArmy(index);

    public IEnumerator<Army> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int AddChild(EngineObject child) => Holder.AddChild(child);

    public int RemoveChild(EngineObject child) => Holder.RemoveChild(child);
}
