// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

// The strength a location needs against each of the ten damage types. A city sets only the
// physical strength.
public struct DefenseRequirements
{
    public Array10<int> DamageStrengths;

    public int Strength
    {
        readonly get => DamageStrengths[(int)DamageType.Physical];
        set => DamageStrengths[(int)DamageType.Physical] = value;
    }
}
