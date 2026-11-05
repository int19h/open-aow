#nullable disable
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.DisplayC.TDisplayCtrlStatusElement;

namespace Aow.Units.DisplayC;

// The primary constructor captures host before the base inserts this component.
public sealed class TSdlDC(IDisplayHost host, TComponent owner = null) : TDisplayCtrl(owner)
{
    private readonly TSurface[] surfaces = new TSurface[3];
    private readonly Ptr<TRect> primaryRegion = Ptr<TRect>.Alloc(1);
    public override void SetPrimarySurfaceRgn(in TRect region) { FPrimarySurfaceRgn = region; primaryRegion.Value = region; }
    public override Ptr<TRect> GetPrimarySurfaceRgn() => primaryRegion;
    public bool UseVerticalBlank { get; set; } = true;
    public override TSurface GetSurface(int index) => index >= 0 && index < surfaces.Length ? surfaces[index] : null;
    public override int Activate(TCustomDisplay display)
    {
        int result = base.Activate(display);
        if (result < 0) return result;
        FStatus = FStatus.With(dsWindowed).With(dsVisible);
        surfaces[0] = new ScreenSurface(host);
        try
        {
            for (int i = 1; i <= (int)display.FBufferingScheme; i++)
            {
                surfaces[i] = new TDIB16Surface(display.FBackSurfaceWidth, display.FBackSurfaceHeight, GFXEUnit.pfRGB555);
                surfaces[i].Cls(display.Color);
            }
            return 0;
        }
        catch { Deactivate(); throw; }
    }
    public override int Deactivate()
    {
        for (int i = 0; i < surfaces.Length; i++) { surfaces[i]?.Free(); surfaces[i] = null; }
        return base.Deactivate();
    }
    public override int BltSurface(int index)
    {
        var surface = GetSurface(index);
        if (surface is null) return DisplayCUnit.DCERR_FAILED;
        TSurfaceDesc descriptor = default;
        int result = surface.Lock(ref descriptor);
        if (result < 0) return result;
        try { host.Present(descriptor); }
        finally { surface.Unlock(); }
        return 0;
    }
    public override int FlipSurface(int index)
    {
        int result = BltSurface(index);
        return result < 0 ? result : DisplayCUnit.DCERR_NOFLIP;
    }
    public override int ResizeBackSurfaces(int width, int height)
    {
        foreach (var surface in surfaces.Skip(1)) surface?.Resize(width, height);
        host.ResizeImage(width, height);
        InvalidateAll();
        return 0;
    }
    public override void GetMousePos(ref int x, ref int y)
    {
        var pointer = host.PointerPosition();
        (x, y) = host.HasFocus && pointer.Inside ? (pointer.X, pointer.Y) : (-1, -1);
        host.ShowSystemCursor(x == -1);
    }
    public override int Hide() { FStatus = FStatus.Without(dsVisible); return 0; }
    public override int Show() { FStatus = FStatus.With(dsVisible); return 0; }
    public override bool WndProc(ref TMessage message) => false;
    public override int SupportDisplayMode(TCustomDisplay display) => 0;
    public override void WindowRecreated() { }

    private sealed class ScreenSurface(IDisplayHost host) : TSurface
    {
        public override int GetWidth() => host.ClientSize.Width;
        public override int GetHeight() => host.ClientSize.Height;
        public override int GetPixelFormatID() => GFXEUnit.pfRGB555;
        public override int Lock(ref TSurfaceDesc descriptor) => GFXEUnit.GFXE_NOTSUPPORTED;
    }
}
