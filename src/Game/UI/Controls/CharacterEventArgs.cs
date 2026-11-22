namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class CharacterEventArgs : EventArgs
{
    public required byte Character { get; set; }
}
