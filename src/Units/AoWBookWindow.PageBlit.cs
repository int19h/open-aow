using Aow.Game.Draw;
using Aow.Rtl;
using TPageTurnMode = Aow.Game.Ui.Grids.TPageTurnMode;

namespace Aow.Game.Ui.Windows;

public partial class TAoWBookWindow
{
    public override void Blt(TSurface surface)
    {
        if (FPageSurface is not null && (FPageSurface.PixelFormatID != FBackSurface.PixelFormatID ||
            FPageSurface.Width != FBackSurface.Width || FPageSurface.Height != FBackSurface.Height ||
            FPageSurface is TDDrawSurface != FBackSurface is TDDrawSurface))
        {
            FPageSurface?.Dispose();
            FPageSurface = null;
        }
        var area = RtlUnit.Rect(0, 0, FWinWidth, FWinHeight);
        if (FPageSurface is null)
        {
            if (FBackSurface is TDDrawSurface direct)
            {
                var flags = direct.Status.Contains(TDDrawSurfaceStatusElement.ssVideoMemory) && FUseVMem
                    ? TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfVideoMemory, TDDSCreationFlagsElement.cfSystemMemory)
                    : TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfSystemMemory);
                var page = new TDDrawSurface();
                page.CreateSurface(FWinWidth, FWinHeight, FBackSurface.PixelFormatID, flags);
                FPageSurface = page;
            }
            else FPageSurface = new TDataSurface(FWinWidth, FWinHeight, FBackSurface.PixelFormatID, null);
            FBackSurface.BltAreaClipped(FPageSurface, area, 0, 0);
        }
        if (!FAnimating)
        {
            base.Blt(surface);
            return;
        }
        if (FFirstAnimationFrame || !FPreviousSurfaceValid)
        {
            area = RtlUnit.Rect(0, 0, FWinWidth, FWinHeight);
            TSurface initial = FPageTurnMode == TPageTurnMode.ptLeft ? FBackSurface : FPageSurface;
            if (FPageTurnMode is TPageTurnMode.ptLeft or TPageTurnMode.ptRight)
            {
                if (!FMasked) initial.BltAreaClipped(surface, area, FWinLeft, FWinTop);
                else initial.BltAreaColorKeyClipped(surface, area, FWinLeft, FWinTop,
                    Aow.Game.Draw.PixelFormats.Color2PixelInfo(FMaskColor, FBackSurface.PixelFormatID, null));
            }
            FFirstAnimationFrame = false;
        }
        TSurfaceDesc destination = default, source = default, oldPage = default;
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
