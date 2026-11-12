
using Aow.Engine;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public static class EngineHost
{
    internal static void ReleaseCrc32Table()
    {
        Crc32Table = [];
    }

    internal static bool VerifyEngineObjectEnabled { get; set; }

    public static List<GameEngine> Engines { get; set; }

    public static List<Plugin> Plugins { get; set; }

    internal static int[] Crc32Table { get; set; } = [];

    internal static void Initialize()
    {
        Crc32Table = [];
        VerifyEngineObjectEnabled = true;
        Engines = new List<GameEngine>();
        Plugins = new List<Plugin>();
    }

    internal static void Finalize()
    {
        ReleaseCrc32Table();
        Engines?.Clear();
        Plugins?.Clear();
    }

}
