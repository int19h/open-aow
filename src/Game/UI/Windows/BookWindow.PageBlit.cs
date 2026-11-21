#nullable disable warnings
using Aow.Game.Draw;
using Aow.Rtl;
using PageTurnMode = Aow.Game.UI.Grids.PageTurnMode;

namespace Aow.Game.UI.Windows;

public sealed partial class BookWindow
{
    public override void Blit(Surface? surface)
    {
        if (_pageSurface is not null && (_pageSurface.PixelFormatId != BackSurface.PixelFormatId ||
            _pageSurface.Width != BackSurface.Width || _pageSurface.Height != BackSurface.Height))
        {
            _pageSurface?.Dispose();
            _pageSurface = null;
        }
        Rect area = new(0, 0, WinWidth, WinHeight);
        if (_pageSurface is null)
        {
            _pageSurface = new DataSurface(WinWidth, WinHeight, BackSurface.PixelFormatId, palette: null);
            _ = BackSurface.BlitAreaClipped(_pageSurface, area, 0, 0);
        }
        if (!Animating)
        {
            base.Blit(surface);
            return;
        }
        if (_firstAnimationFrame || !_previousSurfaceValid)
        {
            area = new Rect(0, 0, WinWidth, WinHeight);
            Surface? initial = _pageTurnMode == PageTurnMode.Left ? BackSurface : _pageSurface;
            if (_pageTurnMode is PageTurnMode.Left or PageTurnMode.Right)
            {
                _ = !Masked
                    ? initial.BlitAreaClipped(surface, area, WinLeft, WinTop)
                    : initial.BlitAreaColorKeyClipped(surface, area, WinLeft, WinTop,
                    colorKey: PixelFormats.Color2PixelInfo(MaskColor, BackSurface.PixelFormatId, palette: null));
            }
            _firstAnimationFrame = false;
        }
        LockedSurface destination = default, source = default, oldPage = default;
        _ = surface.Lock(ref destination);
        _ = BackSurface.Lock(ref source);
        _ = _pageSurface.Lock(ref oldPage);
        PageTurn.RenderScene(WinLeft + BorderWidth, WinTop + BorderHeight, destination,
            BorderWidth, BorderHeight, oldPage, BorderWidth, BorderHeight, source, R: ref area);
        _ = _pageSurface.Unlock();
        _ = BackSurface.Unlock();
        _ = surface.Unlock();
        int top = area.Top;
        // AOWTools keeps the area above the window intact
        if (top >= WinTop && top <= WinTop + FrameTop)
        {
            top = WinTop - 1;
        }
        BackRectangle = BackRectangle with { Top = top, Height = WinTop - area.Top + WinHeight };
        SurfaceValid = true;
        RaiseBlitted(this, surface, WinLeft, WinTop);
    }
}
