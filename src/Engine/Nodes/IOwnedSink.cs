namespace Aow.Engine.Nodes;

/// <summary>
/// The write side of <see cref="IOwned{TOwner}"/>. Setting the owner links the object into the
/// owner's children, as the node's own owner setter does.
/// </summary>
public interface IOwnedSink<in TOwner>
{
    void SetOwner(TOwner owner);
}
