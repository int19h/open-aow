// Translated from SmokeCloud.pas.
using Aow.Engine.Buffers;

namespace Aow.Game.Magic.Spells;

public struct CloudSpellFields
{
    public FixedArray_byte_8 Reserved;
    private string name;

    public string Name { readonly get => name ?? ""; set => name = value; }
}
