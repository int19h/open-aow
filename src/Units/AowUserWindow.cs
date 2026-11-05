// Translated from AowUserWindow.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWWindow;

namespace Aow.Units.AowUserWindow;

// extended drawing event
public partial class TAoWUserWindow : TAOWWindow
{
    public TAoWUserWindow(TComponent owner) : base(owner)
    { }

    public TDrawSurfaceExEvent FOnDrawSurfaceEx;
    // AowUserWindow.pas:29
    public override void DrawSurface(TSurface Surface)
    {
        bool Handled;
        Handled = false;
        if (FOnDrawSurfaceEx != null)
        {
            FOnDrawSurfaceEx(Surface, ref Handled);
        }

        if (!Handled)
        {
            base.DrawSurface(Surface);
        }
    }

    [Published]
    public TDrawSurfaceExEvent OnDrawSurfaceEx { get => FOnDrawSurfaceEx; set => FOnDrawSurfaceEx = value; }
}

public delegate void TDrawSurfaceExEvent(TSurface Surface, ref bool Handled);
public static partial class AowUserWindow
{
    // AowUserWindow.pas:38
    public static void Register()
    {
        RegisterComponents("AOW Tools", [typeof(TAoWUserWindow)]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
