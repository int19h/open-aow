// Translated from Ability.PAS (TAoWHSSet.Create registers the overlay names) and AoWTC.pas (the combat ooze).
using Aow.Engine;

namespace Aow.Game.World.Map;

/// <summary>
/// The overlay of a map field, numbered in the order the game pack registers the overlay names. A move
/// point table holds the column of no overlay first and then one column per overlay. A field without an
/// overlay holds null, the original -1.
/// </summary>
[OriginalNone(-1)]
public enum OverlayType : byte
{
    Mountain = 0,
    DenseVegetation = 1,
    Hill = 2,
    Vegetation = 3,
    Road = 4,
    Bridge = 5,
    Rubble = 6,
    Structure = 7,
    Obstacle = 8,
    LightVegetation = 9,
    Solid = 10,

    /// <summary>The ooze a combat spell leaves on a battle field (AoWTC.pas:4097). The game pack registers no name for it.</summary>
    Ooze = 11,
}

/// <summary>The conversions between an overlay and the original overlay number, where -1 is no overlay.</summary>
public static class OverlayTypeConversions
{
    extension(OverlayType)
    {
        /// <summary>The overlay of an original overlay number: -1 is no overlay, 0 to 11 a member. Any other number throws.</summary>
        public static OverlayType? FromOriginal(int value) => value switch
        {
            -1 => null,
            >= 0 and <= 11 => (OverlayType)value,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(value), actualValue: value, message: "An overlay number is -1 or 0 to 11."),
        };
    }

    extension(OverlayType? value)
    {
        /// <summary>The original overlay number: -1 for no overlay.</summary>
        public sbyte ToOriginal() => value is { } overlay ? (sbyte)overlay : (sbyte)-1;
    }
}
