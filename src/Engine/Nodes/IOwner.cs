using System.Collections;

namespace Aow.Engine.Nodes;

/// <summary>
/// A node with children, in their stored order. A child slot can be empty. General code filters
/// the children with <c>OfType</c>. A child whose owner is this node is owned by it. Any other
/// child is shared.
/// </summary>
public interface IOwner
{
    IEnumerable Children { get; }
}
