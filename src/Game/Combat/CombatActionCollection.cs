using System.Collections;
using Aow.Game.Combat.Actions;

namespace Aow.Game.Combat;

/// <summary>
/// The combat actions of a combat action list, in the list's child slots and their stored order.
/// The list holds references and does not dispose the actions. This collection reads them typed.
/// A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct CombatActionCollection : IReadOnlyList<CombatAction>
{
    private readonly CombatActionList _list;

    internal CombatActionCollection(CombatActionList list) => _list = list;

    /// <summary>The list that holds the actions.</summary>
    public CombatActionList Holder => _list;

    public int Count => _list.GetCount();

    // The action in slot index, read as the original reads the list, without a range check. The
    // list admits only combat actions, so the cast never fails.
    public CombatAction this[int index] => (CombatAction)_list.GetChild(index);

    public IEnumerator<CombatAction> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Combat2.pas:729
    public CombatAction GetCombatActions(int index) => this[index];
}
