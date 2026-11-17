// Translated from AoWE.pas.

namespace Aow.Game.Events;

public struct ShowMessageInfo
{
    public string Text { readonly get => field ?? ""; set; }

    public int Color { get; set; }
}
