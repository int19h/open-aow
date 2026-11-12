using Aow.Formats.Tree;
using Aow.Game;

namespace Aow.Engine;

/// <summary>
/// A persistable factory bound to the engine of the load. A tree load takes the engine from it
/// where <c>ReadWrite</c> takes <c>Stream.Engine</c>, the engine the storage stream was opened with.
/// </summary>
public interface IEnginePersistableFactory : IPersistableFactory
{
    GameEngine Engine { get; }
}
