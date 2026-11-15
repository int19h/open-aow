// The change an ObservableNode reports. The original Changed call carries no arguments, so a
// call site fills Index, Item and Previous only when it has them, and NewIndex only for a move.
namespace Aow.Engine.Nodes;

public enum ObservableChangeKind
{
    Added,
    Removed,
    Replaced,
    Moved,
    Reset,
}

public readonly record struct ObservableChange(
    ObservableChangeKind Kind,
    int Index,
    EngineObject Item,
    EngineObject Previous,
    int NewIndex)
{
    public static ObservableChange Added(int index, EngineObject item) =>
        new(ObservableChangeKind.Added, Index: index, Item: item, Previous: null, NewIndex: -1);

    public static ObservableChange Removed(int index, EngineObject item) =>
        new(ObservableChangeKind.Removed, Index: index, Item: item, Previous: null, NewIndex: -1);

    public static ObservableChange Replaced(int index, EngineObject item, EngineObject previous) =>
        new(ObservableChangeKind.Replaced, Index: index, Item: item, Previous: previous, NewIndex: -1);

    public static ObservableChange Moved(int index, int newIndex, EngineObject item) =>
        new(ObservableChangeKind.Moved, Index: index, Item: item, Previous: null, NewIndex: newIndex);

    public static ObservableChange Reset() =>
        new(ObservableChangeKind.Reset, Index: -1, Item: null, Previous: null, NewIndex: -1);
}

public delegate void ObservableChangeHandler(ObservableNode sender, ObservableChange change);
