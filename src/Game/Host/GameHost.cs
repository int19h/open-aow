#nullable disable warnings

namespace Aow.Game.Host;

public static class GameHost
{
    public static IGameHost? MainForm { get; set; }

    public static byte DeferredSetupAction { get; set; }
}
