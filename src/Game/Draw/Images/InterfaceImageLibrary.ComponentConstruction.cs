// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Draw.Images;

public sealed partial class InterfaceImageLibrary
{
    public byte[] LayoutData
    {
        set => LayoutSession.ReadImageData(this, data: value);
    }
}
