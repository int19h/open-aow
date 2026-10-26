using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl;

public struct Ptr<T> : IEquatable<Ptr<T>>
{
    public T[]? Array;
    public int Index;
    public readonly ref T Value => ref Array![Index];
    public readonly ref T this[int index] => ref Array![Index + index];
    public readonly bool IsNull => Array is null;
    public static Ptr<T> To(T[] array, int index) => new() { Array = array, Index = index };
    // SYSTEM.PAS:1042 returns nil without calling the allocator for size zero.
    public static Ptr<T> Alloc(int count) => count == 0 ? default : To(new T[count], 0);
    public readonly Ptr<T> Realloc(int count)
    {
        if (count == 0) return default;
        var result = Alloc(count);
        if (!IsNull) System.Array.Copy(Array!, Index, result.Array!, 0, Math.Min(count, Array!.Length - Index));
        return result;
    }
    public static Ptr<T> operator +(Ptr<T> pointer, int offset) => new() { Array = pointer.Array, Index = pointer.Index + offset };
    public static Ptr<T> operator -(Ptr<T> pointer, int offset) => pointer + -offset;
    public static int operator -(Ptr<T> a, Ptr<T> b)
    {
        if (!ReferenceEquals(a.Array, b.Array)) throw new EInvalidOperation("Pointers have different backing arrays");
        return a.Index - b.Index;
    }
    public readonly bool Equals(Ptr<T> other) => ReferenceEquals(Array, other.Array) && Index == other.Index;
    public override readonly bool Equals(object? obj) => obj is Ptr<T> other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Array, Index);
    public static bool operator ==(Ptr<T> a, Ptr<T> b) => a.Equals(b);
    public static bool operator !=(Ptr<T> a, Ptr<T> b) => !a.Equals(b);

    public static void Copy(Ptr<T> source, Ptr<T> destination, int count)
    {
        if (count <= 0) return;
        System.Array.Copy(source.Array!, source.Index, destination.Array!, destination.Index, count);
    }
    public static void Fill(Ptr<T> destination, int count, T value)
    {
        if (count <= 0) return;
        System.Array.Fill(destination.Array!, value, destination.Index, count);
    }
    public static bool CompareMem(Ptr<T> a, Ptr<T> b, int count)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            throw new EInvalidOperation("Byte comparison requires unmanaged elements");
        if (count <= 0) return true;
        var left = a.Array.AsSpan(a.Index, count);
        var right = b.Array.AsSpan(b.Index, count);
        int size = checked(count * Unsafe.SizeOf<T>());
        return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(left)), size)
            .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(right)), size));
    }
    public readonly PtrBox<T> Box() => new(this);
    public static Ptr<T> Unbox(object? value) => value is PtrBox<T> box ? box.Pointer
        : throw new EInvalidCast("Object is not a box of the requested pointer type");
    public static Ptr<T> Unbox(IntOrRef value) => Unbox(value.Ref);
}

public sealed class PtrBox<T>(Ptr<T> pointer) : TObject
{
    public Ptr<T> Pointer = pointer;
}

public static class Bytes
{
    public static Span<byte> Of<T>(ref T value) where T : unmanaged => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1));
    public static Span<byte> Of<T>(ref T value, int count) where T : unmanaged
    {
        var bytes = Of(ref value);
        if (count < 0 || count > bytes.Length) throw new ERangeError("Buffer size exceeds the record");
        return bytes[..count];
    }
}
