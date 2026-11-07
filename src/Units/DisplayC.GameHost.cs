#nullable disable
using Aow.Rtl;
using Aow.Units.DisplayC;

namespace Aow.Platform;

/// <summary>The screen callbacks implemented by the rewritten platform host.</summary>
public interface IGameHost
{
    TDisplay Display { get; }
    TSdlDC DXDC { get; }
    short FCursor { get; set; }
    Units.ImageLib.TLibraryImage FCursorImage { get; set; }
    TRect FLastCursorRect { get; }
    bool FCursorChanged { get; }

    bool CloseRequested();
}

public static class GameHost
{
    public static IGameHost MainForm { get; set; }

    public static byte DeferredSetupAction;
}
