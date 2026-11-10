// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// 24 bit DIB back surfaces. FBack1, FBack2
public partial class Windows24DC : WindowsDC
{

    public Windows24DC(Component owner) : base(owner)
    { }

    internal DIB24Surface FBack1;
    internal DIB24Surface FBack2;

    // DisplayC.pas:2390
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

    // DisplayC.pas:2401
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

    // DisplayC.pas:2413
    public override int Activate(CustomDisplay ADisplay)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }
}
