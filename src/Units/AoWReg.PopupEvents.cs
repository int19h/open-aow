using Aow.Rtl;

namespace Aow.Game.Engine;

public partial class TAoWRegistry
{
    public TSet<TWindowPopupEvent> GetClassicWindowPopupEvents()
    {
        byte image = 0xff;
        rwBinaryData("General", "Classic Window Popup Events", Bytes.Of(ref image), 1, TReadWriteMode.rwRead);
        return TSet<TWindowPopupEvent>.FromMask(image);
    }

    public void SetClassicWindowPopupEvents(TSet<TWindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        rwBinaryData("General", "Classic Window Popup Events", Bytes.Of(ref image), 1, TReadWriteMode.rwWrite);
    }

    public TSet<TWindowPopupEvent> GetSimultaneousWindowPopupEvents()
    {
        byte image = 0xef;
        rwBinaryData("General", "Simultaneous Window Popup Events", Bytes.Of(ref image), 1, TReadWriteMode.rwRead);
        return TSet<TWindowPopupEvent>.FromMask(image);
    }

    public void SetSimultaneousWindowPopupEvents(TSet<TWindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        rwBinaryData("General", "Simultaneous Window Popup Events", Bytes.Of(ref image), 1, TReadWriteMode.rwWrite);
    }
}
