using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Storage;
using Aow.Game.Saves;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Nodes;

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

    public static TList Engines;

    public static TList PlugIns;

    public static TStringList Packages;

    internal static int[] CRC32Table = [];

    internal static void Initialize()
    {
        CRC32Table = [];
        VerifyEObjectEnabled = true;
        Engines = new TList();
        PlugIns = new TList();
        Packages = new TStringList();
    }

    internal static void Finalize()
    {
        ReleaseCRC32Table();
        UnloadExternalPlugIns();
        Packages?.Dispose();
        Engines?.Dispose();
        PlugIns?.Dispose();
    }

}
