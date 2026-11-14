namespace Aow.Rtl;

/// <summary>
/// The I/O image of a game value: a <see cref="StoredLayoutAttribute"/> record that the record
/// catalog describes. <see cref="From"/> and <see cref="ToValue"/> map the two without loss.
/// </summary>
public interface IRecordImage<TImage, TValue> where TImage : unmanaged, IRecordImage<TImage, TValue>
{
    static abstract TImage From(TValue value);

    TValue ToValue();
}
