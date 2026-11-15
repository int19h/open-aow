#nullable disable warnings
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

/// <summary>
/// PE-5-a. A save whose prisoners army holds an item has no engine image. The load dialog
/// lists it from the preview tree, and opening it returns the item. A campaign save nests that
/// map, and loading the campaign returns the item. The image load and the map transfer do not
/// return the file bytes.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class PrisonersSaveListingTests
{
    // A dungeon load links the resource of this id. The fixture pack holds none, so the case adds one.
    private const int PackResourceId = 424242;

    [Fact]
    public void APrisonersItemSurvivesTheLoadDialogListingAndTheOpen()
    {
        GameEngine engine = EngineFixture.Engine;
        GameMap previous = engine.HexSpriteMap;
        // LoadHsm disposes the engine's current map. Park it so this case can put it back.
        engine.SetHexSpriteMapWithoutCurrentMap(null);
        string directory = Path.Combine(EngineFixture.Root, "Save");
        Directory.CreateDirectory(directory);
        string source = Path.Combine(directory, "prisoners-source.oasg");
        string saved = Path.Combine(directory, "prisoners.oasg");
        MapObjectResourceList resources = engine.HexSpriteSet.MapObjectResourceList;
        StructureResource resource = new(null!) { Holder = engine, ResourceId = PackResourceId };
        // An explored dungeon reads this list while it activates. An empty list keeps that read in range.
        resource.ImageSequenceList = new ImageSequenceList(engine.HexSpriteSet.ImageLibraryManager, imageLibrary: null!);
        // Activation stores the engine on the resource. A structure reads that engine while it chooses a terrain type.
        engine.ActivateEngineObject(resource);
        // The fixture engine does not run GameClasses.Register. The running game registers a dungeon there.
        EngineClass dungeonClass = EngineClass.Of<Dungeon>();
        bool registerDungeon = !engine.ClassTable.ContainsKey(dungeonClass.ClassId);
        if (registerDungeon)
        {
            engine.RegisterEngineClasses([dungeonClass]);
        }
        // The fixture host records calls and does not replace a finished save. The physical
        // service does, and it reads the same paths this case writes.
        IFileSystem recordedFiles = Runtime.Files;
        PhysicalFileSystem physical = new();
        physical.SetCurrentDirectory(EngineFixture.Root);
        Runtime.Files = physical;
        try
        {
            resources.AddChild(resource);
            File.WriteAllBytes(source, GameFiles.Write(Document()));
            engine.LoadHsm(source, edit: false, progress: null!, profile: GameProfile.WorldMap);
            StrategicMap live = engine.HexSpriteMap as StrategicMap;
            if (live is null)
            {
                Surface(engine, source);
            }

            Assert.NotNull(live);
            Item planted = PrisonerItem(live);
            planted.Attack = -2;
            planted.LibraryId = 3;
            int players = live.Players.GetCount() - 1;
            int width = live.MapContainer.Width;
            int towns = live.StructureControl.CityCount;
            int day = live.Day;
            Assert.Equal(1, players);
            Assert.Equal(48, width);
            // Activation recounts cities from the map. This map has none, so the stored count is cleared.
            Assert.Equal(0, towns);
            Assert.Equal(4, day);

            engine.SaveHsm(saved, progress: null!, profile: GameProfile.WorldMap);
            byte[] fileBytes = File.ReadAllBytes(saved);
            (int Players, int Width, int Towns, int Day) listed = ListLikeTheLoadDialog(engine, saved);
            Assert.Equal(players, listed.Players);
            Assert.Equal(width, listed.Width);
            Assert.Equal(towns, listed.Towns);
            Assert.Equal(day, listed.Day);

            engine.LoadHsm(saved, edit: false, progress: null!, profile: GameProfile.WorldMap);
            StrategicMap opened = engine.HexSpriteMap as StrategicMap;
            if (opened is null)
            {
                Surface(engine, saved);
            }

            Item item = PrisonerItem(opened!);
            Assert.Equal("Blade", item.Name);
            Assert.Equal(7, item.ItemId);
            Assert.Equal(-2, item.Attack);
            Assert.Equal(3, item.LibraryId);

            Assert.Null(FileBoundary.Load(saved, GameProfile.WorldMap));
            GameFileResult prepared = GameBoundary.PrepareLoad(fileBytes, saved, GameProfile.WorldMap, FormatMapping.Validators);
            Assert.True(prepared.Succeeded, prepared.Report.Error?.ToString());
            Assert.False(prepared.HasImage);
            Assert.Null(prepared.Bytes);
            Assert.NotNull(prepared.ImageRefusal);
            Assert.Equal(Codes.Unrepresentable, prepared.ImageRefusal.Code);
            Assert.EndsWith(".Items", prepared.ImageRefusal.Path);
            byte[] canonical = GameFiles.Write(prepared.Document!);
            Assert.Equal(EngineServices.GetCrc32(canonical, canonical.Length), FileBoundary.ImageCrc32(saved, GameProfile.WorldMap));
            ConversionException refused = Assert.Throws<ConversionException>(() => FileBoundary.LoadOrRaise(saved, GameProfile.WorldMap));
            Assert.Equal(Codes.Unrepresentable, refused.Diagnostic.Code);
            Assert.EndsWith(".Items", refused.Diagnostic.Path);
            Assert.Equal("1073741824", refused.Diagnostic.Value);
            Assert.Contains("PrisonersArmy.Items", refused.Diagnostic.Message);
        }
        finally
        {
            Runtime.Files = recordedFiles;
            // Close the loaded map while it is still the current one. An activated army
            // leaves its player there, and a disposed current map has no player list.
            if (engine.HexSpriteMap is StrategicMap loaded && loaded != previous)
            {
                StrategicMap.Current = loaded;
            }

            if (engine.HexSpriteMap != previous)
            {
                engine.HexSpriteMap?.Dispose();
            }

            engine.SetHexSpriteMapWithoutCurrentMap(previous);
            StrategicMap.Current = previous as StrategicMap ?? null!;
            if (registerDungeon)
            {
                engine.UnregisterEngineClasses([dungeonClass]);
            }

            resources.RemoveChild(resource);
            resource.Dispose();
            if (File.Exists(source))
            {
                File.Delete(source);
            }

            if (File.Exists(saved))
            {
                File.Delete(saved);
            }
        }
    }

    [Fact]
    public void ACampaignSaveKeepsAPrisonersItem()
    {
        GameEngine engine = EngineFixture.Engine;
        GameMap previous = engine.HexSpriteMap;
        engine.SetHexSpriteMapWithoutCurrentMap(null);
        string directory = Path.Combine(EngineFixture.Root, "Save");
        Directory.CreateDirectory(directory);
        string source = Path.Combine(directory, "prisoners-campaign-source.oasg");
        string saved = Path.Combine(directory, "prisoners" + FileBoundary.WriteExtension(DocumentKind.CampaignSave));
        MapObjectResourceList resources = engine.HexSpriteSet.MapObjectResourceList;
        StructureResource resource = new(null!) { Holder = engine, ResourceId = PackResourceId };
        resource.ImageSequenceList = new ImageSequenceList(engine.HexSpriteSet.ImageLibraryManager, imageLibrary: null!);
        engine.ActivateEngineObject(resource);
        EngineClass dungeonClass = EngineClass.Of<Dungeon>();
        bool registerDungeon = !engine.ClassTable.ContainsKey(dungeonClass.ClassId);
        if (registerDungeon)
        {
            engine.RegisterEngineClasses([dungeonClass]);
        }

        IFileSystem recordedFiles = Runtime.Files;
        PhysicalFileSystem physical = new();
        physical.SetCurrentDirectory(EngineFixture.Root);
        Runtime.Files = physical;
        try
        {
            resources.AddChild(resource);
            File.WriteAllBytes(source, GameFiles.Write(Document()));
            engine.LoadHsm(source, edit: false, progress: null!, profile: GameProfile.WorldMap);
            StrategicMap live = engine.HexSpriteMap as StrategicMap;
            if (live is null)
            {
                Surface(engine, source);
            }

            Item planted = PrisonerItem(live!);
            planted.Attack = -2;
            planted.LibraryId = 3;

            engine.NewCampaign(EngineClass.Of<Campaign>().ClassId);
            engine.SaveCampaign(saved);
            GameFileResult prepared = GameBoundary.PrepareLoad(File.ReadAllBytes(saved), fileName: saved, use: GameProfile.Campaign, validators: FormatMapping.Validators);
            Assert.True(prepared.Succeeded, prepared.Report.Error?.ToString());
            Assert.False(prepared.HasImage);
            Assert.Null(prepared.Bytes);
            Assert.NotNull(prepared.ImageRefusal);
            Assert.Equal(Codes.Unrepresentable, prepared.ImageRefusal.Code);
            Assert.EndsWith(".Items", prepared.ImageRefusal.Path);
            Assert.Equal("1073741824", prepared.ImageRefusal.Value);
            Assert.Contains("PrisonersArmy.Items", prepared.ImageRefusal.Message);
            ConversionException exported = Assert.Throws<ConversionException>(() => GameFiles.Export(prepared.Document!));
            Assert.Equal(prepared.ImageRefusal.Path, exported.Diagnostic.Path);
            Assert.Equal(Codes.Unrepresentable, exported.Diagnostic.Code);

            engine.LoadCampaign(saved);
            Assert.True(GameFiles.IsJsonCandidate(engine.Campaign!.Stream.GetBuffer().AsSpan(0, (int)engine.Campaign.Stream.Length)));
            engine.LoadCampaignMap(null!);
            StrategicMap opened = engine.HexSpriteMap as StrategicMap;
            Assert.NotNull(opened);
            Item item = PrisonerItem(opened!);
            Assert.Equal("Blade", item.Name);
            Assert.Equal(7, item.ItemId);
            Assert.Equal(-2, item.Attack);
            Assert.Equal(3, item.LibraryId);
        }
        finally
        {
            Runtime.Files = recordedFiles;
            if (engine.HexSpriteMap is StrategicMap loaded && loaded != previous)
            {
                StrategicMap.Current = loaded;
            }

            if (engine.HexSpriteMap != previous)
            {
                engine.HexSpriteMap?.Dispose();
            }

            engine.SetHexSpriteMapWithoutCurrentMap(previous);
            StrategicMap.Current = previous as StrategicMap ?? null!;
            engine.CloseCampaign();
            if (registerDungeon)
            {
                engine.UnregisterEngineClasses([dungeonClass]);
            }

            resources.RemoveChild(resource);
            resource.Dispose();
            if (File.Exists(source))
            {
                File.Delete(source);
            }

            if (File.Exists(saved))
            {
                File.Delete(saved);
            }
        }
    }

    // FileLBChange reads these four values off the preview map. The dialog text is the player
    // count minus the independent player, the size letter of the width, the city count and the day.
    private static (int Players, int Width, int Towns, int Day) ListLikeTheLoadDialog(GameEngine engine, string file)
    {
        StrategicMap map = engine.CreatePreviewHsm(file, GameProfile.WorldMap) as StrategicMap;
        if (map is null)
        {
            Surface(engine, file);
        }

        try
        {
            return (map!.Players.GetCount() - 1, map.MapContainer.Width, map.StructureControl.CityCount, map.Day);
        }
        finally
        {
            map?.Dispose();
        }
    }

    private static void Surface(GameEngine engine, string file)
    {
        GameFileTree tree = FileBoundary.OpenOrRaise(file, GameProfile.WorldMap);
        StrategicMap map = (StrategicMap)engine.CreateEngineObject(tree.ClassId, engine);
        StrategicMap previous = StrategicMap.Current;
        StrategicMap.Current = map;
        Exception? caught = null;
        try
        {
            engine.ReadTree(tree, objectValue: map, progress: null, preview: false);
            engine.ActivateEngineObject(map);
        }
        catch (Exception error)
        {
            caught = error;
        }
        finally
        {
            try
            {
                map.Dispose();
            }
            catch (Exception error)
            {
                caught ??= error;
            }
            StrategicMap.Current = previous;
        }
        Assert.Fail(caught?.ToString() ?? FileBoundary.LastFailure?.ToString() ?? "the map load returned null");
    }

    // A hex sprite accepts only a field as its owner, so the dungeon stands on the one field.
    private static Item PrisonerItem(StrategicMap map)
    {
        MapLevel level = map.MapContainer[0];
        for (int fieldIndex = 0; fieldIndex < level.GetCount(); fieldIndex++)
        {
            MapField field = (MapField)level.GetChild(fieldIndex);
            for (int spriteIndex = 0; spriteIndex < field.GetCount(); spriteIndex++)
            {
                if (field.GetChild(spriteIndex) is Dungeon dungeon)
                {
                    Assert.Equal(1, dungeon.Prisoners.Items.Count);
                    return dungeon.Prisoners.Items[0];
                }
            }
        }
        Assert.Fail("The map has no dungeon.");
        return null!;
    }

    private static JsonObject Document()
    {
        JsonObject blade = new JsonObject()
            .Add("$class", new JsonString("Item"))
            .Add("$slot", new JsonInteger(0))
            .Add("Name", new JsonString("Blade"))
            .Add("ItemId", new JsonInteger(7))
            .Add("LibraryId", new JsonInteger(3));
        JsonObject dungeon = new JsonObject()
            .Add("$class", new JsonString("Dungeon"))
            .Add("ResourceId", new JsonInteger(PackResourceId))
            .Add("Prisoners", new JsonObject().Add("Items", new JsonObject().Add("Children", new JsonArray().Add(blade))));
        JsonObject level = new JsonObject()
            .Add("$class", new JsonString("MapLevel"))
            .Add("$slot", new JsonInteger(0))
            .Add("Width", new JsonInteger(1))
            .Add("Height", new JsonInteger(1))
            .Add("Fields", new JsonArray().Add(new JsonArray().Add(new JsonObject().Add("HexSpriteList", new JsonArray().Add(dungeon)))));
        JsonObject container = new JsonObject()
            .Add("$class", new JsonString("MapContainer"))
            .Add("$slot", new JsonInteger(6))
            .Add("Width", new JsonInteger(48))
            .Add("Height", new JsonInteger(1))
            .Add("LevelCount", new JsonInteger(1))
            .Add("Children", new JsonArray().Add(level));
        // A load clears the player list the constructor built. The independent player and one
        // ruler are what the load dialog counts, and an army leaves the independent player's list
        // when the map closes.
        JsonObject players = new JsonObject().Add("Children", new JsonArray()
            .Add(new JsonObject().Add("$class", new JsonString("Player")).Add("$slot", new JsonInteger(0)).Add("PlayerIndex", new JsonInteger(0)))
            .Add(new JsonObject().Add("$class", new JsonString("Player")).Add("$slot", new JsonInteger(1)).Add("PlayerIndex", new JsonInteger(1))));
        JsonObject root = new JsonObject()
            .Add("$class", new JsonString("StrategicMap"))
            .Add("Day", new JsonInteger(4))
            .Add("Players", players)
            .Add("StructureControl", new JsonObject().Add("CityCount", new JsonInteger(2)))
            .Add("Children", new JsonArray().Add(container));
        return new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
            .Add("$kind", new JsonString("save"))
            .Add("$container", new JsonObject().Add("profile", new JsonString("world-hsm")).Add("compression", new JsonString("cfs")).Add("level", new JsonInteger(2)))
            .Add("root", root);
    }
}
