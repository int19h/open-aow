namespace Aow.Rtl;

/// <summary>
/// The I/O image of a game value: a <see cref="StoredLayoutAttribute"/> record that the record
/// catalog describes and the storage streams move as bytes. <see cref="From"/> and
/// <see cref="ToValue"/> map the two without loss.
/// </summary>
public interface IRecordImage<TImage, TValue> where TImage : unmanaged, IRecordImage<TImage, TValue>
{
    static abstract TImage From(TValue value);

    TValue ToValue();
}

public static partial class RtlUnit
{
    /// <summary>
    /// A catalog record stored by <c>rwData</c>: the value's image moves through the byte
    /// transfer, and a load maps the image back to the value.
    /// </summary>
    public static void TransferRecord<TImage, TValue>(ref TValue value, int size, int id, BytesTransfer transfer)
        where TImage : unmanaged, IRecordImage<TImage, TValue>
    {
        var image = TImage.From(value);
        transfer(Bytes.Of(ref image, size), size, id);
        value = image.ToValue();
    }
}
