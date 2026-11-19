using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// Moves one catalog record through <see cref="IPersister"/>. The value is an I/O image, a
/// <see cref="StoredLayoutAttribute"/> struct that the record catalog describes under its bound
/// record name. Game code maps its own value to the image (<c>PersistSupport.Record</c>). Field
/// names and types come from the record catalog. Integer zeros are omitted. Enumerations and sets
/// are stored, including zero. Declared padding bytes are ignored on read, written as 0 and never
/// enter the tree. A one-field set wrapper is that set, under the schema name.
/// </summary>
public static class RecordValues
{
    public static T Transfer<T>(IPersister persister, string name, T value) where T : unmanaged
    {
        if (!typeof(T).IsDefined(typeof(StoredLayoutAttribute), inherit: false))
        {
            throw new InvalidOperationException(typeof(T).Name + " is not a stored image. A record moves through its [StoredLayout] image.");
        }

        RecordTypeInfo info = RecordCatalog.Require(RecordTypeInfo.StoredName(typeof(T)), null, name);
        byte[] image = new byte[Unsafe.SizeOf<T>()];
        MemoryMarshal.Write(image, in value);
        if (image.Length < info.Size)
        {
            throw new InvalidOperationException("Record " + info.Name + " is larger than its C# value.");
        }

        if (info.BareSet)
        {
            ((SetRecordField)info.Fields[0]).TransferNamed(persister, name, image.AsSpan(info.Fields[0].Offset, info.Fields[0].Size));
        }
        else if (persister.Loading)
        {
            Array.Clear(image);
            persister.Record(name, body => Apply(body, info, image));
        }
        else
        {
            persister.Record(name, body => Apply(body, info, image));
        }

        return MemoryMarshal.Read<T>(image);
    }

    internal static void Apply(IPersister persister, RecordTypeInfo info, Span<byte> image)
    {
        foreach (RecordFieldInfo field in info.Fields)
        {
            field.Transfer(persister, image.Slice(field.Offset, field.Size));
        }
    }
}
