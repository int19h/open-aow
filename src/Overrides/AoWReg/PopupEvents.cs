using Aow.Rtl;

namespace Aow.Units.AoWReg;

public partial class TAoWRegistry
{
    public TSet<TWindowPopupEvent> GetClassicWindowPopupEvents()
    {
        byte image = 0xff;
        rwBinaryData("General", "Classic Window Popup Events", Bytes.Of(ref image), 1, TReadWriteMode.rwRead);
        return TSet<TWindowPopupEvent>.FromMask(image);
    }
}
