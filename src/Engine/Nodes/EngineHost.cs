
using Aow.Engine;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public static class EngineHost
{
    internal static void UnloadExternalPlugIns()
    {
        string S;
        for (int I = Packages.Count - 1; I >= 0; I--)
        {
            try
            {
                S = Packages[I];
                // { the original hands the name, not the handle in Objects, to UnloadPackage }
                /* TODO: Implement the native Delphi package loader. */
                throw new NotImplementedException();
                Packages.Delete(I);
            }
            catch
            {
                S = "Could not release PlugIn: " + Packages[I];
                throw new InvalidOperationException(S);
            }
        }

        Packages.Clear();
    }

    internal static void ReleaseCrc32Table()
    {
        s_crc32Table = [];
    }

    internal static bool s_verifyEngineObjectEnabled;

    public static List<GameEngine> Engines;

    public static List<Plugin> Plugins;

    public static StringList Packages;

    internal static int[] s_crc32Table = [];

    internal static void Initialize()
    {
        s_crc32Table = [];
        s_verifyEngineObjectEnabled = true;
        Engines = new List<GameEngine>();
        Plugins = new List<Plugin>();
        Packages = new StringList();
    }

    internal static void Finalize()
    {
        ReleaseCrc32Table();
        UnloadExternalPlugIns();
        Packages?.Dispose();
        Engines?.Clear();
        Plugins?.Clear();
    }

}
