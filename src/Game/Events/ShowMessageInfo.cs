// Translated from AoWE.pas.

namespace Aow.Game.Events;

public struct ShowMessageInfo
{
    private string _text;

    public string Text { readonly get => _text ?? ""; set => _text = value; }

    public int Color;
}
