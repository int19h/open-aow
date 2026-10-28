using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl;

public delegate ref T PointerElement<T>(int index);

public struct Ptr<T> : IEquatable<Ptr<T>>, IPointerBox
{
    public T[]? Array;
    public int Index;
    private PointerElement<T>? view;
    private object? storage;
    private int byteOffset;
    private int storageBytes;
    private readonly object? Storage => storage ?? Array;
    private readonly int StorageBytes => Array is null ? storageBytes : checked(Array.Length * Unsafe.SizeOf<T>());
    public readonly object? StorageIdentity => Storage;
    public readonly int ByteIndex => byteOffset + Index * Unsafe.SizeOf<T>();
    public readonly ref T Value => ref this[0];
    private readonly ref T RawReference(int index) => ref view is null
        ? ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(Array!), Index + index) : ref view(Index + index);
    public readonly ref T this[int index]
    {
        get
        {
            if (Array is not null) return ref Array[Index + index];
            if (IsNull) throw new NullReferenceException();
            long offset = (long)ByteIndex + (long)index * Unsafe.SizeOf<T>();
            if (offset < 0 || offset + Unsafe.SizeOf<T>() > StorageBytes) throw new IndexOutOfRangeException();
            return ref RawReference(index);
        }
    }
    public readonly bool IsNull => Storage is null;
    public readonly Span<T> AsSpan(int count)
    {
        if (count == 0) return Span<T>.Empty;
        if (Array is not null) return Array.AsSpan(Index, count);
        if (count < 0 || ByteIndex < 0 || (long)ByteIndex + (long)count * Unsafe.SizeOf<T>() > StorageBytes)
            throw new ArgumentOutOfRangeException(nameof(count));
        return MemoryMarshal.CreateSpan(ref Value, count);
    }
    public readonly Span<T> AsSpan() => AsSpan((StorageBytes - ByteIndex) / Unsafe.SizeOf<T>());
    public readonly Ptr<U> Reinterpret<U>(int offset = 0)
    {
        if (IsNull) return default;
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() || RuntimeHelpers.IsReferenceOrContainsReferences<U>())
            throw new EInvalidCast("Pointer view requires unmanaged elements");
        var pointer = this;
        return new Ptr<U>
        {
            storage = Storage,
            storageBytes = StorageBytes,
            byteOffset = ByteIndex + offset,
            view = (int i) => ref Unsafe.As<byte, U>(ref Unsafe.Add(ref Unsafe.As<T, byte>(ref pointer.RawReference(0)), offset + i * Unsafe.SizeOf<U>()))
        };
    }
    public static implicit operator Span<T>(Ptr<T> pointer) => pointer.AsSpan();
    public static implicit operator ReadOnlySpan<T>(Ptr<T> pointer) => pointer.AsSpan();
    public static Ptr<T> To(T[] array, int index) => new() { Array = array, Index = index };
    // SYSTEM.PAS:1042 returns nil without calling the allocator for size zero.
    public static Ptr<T> Alloc(int count) => count == 0 ? default : To(new T[count], 0);
    public readonly Ptr<T> Realloc(int count)
    {
        if (count == 0) return default;
        var result = Alloc(count);
        if (!IsNull)
        {
            int copied = Math.Min(count, (StorageBytes - ByteIndex) / Unsafe.SizeOf<T>());
            AsSpan(copied).CopyTo(result.AsSpan(copied));
        }
        return result;
    }
    public static Ptr<T> operator +(Ptr<T> pointer, int offset) { pointer.Index += offset; return pointer; }
    public static Ptr<T> operator -(Ptr<T> pointer, int offset) => pointer + -offset;
    public static int operator -(Ptr<T> a, Ptr<T> b)
    {
        if (!ReferenceEquals(a.Storage, b.Storage)) throw new EInvalidOperation("Pointers have different backing arrays");
        return (a.ByteIndex - b.ByteIndex) / Unsafe.SizeOf<T>();
    }
    public readonly bool Equals(Ptr<T> other) => ReferenceEquals(Storage, other.Storage) && ByteIndex == other.ByteIndex;
    public override readonly bool Equals(object? obj) => obj is Ptr<T> other && Equals(other);
    public override readonly int GetHashCode() => HashCode.Combine(Storage, ByteIndex);
    public static bool operator ==(Ptr<T> a, Ptr<T> b) => a.Equals(b);
    public static bool operator !=(Ptr<T> a, Ptr<T> b) => !a.Equals(b);

    public static void Copy(Ptr<T> source, Ptr<T> destination, int count)
    {
        if (count <= 0) return;
        source.AsSpan(count).CopyTo(destination.AsSpan(count));
    }
    public static void Fill(Ptr<T> destination, int count, T value)
    {
        if (count <= 0) return;
        destination.AsSpan(count).Fill(value);
    }
    public static bool CompareMem(Ptr<T> a, Ptr<T> b, int count)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            throw new EInvalidOperation("Byte comparison requires unmanaged elements");
        if (count <= 0) return true;
        var left = a.AsSpan(count);
        var right = b.AsSpan(count);
        int size = checked(count * Unsafe.SizeOf<T>());
        return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(left)), size)
            .SequenceEqual(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(right)), size));
    }
    public readonly PtrBox<T> Box() => new(this);
    public readonly Ptr<U> As<U>() => Reinterpret<U>();
    public static Ptr<T> Unbox(object? value) => value is PtrBox<T> box ? box.Pointer
        : throw new EInvalidCast("Object is not a box of the requested pointer type");
    public static Ptr<T> Unbox(IntOrRef value) => Unbox(value.Ref);
    public static Ptr<T> Unbox(AnsiString value) => typeof(T) == typeof(byte) ? value.AsPtr().Reinterpret<T>()
        : throw new EInvalidCast("A string pointer requires byte elements");
    public static Ptr<T> FromBox(object? value) => value is PtrBox<T> box ? box.Pointer
        : value is IPointerBox pointer ? pointer.As<T>()
        : throw new EInvalidCast("Object has no pointer storage");
}

public interface IPointerBox
{
    Ptr<T> As<T>();
    object? StorageIdentity { get; }
    int ByteIndex { get; }
}

public sealed class PtrBox<T>(Ptr<T> pointer) : TObject, IPointerBox
{
    public Ptr<T> Pointer = pointer;
    public Ptr<U> As<U>() => Pointer.Reinterpret<U>();
    public object? StorageIdentity => Pointer.StorageIdentity;
    public int ByteIndex => Pointer.ByteIndex;
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
