#nullable disable warnings
using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Host;

/// <summary>The screen callbacks implemented by the rewritten platform host.</summary>
public interface IGameHost
{
    Display Display { get; }
    SdlDisplayController DisplayController { get; }
    int FCursor { get; set; }
    Draw.Images.LibraryImage? FCursorImage { get; set; }
    Rect FLastCursorRectangle { get; }
    bool FCursorChanged { get; }

    bool CloseRequested();
}
