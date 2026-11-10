// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// 16 bit DIB back surfaces. FPixelFormat, FBack1, FBack2
public partial class Windows16DC : WindowsDC
{

    internal Win16DCPixelFormat FPixelFormat;
    internal DIB16Surface FBack1;
    internal DIB16Surface FBack2;

    // DisplayC.pas:2343
    internal void SetPixelFormat(Win16DCPixelFormat Value)
    {
        if (!FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            FPixelFormat = Value;
        }
        else
        {
            ShowMessage("Cannot change PixelFormat when activated");
        }
    }

    // DisplayC.pas:2314
    public Windows16DC(Component owner) : base(owner)
    {
        FPixelFormat = Win16DCPixelFormat.Auto;
    }

    // DisplayC.pas:2320
    public override Surface GetSurface(int Index)
    {
        Surface Result;
        switch (Index)
        {
            case 0:
                Result = FGDISurface;
                break;
            case 1:
                Result = FBack1;
                break;
            case 2:
                Result = FBack2;
                break;
            default:
                Result = null;
                break;
        }

        return Result;
    }

    // DisplayC.pas:2331
    public override int Deactivate()
    {
        if (FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            FBack1?.Dispose();
            FBack1 = null;
            FBack2?.Dispose();
            FBack2 = null;
        }

        return base.Deactivate();
    }

    // DisplayC.pas:2351
    public override int Activate(CustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    [Published]
    public Win16DCPixelFormat PixelFormat { get => FPixelFormat; set => SetPixelFormat(value); }

    public enum Win16DCPixelFormat : byte
    {
        Auto = 0, Rgb555 = 1, Rgb565 = 2
    }

}
