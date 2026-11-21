using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aow;

/// <summary>
/// Names the single-value enum whose members a <see cref="FlagsAttribute"/> enum holds as a set,
/// such as <c>DamageTypes</c> for <c>DamageType</c>. Member n of the element enum is bit n of the flags enum.
/// The stored set takes the element enum's stored name and member names.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class FlagsOfAttribute(Type element) : Attribute
{
    public Type Element { get; } = element;
}

/// <summary>Membership, conversion and the stored image of the <see cref="FlagsAttribute"/> enums that replace Delphi sets.</summary>
public static class FlagsExtensions
{
    extension<T>(T flags) where T : unmanaged, Enum
    {
        /// <summary>
        /// The member is in this value: the Delphi <c>member in flags</c>, for one member. <c>None</c>, the flag of a
        /// single value outside the enum, is in no value. Typed, unlike <see cref="Enum.HasFlag"/>.
        /// </summary>
        public bool Contains(T member) => (Bits(flags) & Bits(member)) != 0;
    }

    /// <summary>The bits of a flags value, zero-extended.</summary>
    public static ulong Bits<T>(T value) where T : unmanaged, Enum => Unsafe.SizeOf<T>() switch
    {
        1 => Unsafe.As<T, byte>(ref value),
        2 => Unsafe.As<T, ushort>(ref value),
        4 => Unsafe.As<T, uint>(ref value),
        _ => Unsafe.As<T, ulong>(ref value),
    };

    /// <summary>The flags value of the given bits. Bits beyond the underlying type are cut.</summary>
    public static T FromBits<T>(ulong bits) where T : unmanaged, Enum
    {
        T value = default;
        switch (Unsafe.SizeOf<T>())
        {
            case 1:
                Unsafe.As<T, byte>(ref value) = (byte)bits;
                break;
            case 2:
                Unsafe.As<T, ushort>(ref value) = (ushort)bits;
                break;
            case 4:
                Unsafe.As<T, uint>(ref value) = (uint)bits;
                break;
            default:
                Unsafe.As<T, ulong>(ref value) = bits;
                break;
        }

        return value;
    }

    /// <summary>The enum whose stored name and member names a stored set of this flags enum takes: the <see cref="FlagsOfAttribute"/> element, or the flags enum itself.</summary>
    public static Type StoredElement(Type flags)
    {
        ArgumentNullException.ThrowIfNull(flags);
        return flags.GetCustomAttribute<FlagsOfAttribute>()?.Element ?? flags;
    }

    /// <summary>A <see cref="FlagsAttribute"/> enum, the type of a set.</summary>
    public static bool IsFlags(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type.IsEnum && type.IsDefined(typeof(FlagsAttribute), inherit: false);
    }

    /// <summary>
    /// The bit numbers of the members of a flags enum: one per single-bit member, by name. The empty
    /// member and named combinations have none.
    /// </summary>
    public static IEnumerable<(string Name, int Bit)> MemberBits(Type flags)
    {
        ArgumentNullException.ThrowIfNull(flags);
        foreach (string name in Enum.GetNames(flags))
        {
            ulong value = Convert.ToUInt64(value: Enum.Parse(flags, value: name), provider: System.Globalization.CultureInfo.InvariantCulture);
            if (BitOperations.IsPow2(value))
            {
                yield return (name, BitOperations.TrailingZeroCount(value));
            }
        }
    }

    /// <summary>
    /// The bytes the Delphi set image of a flags enum takes: from bit 0 to the highest member, with
    /// three bytes stored as four. The element enum's ordinals decide for a <see cref="FlagsOfAttribute"/> enum.
    /// </summary>
    public static int StorageSize(Type flags)
    {
        Type element = StoredElement(flags);
        int highest = element == flags
            ? MemberBits(flags).Select(m => m.Bit).DefaultIfEmpty(0).Max()
            : Enum.GetValues(element).Cast<object>().Select(v => (int)Convert.ToInt64(value: v, provider: System.Globalization.CultureInfo.InvariantCulture)).DefaultIfEmpty(0).Max();
        int size = (highest / 8) + 1;
        return size == 3 ? 4 : size;
    }
}
