namespace Aow.Engine.Nodes;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class CollectionChangedEventArgs(ObservableChange change) : EventArgs
{
    public ObservableChange Change { get; } = change;
}
