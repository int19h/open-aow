using Aow.Rtl;

namespace Aow.Game.Engine;

public partial class AoWRegistry
{
    public Set<WindowPopupEvent> GetClassicWindowPopupEvents()
    {
        byte image = 0xff;
        rwBinaryData("General", "Classic Window Popup Events", Bytes.Of(ref image), 1, ReadWriteMode.Read);
        return Set<WindowPopupEvent>.FromMask(image);
    }

    public void SetClassicWindowPopupEvents(Set<WindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        rwBinaryData("General", "Classic Window Popup Events", Bytes.Of(ref image), 1, ReadWriteMode.Write);
    }

    public Set<WindowPopupEvent> GetSimultaneousWindowPopupEvents()
    {
        byte image = 0xef;
        rwBinaryData("General", "Simultaneous Window Popup Events", Bytes.Of(ref image), 1, ReadWriteMode.Read);
        return Set<WindowPopupEvent>.FromMask(image);
    }

    public void SetSimultaneousWindowPopupEvents(Set<WindowPopupEvent> value)
    {
        byte image = (byte)value.Mask;
        rwBinaryData("General", "Simultaneous Window Popup Events", Bytes.Of(ref image), 1, ReadWriteMode.Write);
    }
}
