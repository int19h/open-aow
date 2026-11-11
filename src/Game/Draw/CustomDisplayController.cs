// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// the interface of a display controller. 35 new virtual methods, all abstract but GetSurface
// and BltInvalidatedSurface
public partial class CustomDisplayController : Component
{
    public CustomDisplayController(Component owner) : base(owner)
    { }

    // DisplayC.pas:74
    public virtual Matrix GetInvMatrix()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:75
    public virtual void SetPrimarySurfaceRgn(in Rect R)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:76
    public virtual Rect? GetPrimarySurfaceRgn()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:77
    public virtual Set<DisplayControllerStatusElement> GetStatus()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:78
    public virtual CustomDisplay GetDisplay()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:79
    public virtual Palette GetPalette()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:554
    public virtual Surface GetSurface(int Index) => null;

    // DisplayC.pas:81
    public virtual int GetSurfaceCount()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:82
    public virtual bool WndProc(ref Message Message)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:83
    public virtual int SupportDisplayMode(CustomDisplay ADisplay)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:84
    public virtual void WindowRecreated()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:85
    public virtual void ShowError(string Msg)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:86
    public virtual int GetHPalette()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:87
    public virtual int Hide()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:88
    public virtual int Show()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:89
    public virtual void GetPixPosofBlock(int BX, int BY, Rect R, ref int PX, ref int PY, ref int BW, ref int BH)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:90
    public virtual void GetBlockPosofPix(int PX, int PY, Rect R, ref int BX, ref int BY)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:91
    public virtual int GetInvBlockSize(int X, int Y, Matrix Matrix, ref int BW, ref int BH)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:92
    public virtual void NewFrame()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:93
    public virtual void ShowFrameRate()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:94
    public virtual int Activate(CustomDisplay ADisplay)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:95
    public virtual int Deactivate()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:96
    public virtual int FlipSurface(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:97
    public virtual int BltSurface(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:562
    // { Blits the invalidated blocks of back surface Index to the primary surface: every block the
    //   matrix marks is merged with its marked neighbours (GetInvBlockSize) into one rectangle, and
    //   the marks count down, so a block is blitted once per back surface. }
    public virtual int BltInvalidatedSurface(int Index)
    {
        int Result;
        Matrix M;
        int BW = default;
        int BH = default;
        int PX2 = default;
        int PY2 = default;
        int BlkW = default;
        int BlkH = default;
        int V;
        Rect ClipR = default;
        Rect SrcR = default;
        Rect DstR = default;
        if (GetInvMatrix() != null)
        {
            Result = 0;
            var invMatrix = GetInvMatrix();
            M = new Matrix(GetInvMatrix().Width, invMatrix.Height);
            for (int I = 0, last = GetInvMatrix().Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = GetInvMatrix().Height - 1; J <= lastJ; J++)
                {
                    if (GetInvMatrix().GetVal((byte)I, (byte)J) != 0)
                    {
                        M.SetVal((byte)I, (byte)J, 1);
                    }
                }
            }

            ClipR.Left = 0;
            ClipR.Right = GetSurface(1).Width;
            ClipR.Top = 0;
            ClipR.Bottom = GetSurface(1).Height;
            for (int J = 0, lastJ = M.Height - 1; J <= lastJ; J++)
            {
                for (int I = 0, last = M.Width - 1; I <= last; I++)
                {
                    if (Result >= 0)
                    {
                        V = GetInvMatrix().GetVal((byte)I, (byte)J);
                        if (V != 0)
                        {
                            GetInvMatrix().SetVal((byte)I, (byte)J, (byte)(V - 1));
                        }

                        if (M.GetVal((byte)I, (byte)J) != 0)
                        {
                            GetInvBlockSize(I, J, M, ref BW, ref BH);
                            GetPixPosofBlock(I, J, ClipR, ref SrcR.Left, ref SrcR.Top, ref BlkW, ref BlkH);
                            if (BW > 1 || BH > 1)
                            {
                                GetPixPosofBlock(I + BW - 1, J + BH - 1, ClipR, ref PX2, ref PY2, ref BlkW, ref BlkH);
                                SrcR.Right = PX2 + BlkW;
                                SrcR.Bottom = PY2 + BlkH;
                            }
                            else
                            {
                                SrcR.Right = SrcR.Left + BlkW;
                                SrcR.Bottom = SrcR.Top + BlkH;
                            }

                            var primarySurfaceRgn = GetPrimarySurfaceRgn();
                            GetPixPosofBlock(I, J, primarySurfaceRgn.Value, ref DstR.Left, ref DstR.Top, ref BlkW, ref BlkH);
                            var primarySurfaceRgn2 = GetPrimarySurfaceRgn();
                            DstR.Left += primarySurfaceRgn2.Value.Left;
                            var primarySurfaceRgn3 = GetPrimarySurfaceRgn();
                            DstR.Top += primarySurfaceRgn3.Value.Top;
                            if (BW > 1 || BH > 1)
                            {
                                var primarySurfaceRgn4 = GetPrimarySurfaceRgn();
                                GetPixPosofBlock(I + BW - 1, J + BH - 1, primarySurfaceRgn4.Value, ref PX2, ref PY2, ref BlkW, ref BlkH);
                                var primarySurfaceRgn5 = GetPrimarySurfaceRgn();
                                DstR.Right = PX2 + BlkW + primarySurfaceRgn5.Value.Left;
                                var primarySurfaceRgn6 = GetPrimarySurfaceRgn();
                                DstR.Bottom = PY2 + BlkH + primarySurfaceRgn6.Value.Top;
                            }
                            else
                            {
                                DstR.Right = DstR.Left + BlkW;
                                DstR.Bottom = DstR.Top + BlkH;
                            }

                            var surface = GetSurface(0);
                            Result = GetSurface(Index).BltStretch(surface, SrcR, DstR);
                            for (int X = 0, lastX = BW - 1; X <= lastX; X++)
                            {
                                for (int Y = 0, lastY = BH - 1; Y <= lastY; Y++)
                                {
                                    M.SetVal((byte)(I + X), (byte)(J + Y), 0);
                                }
                            }
                        }
                    }
                }
            }

            M.Dispose();
        }
        else
        {
            Result = DisplayCodes.DCERR_FAILED;
        }

        return Result;
    }

    // DisplayC.pas:99
    public virtual int Handle()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:100
    public virtual WinControl OutputWnd()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:101
    public virtual int ResizeBackSurfaces(int AWidth, int AHeight)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:102
    public virtual void GetMousePos(ref int X, ref int Y)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:103
    public virtual int RealizePaletNow()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:104
    public virtual int SetPalet(Palette Palette)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:105
    public virtual int WaitForVerticalBlank()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:106
    public virtual void InitInvMatrix(byte AWidth, byte AHeight)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:107
    public virtual void Invalidate(int X, int Y, int W, int H)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:108
    public virtual void InvalidateAll()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }
}
