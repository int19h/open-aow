using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Formats.Tree;

/// <summary>
/// Moves one catalog record through <see cref="IPersister"/>. Field names and types come from the
/// record catalog. Integer zeros are omitted. Enumerations and sets are stored, including zero.
/// Reserved bytes and padding are <c>$reserved</c> when any byte is nonzero.
/// A one-field set wrapper is that set, under the schema name.
/// </summary>
public static class RecordValues
{
    public static void Transfer<T>(IPersister persister, string name, ref T value) where T : unmanaged
    {
        var info = RecordCatalog.Require(typeof(T).Name, null, name);
        var image = new byte[Unsafe.SizeOf<T>()];
        MemoryMarshal.Write(image, in value);
        if (image.Length < info.Size)
            throw new InvalidOperationException("Record " + info.Name + " is larger than its C# value.");
        if (info.BareSet)
            ((SetRecordField)info.Fields[0]).TransferNamed(persister, name, image.AsSpan(info.Fields[0].Offset, info.Fields[0].Size));
        else if (persister.Loading)
        {
            Array.Clear(image);
            persister.Record(name, body => Apply(body, info, image));
        }
        else persister.Record(name, body => Apply(body, info, image));
        value = MemoryMarshal.Read<T>(image);
    }

    internal static void Apply(IPersister persister, RecordTypeInfo info, Span<byte> image)
    {
        int residueAt = info.Residue.Count == 0 ? -1 : info.Residue[0].Offset;
        bool residue = false;
        foreach (var field in info.Fields)
        {
            if (!residue && residueAt >= 0 && residueAt < field.Offset)
            {
                ApplyResidue(persister, info, image);
                residue = true;
            }
            field.Transfer(persister, image.Slice(field.Offset, field.Size));
        }
        if (!residue) ApplyResidue(persister, info, image);
    }

    private static void ApplyResidue(IPersister persister, RecordTypeInfo info, Span<byte> image)
    {
        if (info.ResidueLength == 0) return;
        var residue = new byte[info.ResidueLength];
        int at = 0;
        foreach (var (offset, length) in info.Residue)
        {
            image.Slice(offset, length).CopyTo(residue.AsSpan(at));
            at += length;
        }
        bool any = false;
        foreach (var b in residue)
            if (b != 0) any = true;
        if (persister.Loading || any) persister.Bytes("$reserved", residue);
        if (!persister.Loading) return;
        at = 0;
        foreach (var (offset, length) in info.Residue)
        {
            residue.AsSpan(at, length).CopyTo(image.Slice(offset));
            at += length;
        }
    }
}
