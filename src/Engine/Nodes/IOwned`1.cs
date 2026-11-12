namespace Aow.Engine.Nodes;

/// <summary>
/// An object whose owner has a known type. The class declares the owner type, and every write of
/// the owner checks it once, so this read needs no cast.
/// </summary>
public interface IOwned<out TOwner> : IOwned
{
    new TOwner Owner { get; }
}
