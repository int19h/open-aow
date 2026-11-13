#nullable enable
using Aow.Formats.Tree;
using Aow.Game;

namespace Aow.Engine;

/// <summary>
/// A persistable factory bound to the engine of the load. A tree load takes the engine from it
/// where <c>ReadWrite</c> takes <c>Stream.Engine</c>, the engine the storage stream was opened with,
/// and the progress handler where <c>ReadWrite</c> calls <c>Stream.ShowProgress</c>.
/// </summary>
public interface IEnginePersistableFactory : IPersistableFactory
{
    GameEngine Engine { get; }

    /// <summary>The progress handler of the load, which <c>ReadWrite</c> reaches through <c>Stream.ShowProgress</c>.</summary>
    ProgressEvent? Progress => null;
}
