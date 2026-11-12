namespace Aow.Engine.Nodes;

/// <summary>
/// An object in the ownership tree. The owner disposes it, routes messages to it and saves it as
/// part of itself. General walks (the engine, the object path, the owned-child tests of message
/// delivery, clearing and saving) read the owner through this interface.
/// </summary>
public interface IOwned
{
    IOwner Owner { get; }
}
