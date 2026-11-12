using Aow.Game.Draw;
using Aow.Rtl;
using PageTurnMode = Aow.Game.UI.Grids.PageTurnMode;

namespace Aow.Game.UI.Windows;

public partial class BookWindow
{
    public override void Blit(Surface surface)
    {
        if (_pageSurface is not null && (_pageSurface.PixelFormatId != BackSurface.PixelFormatId ||
            _pageSurface.Width != BackSurface.Width || _pageSurface.Height != BackSurface.Height))
        {
            _pageSurface?.Dispose();
            _pageSurface = null;
        }
        var area = new Rect(0, 0, WinWidth, WinHeight);
        if (_pageSurface is null)
        {
            _pageSurface = new DataSurface(WinWidth, WinHeight, BackSurface.PixelFormatId, null);
            BackSurface.BlitAreaClipped(_pageSurface, area, 0, 0);
        }
        if (!Animating)
        {
            base.Blit(surface);
            return;
        }
        if (_firstAnimationFrame || !_previousSurfaceValid)
        {
            area = new Rect(0, 0, WinWidth, WinHeight);
            Surface initial = _pageTurnMode == PageTurnMode.Left ? BackSurface : _pageSurface;
            if (_pageTurnMode is PageTurnMode.Left or PageTurnMode.Right)
            {
                if (!Masked) initial.BlitAreaClipped(surface, area, WinLeft, WinTop);
                else initial.BlitAreaColorKeyClipped(surface, area, WinLeft, WinTop,
                    PixelFormats.Color2PixelInfo(_maskColor, BackSurface.PixelFormatId, null));
            }
            _firstAnimationFrame = false;
        }
        LockedSurface destination = default, source = default, oldPage = default;
        surface.Lock(ref destination);
        BackSurface.Lock(ref source);
        _pageSurface.Lock(ref oldPage);
        PageTurn.RenderScene(WinLeft + _borderWidth, WinTop + _borderHeight, destination,
            _borderWidth, _borderHeight, oldPage, _borderWidth, _borderHeight, source, ref area);
        _pageSurface.Unlock();
        BackSurface.Unlock();
        surface.Unlock();
        BackRectangle.Top = area.Top;
        // AOWTools keeps the area above the window intact
        if (BackRectangle.Top >= WinTop && BackRectangle.Top <= WinTop + FrameTop) BackRectangle.Top = WinTop - 1;
        BackRectangle.Bottom = WinTop - area.Top + WinHeight;
        SurfaceValid = true;
        RaiseDrawSurface(this, surface, WinLeft, WinTop);
    }
}
