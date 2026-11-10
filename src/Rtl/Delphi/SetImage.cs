namespace Aow.Rtl;

public delegate void RwData(Span<byte> buffer, int size, int id);

public static partial class RtlUnit
{
    public static void RwSet<E>(ref Set<E> value, int size, int id, RwData transfer, int low = 0) where E : unmanaged
    {
        ulong image = value.Image;
        transfer(Bytes.Of(ref image, size), size, id);
        value = Set<E>.FromMask(image << (low / 8 * 8));
    }
}
