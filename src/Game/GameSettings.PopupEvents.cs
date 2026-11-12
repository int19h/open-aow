using Aow.Rtl;

namespace Aow.Game;

public partial class GameSettings
{
    public Set<WindowPopupEvent> GetClassicWindowPopupEvents()
    {
        byte image = 0xff;
        Bytes("General", "Classic Window Popup Events", Rtl.Bytes.Of(ref image), 1, ReadWriteMode.Read);
        return Set<WindowPopupEvent>.FromMask(image);
    }

    public void SetClassicWindowPopupEvents(Set<WindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        Bytes("General", "Classic Window Popup Events", Rtl.Bytes.Of(ref image), 1, ReadWriteMode.Write);
    }

    public Set<WindowPopupEvent> GetSimultaneousWindowPopupEvents()
    {
        byte image = 0xef;
        Bytes("General", "Simultaneous Window Popup Events", Rtl.Bytes.Of(ref image), 1, ReadWriteMode.Read);
        return Set<WindowPopupEvent>.FromMask(image);
    }

    public void SetSimultaneousWindowPopupEvents(Set<WindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        Bytes("General", "Simultaneous Window Popup Events", Rtl.Bytes.Of(ref image), 1, ReadWriteMode.Write);
    }
}
