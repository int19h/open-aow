
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

    internal static void ReleaseCRC32Table()
    {
        CRC32Table = [];
    }

    internal static bool VerifyEObjectEnabled;

    public static List<GameEngine> Engines;

    public static List<PlugIn> PlugIns;

    public static StringList Packages;

    internal static int[] CRC32Table = [];

    internal static void Initialize()
    {
        CRC32Table = [];
        VerifyEObjectEnabled = true;
        Engines = new List<GameEngine>();
        PlugIns = new List<PlugIn>();
        Packages = new StringList();
    }

    internal static void Finalize()
    {
        ReleaseCRC32Table();
        UnloadExternalPlugIns();
        Packages?.Dispose();
        Engines?.Clear();
        PlugIns?.Clear();
    }

}
