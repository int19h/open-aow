namespace Aow.Engine.Nodes;

/// <summary>
/// A node whose children have a known type, read and added through the same interface.
/// </summary>
public interface IChildList<TChild> : IOwner<TChild>, IChildSink<TChild>;
