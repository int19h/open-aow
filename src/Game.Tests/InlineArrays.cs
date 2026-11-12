using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Game.Tests;

// The snapshots record an inline array as the list of its elements, as they record an array.
internal static class InlineArrays
{
    internal static object?[]? Elements(object value)
    {
        Type type = value.GetType();
        if (type.GetCustomAttribute<InlineArrayAttribute>() is not { } inline) return null;
        Type element = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().FieldType;
        return (object?[])typeof(InlineArrays).GetMethod(nameof(Read), BindingFlags.Static | BindingFlags.NonPublic)!
            .MakeGenericMethod(type, element).Invoke(null, [value, inline.Length])!;
    }

    private static object?[] Read<TArray, TElement>(object boxed, int length) where TArray : struct
    {
        TArray value = (TArray)boxed;
        var elements = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TArray, TElement>(ref value), length);
        var result = new object?[length];
        for (int i = 0; i < length; i++) result[i] = elements[i];
        return result;
    }
}
