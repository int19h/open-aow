// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// the windowed controllers (GDI). FDC, FWnd, FGDISurface (the window, surface 0)
public partial class WindowsDC : DisplayCtrl
{

    internal int FDC;
    internal int FWnd;
    internal DisplayGDISurface FGDISurface;

    // DisplayC.pas:1894
    public WindowsDC(Component owner) : base(owner)
    {
        InitInvMatrix(4, 4);
        FStatus = FStatus + Set<DisplayCtrlStatusElement>.Of(DisplayCtrlStatusElement.Windowed);
    }

    // DisplayC.pas:1901
    public override int Activate(CustomDisplay ADisplay)
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1920
    public override int Deactivate()
    {
        /* TODO: Implement Windows.ReleaseDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1932
    public override void WindowRecreated()
    {
        /* TODO: Implement Windows.GetDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:1939
    public override int SupportDisplayMode(CustomDisplay ADisplay) => 0;

    // DisplayC.pas:1944
    public override int BltSurface(int Index)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        return Result;
    }

    // DisplayC.pas:1949
    public override int FlipSurface(int Index)
    {
        int Result;
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        if (Result >= 0)
        {
            Result = DisplayCodes.DCERR_NOFLIP;
        }

        return Result;
    }

    // DisplayC.pas:1957
    // { In a window every block is a block of its own. }
    public override int GetInvBlockSize(int X, int Y, Matrix Matrix, ref int BW, ref int BH)
    {
        if (GetInvMatrix().GetVal((byte)X, (byte)Y) != 0)
        {
            BW = 1;
            BH = 1;
        }
        else
        {
            BW = 0;
            BH = 0;
        }

        return BW * BH;
    }

    // DisplayC.pas:1972
    public override void ShowFrameRate()
    {
        /* TODO: Implement Windows.TextOut in the platform or editor service. */
        throw new NotImplementedException();
    }

}
