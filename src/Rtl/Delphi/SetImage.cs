namespace Aow.Rtl;

public static class SetImage
{
    public static Set<E> TransferSet<E>(Set<E> value, int size, int id, Action<Span<byte>, int, int> transfer, int low = 0) where E : unmanaged
    {
        ulong image = value.Image;
        transfer(Bytes.Of(ref image, count: size), size, id);
        value = Set<E>.FromMask(image << (low / 8 * 8));
        return value;
    }
}
