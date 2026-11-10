// Translated from GFXE.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// a TOpenDialog whose filter lists the registered image file classes
public partial class OpenImageFileDialog : OpenDialog
{
    // GFXE.pas:972
    public override void Loaded()
    {
        /* TODO: Implement Dialogs.Filter in the platform or editor service. */
        throw new NotImplementedException();
    }

    // GFXE.pas:966

    public OpenImageFileDialog(Component owner) : base(owner)
    {
        /* TODO: Implement Dialogs.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}
