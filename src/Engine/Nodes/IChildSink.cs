namespace Aow.Engine.Nodes;

/// <summary>
/// The write side of <see cref="IOwner{TChild}"/>. Adding a child appends it to the children and
/// returns its index.
/// </summary>
public interface IChildSink<in TChild>
{
    int AddChild(TChild child);
}
