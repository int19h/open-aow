namespace Aow.Rtl;

public delegate void BytesTransfer(Span<byte> buffer, int size, int id);

public static class SetImage
{
    public static void TransferSet<E>(ref Set<E> value, int size, int id, BytesTransfer transfer, int low = 0) where E : unmanaged
    {
        ulong image = value.Image;
        transfer(Bytes.Of(ref image, count: size), size, id);
        value = Set<E>.FromMask(image << (low / 8 * 8));
    }
}
