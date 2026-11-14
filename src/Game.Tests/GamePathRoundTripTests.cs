using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// The profiles the gate's runs do not save (issue 185, PE-3-g): a tactical map, a scenario set,
/// a hero list and an item list go through the game's own save and load routines, which write
/// and read the tree through the file boundary (own-formats.md 4.2, persistence-tree.md 6). The
/// loaded object writes the image the saved object writes, and a second save writes the same
/// document.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class GamePathRoundTripTests : IDisposable
{
    // The game's saves replace their files through the layered file service, as on a host: the
    // root holds what the test puts there and the user layer what the game writes.
    private readonly string _root = Path.Combine(Path.GetTempPath(), "open-aow-round-trip-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem _previous = Runtime.Files;

    public GamePathRoundTripTests()
    {
        Directory.CreateDirectory(Path.Combine(_root, "root", "RoundTrip"));
        Directory.CreateDirectory(Path.Combine(_root, "user"));
    }

    // The engine state of each case installs the fixture's services first, so a case installs the
    // layers when it starts.
    private void UseLayers() => Runtime.Files = new LayeredFileSystem(Path.Combine(_root, "root"), userDirectory: Path.Combine(_root, "user"));

    public void Dispose()
    {
        Runtime.Files = _previous;
        Directory.Delete(_root, recursive: true);
    }

    private string Folder => Path.Combine(_root, "root", "RoundTrip");

    private static byte[] ReadBack(string file)
    {
        using var stream = Runtime.Files.Open(file, mode: Runtime.fmOpenRead | Runtime.fmShareDenyWrite);
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static GameEngine Engine => EngineFixture.Engine;

    [Fact]
    public void AHeroListRoundTripsThroughTheGame()
    {
        UseLayers();
        using var saved = new HeroLibrary(null!) { Holder = Engine, LibraryName = "Heroes of the test" };
        string file = Path.Combine(Folder, "test.oahl");
        Engine.WriteToFile(file, objectValue: saved, progress: null, profile: GameProfile.HeroList);
        using var loaded = new HeroLibrary(null!) { Holder = Engine };
        Engine.ReadFromFile(file, objectValue: loaded, progress: null, profile: GameProfile.HeroList);
        Assert.Equal("Heroes of the test", loaded.LibraryName);
        Assert.Equal(Image(saved), Image(loaded));
        SavesTheSameDocumentAgain(file, () => Engine.WriteToFile(file, objectValue: loaded, progress: null, profile: GameProfile.HeroList));
    }

    [Fact]
    public void AnItemListRoundTripsThroughTheGame()
    {
        UseLayers();
        using var saved = new ItemLibrary(null!) { Holder = Engine, Name = "Items of the test" };
        new Item(null!) { Holder = Engine, ItemId = 1, Name = "Sword", Attack = -2, LibraryId = 3 }.SetOwner(saved);
        new Item(null!) { Holder = Engine, ItemId = 2, Name = "Shield", LibraryId = 4 }.SetOwner(saved);
        string file = Path.Combine(Folder, "test.oail");
        Engine.WriteToFile(file, objectValue: saved, progress: null, profile: GameProfile.ItemList);
        using var loaded = new ItemLibrary(null!) { Holder = Engine };
        Engine.ReadFromFile(file, objectValue: loaded, progress: null, profile: GameProfile.ItemList);
        Assert.Equal(2, loaded.Count);
        Assert.Equal(Image(saved), Image(loaded));
        SavesTheSameDocumentAgain(file, () => Engine.WriteToFile(file, objectValue: loaded, progress: null, profile: GameProfile.ItemList));
    }

    // SaveHSS writes the set and the nine game set files beside it, and LoadHSS reads them back
    // through GamePack's tree method. The set is the fixture's, so its state goes back to it.
    [Fact]
    public void AScenarioSetRoundTripsThroughTheGame()
    {
        UseLayers();
        var set = Engine.HexSpriteSet;
        var before = Image(set);
        string file = Path.Combine(Folder, "Release.ohss");
        Engine.SaveHss(file, progress: null, profile: GameProfile.ScenarioSet);
        var document = GameFiles.Read(ReadBack(file)).Document;
        Assert.Equal("scenario-set", ((JsonString)((JsonObject)document["$container"]!)["profile"]!).Value);
        Engine.LoadHss(file, edit: false, progress: null, profile: GameProfile.ScenarioSet);
        Assert.Same(set, Engine.HexSpriteSet);
        Assert.Equal(before, Image(set));
        SavesTheSameDocumentAgain(file, () => Engine.SaveHss(file, progress: null, profile: GameProfile.ScenarioSet));
    }

    // A tactical map of one level, three fields wide and two high, with no hexagon sprite: the
    // fixture links no sprite to a resource. The map editor loads and saves tactical maps, so
    // the map loads for editing, and a map outside a battle activates only there.
    [Fact]
    public void ATacticalMapRoundTripsThroughTheGame()
    {
        UseLayers();
        var rows = new JsonArray();
        for (int y = 0; y < 2; y++)
        {
            var row = new JsonArray();
            for (int x = 0; x < 3; x++) row.Add(new JsonObject().Add("FieldFlags", new JsonInteger(x * 10 + y + 1)));
            rows.Add(row);
        }
        var level = new JsonObject()
            .Add("$class", new JsonString("CombatMapLevel")).Add("$slot", new JsonInteger(0))
            .Add("Width", new JsonInteger(3)).Add("Height", new JsonInteger(2)).Add("Fields", rows).Add("Children", new JsonArray());
        var container = new JsonObject()
            .Add("$class", new JsonString("MapContainer")).Add("$slot", new JsonInteger(6))
            .Add("Width", new JsonInteger(3)).Add("Height", new JsonInteger(2)).Add("LevelCount", new JsonInteger(1))
            .Add("Children", new JsonArray().Add(level));
        var root = new JsonObject()
            .Add("$class", new JsonString("CombatMap")).Add("ViewPixelX", new JsonInteger(12)).Add("ViewPixelY", new JsonInteger(8))
            .Add("Children", new JsonArray().Add(container));
        var document = new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName)).Add("$version", new JsonInteger(OwnFormat.CurrentVersion)).Add("$kind", new JsonString("map"))
            .Add("$container", new JsonObject().Add("profile", new JsonString("tactical-hsm")).Add("compression", new JsonString("cfs")).Add("level", new JsonInteger(2)))
            .Add("root", root);
        byte[] constructed = GameFiles.Write(document);
        string source = Path.Combine(Folder, "constructed.ohsm");
        string file = Path.Combine(Folder, "saved.ohsm");
        using (var stream = Runtime.Files.Open(source, mode: Runtime.fmCreate)) stream.Write(constructed);
        // The game registers the combat classes when combat starts (CombatClasses.Register).
        EngineClass[] combat = [.. new[] { EngineClass.Of<CombatMap>(), EngineClass.Of<CombatMapLevel>() }.Where(c => !Engine.ClassTable.ContainsKey(c.ClassId))];
        Engine.RegisterEngineClasses(combat);
        var previous = Engine.HexSpriteMap;
        try
        {
            Engine.LoadHsm(source, edit: true, progress: null!, profile: GameProfile.TacticalMap);
            Assert.True(Engine.HexSpriteMap is CombatMap, FileBoundary.LastFailure?.ToString());
            Engine.SaveHsm(file, progress: null!, profile: GameProfile.TacticalMap);
            Assert.Equal(constructed, ReadBack(file));
            Engine.LoadHsm(file, edit: true, progress: null!, profile: GameProfile.TacticalMap);
            SavesTheSameDocumentAgain(file, () => Engine.SaveHsm(file, progress: null!, profile: GameProfile.TacticalMap));
        }
        finally
        {
            if (Engine.HexSpriteMap != previous) Engine.HexSpriteMap?.Dispose();
            Engine.SetHexSpriteMapWithoutCurrentMap(previous);
            Engine.UnregisterEngineClasses(combat);
        }
    }

    private static void SavesTheSameDocumentAgain(string file, Action save)
    {
        byte[] first = ReadBack(file);
        save();
        Assert.Equal(first, ReadBack(file));
    }

    // The image the storage stream writes, the state the old game path saved.
    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, engine: Engine, partial: false))
        {
            objectValue.MainReadWrite(writer);
        }
        return memory.ToArray();
    }
}
