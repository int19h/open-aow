using Aow.Game.Draw;
using Aow.Rtl;
using PageTurnMode = Aow.Game.Ui.Grids.PageTurnMode;

namespace Aow.Game.Ui.Windows;

public partial class AoWBookWindow
{
    public override void Blt(Surface surface)
    {
        if (FPageSurface is not null && (FPageSurface.PixelFormatID != FBackSurface.PixelFormatID ||
            FPageSurface.Width != FBackSurface.Width || FPageSurface.Height != FBackSurface.Height ||
            FPageSurface is DDrawSurface != FBackSurface is DDrawSurface))
        {
            FPageSurface?.Dispose();
            FPageSurface = null;
        }
        var area = new Rect(0, 0, FWinWidth, FWinHeight);
        if (FPageSurface is null)
        {
            if (FBackSurface is DDrawSurface direct)
            {
                var flags = direct.Status.Contains(DDrawSurfaceStatusElement.VideoMemory) && FUseVMem
                    ? Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.VideoMemory, DDSCreationFlagsElement.SystemMemory)
                    : Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.SystemMemory);
                var page = new DDrawSurface();
                page.CreateSurface(FWinWidth, FWinHeight, FBackSurface.PixelFormatID, flags);
                FPageSurface = page;
            }
            else FPageSurface = new DataSurface(FWinWidth, FWinHeight, FBackSurface.PixelFormatID, null);
            FBackSurface.BltAreaClipped(FPageSurface, area, 0, 0);
        }
        if (!FAnimating)
        {
            base.Blt(surface);
            return;
        }
        if (FFirstAnimationFrame || !FPreviousSurfaceValid)
        {
            area = new Rect(0, 0, FWinWidth, FWinHeight);
            Surface initial = FPageTurnMode == PageTurnMode.Left ? FBackSurface : FPageSurface;
            if (FPageTurnMode is PageTurnMode.Left or PageTurnMode.Right)
            {
                if (!FMasked) initial.BltAreaClipped(surface, area, FWinLeft, FWinTop);
                else initial.BltAreaColorKeyClipped(surface, area, FWinLeft, FWinTop,
                    Aow.Game.Draw.PixelFormats.Color2PixelInfo(FMaskColor, FBackSurface.PixelFormatID, null));
            }
            FFirstAnimationFrame = false;
        }
        SurfaceDesc destination = default, source = default, oldPage = default;
        surface.Lock(ref destination);
        FBackSurface.Lock(ref source);
        FPageSurface.Lock(ref oldPage);
        FPageTurn.RenderScene(FWinLeft + FBorderWidth, FWinTop + FBorderHeight, destination,
            FBorderWidth, FBorderHeight, oldPage, FBorderWidth, FBorderHeight, source, ref area);
        FPageSurface.Unlock();
        FBackSurface.Unlock();
        surface.Unlock();
        FBackRect.Top = area.Top;
        // AOWTools keeps the area above the window intact
        if (FBackRect.Top >= FWinTop && FBackRect.Top <= FWinTop + FFrameTop) FBackRect.Top = FWinTop - 1;
        FBackRect.Bottom = FWinTop - area.Top + FWinHeight;
        FSurfaceValid = true;
        FOnDrawSurface?.Invoke(this, surface, FWinLeft, FWinTop);
    }
}
