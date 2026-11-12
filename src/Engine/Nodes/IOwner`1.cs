namespace Aow.Engine.Nodes;

/// <summary>
/// A node whose children have a known type, read by index in their stored order. Every write of a
/// child checks the type once, so this read needs no cast.
/// </summary>
public interface IOwner<out TChild> : IOwner
{
    int Count { get; }

    TChild this[int index] { get; }
}
