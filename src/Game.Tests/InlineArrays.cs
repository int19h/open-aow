using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Game.Tests;

// The snapshots record an inline array as the list of its elements, as they record an array.
internal static class InlineArrays
{
    internal static Type ElementType(object value)
        => value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().FieldType;

    internal static object?[]? Elements(object value)
    {
        Type type = value.GetType();
        return type.GetCustomAttribute<InlineArrayAttribute>() is not { } inline
            ? null
            : (object?[])typeof(InlineArrays).GetMethod(nameof(Read), bindingAttr: BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(type, ElementType(value)).Invoke(null, parameters: [value, inline.Length])!;
    }

    private static object?[] Read<TArray, TElement>(object boxed, int length) where TArray : struct
    {
        TArray value = (TArray)boxed;
        ReadOnlySpan<TElement> elements = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TArray, TElement>(ref value), length);
        object?[] result = new object?[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = elements[i];
        }

        return result;
    }
}
