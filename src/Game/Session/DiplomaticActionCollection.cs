#nullable disable warnings
using System.Collections;
using Aow.Rtl;

namespace Aow.Game.Session;

/// <summary>
/// The diplomatic actions of a diplomatic action list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the actions. This collection reads them
/// typed. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct DiplomaticActionCollection : IReadOnlyList<DiplomaticAction>
{
    internal DiplomaticActionCollection(DiplomaticActionList list) => Holder = list;

    /// <summary>The list that holds the actions.</summary>
    public DiplomaticActionList Holder { get; }

    public int Count => Holder.GetCount();

    // The action in slot index, read as the original reads the list, without a range check. The
    // list admits only diplomatic actions, so the cast never fails.
    public DiplomaticAction this[int index] => Holder.GetChild<DiplomaticAction>(index);

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
    public DiplomaticAction? FindLastAction(int kind, DiplomaticActionReaction reaction, int source, int target, PlayerId? other) => this.FindLast(item => item.Proposal.ToFields().Kind == kind && item.Reaction == reaction && (source == PlayerIdExtensions.ToOriginal(item.Source) || source == -1) && (PlayerIdExtensions.ToOriginal(item.Target) == target || target == -1) && (item.OtherPlayer == other || other is null));
}
