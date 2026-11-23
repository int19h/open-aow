using Aow.Graphics;
// Translated from AoWE.pas.

namespace Aow.Game.Events;

public struct ShowMessageInfo
{
    public string Text { readonly get => field ?? ""; set; }

    public Color Color { get; set; }
}
