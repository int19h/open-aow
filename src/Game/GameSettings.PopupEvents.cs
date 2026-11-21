
namespace Aow.Game;

public sealed partial class GameSettings
{
    public WindowPopupEvents GetClassicWindowPopupEvents()
    {
        byte image = 0xff;
        Bytes("General", "Classic Window Popup Events", Rtl.Bytes.Of(ref image), size: 1, mode: ReadWriteMode.Read);
        return (WindowPopupEvents)image;
    }

    public void SetClassicWindowPopupEvents(WindowPopupEvents value)
    {
        byte image = (byte)value;
        Bytes("General", "Classic Window Popup Events", Rtl.Bytes.Of(ref image), size: 1, mode: ReadWriteMode.Write);
    }

    public WindowPopupEvents GetSimultaneousWindowPopupEvents()
    {
        byte image = 0xef;
        Bytes("General", "Simultaneous Window Popup Events", Rtl.Bytes.Of(ref image), size: 1, mode: ReadWriteMode.Read);
        return (WindowPopupEvents)image;
    }

    public void SetSimultaneousWindowPopupEvents(WindowPopupEvents value)
    {
        byte image = (byte)value;
        Bytes("General", "Simultaneous Window Popup Events", Rtl.Bytes.Of(ref image), size: 1, mode: ReadWriteMode.Write);
    }
}
