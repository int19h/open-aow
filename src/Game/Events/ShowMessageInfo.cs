// Translated from AoWE.pas.

namespace Aow.Game.Events;

public struct ShowMessageInfo
{
    private string text;

    public string Text { readonly get => text ?? ""; set => text = value; }

    public int Color;
}
