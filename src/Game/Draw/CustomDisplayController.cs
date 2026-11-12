// Translated from DisplayC.pas.
using Aow.Rtl;

namespace Aow.Game.Draw;

// the interface of a display controller. 35 new virtual methods, all abstract but GetSurface
// and BlitInvalidatedSurface
public partial class CustomDisplayController : Component
{
    public CustomDisplayController(Component owner) : base(owner)
    { }

    // DisplayC.pas:74
    public virtual Matrix GetInverseMatrix()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:75
    public virtual void SetPrimarySurfaceRegion(in Rect R)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:76
    public virtual Rect? GetPrimarySurfaceRegion()
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
    public virtual Surface GetSurface(int index) => null;

    // DisplayC.pas:81
    public virtual int GetSurfaceCount()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:82
    public virtual bool WindowProc(ref Message message)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:83
    public virtual int SupportDisplayMode(CustomDisplay display)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:84
    public virtual void WindowRecreated()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:85
    public virtual void ShowError(string message)
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
    public virtual void GetPixelPositionOfBlock(int bx, int by, Rect R, ref int pixelX, ref int pixelY, ref int bw, ref int bh)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:90
    public virtual void GetBlockPositionOfPixel(int pixelX, int pixelY, Rect R, ref int bx, ref int by)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:91
    public virtual int GetInverseBlockSize(int X, int Y, Matrix matrix, ref int bw, ref int bh)
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
    public virtual int Activate(CustomDisplay display)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:95
    public virtual int Deactivate()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:96
    public virtual int FlipSurface(int index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:97
    public virtual int BlitSurface(int index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:562
    // { Blits the invalidated blocks of back surface Index to the primary surface: every block the
    //   matrix marks is merged with its marked neighbours (GetInverseBlockSize) into one rectangle, and
    //   the marks count down, so a block is blitted once per back surface. }
    public virtual int BlitInvalidatedSurface(int index)
    {
        int result;
        Matrix M;
        int bw = default;
        int bh = default;
        int pixelX2 = default;
        int pixelY2 = default;
        int blockWidth = default;
        int blockHeight = default;
        int V;
        Rect clipR = default;
        Rect sourceR = default;
        Rect destinationR = default;
        if (GetInverseMatrix() != null)
        {
            result = 0;
            var inverseMatrix = GetInverseMatrix();
            M = new Matrix(GetInverseMatrix().Width, inverseMatrix.Height);
            for (int I = 0, last = GetInverseMatrix().Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = GetInverseMatrix().Height - 1; J <= lastJ; J++)
                {
                    if (GetInverseMatrix().GetValue((byte)I, (byte)J) != 0)
                    {
                        M.SetValue((byte)I, (byte)J, 1);
                    }
                }
            }

            clipR.Left = 0;
            clipR.Right = GetSurface(1).Width;
            clipR.Top = 0;
            clipR.Bottom = GetSurface(1).Height;
            for (int J = 0, lastJ = M.Height - 1; J <= lastJ; J++)
            {
                for (int I = 0, last = M.Width - 1; I <= last; I++)
                {
                    if (result >= 0)
                    {
                        V = GetInverseMatrix().GetValue((byte)I, (byte)J);
                        if (V != 0)
                        {
                            GetInverseMatrix().SetValue((byte)I, (byte)J, (byte)(V - 1));
                        }

                        if (M.GetValue((byte)I, (byte)J) != 0)
                        {
                            GetInverseBlockSize(I, J, M, ref bw, ref bh);
                            GetPixelPositionOfBlock(I, J, clipR, ref sourceR.Left, ref sourceR.Top, ref blockWidth, ref blockHeight);
                            if (bw > 1 || bh > 1)
                            {
                                GetPixelPositionOfBlock(I + bw - 1, J + bh - 1, clipR, ref pixelX2, ref pixelY2, ref blockWidth, ref blockHeight);
                                sourceR.Right = pixelX2 + blockWidth;
                                sourceR.Bottom = pixelY2 + blockHeight;
                            }
                            else
                            {
                                sourceR.Right = sourceR.Left + blockWidth;
                                sourceR.Bottom = sourceR.Top + blockHeight;
                            }

                            var primarySurfaceRegion = GetPrimarySurfaceRegion();
                            GetPixelPositionOfBlock(I, J, primarySurfaceRegion.Value, ref destinationR.Left, ref destinationR.Top, ref blockWidth, ref blockHeight);
                            var primarySurfaceRegion2 = GetPrimarySurfaceRegion();
                            destinationR.Left += primarySurfaceRegion2.Value.Left;
                            var primarySurfaceRegion3 = GetPrimarySurfaceRegion();
                            destinationR.Top += primarySurfaceRegion3.Value.Top;
                            if (bw > 1 || bh > 1)
                            {
                                var primarySurfaceRegion4 = GetPrimarySurfaceRegion();
                                GetPixelPositionOfBlock(I + bw - 1, J + bh - 1, primarySurfaceRegion4.Value, ref pixelX2, ref pixelY2, ref blockWidth, ref blockHeight);
                                var primarySurfaceRegion5 = GetPrimarySurfaceRegion();
                                destinationR.Right = pixelX2 + blockWidth + primarySurfaceRegion5.Value.Left;
                                var primarySurfaceRegion6 = GetPrimarySurfaceRegion();
                                destinationR.Bottom = pixelY2 + blockHeight + primarySurfaceRegion6.Value.Top;
                            }
                            else
                            {
                                destinationR.Right = destinationR.Left + blockWidth;
                                destinationR.Bottom = destinationR.Top + blockHeight;
                            }

                            var surface = GetSurface(0);
                            result = GetSurface(index).BlitStretch(surface, sourceR, destinationR);
                            for (int X = 0, lastX = bw - 1; X <= lastX; X++)
                            {
                                for (int Y = 0, lastY = bh - 1; Y <= lastY; Y++)
                                {
                                    M.SetValue((byte)(I + X), (byte)(J + Y), 0);
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
            result = DisplayCodes.DCERR_FAILED;
        }

        return result;
    }

    // DisplayC.pas:99
    public virtual int Handle()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:100
    public virtual WinControl OutputWindow()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:101
    public virtual int ResizeBackSurfaces(int width, int height)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:102
    public virtual void GetMousePosition(ref int X, ref int Y)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:103
    public virtual int RealizePaletteNow()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:104
    public virtual int SetPalette(Palette palette)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:105
    public virtual int WaitForVerticalBlank()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // DisplayC.pas:106
    public virtual void InitializeInverseMatrix(byte width, byte height)
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
