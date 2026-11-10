#nullable disable
using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Host;

/// <summary>The screen callbacks implemented by the rewritten platform host.</summary>
public interface IGameHost
{
    TDisplay Display { get; }
    TSdlDC DXDC { get; }
    short FCursor { get; set; }
    Aow.Game.Draw.Images.TLibraryImage FCursorImage { get; set; }
    TRect FLastCursorRect { get; }
    bool FCursorChanged { get; }

    bool CloseRequested();
}

public static class GameHost
{
    public static IGameHost MainForm { get; set; }

    public static byte DeferredSetupAction;
}
