namespace Aow.Game.Draw;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class DisplayControllerEventArgs(DisplayController displayController) : EventArgs
{
    public DisplayController DisplayController { get; } = displayController;
}
