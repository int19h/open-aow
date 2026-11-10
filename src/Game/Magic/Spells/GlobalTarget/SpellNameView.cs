// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells.GlobalTarget;

public struct SpellNameView
{
    public FixedArray_byte_8 Reserved;
    private string name;

    public string Name { readonly get => name ?? ""; set => name = value; }
}
