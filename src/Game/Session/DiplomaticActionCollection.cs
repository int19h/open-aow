using System.Collections;

using Aow.Engine.Nodes;

namespace Aow.Game.Session;

/// <summary>
/// The diplomatic actions of a diplomatic action list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the actions. This collection reads them
/// typed. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct DiplomaticActionCollection : IReadOnlyList<DiplomaticAction>
{
    private readonly DiplomaticActionList _list;

    internal DiplomaticActionCollection(DiplomaticActionList list) => _list = list;

    /// <summary>The list that holds the actions.</summary>
    public DiplomaticActionList Holder => _list;

    public int Count => _list.GetCount();

    // The action in slot index, read as the original reads the list, without a range check. The
    // list admits only diplomatic actions, so the cast never fails.
    public DiplomaticAction this[int index] => (DiplomaticAction)_list.GetChild(index);

    public IEnumerator<DiplomaticAction> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Diplomacy.pas:1097
    public DiplomaticAction GetDiplomaticActions(int index) => this[index];

    // Diplomacy.pas:1100
    public DiplomaticAction FindLastAction(byte kind, DiplomaticActionReaction reaction, sbyte source, sbyte target, sbyte other)
    {
        int index;
        DiplomaticAction candidate;
        DiplomaticAction result = null;
        index = Count;
        while (result == null && index != 0)
        {
            index -= 1;
            candidate = this[index];
            if (candidate.Proposal.ToFields().Kind == kind && candidate.Reaction == reaction && (source == candidate.Source || source == -1) && (candidate.Target == target || target == -1) && (candidate.OtherPlayer == other || other == -1))
            {
                result = candidate;
            }
        }

        return result;
    }
}
