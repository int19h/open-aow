namespace Aow.Engine.Nodes;

/// <summary>
/// A node whose children have a known type, read by index in their stored order. Every write of a
/// child checks the type once, so this read needs no cast.
/// </summary>
public interface IOwner<out TChild> : IOwner, IReadOnlyList<TChild>
{
    new int Count { get; }

    new TChild this[int index] { get; }

    /// <summary>
    /// Lets a foreach walk the children of a node in index order: the children from the first to the
    /// last, read through the indexer. Each step reads the count again, as the index loop over a live
    /// count does.
    /// </summary>
    IEnumerator<TChild> IEnumerable<TChild>.GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}
