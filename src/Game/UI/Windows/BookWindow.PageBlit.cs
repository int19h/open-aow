using Aow.Game.Draw;
using Aow.Rtl;
using PageTurnMode = Aow.Game.UI.Grids.PageTurnMode;

namespace Aow.Game.UI.Windows;

public partial class BookWindow
{
    public override void Blt(Surface surface)
    {
        if (FPageSurface is not null && (FPageSurface.PixelFormatID != FBackSurface.PixelFormatID ||
            FPageSurface.Width != FBackSurface.Width || FPageSurface.Height != FBackSurface.Height ||
            FPageSurface is DirectDrawSurface != FBackSurface is DirectDrawSurface))
        {
            FPageSurface?.Dispose();
            FPageSurface = null;
        }
        var area = new Rect(0, 0, FWinWidth, FWinHeight);
        if (FPageSurface is null)
        {
            if (FBackSurface is DirectDrawSurface direct)
            {
                var flags = direct.Status.Contains(DirectDrawSurfaceStatusElement.VideoMemory) && FUseVMem
                    ? Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.VideoMemory, DirectDrawSurfaceCreationFlagsElement.SystemMemory)
                    : Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                var page = new DirectDrawSurface();
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
                    PixelFormats.Color2PixelInfo(FMaskColor, FBackSurface.PixelFormatID, null));
            }
            FFirstAnimationFrame = false;
        }
        LockedSurface destination = default, source = default, oldPage = default;
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
