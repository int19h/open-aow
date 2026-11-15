using System.Collections;

namespace Aow.Game.AI.Groups;

/// <summary>
/// The unit productions of a unit production list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the productions. This collection reads
/// them typed. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct AIGroupUnitProductionCollection : IReadOnlyList<AIGroupUnitProduction>
{
    private readonly AIGroupUnitProductionList _list;

    internal AIGroupUnitProductionCollection(AIGroupUnitProductionList list) => _list = list;

    /// <summary>The list that holds the productions.</summary>
    public AIGroupUnitProductionList Holder => _list;

    public int Count => _list.GetCount();

    // The production in slot index, read as the original reads the list, without a range check.
    // The list admits only unit productions, so the cast never fails.
    public AIGroupUnitProduction this[int index] => (AIGroupUnitProduction)_list.GetChild(index);

    public IEnumerator<AIGroupUnitProduction> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // AIGroup.pas:2435. The original reads through the range-checked child.
    public AIGroupUnitProduction GetUnitProductions(int index) => (AIGroupUnitProduction)_list.GetChildInRange(index);
}
