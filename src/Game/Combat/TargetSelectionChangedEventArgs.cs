namespace Aow.Game.Combat;

/// <summary>The change a raise of TargetSelectionControl.Changed reports (issue 326).</summary>
public sealed class TargetSelectionChangedEventArgs : EventArgs
{
    public required TargetSelectionChange Change { get; init; }
}
