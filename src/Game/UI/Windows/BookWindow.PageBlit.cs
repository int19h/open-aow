using Aow.Game.Draw;
using Aow.Rtl;
using PageTurnMode = Aow.Game.UI.Grids.PageTurnMode;

namespace Aow.Game.UI.Windows;

public partial class BookWindow
{
    public override void Blit(Surface surface)
    {
        if (PageSurface is not null && (PageSurface.PixelFormatId != BackSurface.PixelFormatId ||
            PageSurface.Width != BackSurface.Width || PageSurface.Height != BackSurface.Height ||
            PageSurface is DirectDrawSurface != BackSurface is DirectDrawSurface))
        {
            PageSurface?.Dispose();
            PageSurface = null;
        }
        var area = new Rect(0, 0, _winWidth, _winHeight);
        if (PageSurface is null)
        {
            if (BackSurface is DirectDrawSurface direct)
            {
                var flags = direct.Status.Contains(DirectDrawSurfaceStatusElement.VideoMemory) && _useVideoMemory
                    ? Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.VideoMemory, DirectDrawSurfaceCreationFlagsElement.SystemMemory)
                    : Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                var page = new DirectDrawSurface();
                page.CreateSurface(_winWidth, _winHeight, BackSurface.PixelFormatId, flags);
                PageSurface = page;
            }
            else PageSurface = new DataSurface(_winWidth, _winHeight, BackSurface.PixelFormatId, null);
            BackSurface.BlitAreaClipped(PageSurface, area, 0, 0);
        }
        if (!Animating)
        {
            base.Blit(surface);
            return;
        }
        if (FirstAnimationFrame || !PreviousSurfaceValid)
        {
            area = new Rect(0, 0, _winWidth, _winHeight);
            Surface initial = PageTurnMode == PageTurnMode.Left ? BackSurface : PageSurface;
            if (PageTurnMode is PageTurnMode.Left or PageTurnMode.Right)
            {
                if (!_masked) initial.BlitAreaClipped(surface, area, _winLeft, _winTop);
                else initial.BlitAreaColorKeyClipped(surface, area, _winLeft, _winTop,
                    PixelFormats.Color2PixelInfo(_maskColor, BackSurface.PixelFormatId, null));
            }
            FirstAnimationFrame = false;
        }
        LockedSurface destination = default, source = default, oldPage = default;
        surface.Lock(ref destination);
        BackSurface.Lock(ref source);
        PageSurface.Lock(ref oldPage);
        PageTurn.RenderScene(_winLeft + _borderWidth, _winTop + _borderHeight, destination,
            _borderWidth, _borderHeight, oldPage, _borderWidth, _borderHeight, source, ref area);
        PageSurface.Unlock();
        BackSurface.Unlock();
        surface.Unlock();
        BackRectangle.Top = area.Top;
        // AOWTools keeps the area above the window intact
        if (BackRectangle.Top >= _winTop && BackRectangle.Top <= _winTop + FrameTop) BackRectangle.Top = _winTop - 1;
        BackRectangle.Bottom = _winTop - area.Top + _winHeight;
        SurfaceValid = true;
        _onDrawSurface?.Invoke(this, surface, _winLeft, _winTop);
    }
}
