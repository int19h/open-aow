#nullable disable
using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Host;

/// <summary>The screen callbacks implemented by the rewritten platform host.</summary>
public interface IGameHost
{
    Display Display { get; }
    SdlDC DXDC { get; }
    short FCursor { get; set; }
    Draw.Images.LibraryImage FCursorImage { get; set; }
    Rect FLastCursorRect { get; }
    bool FCursorChanged { get; }

    bool CloseRequested();
}

public static class GameHost
{
    public static IGameHost MainForm { get; set; }

    public static byte DeferredSetupAction;
}
