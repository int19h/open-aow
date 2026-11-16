using System.Collections;
using Aow.Game.Commands;

namespace Aow.Game.Net;

/// <summary>
/// The token events of a token event reference list, in the list's child slots and their stored
/// order. The list holds references and does not dispose the events. This collection reads them
/// typed. A default value has no list and throws on use, as a missing list does.
/// </summary>
public readonly struct TokenEventCollection : IReadOnlyList<TokenEvent>
{
    private readonly TokenEventReferenceList _list;

    internal TokenEventCollection(TokenEventReferenceList list) => _list = list;

    /// <summary>The list that holds the events.</summary>
    public TokenEventReferenceList Holder => _list;

    public int Count => _list.GetCount();

    // The event in slot index, read as the original reads the list, without a range check. The
    // list admits only token events.
    public TokenEvent this[int index] => _list.GetChild<TokenEvent>(index);

    public IEnumerator<TokenEvent> GetEnumerator()
    {
        for (int index = 0; index < Count; index++)
        {
            yield return this[index];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
