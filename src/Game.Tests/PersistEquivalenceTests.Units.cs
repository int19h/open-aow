using Aow.Engine.Nodes;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// PE-3-b: the classes of Game/Units and Game/Magic/Abilities whose instances the installed corpus
// does not hold, and both sides of each branch that depends on game state.
public sealed partial class PersistEquivalenceTests
{
    [Fact]
    public void AnArmyWithAUnitMatchesTheConverter()
    {
        WithUnitResource(resource => WithDestroyingMap(_ =>
        {
            var army = new Army(null!) { Holder = EngineFixture.Engine };
            var unit = new Unit(null!) { Holder = EngineFixture.Engine, UnitResource = resource, UnitId = 7, GroupId = 2, MoraleValue = -3, Player = 1 };
            unit.SetOwner(army);
            Check("Army", army);
        }));
    }

    // A unit with no resource stores -1, and a load of that index refuses on both paths with the
    // message of ReadWrite.
    [Fact]
    public void AUnitWithoutAResourceStoresMinusOneAndBothLoadsRefuseIt()
    {
        using var unit = new Unit(null!) { Holder = EngineFixture.Engine };
        var classValue = s_schema.Get("Unit");
        var image = Image(unit);
        var tree = TreeWriter.Write(unit, Factory);
        EqualBytes(image, OriginalBackend.Write(tree, classValue, s_schema, "game"), "Unit original");
        var document = DocumentBackend.Write(tree, classValue, s_schema, "game");
        Assert.Equal(ushort.MaxValue, ((JsonInteger)document["UnitResourceIndex"]!).Value);
        EqualJson(PilotImage.Decode(image, "Unit"), document, "Unit write");
        using var viaReadWrite = new Unit(null!) { Holder = EngineFixture.Engine };
        using var viaPersist = new Unit(null!) { Holder = EngineFixture.Engine };
        var streamError = Assert.Throws<KeyNotFoundException>(() => Load(viaReadWrite, image));
        var treeError = Assert.Throws<KeyNotFoundException>(() => TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory));
        Assert.Equal("unit resource missing (-1)", streamError.Message);
        Assert.Equal(streamError.Message, treeError.Message);
    }

    // Hero.ReadWrite skips the inventory and the worn items and keeps the player it had while the
    // hero updates from its library. The write of such a hero and both loads into one agree.
    [Fact]
    public void AHeroUpdatingFromItsLibraryKeepsItsPlayerAndSkipsItsItems()
    {
        WithDestroyingMap(_ =>
        {
            var classValue = s_schema.Get("Hero");
            Check("Hero", new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Plain", Player = 2, LibraryId = 4 });
            var updating = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Updating", Player = 2 };
            updating.Mode = Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
            var written = DocumentBackend.Write(TreeWriter.Write(updating, Factory), classValue, s_schema, "game");
            Assert.Null(written.Find("Inventory"));
            Assert.Null(written.Find("HeroItems"));
            // A blank load is not updating, so it writes the items the converter's image lacks.
            Check("Hero", updating, converterRead: false);

            using var source = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Source", Player = 2 };
            new Item(null!) { Holder = EngineFixture.Engine, ItemId = 9, Name = "Ring" }.SetOwner(source.Inventory);
            var image = Image(source);
            using var viaReadWrite = Updating(player: 5);
            using var viaPersist = Updating(player: 5);
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
            Assert.Equal(5, viaReadWrite.Player);
            Assert.Equal(5, viaPersist.Player);
            Assert.Equal("Source", viaPersist.HeroName);
            Assert.Null(viaReadWrite.Inventory.GetItem(0));
            Assert.Null(viaPersist.Inventory.GetItem(0));
            EqualBytes(Image(viaReadWrite), Image(viaPersist), "Hero updating load");

            using var plain = new Hero(null!) { Holder = EngineFixture.Engine, Player = 5 };
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), plain, Factory);
            Assert.Equal(2, plain.Player);
            Assert.Equal(9, plain.Inventory.GetItem(0).ItemId);
        });

        static Hero Updating(sbyte player)
        {
            var hero = new Hero(null!) { Holder = EngineFixture.Engine, Player = player };
            hero.Mode = Set<HeroModeElement>.Of(HeroModeElement.UpdatingFromLibrary);
            return hero;
        }
    }

    // A hero of an older document stores no experience and its old experience under property 9.
    // Both loads derive the experience from it, and neither write stores it again.
    [Fact]
    public void AnOldHeroDerivesItsExperienceOnBothLoads()
    {
        WithDestroyingMap(_ =>
        {
            var classValue = s_schema.Get("Hero");
            using var source = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Old", Experience = 0 };
            var document = PilotImage.Decode(Image(source), "Hero");
            int at = document.Members.FindIndex(member => member.Name == "HeroName");
            document.Members.Insert(at, new JsonMember("OldExperience", new JsonInteger(3 << 5)));
            var image = PilotImage.Encode(document, "Hero");
            using var viaReadWrite = new Hero(null!) { Holder = EngineFixture.Engine };
            using var viaPersist = new Hero(null!) { Holder = EngineFixture.Engine };
            Load(viaReadWrite, image);
            TreeReader.Read(OriginalBackend.Read(image, classValue, s_schema, "game"), viaPersist, Factory);
            Assert.Equal(source.LevelToExperience(4), viaReadWrite.Experience);
            Assert.Equal(viaReadWrite.Experience, viaPersist.Experience);
            EqualBytes(Image(viaReadWrite), Image(viaPersist), "Hero old experience");
            Assert.Null(DocumentBackend.Write(TreeWriter.Write(viaPersist, Factory), classValue, s_schema, "game").Find("OldExperience"));
        });
    }

    [Fact]
    public void ArmyIdMatchesTheConverter()
    {
        Check("ArmyId", new ArmyId(null!) { Holder = EngineFixture.Engine });
        var full = new ArmyId(null!) { Holder = EngineFixture.Engine, Player = -1, X = sbyte.MinValue, Y = sbyte.MaxValue, MapLevel = 1 };
        full.UnitIds.Add(int.MinValue);
        full.UnitIds.Add(0);
        full.UnitIds.Add(int.MaxValue);
        Check("ArmyId", full);
    }

    [Fact]
    public void ItemLibraryMatchesTheConverter()
    {
        Check("ItemLibrary", new ItemLibrary(null!) { Holder = EngineFixture.Engine });
        var library = new ItemLibrary(null!) { Holder = EngineFixture.Engine, Name = "Mine" };
        PlainItem(itemId: 1, name: "Sword").SetOwner(library);
        PlainItem(itemId: 2, name: "Shield").SetOwner(library);
        Check("ItemLibrary", library);
    }

    // A unit graphics resource with its five sound libraries. The sound libraries hold no
    // sequence, because SfxSequence belongs to a later family. GameSetTests and the image
    // sequence tests of PE-2c-5 cover the sequences.
    [Fact]
    public void UnitGraphicsResourceMatchesTheConverter()
    {
        Check("UnitGraphicsResource", new UnitGraphicsResource(null!) { Holder = EngineFixture.Engine });
        var full = new UnitGraphicsResource(null!) { Holder = EngineFixture.Engine, Name = "Archer", MoveAnimationSpeed = 255 };
        Check("UnitGraphicsResource", full);
    }

    // The library managers store nothing of their own: ReadWrite and PersistFields read and write
    // the system library as Heroes.PFS and ITEMS.PFS beside the scenario set, here in the
    // fixture's temporary root.
    [Fact]
    public void TheLibraryManagersReadAndWriteTheirSystemLibraryAlike()
    {
        string heroes = Path.Combine(EngineFixture.Root, "Heroes.PFS");
        string items = Path.Combine(EngineFixture.Root, "ITEMS.PFS");
        try
        {
            var heroManager = new HeroLibraryManager(null!) { Holder = EngineFixture.Engine };
            heroManager.SystemLibrary.LibraryName = "System";
            Check("HeroLibraryManager", heroManager);
            var heroBytes = File.ReadAllBytes(heroes);
            using (var viaPersist = new HeroLibraryManager(null!) { Holder = EngineFixture.Engine })
            {
                TreeReader.Read(DocumentBackend.Read(new JsonObject(), s_schema.Get("HeroLibraryManager"), s_schema, "game"), viaPersist, Factory);
                Assert.Equal("System", viaPersist.SystemLibrary.LibraryName);
            }
            using (var writer = new HeroLibraryManager(null!) { Holder = EngineFixture.Engine })
            {
                writer.SystemLibrary.LibraryName = "System";
                TreeWriter.Write(writer, Factory);
                Assert.Equal(heroBytes, File.ReadAllBytes(heroes));
            }

            var itemManager = new ItemLibraryManager(null!) { Holder = EngineFixture.Engine };
            itemManager.SystemLibrary.Name = "System";
            PlainItem(itemId: 5, name: "Staff").SetOwner(itemManager.SystemLibrary);
            Check("ItemLibraryManager", itemManager);
            using (var viaPersist = new ItemLibraryManager(null!) { Holder = EngineFixture.Engine })
            {
                TreeReader.Read(DocumentBackend.Read(new JsonObject(), s_schema.Get("ItemLibraryManager"), s_schema, "game"), viaPersist, Factory);
                Assert.Equal("System", viaPersist.SystemLibrary.Name);
                Assert.Equal("Staff", viaPersist.SystemLibrary[0].Name);
            }
        }
        finally
        {
            File.Delete(heroes);
            File.Delete(items);
        }
    }

    // A unit loads its resource from the game set by index, so the test gives the game set a
    // list of one resource and puts the fixture's list back after.
    private static void WithUnitResource(Action<UnitResource> body)
    {
        var gameSet = GamePack.Current;
        var previous = gameSet.UnitResources;
        var resources = new UnitResourceList(null!) { Holder = EngineFixture.Engine };
        var resource = new UnitResource(resources);
        gameSet.UnitResources = resources;
        try
        {
            Assert.Equal(0, resource.UnitResourceIndex());
            body(resource);
        }
        finally
        {
            gameSet.UnitResources = previous;
            resources.Dispose();
        }
    }
}
