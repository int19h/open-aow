using System.Diagnostics;
using System.Text.Json;
using Aow.Engine;
using Aow.Formats;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Shim = Aow.Rtl.RtlUnit;

namespace Aow.Headless;

/// <summary>
/// The load and save measurement of the pacing layer (own-formats.md 9.3): one file per process,
/// through the game's own routines and the file boundary. The original load reads the file, the
/// save writes its own document to the run directory, and the own load reads that document back.
/// The peak is the process's peak working set. A world map or save loads with LoadHSM, a scenario
/// set with LoadHSS, and a campaign with LoadCampaign and its map with LoadHSM, which SaveCampaign
/// embeds.
/// </summary>
internal static class PersistenceMeasurement
{
    public static int Run(string root, string kind, string file, string? map, string output)
    {
        Directory.CreateDirectory(output);
        var host = new HeadlessHost(root, output, settings: null, input: Path.GetFullPath(file));
        Shim.InstallHost(host);
        Startup.InitializeAll();
        var engine = Startup.CreateEngine();
        NullSoundSystem.Install();
        CombatClasses.Register(engine);
        // The world map screen registers the campaign classes when it starts (WorldMap.RegisterWorldMapCampaign).
        if (kind == "campaign")
            engine.RegisterEngineClasses([EngineClass.Of<CampaignScenarioPage>(), EngineClass.Of<WorldMapCampaignScenario>(), EngineClass.Of<WorldMapCampaign>()]);
        string extension = kind switch
        {
            "map" => DocumentKind.Save.OwnExtension,
            "set" => DocumentKind.ScenarioSet.OwnExtension,
            "campaign" => DocumentKind.CampaignSave.OwnExtension,
            _ => throw new ArgumentException("--measure-persistence takes map, set or campaign"),
        };
        // A scenario set stores the game set files beside it, and its load reads the CRC of the
        // RELEASE.HSS beside it, so its own document is that name in the run's Release folder.
        string saved = kind == "set"
            ? Path.Combine(host.GameRoot, "Release", "Release" + extension)
            : Path.Combine(host.GameRoot, "Save", "measure" + extension);
        var watch = Stopwatch.StartNew();
        switch (kind)
        {
            case "map":
                engine.LoadHsm(Path.GetFullPath(file), edit: false, progress: null!, profile: GameProfile.WorldMap);
                Require(engine.HexSpriteMap is not null, "original load");
                break;
            case "set":
                engine.LoadHss(Path.GetFullPath(file), edit: false, progress: null!, profile: GameProfile.ScenarioSet);
                break;
            default:
                engine.LoadCampaign(Path.GetFullPath(file));
                engine.LoadHsm(Path.GetFullPath(map ?? throw new ArgumentException("a campaign needs --map")), edit: false, progress: null!, profile: GameProfile.WorldMap);
                Require(engine.Campaign is not null && engine.HexSpriteMap is not null, "original load");
                break;
        }
        double originalLoad = watch.Elapsed.TotalMilliseconds;
        watch.Restart();
        switch (kind)
        {
            case "map":
                engine.SaveHsm(saved, progress: null!, profile: GameProfile.WorldMap);
                break;
            case "set":
                engine.SaveHss(saved, progress: null!, profile: GameProfile.ScenarioSet);
                break;
            default:
                engine.SaveCampaign(saved);
                break;
        }
        double save = watch.Elapsed.TotalMilliseconds;
        engine.CloseHsm(null!);
        watch.Restart();
        switch (kind)
        {
            case "map":
                engine.LoadHsm(saved, edit: false, progress: null!, profile: GameProfile.WorldMap);
                Require(engine.HexSpriteMap is not null, "own load");
                break;
            case "set":
                engine.LoadHss(saved, edit: false, progress: null!, profile: GameProfile.ScenarioSet);
                break;
            default:
                engine.LoadCampaign(saved);
                engine.LoadCampaignMap(null!);
                Require(engine.Campaign is not null && engine.HexSpriteMap is not null, "own load");
                break;
        }
        double ownLoad = watch.Elapsed.TotalMilliseconds;
        using var process = Process.GetCurrentProcess();
        process.Refresh();
        double peak = process.PeakWorkingSet64 / 1048576.0;
        long original = new FileInfo(Path.GetFullPath(file)).Length;
        long own = host.Files.FileExists(saved) ? ReadLength(host, path: saved) : 0;
        string name = Path.GetFileName(file);
        File.WriteAllText(Path.Combine(output, "persistence.json"), contents: JsonSerializer.Serialize(new
        {
            file = name,
            kind,
            original_bytes = original,
            own_bytes = own,
            original_load_ms = originalLoad,
            save_ms = save,
            own_load_ms = ownLoad,
            peak_mib = peak,
        }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
        Console.WriteLine($"persistence {name}: original load {originalLoad:F0} ms, save {save:F0} ms, own load {ownLoad:F0} ms, peak {peak:F1} MiB, {original} original bytes, {own} own bytes");
        return 0;
    }

    private static long ReadLength(HeadlessHost host, string path)
    {
        using var stream = host.Files.Open(path, mode: Shim.fmOpenRead | Shim.fmShareDenyWrite);
        return stream.Length;
    }

    private static void Require(bool condition, string step)
    {
        if (!condition) throw new InvalidDataException(step + " failed" + (FileBoundary.LastFailure is { } refused ? ": " + refused : ""));
    }
}
